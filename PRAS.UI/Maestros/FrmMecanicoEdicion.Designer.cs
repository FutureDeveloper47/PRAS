namespace PRAS.UI
{
    partial class FrmMecanicoEdicion
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
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblEspecialidad = new Label();
            txtEspecialidad = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblObligatorios = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(20, 23);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(62, 15);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre: *";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(130, 20);
            txtNombre.MaxLength = 50;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(250, 23);
            txtNombre.TabIndex = 1;
            //
            // lblApellido
            //
            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(20, 58);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(62, 15);
            lblApellido.TabIndex = 2;
            lblApellido.Text = "Apellido: *";
            //
            // txtApellido
            //
            txtApellido.Location = new Point(130, 55);
            txtApellido.MaxLength = 50;
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(250, 23);
            txtApellido.TabIndex = 3;
            //
            // lblEspecialidad
            //
            lblEspecialidad.AutoSize = true;
            lblEspecialidad.Location = new Point(20, 93);
            lblEspecialidad.Name = "lblEspecialidad";
            lblEspecialidad.Size = new Size(75, 15);
            lblEspecialidad.TabIndex = 4;
            lblEspecialidad.Text = "Especialidad:";
            //
            // txtEspecialidad
            //
            txtEspecialidad.Location = new Point(130, 90);
            txtEspecialidad.MaxLength = 50;
            txtEspecialidad.Name = "txtEspecialidad";
            txtEspecialidad.PlaceholderText = "Ej: chapa y pintura";
            txtEspecialidad.Size = new Size(250, 23);
            txtEspecialidad.TabIndex = 5;
            //
            // lblTelefono
            //
            lblTelefono.AutoSize = true;
            lblTelefono.Location = new Point(20, 128);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(55, 15);
            lblTelefono.TabIndex = 6;
            lblTelefono.Text = "Teléfono:";
            //
            // txtTelefono
            //
            txtTelefono.Location = new Point(130, 125);
            txtTelefono.MaxLength = 20;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(160, 23);
            txtTelefono.TabIndex = 7;
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(20, 163);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 8;
            lblEmail.Text = "Email:";
            //
            // txtEmail
            //
            txtEmail.Location = new Point(130, 160);
            txtEmail.MaxLength = 100;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(250, 23);
            txtEmail.TabIndex = 9;
            //
            // lblObligatorios
            //
            lblObligatorios.AutoSize = true;
            lblObligatorios.ForeColor = SystemColors.GrayText;
            lblObligatorios.Location = new Point(20, 206);
            lblObligatorios.Name = "lblObligatorios";
            lblObligatorios.Size = new Size(105, 15);
            lblObligatorios.TabIndex = 10;
            lblObligatorios.Text = "(*) Campo obligatorio";
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(204, 200);
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
            btnCancelar.Location = new Point(295, 200);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(85, 28);
            btnCancelar.TabIndex = 12;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmMecanicoEdicion
            //
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(404, 245);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(lblApellido);
            Controls.Add(txtApellido);
            Controls.Add(lblEspecialidad);
            Controls.Add(txtEspecialidad);
            Controls.Add(lblTelefono);
            Controls.Add(txtTelefono);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblObligatorios);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmMecanicoEdicion";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Mecánico";
            Load += FrmMecanicoEdicion_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblEspecialidad;
        private TextBox txtEspecialidad;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblObligatorios;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
