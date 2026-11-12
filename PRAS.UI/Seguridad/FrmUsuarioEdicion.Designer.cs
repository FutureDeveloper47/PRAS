namespace PRAS.UI
{
    partial class FrmUsuarioEdicion
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
            lblNombreUsuario = new Label();
            txtNombreUsuario = new TextBox();
            lblRol = new Label();
            cboRol = new ComboBox();
            lblMecanico = new Label();
            cboMecanico = new ComboBox();
            lblContrasena = new Label();
            txtContrasena = new TextBox();
            lblConfirmacion = new Label();
            txtConfirmacion = new TextBox();
            lblAyudaContrasena = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            //
            // lblNombreUsuario
            //
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.Location = new Point(20, 23);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(55, 15);
            lblNombreUsuario.TabIndex = 0;
            lblNombreUsuario.Text = "Usuario: *";
            //
            // txtNombreUsuario
            //
            txtNombreUsuario.Location = new Point(150, 20);
            txtNombreUsuario.MaxLength = 50;
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.Size = new Size(200, 23);
            txtNombreUsuario.TabIndex = 1;
            //
            // lblRol
            //
            lblRol.AutoSize = true;
            lblRol.Location = new Point(20, 58);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(34, 15);
            lblRol.TabIndex = 2;
            lblRol.Text = "Rol: *";
            //
            // cboRol
            //
            cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRol.Location = new Point(150, 55);
            cboRol.Name = "cboRol";
            cboRol.Size = new Size(200, 23);
            cboRol.TabIndex = 3;
            cboRol.SelectedIndexChanged += cboRol_SelectedIndexChanged;
            //
            // lblMecanico
            //
            lblMecanico.AutoSize = true;
            lblMecanico.Location = new Point(20, 93);
            lblMecanico.Name = "lblMecanico";
            lblMecanico.Size = new Size(64, 15);
            lblMecanico.TabIndex = 4;
            lblMecanico.Text = "Mecánico:";
            //
            // cboMecanico
            //
            cboMecanico.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMecanico.Location = new Point(150, 90);
            cboMecanico.Name = "cboMecanico";
            cboMecanico.Size = new Size(200, 23);
            cboMecanico.TabIndex = 5;
            //
            // lblContrasena
            //
            lblContrasena.AutoSize = true;
            lblContrasena.Location = new Point(20, 128);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(70, 15);
            lblContrasena.TabIndex = 6;
            lblContrasena.Text = "Contraseña:";
            //
            // txtContrasena
            //
            txtContrasena.Location = new Point(150, 125);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.Size = new Size(200, 23);
            txtContrasena.TabIndex = 7;
            txtContrasena.UseSystemPasswordChar = true;
            //
            // lblConfirmacion
            //
            lblConfirmacion.AutoSize = true;
            lblConfirmacion.Location = new Point(20, 163);
            lblConfirmacion.Name = "lblConfirmacion";
            lblConfirmacion.Size = new Size(121, 15);
            lblConfirmacion.TabIndex = 8;
            lblConfirmacion.Text = "Repetir contraseña:";
            //
            // txtConfirmacion
            //
            txtConfirmacion.Location = new Point(150, 160);
            txtConfirmacion.Name = "txtConfirmacion";
            txtConfirmacion.Size = new Size(200, 23);
            txtConfirmacion.TabIndex = 9;
            txtConfirmacion.UseSystemPasswordChar = true;
            //
            // lblAyudaContrasena
            //
            lblAyudaContrasena.ForeColor = SystemColors.GrayText;
            lblAyudaContrasena.Location = new Point(150, 188);
            lblAyudaContrasena.Name = "lblAyudaContrasena";
            lblAyudaContrasena.Size = new Size(220, 32);
            lblAyudaContrasena.TabIndex = 10;
            lblAyudaContrasena.Text = "Mínimo 6 caracteres.";
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(174, 230);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(85, 28);
            btnGuardar.TabIndex = 11;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(265, 230);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(85, 28);
            btnCancelar.TabIndex = 12;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmUsuarioEdicion
            //
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(380, 275);
            Controls.Add(lblNombreUsuario);
            Controls.Add(txtNombreUsuario);
            Controls.Add(lblRol);
            Controls.Add(cboRol);
            Controls.Add(lblMecanico);
            Controls.Add(cboMecanico);
            Controls.Add(lblContrasena);
            Controls.Add(txtContrasena);
            Controls.Add(lblConfirmacion);
            Controls.Add(txtConfirmacion);
            Controls.Add(lblAyudaContrasena);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmUsuarioEdicion";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Usuario";
            Load += FrmUsuarioEdicion_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNombreUsuario;
        private TextBox txtNombreUsuario;
        private Label lblRol;
        private ComboBox cboRol;
        private Label lblMecanico;
        private ComboBox cboMecanico;
        private Label lblContrasena;
        private TextBox txtContrasena;
        private Label lblConfirmacion;
        private TextBox txtConfirmacion;
        private Label lblAyudaContrasena;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
