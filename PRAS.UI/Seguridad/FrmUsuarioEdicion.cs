using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Ficha de un usuario. Se usa para las dos operaciones:
    /// - Alta (CU .002): se abre con usuario = null.
    /// - Modificar (CU .003): se abre con el usuario elegido en la grilla.
    /// Si el rol es Mecánico, se elige también a qué mecánico del taller corresponde.
    /// </summary>
    public partial class FrmUsuarioEdicion : FormBase
    {
        private readonly UsuarioService _usuarioService = new UsuarioService();
        private readonly MecanicoService _mecanicoService = new MecanicoService();
        private readonly Usuario _usuario;   // null es alta
        private bool EsAlta => _usuario == null;

        public FrmUsuarioEdicion(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
        }

        private void FrmUsuarioEdicion_Load(object sender, EventArgs e)
        {
            try
            {
                CargarMecanicos();
                CargarRoles();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }

            if (EsAlta)
            {
                Text = "Alta de usuario";
                lblContrasena.Text = "Contraseña".Traducir() + ": *";
                lblConfirmacion.Text = "Repetir contraseña".Traducir() + ": *";
                cboRol.SelectedValue = Rol.Mecanico;
                cboMecanico.SelectedIndex = -1;   // que el administrador elija el mecánico a propósito
            }
            else
            {
                Text = "Modificar usuario";
                txtNombreUsuario.Text = _usuario.NombreUsuario;
                cboRol.SelectedValue = _usuario.Rol;
                if (_usuario.IdMecanico != null)
                    cboMecanico.SelectedValue = _usuario.IdMecanico.Value;
                else
                    cboMecanico.SelectedIndex = -1;
                lblAyudaContrasena.Text = "Dejar vacío para no cambiar la contraseña. Mínimo 6 caracteres.".Traducir();
            }

            ActualizarComboMecanico();
        }

        /// <summary>Llena el combo con los roles. Muestra el texto con ñ pero guarda el valor del enum.</summary>
        private void CargarRoles()
        {
            var roles = new List<KeyValuePair<Rol, string>>();
            foreach (Rol rol in Enum.GetValues(typeof(Rol)))
                roles.Add(new KeyValuePair<Rol, string>(rol, Sesion.TextoRol(rol)));

            cboRol.DisplayMember = "Value";
            cboRol.ValueMember = "Key";
            cboRol.DataSource = roles;
        }

        /// <summary>
        /// Llena el combo con los mecánicos activos. Si el usuario que se modifica ya estaba
        /// vinculado a un mecánico que después se deshabilitó, también se muestra ese.
        /// </summary>
        private void CargarMecanicos()
        {
            int? idMecanicoActual = EsAlta ? null : _usuario.IdMecanico;

            var mecanicos = new List<Mecanico>();
            foreach (Mecanico mecanico in _mecanicoService.Listar("", true))
                if (mecanico.Activo || mecanico.IdMecanico == idMecanicoActual)
                    mecanicos.Add(mecanico);

            cboMecanico.DisplayMember = "NombreCompleto";
            cboMecanico.ValueMember = "IdMecanico";
            cboMecanico.DataSource = mecanicos;
        }

        /// <summary>El combo de mecánico solo se usa si el rol elegido es Mecánico.</summary>
        private void ActualizarComboMecanico()
        {
            bool esMecanico = cboRol.SelectedValue is Rol rol && rol == Rol.Mecanico;
            cboMecanico.Enabled = esMecanico;
            lblMecanico.Text = "Mecánico".Traducir() + (esMecanico ? ": *" : ":");
            if (!esMecanico)
                cboMecanico.SelectedIndex = -1;
        }

        private void cboRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarComboMecanico();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Armamos un objeto nuevo con lo cargado. Las validaciones las hace la BLL.
            var usuario = new Usuario
            {
                IdUsuario = EsAlta ? 0 : _usuario.IdUsuario,
                NombreUsuario = txtNombreUsuario.Text,
                Rol = (Rol)cboRol.SelectedValue,
                IdMecanico = cboMecanico.SelectedValue as int?   // null si no eligió ninguno
            };

            try
            {
                if (EsAlta)
                {
                    _usuarioService.Alta(usuario, txtContrasena.Text, txtConfirmacion.Text);
                    Mensajes.Info("Usuario dado de alta correctamente.");
                }
                else
                {
                    _usuarioService.Modificar(usuario, txtContrasena.Text, txtConfirmacion.Text);
                    Mensajes.Info("Usuario modificado correctamente.");
                }
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
