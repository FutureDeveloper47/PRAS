using PRAS.BLL;
using PRAS.BLL.Excepciones;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// CU Seguridad - Recuperar contraseña (se abre desde el ingreso: "¿Olvidó su contraseña?").
    /// Pasos:
    /// 1) Escribe su usuario y toca "Ver pregunta de seguridad".
    /// 2) Contesta la pregunta y escribe la contraseña nueva dos veces.
    /// 3) Si la respuesta es correcta, se guarda la contraseña nueva.
    /// Abajo se muestran los datos del administrador por si no la puede recuperar.
    /// Toda la lógica está en la BLL (UsuarioService); esta pantalla solo muestra y avisa.
    /// </summary>
    public partial class FrmRecuperarContrasena : FormBase
    {
        private const int MaximoIntentos = 3;

        private readonly UsuarioService _usuarioService = new UsuarioService();
        private readonly SoporteService _soporteService = new SoporteService();
        private int _intentosFallidos;

        /// <summary>
        /// "mecanico — Carlos Ferreyra (Mecánico)": a quién pertenece el usuario escrito.
        /// Se crea por código debajo del usuario y corre hacia abajo lo que sigue.
        /// </summary>
        private readonly Label lblTitular = new Label();
        private const int AltoTitular = 26;

        /// <param name="usuario">Lo que ya había escrito en el ingreso (puede venir vacío).</param>
        public FrmRecuperarContrasena(string usuario)
        {
            InitializeComponent();
            txtUsuario.Text = usuario ?? "";
            AgregarTitular();
        }

        private void AgregarTitular()
        {
            // Todo lo que está debajo del usuario baja un renglón para hacerle lugar.
            foreach (Control control in Controls)
                if (control != lblTitulo && control.Top > txtUsuario.Bottom)
                    control.Top += AltoTitular;
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + AltoTitular);

            lblTitular.Name = "lblTitular";
            lblTitular.AutoSize = false;
            lblTitular.SetBounds(txtUsuario.Left, txtUsuario.Bottom + 6, btnVerPregunta.Right - txtUsuario.Left, 20);
            lblTitular.Font = new Font(Font, FontStyle.Bold);
            lblTitular.ForeColor = Tema.Primario;
            Controls.Add(lblTitular);
        }

        private void FrmRecuperarContrasena_Load(object sender, EventArgs e)
        {
            lblContacto.Text = TextoContacto(_soporteService);
        }

        /// <summary>Nombre, celular y email del administrador (salen de App.config).</summary>
        internal static string TextoContacto(SoporteService soporte)
        {
            string texto = soporte.NombreAdministrador();
            if (soporte.TelefonoAdministrador() != "")
                texto += "\n" + "Celular (WhatsApp)".Traducir() + ": " + soporte.TelefonoAdministrador();
            texto += "\n" + "Email: " + soporte.EmailAdministrador();
            return texto;
        }

        /// <summary>Paso 1: busca la pregunta de seguridad del usuario.</summary>
        private void btnVerPregunta_Click(object sender, EventArgs e)
        {
            try
            {
                DatosRecuperacion datos = _usuarioService.ObtenerDatosRecuperacion(txtUsuario.Text);

                // A quién pertenece: "mecanico — Carlos Ferreyra (Mecánico)" (el rol en el idioma elegido).
                string rol = Sesion.TextoRol(datos.Rol).Traducir();
                lblTitular.Text = string.IsNullOrEmpty(datos.NombrePersona)
                    ? $"{datos.NombreUsuario} ({rol})"
                    : $"{datos.NombreUsuario} — {datos.NombrePersona} ({rol})";

                // La pregunta se guarda en español; si es una de las preguntas del sistema se ve traducida.
                lblPregunta.Text = datos.Pregunta.TraducirMensaje();
                txtUsuario.ReadOnly = true;
                HabilitarPaso2(true);
                txtRespuesta.Focus();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Paso 2: valida la respuesta y guarda la contraseña nueva.</summary>
        private void btnGuardarContrasena_Click(object sender, EventArgs e)
        {
            try
            {
                _usuarioService.RecuperarContrasena(txtUsuario.Text, txtRespuesta.Text, txtNueva.Text, txtConfirmacion.Text);

                Mensajes.Info("La contraseña se cambió correctamente. Ya puede ingresar con la contraseña nueva.");
                DialogResult = DialogResult.OK;
            }
            catch (RespuestaSeguridadIncorrectaException ex)
            {
                // Respuesta incorrecta: le damos pocos intentos para que no se pueda adivinar probando.
                _intentosFallidos++;
                Mensajes.Error(ex);
                txtRespuesta.Clear();

                if (_intentosFallidos >= MaximoIntentos)
                {
                    Mensajes.Advertencia("Superó los intentos permitidos. Comuníquese con el administrador.");
                    DialogResult = DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void chkMostrar_CheckedChanged(object sender, EventArgs e)
        {
            txtNueva.UseSystemPasswordChar = !chkMostrar.Checked;
            txtConfirmacion.UseSystemPasswordChar = !chkMostrar.Checked;
        }

        private void HabilitarPaso2(bool habilitar)
        {
            txtRespuesta.Enabled = habilitar;
            txtNueva.Enabled = habilitar;
            txtConfirmacion.Enabled = habilitar;
            btnGuardarContrasena.Enabled = habilitar;
            btnVerPregunta.Enabled = !habilitar;
        }
    }
}
