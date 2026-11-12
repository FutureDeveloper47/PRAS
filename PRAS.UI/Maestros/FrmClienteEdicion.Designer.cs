namespace PRAS.UI
{
    partial class FrmClienteEdicion
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
            lblDNI = new Label();
            txtDNI = new TextBox();
            lblCUIT = new Label();
            txtCUIT = new TextBox();
            lblDomicilio = new Label();
            txtDomicilio = new TextBox();
            lblLocalidad = new Label();
            txtLocalidad = new TextBox();
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
            // lblDNI
            //
            lblDNI.AutoSize = true;
            lblDNI.Location = new Point(20, 93);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(38, 15);
            lblDNI.TabIndex = 4;
            lblDNI.Text = "DNI: *";
            //
            // txtDNI
            //
            txtDNI.Location = new Point(130, 90);
            txtDNI.MaxLength = 10;
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(120, 23);
            txtDNI.TabIndex = 5;
            //
            // lblCUIT
            //
            lblCUIT.AutoSize = true;
            lblCUIT.Location = new Point(20, 128);
            lblCUIT.Name = "lblCUIT";
            lblCUIT.Size = new Size(35, 15);
            lblCUIT.TabIndex = 6;
            lblCUIT.Text = "CUIT:";
            //
            // txtCUIT
            //
            txtCUIT.Location = new Point(130, 125);
            txtCUIT.MaxLength = 13;
            txtCUIT.Name = "txtCUIT";
            txtCUIT.PlaceholderText = "20-12345678-9";
            txtCUIT.Size = new Size(120, 23);
            txtCUIT.TabIndex = 7;
            //
            // lblDomicilio
            //
            lblDomicilio.AutoSize = true;
            lblDomicilio.Location = new Point(20, 163);
            lblDomicilio.Name = "lblDomicilio";
            lblDomicilio.Size = new Size(61, 15);
            lblDomicilio.TabIndex = 8;
            lblDomicilio.Text = "Domicilio:";
            //
            // txtDomicilio
            //
            txtDomicilio.Location = new Point(130, 160);
            txtDomicilio.MaxLength = 100;
            txtDomicilio.Name = "txtDomicilio";
            txtDomicilio.Size = new Size(250, 23);
            txtDomicilio.TabIndex = 9;
            //
            // lblLocalidad
            //
            lblLocalidad.AutoSize = true;
            lblLocalidad.Location = new Point(20, 198);
            lblLocalidad.Name = "lblLocalidad";
            lblLocalidad.Size = new Size(61, 15);
            lblLocalidad.TabIndex = 10;
            lblLocalidad.Text = "Localidad:";
            //
            // txtLocalidad
            //
            txtLocalidad.Location = new Point(130, 195);
            txtLocalidad.MaxLength = 50;
            txtLocalidad.Name = "txtLocalidad";
            txtLocalidad.Size = new Size(250, 23);
            txtLocalidad.TabIndex = 11;
            //
            // lblTelefono
            //
            lblTelefono.AutoSize = true;
            lblTelefono.Location = new Point(20, 233);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(55, 15);
            lblTelefono.TabIndex = 12;
            lblTelefono.Text = "Teléfono:";
            //
            // txtTelefono
            //
            txtTelefono.Location = new Point(130, 230);
            txtTelefono.MaxLength = 20;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(160, 23);
            txtTelefono.TabIndex = 13;
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(20, 268);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 14;
            lblEmail.Text = "Email:";
            //
            // txtEmail
            //
            txtEmail.Location = new Point(130, 265);
            txtEmail.MaxLength = 100;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(250, 23);
            txtEmail.TabIndex = 15;
            //
            // lblObligatorios
            //
            lblObligatorios.AutoSize = true;
            lblObligatorios.ForeColor = SystemColors.GrayText;
            lblObligatorios.Location = new Point(20, 311);
            lblObligatorios.Name = "lblObligatorios";
            lblObligatorios.Size = new Size(105, 15);
            lblObligatorios.TabIndex = 16;
            lblObligatorios.Text = "(*) Campo obligatorio";
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(204, 305);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(85, 28);
            btnGuardar.TabIndex = 17;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(295, 305);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(85, 28);
            btnCancelar.TabIndex = 18;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmClienteEdicion
            //
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(404, 350);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(lblApellido);
            Controls.Add(txtApellido);
            Controls.Add(lblDNI);
            Controls.Add(txtDNI);
            Controls.Add(lblCUIT);
            Controls.Add(txtCUIT);
            Controls.Add(lblDomicilio);
            Controls.Add(txtDomicilio);
            Controls.Add(lblLocalidad);
            Controls.Add(txtLocalidad);
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
            Name = "FrmClienteEdicion";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cliente";
            Load += FrmClienteEdicion_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblDNI;
        private TextBox txtDNI;
        private Label lblCUIT;
        private TextBox txtCUIT;
        private Label lblDomicilio;
        private TextBox txtDomicilio;
        private Label lblLocalidad;
        private TextBox txtLocalidad;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblObligatorios;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
