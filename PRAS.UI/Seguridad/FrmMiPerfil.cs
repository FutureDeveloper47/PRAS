using System.Globalization;
using PRAS.BLL;
using PRAS.Services.Facade;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// CU Seguridad - Mi perfil. Cualquier usuario (sin importar su rol) puede:
    /// - ver sus datos (usuario y rol) y cambiar su idioma,
    /// - cambiar su contraseña (escribiendo la actual),
    /// - elegir su pregunta de seguridad para poder recuperar la contraseña desde el ingreso.
    /// </summary>
    public partial class FrmMiPerfil : FormBase
    {
        private readonly UsuarioService _usuarioService = new UsuarioService();

        /// <summary>Solo lo ve quien tiene el permiso de administrar usuarios (patente GestionarUsuarios).</summary>
        private readonly Button btnAdministrarUsuarios = new Button();

        public FrmMiPerfil()
        {
            InitializeComponent();
            AgregarAdministrarUsuarios();
        }

        /// <summary>
        /// Botón "Administrar usuarios" (a la izquierda de Cerrar). Se decide por el PERMISO y no por el rol:
        /// si el administrador le saca la patente a alguien, el botón desaparece (Composite).
        /// </summary>
        private void AgregarAdministrarUsuarios()
        {
            if (!Sesion.TienePermiso(Funcion.GestionarUsuarios))
                return;

            btnAdministrarUsuarios.Name = "btnAdministrarUsuarios";
            btnAdministrarUsuarios.Text = "Administrar usuarios";
            btnAdministrarUsuarios.SetBounds(grpDatos.Left, btnCerrar.Top, 200, btnCerrar.Height);
            btnAdministrarUsuarios.Click += (sender, e) =>
            {
                using (var frm = new FrmUsuarios())
                    frm.ShowDialog(this);
            };
            Controls.Add(btnAdministrarUsuarios);
        }

        private void FrmMiPerfil_Load(object sender, EventArgs e)
        {
            var usuario = Sesion.UsuarioActual;
            lblUsuarioValor.Text = usuario.NombreUsuario;
            lblRolValor.Text = Sesion.TextoRol(usuario.Rol).Traducir();

            CargarIdiomas();

            CargarPreguntas();
            MostrarEstadoPregunta();
        }

        /// <summary>Llena el desplegable de preguntas (se vuelve a llenar si cambia el idioma).</summary>
        private void CargarPreguntas()
        {
            cmbPregunta.Items.Clear();
            foreach (string pregunta in _usuarioService.PreguntasDeSeguridad())
                cmbPregunta.Items.Add(new OpcionTraducida(pregunta));
        }

        private void CargarIdiomas()
        {
            cmbIdioma.Items.Clear();
            foreach (CultureInfo idioma in IdiomaService.ObtenerIdiomas())
            {
                cmbIdioma.Items.Add(new OpcionIdioma(idioma));
                if (idioma.Name == IdiomaService.IdiomaActual().Name)
                    cmbIdioma.SelectedIndex = cmbIdioma.Items.Count - 1;
            }
        }

        /// <summary>Muestra si ya tiene pregunta cargada y, si la tiene, la deja elegida en el desplegable.</summary>
        private void MostrarEstadoPregunta()
        {
            string actual = Sesion.UsuarioActual.PreguntaSeguridad;

            if (string.IsNullOrEmpty(actual))
            {
                lblPreguntaEstado.Text = "Todavía no cargó una pregunta de seguridad.".Traducir();
                lblPreguntaEstado.ForeColor = Tema.Peligro;
                return;
            }

            lblPreguntaEstado.Text = "Ya tiene una pregunta de seguridad cargada. Puede cambiarla.".Traducir();
            lblPreguntaEstado.ForeColor = Color.FromArgb(46, 125, 50);   // verde

            foreach (OpcionTraducida opcion in cmbPregunta.Items)
            {
                if (opcion.Original == actual)
                    cmbPregunta.SelectedItem = opcion;
            }
        }

        /// <summary>Cambia el idioma, lo guarda como preferido del usuario y vuelve a traducir esta pantalla.</summary>
        private void btnAplicarIdioma_Click(object sender, EventArgs e)
        {
            if (cmbIdioma.SelectedItem is not OpcionIdioma opcion)
                return;

            try
            {
                IdiomaService.CambiarIdioma(opcion.Idioma.Name);
                _usuarioService.CambiarIdioma(opcion.Idioma.Name);

                Traductor.Traducir(this);
                lblRolValor.Text = Sesion.TextoRol(Sesion.UsuarioActual.Rol).Traducir();
                CargarPreguntas();   // las preguntas se vuelven a mostrar en el idioma nuevo
                MostrarEstadoPregunta();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnGuardarContrasena_Click(object sender, EventArgs e)
        {
            try
            {
                _usuarioService.CambiarMiContrasena(txtActual.Text, txtNueva.Text, txtConfirmacion.Text);
                Mensajes.Info("La contraseña se cambió correctamente.");
                txtActual.Clear();
                txtNueva.Clear();
                txtConfirmacion.Clear();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnGuardarPregunta_Click(object sender, EventArgs e)
        {
            try
            {
                string pregunta = (cmbPregunta.SelectedItem as OpcionTraducida)?.Original;
                _usuarioService.GuardarMiPreguntaSeguridad(pregunta, txtRespuesta.Text);

                Mensajes.Info("La pregunta de seguridad se guardó correctamente.");
                txtRespuesta.Clear();
                MostrarEstadoPregunta();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Idioma del desplegable: se muestra con su nombre en su propio idioma ("English (United States)").</summary>
        private class OpcionIdioma
        {
            public CultureInfo Idioma { get; }

            public OpcionIdioma(CultureInfo idioma)
            {
                Idioma = idioma;
            }

            public override string ToString()
            {
                return Idioma.NativeName;
            }
        }
    }
}
