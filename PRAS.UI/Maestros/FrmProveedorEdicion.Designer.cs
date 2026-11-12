namespace PRAS.UI
{
    partial class FrmProveedorEdicion
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
            lblRazonSocial = new Label();
            txtRazonSocial = new TextBox();
            lblCUIT = new Label();
            txtCUIT = new TextBox();
            lblDomicilio = new Label();
            txtDomicilio = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblObligatorios = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            //
            // lblRazonSocial
            //
            lblRazonSocial.AutoSize = true;
            lblRazonSocial.Location = new Point(20, 23);
            lblRazonSocial.Name = "lblRazonSocial";
            lblRazonSocial.Size = new Size(82, 15);
            lblRazonSocial.TabIndex = 0;
            lblRazonSocial.Text = "Razón social: *";
            //
            // txtRazonSocial
            //
            txtRazonSocial.Location = new Point(130, 20);
            txtRazonSocial.MaxLength = 100;
            txtRazonSocial.Name = "txtRazonSocial";
            txtRazonSocial.Size = new Size(250, 23);
            txtRazonSocial.TabIndex = 1;
            //
            // lblCUIT
            //
            lblCUIT.AutoSize = true;
            lblCUIT.Location = new Point(20, 58);
            lblCUIT.Name = "lblCUIT";
            lblCUIT.Size = new Size(45, 15);
            lblCUIT.TabIndex = 2;
            lblCUIT.Text = "CUIT: *";
            //
            // txtCUIT
            //
            txtCUIT.Location = new Point(130, 55);
            txtCUIT.MaxLength = 13;
            txtCUIT.Name = "txtCUIT";
            txtCUIT.PlaceholderText = "30-12345678-9";
            txtCUIT.Size = new Size(120, 23);
            txtCUIT.TabIndex = 3;
            //
            // lblDomicilio
            //
            lblDomicilio.AutoSize = true;
            lblDomicilio.Location = new Point(20, 93);
            lblDomicilio.Name = "lblDomicilio";
            lblDomicilio.Size = new Size(61, 15);
            lblDomicilio.TabIndex = 4;
            lblDomicilio.Text = "Domicilio:";
            //
            // txtDomicilio
            //
            txtDomicilio.Location = new Point(130, 90);
            txtDomicilio.MaxLength = 100;
            txtDomicilio.Name = "txtDomicilio";
            txtDomicilio.Size = new Size(250, 23);
            txtDomicilio.TabIndex = 5;
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
            // FrmProveedorEdicion
            //
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(404, 245);
            Controls.Add(lblRazonSocial);
            Controls.Add(txtRazonSocial);
            Controls.Add(lblCUIT);
            Controls.Add(txtCUIT);
            Controls.Add(lblDomicilio);
            Controls.Add(txtDomicilio);
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
            Name = "FrmProveedorEdicion";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Proveedor";
            Load += FrmProveedorEdicion_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRazonSocial;
        private TextBox txtRazonSocial;
        private Label lblCUIT;
        private TextBox txtCUIT;
        private Label lblDomicilio;
        private TextBox txtDomicilio;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblObligatorios;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
