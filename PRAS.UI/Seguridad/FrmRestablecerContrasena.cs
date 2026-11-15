using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Mi perfil > Administrar usuarios > Restablecer contraseña.
    /// El administrador escribe dos veces una contraseña TEMPORAL para otro usuario y se la comunica.
    /// La contraseña anterior no se muestra porque no se puede: solo está guardado su hash SHA-256.
    /// La regla (permiso, largo mínimo, no la propia) está en la BLL (UsuarioService.RestablecerContrasena).
    /// No tiene .Designer: se arma por código.
    /// </summary>
    public class FrmRestablecerContrasena : FormBase
    {
        private readonly UsuarioService _usuarioService = new UsuarioService();
        private readonly Usuario _usuario;

        private readonly Label lblTitulo = new Label();
        private readonly Label lblUsuario = new Label();
        private readonly Label lblAviso = new Label();
        private readonly Label lblNueva = new Label();
        private readonly TextBox txtNueva = new TextBox();
        private readonly Label lblConfirmacion = new Label();
        private readonly TextBox txtConfirmacion = new TextBox();
        private readonly CheckBox chkMostrar = new CheckBox();
        private readonly Button btnGuardar = new Button();
        private readonly Button btnCancelar = new Button();

        public FrmRestablecerContrasena(Usuario usuario)
        {
            _usuario = usuario;

            // SuspendLayout/ResumeLayout: al cerrar el bloque, WinForms agranda todo según la escala
            // de Windows (AutoScaleMode.Dpi), igual que en las pantallas hechas con el diseñador.
            SuspendLayout();
            Name = "FrmRestablecerContrasena";
            Text = "Restablecer contraseña";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(460, 350);
            Font = new Font("Segoe UI", 9.75F);

            lblTitulo.Name = "lblTitulo";
            lblTitulo.Text = "Restablecer contraseña";

            // El nombre de usuario es un dato: va en un cuadro de solo lectura para que no se traduzca.
            lblUsuario.Name = "lblUsuarioTitulo";
            lblUsuario.Text = "Usuario:";
            lblUsuario.SetBounds(24, 72, 90, 22);
            var txtUsuario = new TextBox
            {
                Name = "txtUsuario",
                Text = usuario.NombrePersona == null ? usuario.NombreUsuario : $"{usuario.NombreUsuario} — {usuario.NombrePersona}",
                ReadOnly = true,
                TabStop = false
            };
            txtUsuario.SetBounds(120, 70, 316, 24);

            lblAviso.Name = "lblAviso";
            lblAviso.Text = "La contraseña actual no se puede ver: se guarda con hash SHA-256. Escriba una contraseña temporal y comuníquesela al usuario; después la puede cambiar en Mi perfil.";
            lblAviso.SetBounds(24, 104, 412, 62);
            lblAviso.ForeColor = Tema.TextoApagado;

            lblNueva.Text = "Contraseña temporal:";
            lblNueva.SetBounds(24, 172, 412, 20);
            txtNueva.Name = "txtNueva";
            txtNueva.UseSystemPasswordChar = true;
            txtNueva.SetBounds(24, 194, 412, 24);

            lblConfirmacion.Text = "Repetir contraseña:";
            lblConfirmacion.SetBounds(24, 226, 412, 20);
            txtConfirmacion.Name = "txtConfirmacion";
            txtConfirmacion.UseSystemPasswordChar = true;
            txtConfirmacion.SetBounds(24, 248, 412, 24);

            chkMostrar.Text = "Mostrar contraseña";
            chkMostrar.SetBounds(24, 280, 250, 24);
            chkMostrar.CheckedChanged += (s, e) =>
            {
                txtNueva.UseSystemPasswordChar = !chkMostrar.Checked;
                txtConfirmacion.UseSystemPasswordChar = !chkMostrar.Checked;
            };

            btnGuardar.Name = "btnGuardar";
            btnGuardar.Text = "Guardar";
            btnGuardar.SetBounds(206, 306, 110, 32);
            btnGuardar.Click += btnGuardar_Click;

            btnCancelar.Name = "btnCancelar";
            btnCancelar.Text = "Cancelar";
            btnCancelar.SetBounds(326, 306, 110, 32);
            btnCancelar.Click += (s, e) => Close();

            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;
            Controls.AddRange(new Control[] { lblTitulo, lblUsuario, txtUsuario, lblAviso, lblNueva, txtNueva,
                                              lblConfirmacion, txtConfirmacion, chkMostrar, btnGuardar, btnCancelar });
            ResumeLayout(false);
            PerformLayout();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                _usuarioService.RestablecerContrasena(_usuario.IdUsuario, txtNueva.Text, txtConfirmacion.Text);
                Mensajes.Info($"Se restableció la contraseña de {_usuario.NombreUsuario}.");
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
