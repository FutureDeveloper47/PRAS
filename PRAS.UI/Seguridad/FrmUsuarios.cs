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

        // Columnas y botón agregados por código (Mi perfil > Administrar usuarios).
        private readonly DataGridViewTextBoxColumn colNombre = new DataGridViewTextBoxColumn();
        private readonly DataGridViewTextBoxColumn colEstado = new DataGridViewTextBoxColumn();
        private readonly DataGridViewTextBoxColumn colUltimoIngreso = new DataGridViewTextBoxColumn();
        private readonly Button btnRestablecer = new Button();
        private readonly Label lblAvisoContrasenas = new Label();

        public FrmUsuarios()
        {
            InitializeComponent();
            dgvUsuarios.AutoGenerateColumns = false;   // usamos solo las columnas del diseñador
            AgregarControlDeUsuarios();
        }

        /// <summary>
        /// Lista completa para el control de usuarios: usuario, nombre, rol, estado y último ingreso,
        /// y el botón "Restablecer contraseña". Las contraseñas NO aparecen en ningún lado:
        /// se guardan con hash SHA-256 y no se pueden leer.
        /// </summary>
        private void AgregarControlDeUsuarios()
        {
            // Se ven todos los usuarios, también los deshabilitados: ahí aparecen las cuentas pedidas
            // desde el ingreso (Solicitar acceso), que esperan que el administrador las habilite.
            chkMostrarDeshabilitados.Checked = true;

            // La pantalla se agranda para que entren las columnas nuevas (la grilla está anclada y crece sola).
            ClientSize = new Size(Math.Max(ClientSize.Width, 900), Math.Max(ClientSize.Height, 440));

            colNombre.Name = "colNombre";
            colNombre.HeaderText = "Nombre y apellido";
            colNombre.DataPropertyName = nameof(Usuario.NombrePersona);   // del mecánico o de sus datos personales
            colNombre.ReadOnly = true;

            colEstado.Name = "colEstado";
            colEstado.HeaderText = "Estado";
            colEstado.DataPropertyName = nameof(Usuario.Activo);
            colEstado.ReadOnly = true;

            colUltimoIngreso.Name = "colUltimoIngreso";
            colUltimoIngreso.HeaderText = "Último ingreso";
            colUltimoIngreso.DataPropertyName = nameof(Usuario.UltimoIngreso);
            colUltimoIngreso.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            colUltimoIngreso.ReadOnly = true;

            // El nombre reemplaza a la columna "Mecánico" (ya muestra el nombre del mecánico)
            // y "Estado" en texto reemplaza a la casilla "Activo".
            colMecanico.Visible = false;
            colActivo.Visible = false;
            dgvUsuarios.Columns.Insert(1, colNombre);
            dgvUsuarios.Columns.Add(colEstado);
            dgvUsuarios.Columns.Add(colUltimoIngreso);

            // Botones de la derecha más anchos, y el nuevo debajo de Deshabilitar.
            int anchoBotones = 180;
            int x = ClientSize.Width - anchoBotones - 12;
            dgvUsuarios.Width = x - dgvUsuarios.Left - 12;
            foreach (Button boton in new[] { btnNuevo, btnModificar, btnDeshabilitar, btnCerrar })
                boton.SetBounds(x, boton.Top, anchoBotones, boton.Height);

            btnRestablecer.Name = "btnRestablecer";
            btnRestablecer.Text = "Restablecer contraseña";
            btnRestablecer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRestablecer.SetBounds(x, btnDeshabilitar.Bottom + 6, anchoBotones, btnDeshabilitar.Height);
            btnRestablecer.Click += btnRestablecer_Click;
            Controls.Add(btnRestablecer);

            lblAvisoContrasenas.Name = "lblAvisoContrasenas";
            lblAvisoContrasenas.Text = "Las contraseñas no se pueden ver: se guardan con hash SHA-256.";
            lblAvisoContrasenas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblAvisoContrasenas.SetBounds(x, btnRestablecer.Bottom + 8, anchoBotones, 110);
            lblAvisoContrasenas.ForeColor = Tema.TextoApagado;
            Controls.Add(lblAvisoContrasenas);
        }

        /// <summary>El administrador le pone una contraseña temporal a otro usuario.</summary>
        private void btnRestablecer_Click(object sender, EventArgs e)
        {
            Usuario usuario = UsuarioSeleccionado();
            if (usuario == null)
            {
                Mensajes.Advertencia("Seleccione un usuario de la lista.");
                return;
            }

            using var frm = new FrmRestablecerContrasena(usuario);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
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
                e.Value = Sesion.TextoRol(rol).Traducir();
                e.FormattingApplied = true;
            }
            else if (dgvUsuarios.Columns[e.ColumnIndex] == colEstado && e.Value is bool activo)
            {
                e.Value = (activo ? "Habilitado" : "Deshabilitado").Traducir();
                e.CellStyle.ForeColor = activo ? Tema.Texto : Tema.Peligro;
                e.FormattingApplied = true;
            }
            else if (dgvUsuarios.Columns[e.ColumnIndex] == colUltimoIngreso && e.Value == null)
            {
                e.Value = "Nunca".Traducir();
                e.FormattingApplied = true;
            }
            else if (dgvUsuarios.Columns[e.ColumnIndex] == colNombre && e.Value == null)
            {
                e.Value = "-";
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
