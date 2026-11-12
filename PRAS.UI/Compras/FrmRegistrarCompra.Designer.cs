namespace PRAS.UI
{
    partial class FrmRegistrarCompra
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
            lblVehiculoTitulo = new Label();
            lblAnio = new Label();
            nudAnio = new NumericUpDown();
            lblDominio = new Label();
            txtDominio = new TextBox();
            lblModelo = new Label();
            txtModelo = new TextBox();
            lblEstado = new Label();
            txtEstado = new TextBox();
            lblMarca = new Label();
            txtMarca = new TextBox();
            lblColor = new Label();
            txtColor = new TextBox();
            lblChasis = new Label();
            txtChasis = new TextBox();
            lblMotor = new Label();
            txtMotor = new TextBox();
            lblCompraTitulo = new Label();
            lblDNIVendedor = new Label();
            txtDNIVendedor = new TextBox();
            btnBuscarVendedor = new Button();
            lblVendedor = new Label();
            btnNuevoCliente = new Button();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            lblPrecio = new Label();
            nudPrecio = new NumericUpDown();
            lblFormaPago = new Label();
            txtFormaPago = new TextBox();
            lblObservaciones = new Label();
            txtObservaciones = new TextBox();
            pnlFaltanDatos = new Panel();
            lblFaltanDatos = new Label();
            pnlExito = new Panel();
            lblExito = new Label();
            lblObligatorios = new Label();
            btnCancelar = new Button();
            btnGuardar = new Button();
            ((System.ComponentModel.ISupportInitialize)nudAnio).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).BeginInit();
            pnlFaltanDatos.SuspendLayout();
            pnlExito.SuspendLayout();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(920, 45);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "PRAS: Programa de Registro Automotor Superior";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblVehiculoTitulo
            //
            lblVehiculoTitulo.AutoSize = true;
            lblVehiculoTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVehiculoTitulo.Location = new Point(40, 60);
            lblVehiculoTitulo.Name = "lblVehiculoTitulo";
            lblVehiculoTitulo.TabIndex = 1;
            lblVehiculoTitulo.Text = "DATOS DEL VEHÍCULO";
            //
            // lblAnio
            //
            lblAnio.AutoSize = true;
            lblAnio.Location = new Point(40, 88);
            lblAnio.Name = "lblAnio";
            lblAnio.TabIndex = 2;
            lblAnio.Text = "AÑO *";
            //
            // nudAnio
            //
            nudAnio.Location = new Point(170, 85);
            nudAnio.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            nudAnio.Minimum = new decimal(new int[] { 1950, 0, 0, 0 });
            nudAnio.Name = "nudAnio";
            nudAnio.Size = new Size(150, 23);
            nudAnio.TabIndex = 3;
            nudAnio.Value = new decimal(new int[] { 2020, 0, 0, 0 });
            //
            // lblDominio
            //
            lblDominio.AutoSize = true;
            lblDominio.Location = new Point(40, 126);
            lblDominio.Name = "lblDominio";
            lblDominio.TabIndex = 4;
            lblDominio.Text = "PATENTE *";
            //
            // txtDominio
            //
            txtDominio.CharacterCasing = CharacterCasing.Upper;
            txtDominio.Location = new Point(170, 123);
            txtDominio.MaxLength = 9;
            txtDominio.Name = "txtDominio";
            txtDominio.PlaceholderText = "AB123CD";
            txtDominio.Size = new Size(150, 23);
            txtDominio.TabIndex = 5;
            //
            // lblModelo
            //
            lblModelo.AutoSize = true;
            lblModelo.Location = new Point(40, 164);
            lblModelo.Name = "lblModelo";
            lblModelo.TabIndex = 6;
            lblModelo.Text = "MODELO *";
            //
            // txtModelo
            //
            txtModelo.Location = new Point(170, 161);
            txtModelo.MaxLength = 50;
            txtModelo.Name = "txtModelo";
            txtModelo.Size = new Size(150, 23);
            txtModelo.TabIndex = 7;
            //
            // lblEstado
            //
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(40, 202);
            lblEstado.Name = "lblEstado";
            lblEstado.TabIndex = 8;
            lblEstado.Text = "ESTADO";
            //
            // txtEstado
            //
            txtEstado.Location = new Point(170, 199);
            txtEstado.Name = "txtEstado";
            txtEstado.ReadOnly = true;
            txtEstado.Size = new Size(150, 23);
            txtEstado.TabIndex = 9;
            txtEstado.TabStop = false;
            //
            // lblMarca
            //
            lblMarca.AutoSize = true;
            lblMarca.Location = new Point(40, 240);
            lblMarca.Name = "lblMarca";
            lblMarca.TabIndex = 10;
            lblMarca.Text = "MARCA *";
            //
            // txtMarca
            //
            txtMarca.Location = new Point(170, 237);
            txtMarca.MaxLength = 50;
            txtMarca.Name = "txtMarca";
            txtMarca.Size = new Size(150, 23);
            txtMarca.TabIndex = 11;
            //
            // lblColor
            //
            lblColor.AutoSize = true;
            lblColor.Location = new Point(40, 278);
            lblColor.Name = "lblColor";
            lblColor.TabIndex = 12;
            lblColor.Text = "COLOR";
            //
            // txtColor
            //
            txtColor.Location = new Point(170, 275);
            txtColor.MaxLength = 30;
            txtColor.Name = "txtColor";
            txtColor.Size = new Size(150, 23);
            txtColor.TabIndex = 13;
            //
            // lblChasis
            //
            lblChasis.AutoSize = true;
            lblChasis.Location = new Point(40, 316);
            lblChasis.Name = "lblChasis";
            lblChasis.TabIndex = 14;
            lblChasis.Text = "Nº CHASIS";
            //
            // txtChasis
            //
            txtChasis.CharacterCasing = CharacterCasing.Upper;
            txtChasis.Location = new Point(170, 313);
            txtChasis.MaxLength = 30;
            txtChasis.Name = "txtChasis";
            txtChasis.Size = new Size(150, 23);
            txtChasis.TabIndex = 15;
            //
            // lblMotor
            //
            lblMotor.AutoSize = true;
            lblMotor.Location = new Point(40, 354);
            lblMotor.Name = "lblMotor";
            lblMotor.TabIndex = 16;
            lblMotor.Text = "Nº MOTOR";
            //
            // txtMotor
            //
            txtMotor.CharacterCasing = CharacterCasing.Upper;
            txtMotor.Location = new Point(170, 351);
            txtMotor.MaxLength = 30;
            txtMotor.Name = "txtMotor";
            txtMotor.Size = new Size(150, 23);
            txtMotor.TabIndex = 17;
            //
            // lblCompraTitulo
            //
            lblCompraTitulo.AutoSize = true;
            lblCompraTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCompraTitulo.Location = new Point(360, 60);
            lblCompraTitulo.Name = "lblCompraTitulo";
            lblCompraTitulo.TabIndex = 18;
            lblCompraTitulo.Text = "DATOS DE LA COMPRA";
            //
            // lblDNIVendedor
            //
            lblDNIVendedor.AutoSize = true;
            lblDNIVendedor.Location = new Point(360, 88);
            lblDNIVendedor.Name = "lblDNIVendedor";
            lblDNIVendedor.TabIndex = 19;
            lblDNIVendedor.Text = "DNI VENDEDOR *";
            //
            // txtDNIVendedor
            //
            txtDNIVendedor.Location = new Point(480, 85);
            txtDNIVendedor.MaxLength = 10;
            txtDNIVendedor.Name = "txtDNIVendedor";
            txtDNIVendedor.Size = new Size(110, 23);
            txtDNIVendedor.TabIndex = 20;
            txtDNIVendedor.TextChanged += txtDNIVendedor_TextChanged;
            txtDNIVendedor.KeyDown += txtDNIVendedor_KeyDown;
            //
            // btnBuscarVendedor
            //
            btnBuscarVendedor.Location = new Point(596, 84);
            btnBuscarVendedor.Name = "btnBuscarVendedor";
            btnBuscarVendedor.Size = new Size(70, 25);
            btnBuscarVendedor.TabIndex = 21;
            btnBuscarVendedor.Text = "Buscar";
            btnBuscarVendedor.UseVisualStyleBackColor = true;
            btnBuscarVendedor.Click += btnBuscarVendedor_Click;
            //
            // lblVendedor
            //
            lblVendedor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVendedor.Location = new Point(480, 126);
            lblVendedor.Name = "lblVendedor";
            lblVendedor.Size = new Size(200, 18);
            lblVendedor.TabIndex = 22;
            lblVendedor.Text = "Sin seleccionar";
            //
            // btnNuevoCliente
            //
            btnNuevoCliente.Location = new Point(480, 159);
            btnNuevoCliente.Name = "btnNuevoCliente";
            btnNuevoCliente.Size = new Size(186, 25);
            btnNuevoCliente.TabIndex = 23;
            btnNuevoCliente.Text = "Nuevo cliente";
            btnNuevoCliente.UseVisualStyleBackColor = true;
            btnNuevoCliente.Click += btnNuevoCliente_Click;
            //
            // lblFecha
            //
            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(360, 202);
            lblFecha.Name = "lblFecha";
            lblFecha.TabIndex = 24;
            lblFecha.Text = "FECHA *";
            //
            // dtpFecha
            //
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(480, 199);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(186, 23);
            dtpFecha.TabIndex = 25;
            //
            // lblPrecio
            //
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(360, 240);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.TabIndex = 26;
            lblPrecio.Text = "PRECIO *";
            //
            // nudPrecio
            //
            nudPrecio.DecimalPlaces = 2;
            nudPrecio.Location = new Point(480, 237);
            nudPrecio.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            nudPrecio.Name = "nudPrecio";
            nudPrecio.Size = new Size(186, 23);
            nudPrecio.TabIndex = 27;
            nudPrecio.ThousandsSeparator = true;
            //
            // lblFormaPago
            //
            lblFormaPago.AutoSize = true;
            lblFormaPago.Location = new Point(360, 278);
            lblFormaPago.Name = "lblFormaPago";
            lblFormaPago.TabIndex = 28;
            lblFormaPago.Text = "FORMA DE PAGO";
            //
            // txtFormaPago
            //
            txtFormaPago.Location = new Point(480, 275);
            txtFormaPago.Name = "txtFormaPago";
            txtFormaPago.ReadOnly = true;
            txtFormaPago.Size = new Size(186, 23);
            txtFormaPago.TabIndex = 29;
            txtFormaPago.TabStop = false;
            //
            // lblObservaciones
            //
            lblObservaciones.AutoSize = true;
            lblObservaciones.Location = new Point(360, 316);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.TabIndex = 30;
            lblObservaciones.Text = "OBSERVACIONES";
            //
            // txtObservaciones
            //
            txtObservaciones.Location = new Point(480, 313);
            txtObservaciones.MaxLength = 500;
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.PlaceholderText = "Daños visibles, papeles...";
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Size = new Size(186, 61);
            txtObservaciones.TabIndex = 31;
            //
            // pnlFaltanDatos
            //
            pnlFaltanDatos.BorderStyle = BorderStyle.FixedSingle;
            pnlFaltanDatos.Controls.Add(lblFaltanDatos);
            pnlFaltanDatos.Location = new Point(700, 82);
            pnlFaltanDatos.Name = "pnlFaltanDatos";
            pnlFaltanDatos.Size = new Size(190, 110);
            pnlFaltanDatos.TabIndex = 32;
            pnlFaltanDatos.Visible = false;
            //
            // lblFaltanDatos
            //
            lblFaltanDatos.Dock = DockStyle.Fill;
            lblFaltanDatos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFaltanDatos.ForeColor = Color.DarkRed;
            lblFaltanDatos.Name = "lblFaltanDatos";
            lblFaltanDatos.Padding = new Padding(8);
            lblFaltanDatos.TabIndex = 0;
            lblFaltanDatos.Text = "FALTAN COMPLETAR DATOS PARA SU REGISTRO";
            lblFaltanDatos.TextAlign = ContentAlignment.MiddleCenter;
            //
            // pnlExito
            //
            pnlExito.BorderStyle = BorderStyle.FixedSingle;
            pnlExito.Controls.Add(lblExito);
            pnlExito.Location = new Point(700, 212);
            pnlExito.Name = "pnlExito";
            pnlExito.Size = new Size(190, 110);
            pnlExito.TabIndex = 33;
            pnlExito.Visible = false;
            //
            // lblExito
            //
            lblExito.Dock = DockStyle.Fill;
            lblExito.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblExito.ForeColor = Color.DarkGreen;
            lblExito.Name = "lblExito";
            lblExito.Padding = new Padding(8);
            lblExito.TabIndex = 0;
            lblExito.Text = "AUTO REGISTRADO EXITOSAMENTE!";
            lblExito.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblObligatorios
            //
            lblObligatorios.AutoSize = true;
            lblObligatorios.ForeColor = SystemColors.GrayText;
            lblObligatorios.Location = new Point(40, 440);
            lblObligatorios.Name = "lblObligatorios";
            lblObligatorios.TabIndex = 34;
            lblObligatorios.Text = "(*) Campo obligatorio";
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(600, 430);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 30);
            btnCancelar.TabIndex = 36;
            btnCancelar.Text = "CANCELAR";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(720, 430);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(170, 30);
            btnGuardar.TabIndex = 35;
            btnGuardar.Text = "GUARDAR Y ENVIAR";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // FrmRegistrarCompra
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(920, 480);
            Controls.Add(lblTitulo);
            Controls.Add(lblVehiculoTitulo);
            Controls.Add(lblAnio);
            Controls.Add(nudAnio);
            Controls.Add(lblDominio);
            Controls.Add(txtDominio);
            Controls.Add(lblModelo);
            Controls.Add(txtModelo);
            Controls.Add(lblEstado);
            Controls.Add(txtEstado);
            Controls.Add(lblMarca);
            Controls.Add(txtMarca);
            Controls.Add(lblColor);
            Controls.Add(txtColor);
            Controls.Add(lblChasis);
            Controls.Add(txtChasis);
            Controls.Add(lblMotor);
            Controls.Add(txtMotor);
            Controls.Add(lblCompraTitulo);
            Controls.Add(lblDNIVendedor);
            Controls.Add(txtDNIVendedor);
            Controls.Add(btnBuscarVendedor);
            Controls.Add(lblVendedor);
            Controls.Add(btnNuevoCliente);
            Controls.Add(lblFecha);
            Controls.Add(dtpFecha);
            Controls.Add(lblPrecio);
            Controls.Add(nudPrecio);
            Controls.Add(lblFormaPago);
            Controls.Add(txtFormaPago);
            Controls.Add(lblObservaciones);
            Controls.Add(txtObservaciones);
            Controls.Add(pnlFaltanDatos);
            Controls.Add(pnlExito);
            Controls.Add(lblObligatorios);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmRegistrarCompra";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "SISTEMA DE GESTIÓN AUTOMOTOR - Registrar vehículo";
            Load += FrmRegistrarCompra_Load;
            ((System.ComponentModel.ISupportInitialize)nudAnio).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).EndInit();
            pnlFaltanDatos.ResumeLayout(false);
            pnlExito.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblVehiculoTitulo;
        private Label lblAnio;
        private NumericUpDown nudAnio;
        private Label lblDominio;
        private TextBox txtDominio;
        private Label lblModelo;
        private TextBox txtModelo;
        private Label lblEstado;
        private TextBox txtEstado;
        private Label lblMarca;
        private TextBox txtMarca;
        private Label lblColor;
        private TextBox txtColor;
        private Label lblChasis;
        private TextBox txtChasis;
        private Label lblMotor;
        private TextBox txtMotor;
        private Label lblCompraTitulo;
        private Label lblDNIVendedor;
        private TextBox txtDNIVendedor;
        private Button btnBuscarVendedor;
        private Label lblVendedor;
        private Button btnNuevoCliente;
        private Label lblFecha;
        private DateTimePicker dtpFecha;
        private Label lblPrecio;
        private NumericUpDown nudPrecio;
        private Label lblFormaPago;
        private TextBox txtFormaPago;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private Panel pnlFaltanDatos;
        private Label lblFaltanDatos;
        private Panel pnlExito;
        private Label lblExito;
        private Label lblObligatorios;
        private Button btnCancelar;
        private Button btnGuardar;
    }
}
