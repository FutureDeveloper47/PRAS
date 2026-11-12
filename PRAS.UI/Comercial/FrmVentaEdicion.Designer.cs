namespace PRAS.UI
{
    partial class FrmVentaEdicion
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
            lblVehiculo = new Label();
            cboConsignacion = new ComboBox();
            lblDatosConsignacion = new Label();
            lblDNI = new Label();
            txtDNI = new TextBox();
            btnBuscarCliente = new Button();
            btnNuevoCliente = new Button();
            lblCliente = new Label();
            lblPrecio = new Label();
            numPrecio = new NumericUpDown();
            lblComision = new Label();
            lblMontoComision = new Label();
            lblFormaPago = new Label();
            cboFormaPago = new ComboBox();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            btnGuardar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)numPrecio).BeginInit();
            SuspendLayout();
            //
            // lblVehiculo
            //
            lblVehiculo.AutoSize = true;
            lblVehiculo.Location = new Point(12, 15);
            lblVehiculo.Name = "lblVehiculo";
            lblVehiculo.TabIndex = 0;
            lblVehiculo.Text = "Vehículo en agencia *:";
            //
            // cboConsignacion
            //
            cboConsignacion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboConsignacion.FormattingEnabled = true;
            cboConsignacion.Location = new Point(170, 11);
            cboConsignacion.Name = "cboConsignacion";
            cboConsignacion.Size = new Size(422, 23);
            cboConsignacion.TabIndex = 1;
            cboConsignacion.SelectedIndexChanged += cboConsignacion_SelectedIndexChanged;
            //
            // lblDatosConsignacion
            //
            lblDatosConsignacion.Location = new Point(170, 40);
            lblDatosConsignacion.Name = "lblDatosConsignacion";
            lblDatosConsignacion.Size = new Size(422, 34);
            lblDatosConsignacion.TabIndex = 2;
            lblDatosConsignacion.Text = "";
            //
            // lblDNI
            //
            lblDNI.AutoSize = true;
            lblDNI.Location = new Point(12, 85);
            lblDNI.Name = "lblDNI";
            lblDNI.TabIndex = 3;
            lblDNI.Text = "DNI del comprador *:";
            //
            // txtDNI
            //
            txtDNI.Location = new Point(170, 81);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(120, 23);
            txtDNI.TabIndex = 4;
            //
            // btnBuscarCliente
            //
            btnBuscarCliente.Location = new Point(296, 80);
            btnBuscarCliente.Name = "btnBuscarCliente";
            btnBuscarCliente.Size = new Size(80, 25);
            btnBuscarCliente.TabIndex = 5;
            btnBuscarCliente.Text = "Buscar";
            btnBuscarCliente.UseVisualStyleBackColor = true;
            btnBuscarCliente.Click += btnBuscarCliente_Click;
            //
            // btnNuevoCliente
            //
            btnNuevoCliente.Location = new Point(382, 80);
            btnNuevoCliente.Name = "btnNuevoCliente";
            btnNuevoCliente.Size = new Size(120, 25);
            btnNuevoCliente.TabIndex = 6;
            btnNuevoCliente.Text = "Nuevo cliente...";
            btnNuevoCliente.UseVisualStyleBackColor = true;
            btnNuevoCliente.Click += btnNuevoCliente_Click;
            //
            // lblCliente
            //
            lblCliente.AutoSize = true;
            lblCliente.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCliente.Location = new Point(170, 112);
            lblCliente.Name = "lblCliente";
            lblCliente.TabIndex = 7;
            lblCliente.Text = "(sin cliente)";
            //
            // lblPrecio
            //
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(12, 145);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.TabIndex = 8;
            lblPrecio.Text = "Precio de venta *:";
            //
            // numPrecio
            //
            numPrecio.DecimalPlaces = 2;
            numPrecio.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numPrecio.ThousandsSeparator = true;
            numPrecio.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            numPrecio.Location = new Point(170, 141);
            numPrecio.Name = "numPrecio";
            numPrecio.Size = new Size(150, 23);
            numPrecio.TabIndex = 9;
            numPrecio.ValueChanged += numPrecio_ValueChanged;
            //
            // lblComision
            //
            lblComision.AutoSize = true;
            lblComision.Location = new Point(12, 178);
            lblComision.Name = "lblComision";
            lblComision.TabIndex = 10;
            lblComision.Text = "Comisión de la agencia:";
            //
            // lblMontoComision
            //
            lblMontoComision.AutoSize = true;
            lblMontoComision.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMontoComision.Location = new Point(170, 178);
            lblMontoComision.Name = "lblMontoComision";
            lblMontoComision.TabIndex = 11;
            lblMontoComision.Text = "$ 0,00";
            //
            // lblFormaPago
            //
            lblFormaPago.AutoSize = true;
            lblFormaPago.Location = new Point(12, 211);
            lblFormaPago.Name = "lblFormaPago";
            lblFormaPago.TabIndex = 12;
            lblFormaPago.Text = "Forma de pago *:";
            //
            // cboFormaPago
            //
            cboFormaPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFormaPago.FormattingEnabled = true;
            cboFormaPago.Location = new Point(170, 207);
            cboFormaPago.Name = "cboFormaPago";
            cboFormaPago.Size = new Size(150, 23);
            cboFormaPago.TabIndex = 13;
            //
            // lblFecha
            //
            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(12, 244);
            lblFecha.Name = "lblFecha";
            lblFecha.TabIndex = 14;
            lblFecha.Text = "Fecha de venta *:";
            //
            // dtpFecha
            //
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(170, 240);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(110, 23);
            dtpFecha.TabIndex = 15;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(382, 285);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 30);
            btnGuardar.TabIndex = 16;
            btnGuardar.Text = "Registrar venta";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(498, 285);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 30);
            btnCancelar.TabIndex = 17;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmVentaEdicion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(606, 330);
            Controls.Add(lblVehiculo);
            Controls.Add(cboConsignacion);
            Controls.Add(lblDatosConsignacion);
            Controls.Add(lblDNI);
            Controls.Add(txtDNI);
            Controls.Add(btnBuscarCliente);
            Controls.Add(btnNuevoCliente);
            Controls.Add(lblCliente);
            Controls.Add(lblPrecio);
            Controls.Add(numPrecio);
            Controls.Add(lblComision);
            Controls.Add(lblMontoComision);
            Controls.Add(lblFormaPago);
            Controls.Add(cboFormaPago);
            Controls.Add(lblFecha);
            Controls.Add(dtpFecha);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmVentaEdicion";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar venta";
            Load += FrmVentaEdicion_Load;
            ((System.ComponentModel.ISupportInitialize)numPrecio).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblVehiculo;
        private ComboBox cboConsignacion;
        private Label lblDatosConsignacion;
        private Label lblDNI;
        private TextBox txtDNI;
        private Button btnBuscarCliente;
        private Button btnNuevoCliente;
        private Label lblCliente;
        private Label lblPrecio;
        private NumericUpDown numPrecio;
        private Label lblComision;
        private Label lblMontoComision;
        private Label lblFormaPago;
        private ComboBox cboFormaPago;
        private Label lblFecha;
        private DateTimePicker dtpFecha;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
