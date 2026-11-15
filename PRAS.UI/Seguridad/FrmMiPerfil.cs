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

        private readonly Label lblTema = new Label();
        private readonly ComboBox cmbTema = new ComboBox();
        private bool _cargandoTema;

        public FrmMiPerfil()
        {
            InitializeComponent();
            AgregarTema();
            AgregarAdministrarUsuarios();
        }

        /// <summary>
        /// Renglón "Tema: Claro / Oscuro" en Mis datos (debajo de Idioma). Se aplica al elegirlo:
        /// todas las ventanas abiertas se repintan y queda guardado para la próxima vez.
        /// Lo de abajo se corre un renglón para hacerle lugar.
        /// </summary>
        private void AgregarTema()
        {
            int alto = cmbIdioma.Height + 14;
            foreach (Control control in Controls)
                if (control.Top > grpDatos.Top)
                    control.Top += alto;
            grpDatos.Height += alto;
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + alto);

            lblTema.Text = "Tema:";
            lblTema.AutoSize = true;
            lblTema.Location = new Point(lblIdioma.Left, lblIdioma.Top + alto);
            cmbTema.Name = "cmbTema";
            cmbTema.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTema.Items.AddRange(new object[] { PreferenciasLocales.TemaClaro, PreferenciasLocales.TemaOscuro });   // se ven traducidos
            cmbTema.SetBounds(cmbIdioma.Left, cmbIdioma.Top + alto, cmbIdioma.Width, cmbIdioma.Height);
            cmbTema.SelectedIndexChanged += cmbTema_SelectedIndexChanged;
            grpDatos.Controls.Add(lblTema);
            grpDatos.Controls.Add(cmbTema);

            _cargandoTema = true;
            cmbTema.SelectedIndex = Tema.Oscuro ? 1 : 0;
            _cargandoTema = false;
        }

        private void cmbTema_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoTema)
                return;

            bool oscuro = cmbTema.SelectedIndex == 1;
            Tema.CambiarModo(oscuro);
            PreferenciasLocales.GuardarTema(Sesion.UsuarioActual?.NombreUsuario, oscuro);
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
            lblPreguntaEstado.ForeColor = Tema.Exito;   // verde

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
