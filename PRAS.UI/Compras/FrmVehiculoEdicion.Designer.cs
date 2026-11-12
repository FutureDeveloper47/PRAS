namespace PRAS.UI
{
    partial class FrmVehiculoEdicion
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
            lblDominio = new Label();
            txtDominio = new TextBox();
            lblMarca = new Label();
            txtMarca = new TextBox();
            lblModelo = new Label();
            txtModelo = new TextBox();
            lblAnio = new Label();
            nudAnio = new NumericUpDown();
            lblColor = new Label();
            txtColor = new TextBox();
            lblChasis = new Label();
            txtChasis = new TextBox();
            lblMotor = new Label();
            txtMotor = new TextBox();
            lblEstadoTexto = new Label();
            txtEstado = new TextBox();
            lblCompraTitulo = new Label();
            lblCompra = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)nudAnio).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(560, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "PRAS: Programa de Registro Automotor Superior";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblDominio
            //
            lblDominio.AutoSize = true;
            lblDominio.Location = new Point(40, 73);
            lblDominio.Name = "lblDominio";
            lblDominio.TabIndex = 1;
            lblDominio.Text = "PATENTE *";
            //
            // txtDominio
            //
            txtDominio.CharacterCasing = CharacterCasing.Upper;
            txtDominio.Location = new Point(170, 70);
            txtDominio.MaxLength = 9;
            txtDominio.Name = "txtDominio";
            txtDominio.Size = new Size(150, 23);
            txtDominio.TabIndex = 2;
            //
            // lblMarca
            //
            lblMarca.AutoSize = true;
            lblMarca.Location = new Point(40, 108);
            lblMarca.Name = "lblMarca";
            lblMarca.TabIndex = 3;
            lblMarca.Text = "MARCA *";
            //
            // txtMarca
            //
            txtMarca.Location = new Point(170, 105);
            txtMarca.MaxLength = 50;
            txtMarca.Name = "txtMarca";
            txtMarca.Size = new Size(200, 23);
            txtMarca.TabIndex = 4;
            //
            // lblModelo
            //
            lblModelo.AutoSize = true;
            lblModelo.Location = new Point(40, 143);
            lblModelo.Name = "lblModelo";
            lblModelo.TabIndex = 5;
            lblModelo.Text = "MODELO *";
            //
            // txtModelo
            //
            txtModelo.Location = new Point(170, 140);
            txtModelo.MaxLength = 50;
            txtModelo.Name = "txtModelo";
            txtModelo.Size = new Size(200, 23);
            txtModelo.TabIndex = 6;
            //
            // lblAnio
            //
            lblAnio.AutoSize = true;
            lblAnio.Location = new Point(40, 178);
            lblAnio.Name = "lblAnio";
            lblAnio.TabIndex = 7;
            lblAnio.Text = "AÑO *";
            //
            // nudAnio
            //
            nudAnio.Location = new Point(170, 175);
            nudAnio.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            nudAnio.Minimum = new decimal(new int[] { 1950, 0, 0, 0 });
            nudAnio.Name = "nudAnio";
            nudAnio.Size = new Size(100, 23);
            nudAnio.TabIndex = 8;
            nudAnio.Value = new decimal(new int[] { 2020, 0, 0, 0 });
            //
            // lblColor
            //
            lblColor.AutoSize = true;
            lblColor.Location = new Point(40, 213);
            lblColor.Name = "lblColor";
            lblColor.TabIndex = 9;
            lblColor.Text = "COLOR";
            //
            // txtColor
            //
            txtColor.Location = new Point(170, 210);
            txtColor.MaxLength = 30;
            txtColor.Name = "txtColor";
            txtColor.Size = new Size(150, 23);
            txtColor.TabIndex = 10;
            //
            // lblChasis
            //
            lblChasis.AutoSize = true;
            lblChasis.Location = new Point(40, 248);
            lblChasis.Name = "lblChasis";
            lblChasis.TabIndex = 11;
            lblChasis.Text = "Nº CHASIS";
            //
            // txtChasis
            //
            txtChasis.CharacterCasing = CharacterCasing.Upper;
            txtChasis.Location = new Point(170, 245);
            txtChasis.MaxLength = 30;
            txtChasis.Name = "txtChasis";
            txtChasis.Size = new Size(200, 23);
            txtChasis.TabIndex = 12;
            //
            // lblMotor
            //
            lblMotor.AutoSize = true;
            lblMotor.Location = new Point(40, 283);
            lblMotor.Name = "lblMotor";
            lblMotor.TabIndex = 13;
            lblMotor.Text = "Nº MOTOR";
            //
            // txtMotor
            //
            txtMotor.CharacterCasing = CharacterCasing.Upper;
            txtMotor.Location = new Point(170, 280);
            txtMotor.MaxLength = 30;
            txtMotor.Name = "txtMotor";
            txtMotor.Size = new Size(200, 23);
            txtMotor.TabIndex = 14;
            //
            // lblEstadoTexto
            //
            lblEstadoTexto.AutoSize = true;
            lblEstadoTexto.Location = new Point(40, 318);
            lblEstadoTexto.Name = "lblEstadoTexto";
            lblEstadoTexto.TabIndex = 15;
            lblEstadoTexto.Text = "ESTADO";
            //
            // txtEstado
            //
            txtEstado.Location = new Point(170, 315);
            txtEstado.Name = "txtEstado";
            txtEstado.ReadOnly = true;
            txtEstado.Size = new Size(200, 23);
            txtEstado.TabIndex = 16;
            txtEstado.TabStop = false;
            //
            // lblCompraTitulo
            //
            lblCompraTitulo.AutoSize = true;
            lblCompraTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCompraTitulo.Location = new Point(40, 360);
            lblCompraTitulo.Name = "lblCompraTitulo";
            lblCompraTitulo.TabIndex = 17;
            lblCompraTitulo.Text = "DATOS DE LA COMPRA";
            //
            // lblCompra
            //
            lblCompra.Location = new Point(40, 382);
            lblCompra.Name = "lblCompra";
            lblCompra.Size = new Size(480, 60);
            lblCompra.TabIndex = 18;
            lblCompra.Text = "";
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(300, 455);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 28);
            btnGuardar.TabIndex = 19;
            btnGuardar.Text = "GUARDAR";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(420, 455);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 28);
            btnCancelar.TabIndex = 20;
            btnCancelar.Text = "CANCELAR";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmVehiculoEdicion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(560, 500);
            Controls.Add(lblTitulo);
            Controls.Add(lblDominio);
            Controls.Add(txtDominio);
            Controls.Add(lblMarca);
            Controls.Add(txtMarca);
            Controls.Add(lblModelo);
            Controls.Add(txtModelo);
            Controls.Add(lblAnio);
            Controls.Add(nudAnio);
            Controls.Add(lblColor);
            Controls.Add(txtColor);
            Controls.Add(lblChasis);
            Controls.Add(txtChasis);
            Controls.Add(lblMotor);
            Controls.Add(txtMotor);
            Controls.Add(lblEstadoTexto);
            Controls.Add(txtEstado);
            Controls.Add(lblCompraTitulo);
            Controls.Add(lblCompra);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmVehiculoEdicion";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Vehículo";
            Load += FrmVehiculoEdicion_Load;
            ((System.ComponentModel.ISupportInitialize)nudAnio).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblDominio;
        private TextBox txtDominio;
        private Label lblMarca;
        private TextBox txtMarca;
        private Label lblModelo;
        private TextBox txtModelo;
        private Label lblAnio;
        private NumericUpDown nudAnio;
        private Label lblColor;
        private TextBox txtColor;
        private Label lblChasis;
        private TextBox txtChasis;
        private Label lblMotor;
        private TextBox txtMotor;
        private Label lblEstadoTexto;
        private TextBox txtEstado;
        private Label lblCompraTitulo;
        private Label lblCompra;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
