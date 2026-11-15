namespace PRAS.UI
{
    partial class FrmLogin
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
            lblContrasena = new Label();
            txtContrasena = new TextBox();
            btnIngresar = new Button();
            btnCancelar = new Button();
            lnkOlvideContrasena = new LinkLabel();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(24, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(213, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "PRAS - Ingreso";
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(26, 70);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(50, 15);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Usuario:";
            //
            // txtUsuario
            //
            txtUsuario.Location = new Point(120, 67);
            txtUsuario.MaxLength = 50;
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(180, 23);
            txtUsuario.TabIndex = 2;
            //
            // lblContrasena
            //
            lblContrasena.AutoSize = true;
            lblContrasena.Location = new Point(26, 105);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(70, 15);
            lblContrasena.TabIndex = 3;
            lblContrasena.Text = "Contraseña:";
            //
            // txtContrasena
            //
            txtContrasena.Location = new Point(120, 102);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.Size = new Size(180, 23);
            txtContrasena.TabIndex = 4;
            txtContrasena.UseSystemPasswordChar = true;
            //
            // btnIngresar
            //
            btnIngresar.Location = new Point(120, 145);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(85, 28);
            btnIngresar.TabIndex = 5;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = true;
            btnIngresar.Click += btnIngresar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(215, 145);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(85, 28);
            btnCancelar.TabIndex = 6;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // lnkOlvideContrasena
            //
            lnkOlvideContrasena.AutoSize = true;
            lnkOlvideContrasena.Location = new Point(120, 185);
            lnkOlvideContrasena.Name = "lnkOlvideContrasena";
            lnkOlvideContrasena.Size = new Size(140, 15);
            lnkOlvideContrasena.TabIndex = 7;
            lnkOlvideContrasena.TabStop = true;
            lnkOlvideContrasena.Text = "¿Olvidó su contraseña?";
            lnkOlvideContrasena.LinkClicked += lnkOlvideContrasena_LinkClicked;
            //
            // FrmLogin
            //
            AcceptButton = btnIngresar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(330, 215);
            Controls.Add(lblTitulo);
            Controls.Add(lblUsuario);
            Controls.Add(txtUsuario);
            Controls.Add(lblContrasena);
            Controls.Add(txtContrasena);
            Controls.Add(btnIngresar);
            Controls.Add(btnCancelar);
            Controls.Add(lnkOlvideContrasena);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PRAS - Iniciar sesión";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblContrasena;
        private TextBox txtContrasena;
        private Button btnIngresar;
        private Button btnCancelar;
        private LinkLabel lnkOlvideContrasena;
    }
}
