using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// ABM de Usuarios (solo Administrador): lista los usuarios y desde acá se
    /// da de alta (.002), se modifica (.003) y se deshabilita (.004).
    /// </summary>
    public partial class FrmUsuarios : FormBase
    {
        private readonly UsuarioService _usuarioService = new UsuarioService();

        public FrmUsuarios()
        {
            InitializeComponent();
            dgvUsuarios.AutoGenerateColumns = false;   // usamos solo las columnas del diseñador
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                dgvUsuarios.DataSource = _usuarioService.Listar(chkMostrarDeshabilitados.Checked);
                ActualizarBotonDeshabilitar();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Devuelve el usuario de la fila seleccionada, o null si no hay ninguna.</summary>
        private Usuario UsuarioSeleccionado()
        {
            return dgvUsuarios.CurrentRow?.DataBoundItem as Usuario;
        }

        /// <summary>El mismo botón sirve para deshabilitar o volver a habilitar, según la fila elegida.</summary>
        private void ActualizarBotonDeshabilitar()
        {
            Usuario usuario = UsuarioSeleccionado();
            btnDeshabilitar.Text = ((usuario != null && !usuario.Activo) ? "Habilitar" : "Deshabilitar").Traducir();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var frm = new FrmUsuarioEdicion(null);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Usuario usuario = UsuarioSeleccionado();
            if (usuario == null)
            {
                Mensajes.Advertencia("Seleccione un usuario de la lista.");
                return;
            }

            using var frm = new FrmUsuarioEdicion(usuario);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            Usuario usuario = UsuarioSeleccionado();
            if (usuario == null)
            {
                Mensajes.Advertencia("Seleccione un usuario de la lista.");
                return;
            }

            try
            {
                if (usuario.Activo)
                {
                    if (!Mensajes.Confirmar($"¿Deshabilitar al usuario \"{usuario.NombreUsuario}\"?\nNo podrá ingresar al sistema."))
                        return;
                    _usuarioService.Deshabilitar(usuario.IdUsuario);
                }
                else
                {
                    if (!Mensajes.Confirmar($"¿Volver a habilitar al usuario \"{usuario.NombreUsuario}\"?"))
                        return;
                    _usuarioService.Habilitar(usuario.IdUsuario);
                }
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void chkMostrarDeshabilitados_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                btnModificar_Click(sender, e);
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotonDeshabilitar();
        }

        /// <summary>
        /// Arregla lo que se muestra en algunas columnas:
        /// - Rol: texto amigable ("Dueño" en vez de "Dueno").
        /// - Mecánico: nombre del mecánico vinculado (la grilla no puede leer Usuario.Mecanico sola).
        /// </summary>
        private void dgvUsuarios_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUsuarios.Columns[e.ColumnIndex] == colRol && e.Value is Rol rol)
            {
                e.Value = Sesion.TextoRol(rol);
                e.FormattingApplied = true;
            }
            else if (dgvUsuarios.Columns[e.ColumnIndex] == colMecanico)
            {
                var usuario = dgvUsuarios.Rows[e.RowIndex].DataBoundItem as Usuario;
                // Si no tiene mecánico va "" y no null, porque la grilla necesita un texto.
                e.Value = usuario?.Mecanico?.NombreCompleto ?? "";
                e.FormattingApplied = true;
            }
        }
    }
}
