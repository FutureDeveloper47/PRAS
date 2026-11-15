namespace PRAS.UI
{
    partial class FrmRecuperarContrasena
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            btnVerPregunta = new Button();
            lblPreguntaTitulo = new Label();
            lblPregunta = new Label();
            lblRespuesta = new Label();
            txtRespuesta = new TextBox();
            lblNueva = new Label();
            txtNueva = new TextBox();
            lblConfirmacion = new Label();
            txtConfirmacion = new TextBox();
            chkMostrar = new CheckBox();
            btnGuardarContrasena = new Button();
            btnCancelar = new Button();
            lblContactoTitulo = new Label();
            lblContacto = new Label();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Size = new Size(560, 56);
            lblTitulo.Text = "Recuperar contraseña";
            lblTitulo.Name = "lblTitulo";
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(24, 72);
            lblUsuario.Text = "Usuario:";
            lblUsuario.Name = "lblUsuario";
            //
            // txtUsuario
            //
            txtUsuario.Location = new Point(24, 92);
            txtUsuario.Size = new Size(300, 23);
            txtUsuario.MaxLength = 50;
            txtUsuario.Name = "txtUsuario";
            //
            // btnVerPregunta
            //
            btnVerPregunta.Location = new Point(334, 90);
            btnVerPregunta.Size = new Size(200, 30);
            btnVerPregunta.Text = "Ver pregunta de seguridad";
            btnVerPregunta.UseVisualStyleBackColor = true;
            btnVerPregunta.Name = "btnVerPregunta";
            btnVerPregunta.Click += btnVerPregunta_Click;
            //
            // lblPreguntaTitulo
            //
            lblPreguntaTitulo.AutoSize = true;
            lblPreguntaTitulo.Location = new Point(24, 128);
            lblPreguntaTitulo.Text = "Pregunta de seguridad:";
            lblPreguntaTitulo.Name = "lblPreguntaTitulo";
            //
            // lblPregunta
            //
            lblPregunta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPregunta.Location = new Point(24, 148);
            lblPregunta.Size = new Size(510, 22);
            lblPregunta.Text = "";
            lblPregunta.Name = "lblPregunta";
            //
            // lblRespuesta
            //
            lblRespuesta.AutoSize = true;
            lblRespuesta.Location = new Point(24, 176);
            lblRespuesta.Text = "Respuesta:";
            lblRespuesta.Name = "lblRespuesta";
            //
            // txtRespuesta
            //
            txtRespuesta.Location = new Point(24, 196);
            txtRespuesta.Size = new Size(510, 23);
            txtRespuesta.MaxLength = 100;
            txtRespuesta.Enabled = false;
            txtRespuesta.Name = "txtRespuesta";
            //
            // lblNueva
            //
            lblNueva.AutoSize = true;
            lblNueva.Location = new Point(24, 230);
            lblNueva.Text = "Nueva contraseña:";
            lblNueva.Name = "lblNueva";
            //
            // txtNueva
            //
            txtNueva.Location = new Point(24, 250);
            txtNueva.Size = new Size(510, 23);
            txtNueva.MaxLength = 50;
            txtNueva.UseSystemPasswordChar = true;
            txtNueva.Enabled = false;
            txtNueva.Name = "txtNueva";
            //
            // lblConfirmacion
            //
            lblConfirmacion.AutoSize = true;
            lblConfirmacion.Location = new Point(24, 284);
            lblConfirmacion.Text = "Repetir contraseña:";
            lblConfirmacion.Name = "lblConfirmacion";
            //
            // txtConfirmacion
            //
            txtConfirmacion.Location = new Point(24, 304);
            txtConfirmacion.Size = new Size(510, 23);
            txtConfirmacion.MaxLength = 50;
            txtConfirmacion.UseSystemPasswordChar = true;
            txtConfirmacion.Enabled = false;
            txtConfirmacion.Name = "txtConfirmacion";
            //
            // chkMostrar
            //
            chkMostrar.AutoSize = true;
            chkMostrar.Location = new Point(24, 336);
            chkMostrar.Text = "Mostrar contraseña";
            chkMostrar.Name = "chkMostrar";
            chkMostrar.CheckedChanged += chkMostrar_CheckedChanged;
            //
            // btnGuardarContrasena
            //
            btnGuardarContrasena.Location = new Point(24, 366);
            btnGuardarContrasena.Size = new Size(190, 30);
            btnGuardarContrasena.Enabled = false;
            btnGuardarContrasena.Text = "Cambiar contraseña";
            btnGuardarContrasena.UseVisualStyleBackColor = true;
            btnGuardarContrasena.Name = "btnGuardarContrasena";
            btnGuardarContrasena.Click += btnGuardarContrasena_Click;
            //
            // btnCancelar
            //
            btnCancelar.Location = new Point(224, 366);
            btnCancelar.Size = new Size(110, 30);
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Name = "btnCancelar";
            //
            // lblContactoTitulo
            //
            lblContactoTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblContactoTitulo.Location = new Point(24, 412);
            lblContactoTitulo.Size = new Size(510, 20);
            lblContactoTitulo.Text = "Si no puede recuperarla, comuníquese con el administrador:";
            lblContactoTitulo.Name = "lblContactoTitulo";
            //
            // lblContacto
            //
            lblContacto.Location = new Point(24, 434);
            lblContacto.Size = new Size(510, 56);
            lblContacto.Text = "";
            lblContacto.Name = "lblContacto";
            //
            // FrmRecuperarContrasena
            //
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(560, 500);
            Text = "Recuperar contraseña";
            Controls.Add(lblTitulo);
            Controls.Add(lblUsuario);
            Controls.Add(txtUsuario);
            Controls.Add(btnVerPregunta);
            Controls.Add(lblPreguntaTitulo);
            Controls.Add(lblPregunta);
            Controls.Add(lblRespuesta);
            Controls.Add(txtRespuesta);
            Controls.Add(lblNueva);
            Controls.Add(txtNueva);
            Controls.Add(lblConfirmacion);
            Controls.Add(txtConfirmacion);
            Controls.Add(chkMostrar);
            Controls.Add(btnGuardarContrasena);
            Controls.Add(btnCancelar);
            Controls.Add(lblContactoTitulo);
            Controls.Add(lblContacto);
            Name = "FrmRecuperarContrasena";
            Load += FrmRecuperarContrasena_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Button btnVerPregunta;
        private Label lblPreguntaTitulo;
        private Label lblPregunta;
        private Label lblRespuesta;
        private TextBox txtRespuesta;
        private Label lblNueva;
        private TextBox txtNueva;
        private Label lblConfirmacion;
        private TextBox txtConfirmacion;
        private CheckBox chkMostrar;
        private Button btnGuardarContrasena;
        private Button btnCancelar;
        private Label lblContactoTitulo;
        private Label lblContacto;
    }
}
