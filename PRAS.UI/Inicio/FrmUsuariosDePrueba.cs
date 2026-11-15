using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Tabla de "Usuarios de prueba" que se abre desde el link de la pantalla de ingreso:
    /// usuario, contraseña de prueba, rol y nombre. Sirve para mostrar el sistema con cada perfil.
    ///
    /// El link solo aparece si App.config tiene MostrarUsuariosDePrueba = true (en una instalación
    /// real se pone en false). Las contraseñas que se ven son las de PRUEBA, fijas y conocidas:
    /// no se leen de la base (ahí solo está el hash SHA-256). La BLL comprueba contra el hash que
    /// sigan siendo válidas; si alguien la cambió se muestra "(cambiada)".
    /// No tiene .Designer: se arma por código.
    /// </summary>
    public class FrmUsuariosDePrueba : FormBase
    {
        private readonly DatosPruebaService _datosPruebaService = new DatosPruebaService();

        private readonly Label lblTitulo = new Label();
        private readonly Label lblAclaracion = new Label();
        private readonly DataGridView dgvUsuarios = new DataGridView();
        private readonly Button btnCerrar = new Button();

        /// <summary>Usuario elegido con doble clic (el ingreso lo copia en la casilla "Usuario").</summary>
        public string UsuarioElegido { get; private set; }

        public FrmUsuariosDePrueba()
        {
            Name = "FrmUsuariosDePrueba";
            Text = "Usuarios de prueba";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            // Las medidas están pensadas para pantallas al 100 %: con AutoScaleMode.Dpi Windows las
            // agranda solo en pantallas al 125 % o 150 % (si no, la letra crece y el resto no).
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(760, 440);
            Font = new Font("Segoe UI", 9.75F);

            lblTitulo.Name = "lblTitulo";
            lblTitulo.Text = "Usuarios de prueba";

            lblAclaracion.Name = "lblAclaracion";
            lblAclaracion.Text = "Doble clic en un usuario para copiarlo en el ingreso. La respuesta de la pregunta de seguridad de todos es \"pras\".";
            lblAclaracion.SetBounds(20, 62, 720, 56);

            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.SetBounds(20, 122, 720, 256);
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToResizeRows = false;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.Columns.Add("colUsuario", "Usuario");
            dgvUsuarios.Columns.Add("colContrasena", "Contraseña");
            dgvUsuarios.Columns.Add("colRol", "Rol");
            dgvUsuarios.Columns.Add("colNombre", "Nombre y apellido");
            dgvUsuarios.Columns["colUsuario"].FillWeight = 90;
            dgvUsuarios.Columns["colContrasena"].FillWeight = 100;
            dgvUsuarios.Columns["colRol"].FillWeight = 110;
            dgvUsuarios.Columns["colNombre"].FillWeight = 150;
            dgvUsuarios.CellDoubleClick += dgvUsuarios_CellDoubleClick;

            btnCerrar.Name = "btnCerrar";
            btnCerrar.Text = "Cerrar";
            btnCerrar.SetBounds(620, 390, 120, 34);
            btnCerrar.Click += (s, e) => Close();
            CancelButton = btnCerrar;

            Controls.Add(lblTitulo);
            Controls.Add(lblAclaracion);
            Controls.Add(dgvUsuarios);
            Controls.Add(btnCerrar);

            Load += FrmUsuariosDePrueba_Load;
        }

        private void FrmUsuariosDePrueba_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (UsuarioDePrueba usuario in _datosPruebaService.ListarUsuariosDePrueba())
                {
                    int fila = dgvUsuarios.Rows.Add(
                        usuario.NombreUsuario,
                        usuario.Contrasena ?? "(cambiada)".Traducir(),
                        Sesion.TextoRol(usuario.Rol).Traducir(),
                        (usuario.NombrePersona ?? "-") + (usuario.Activo ? "" : "  (" + "deshabilitado".Traducir() + ")"));
                    dgvUsuarios.Rows[fila].Tag = usuario.NombreUsuario;
                }
                dgvUsuarios.ClearSelection();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            UsuarioElegido = dgvUsuarios.Rows[e.RowIndex].Tag as string;
            DialogResult = DialogResult.OK;
        }
    }
}
