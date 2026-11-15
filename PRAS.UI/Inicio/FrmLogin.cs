using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade;
using PRAS.Services.Facade.Extensions;
using PRAS.BLL.Excepciones;

namespace PRAS.UI
{
    /// <summary>
    /// CU Seguridad.001 - Iniciar Sesión.
    /// Si el login es correcto, el formulario se cierra con DialogResult.OK
    /// y Program.cs abre el menú principal.
    /// </summary>
    public partial class FrmLogin : FormBase
    {
        private const int MaximoIntentos = 3;
        private readonly UsuarioService _usuarioService = new UsuarioService();
        private readonly IntegridadService _integridadService = new IntegridadService();
        private readonly ClienteService _clienteService = new ClienteService();
        private int _intentosFallidos = 0;
        private readonly DatosPruebaService _datosPruebaService = new DatosPruebaService();

        /// <summary>Link "Usuarios de prueba" (se crea por código; solo si App.config lo permite).</summary>
        private readonly LinkLabel lnkUsuariosPrueba = new LinkLabel();

        /// <summary>Link "Solicitar acceso" (CU.Seguridad.016): siempre visible.</summary>
        private readonly LinkLabel lnkSolicitarAcceso = new LinkLabel();

        public FrmLogin()
        {
            InitializeComponent();
            AgregarLinkSolicitarAcceso();
            AgregarLinkUsuariosDePrueba();
        }

        /// <summary>
        /// "Solicitar acceso": una persona sin usuario pide su cuenta. Queda deshabilitada hasta que
        /// un administrador la habilite (así nadie se puede crear un Administrador por su cuenta).
        /// </summary>
        private void AgregarLinkSolicitarAcceso()
        {
            lnkSolicitarAcceso.Name = "lnkSolicitarAcceso";
            lnkSolicitarAcceso.Text = "Solicitar acceso";
            lnkSolicitarAcceso.AutoSize = true;
            lnkSolicitarAcceso.Location = new Point(lnkOlvideContrasena.Left, lnkOlvideContrasena.Bottom + 10);
            lnkSolicitarAcceso.LinkClicked += (sender, e) =>
            {
                using (var frm = new FrmSolicitarCuenta())
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        txtUsuario.Text = frm.UsuarioSolicitado;
                        txtContrasena.Clear();
                    }
                }
            };
            Controls.Add(lnkSolicitarAcceso);
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + 28);
        }

        /// <summary>
        /// Debajo de "¿Olvidó su contraseña?" va el link "Usuarios de prueba", que abre la tabla con
        /// los usuarios para mostrar el sistema. Solo aparece si App.config tiene
        /// MostrarUsuariosDePrueba = true (en una instalación real se apaga).
        /// </summary>
        private void AgregarLinkUsuariosDePrueba()
        {
            if (!_datosPruebaService.MostrarUsuariosDePrueba)
                return;

            lnkUsuariosPrueba.Name = "lnkUsuariosPrueba";
            lnkUsuariosPrueba.Text = "Usuarios de prueba";
            lnkUsuariosPrueba.AutoSize = true;
            lnkUsuariosPrueba.Location = new Point(lnkSolicitarAcceso.Left, lnkSolicitarAcceso.Bottom + 10);
            lnkUsuariosPrueba.LinkClicked += lnkUsuariosPrueba_LinkClicked;
            Controls.Add(lnkUsuariosPrueba);
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + 28);
        }

        private void lnkUsuariosPrueba_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var frm = new FrmUsuariosDePrueba())
            {
                if (frm.ShowDialog(this) == DialogResult.OK && frm.UsuarioElegido != null)
                {
                    txtUsuario.Text = frm.UsuarioElegido;
                    txtContrasena.Clear();
                    txtContrasena.Focus();
                }
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = _usuarioService.IniciarSesion(txtUsuario.Text, txtContrasena.Text);

                // Pasamos al idioma que este usuario eligió la última vez
                // (si nunca eligió, queda en español).
                IdiomaService.CambiarIdioma(usuario.Idioma);

                // Antes de entrar revisamos los dígitos verificadores, para ver que nadie
                // haya tocado la base por afuera del sistema.
                if (!IntegridadCorrecta(usuario))
                {
                    _usuarioService.CerrarSesion();
                    IdiomaService.CambiarIdioma("es-AR");
                    txtContrasena.Clear();
                    return;
                }

                // Datos de clientes cargados antes del encriptado AES: se encriptan ahora (una sola vez).
                _clienteService.EncriptarDatosPendientes();

                DialogResult = DialogResult.OK;   // cierra el formulario
            }
            catch (ReglaNegocioException ex)
            {
                _intentosFallidos++;
                if (_intentosFallidos >= MaximoIntentos)
                {
                    Mensajes.Advertencia($"Superó los {MaximoIntentos} intentos permitidos. El sistema se cerrará.");
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                Mensajes.Advertencia(ex.Message);
                txtContrasena.Clear();
                txtContrasena.Focus();
            }
            catch (Exception ex)
            {
                if (Sesion.HayUsuarioLogueado)
                    _usuarioService.CerrarSesion();
                Mensajes.Error(ex);   // por ejemplo, no hay conexión con la base
            }
        }

        /// <summary>
        /// "¿Olvidó su contraseña?": abre Recuperar contraseña con el usuario que ya escribió.
        /// </summary>
        private void lnkOlvideContrasena_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var frm = new FrmRecuperarContrasena(txtUsuario.Text))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    txtContrasena.Clear();
                    txtContrasena.Focus();
                }
            }
        }

        /// <summary>
        /// Verifica los dígitos verificadores (DVH y DVV). Devuelve true si se puede entrar.
        /// - Si la base está bien: entra.
        /// - Si hay problemas y es Administrador: se le muestra la pantalla de Integridad
        ///   para que recalcule los dígitos o restaure un backup.
        /// - Si hay problemas y NO es Administrador: no puede entrar.
        /// </summary>
        private bool IntegridadCorrecta(Usuario usuario)
        {
            List<string> errores = _integridadService.Verificar();
            if (errores.Count == 0)
                return true;

            if (!Sesion.TienePermiso(Funcion.VerificarIntegridad))
            {
                Mensajes.Advertencia(
                    "Se detectaron cambios en la base de datos realizados por fuera del sistema.".Traducir() + "\n\n" +
                    "Solo un Administrador puede ingresar para revisarlo.".Traducir());
                return false;
            }

            using (var frm = new FrmIntegridad(errores))
            {
                frm.ShowDialog(this);

                if (frm.SeRestauroBackup)
                {
                    Mensajes.Info("La base fue restaurada. Vuelva a iniciar sesión.".Traducir());
                    return false;
                }
            }

            return true;
        }
    }
}
