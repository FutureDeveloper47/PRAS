namespace PRAS.UI
{
    partial class FrmClienteHistorial
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
            lblCliente = new Label();
            lblCompras = new Label();
            dgvCompras = new DataGridView();
            colCompraFecha = new DataGridViewTextBoxColumn();
            colCompraVehiculo = new DataGridViewTextBoxColumn();
            colCompraPrecio = new DataGridViewTextBoxColumn();
            colCompraFormaPago = new DataGridViewTextBoxColumn();
            lblVentas = new Label();
            dgvVentas = new DataGridView();
            colVentaFecha = new DataGridViewTextBoxColumn();
            colVentaVehiculo = new DataGridViewTextBoxColumn();
            colVentaAgencia = new DataGridViewTextBoxColumn();
            colVentaPrecio = new DataGridViewTextBoxColumn();
            colVentaFormaPago = new DataGridViewTextBoxColumn();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCompras).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvVentas).BeginInit();
            SuspendLayout();
            //
            // lblCliente
            //
            lblCliente.AutoSize = true;
            lblCliente.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCliente.Location = new Point(12, 12);
            lblCliente.Name = "lblCliente";
            lblCliente.Size = new Size(60, 20);
            lblCliente.TabIndex = 0;
            lblCliente.Text = "Cliente";
            //
            // lblCompras
            //
            lblCompras.AutoSize = true;
            lblCompras.Location = new Point(12, 45);
            lblCompras.Name = "lblCompras";
            lblCompras.Size = new Size(240, 15);
            lblCompras.TabIndex = 1;
            lblCompras.Text = "Compras (autos que le compró la empresa):";
            //
            // dgvCompras
            //
            dgvCompras.AllowUserToAddRows = false;
            dgvCompras.AllowUserToDeleteRows = false;
            dgvCompras.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvCompras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCompras.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCompras.Columns.AddRange(new DataGridViewColumn[] { colCompraFecha, colCompraVehiculo, colCompraPrecio, colCompraFormaPago });
            dgvCompras.Location = new Point(12, 65);
            dgvCompras.Name = "dgvCompras";
            dgvCompras.ReadOnly = true;
            dgvCompras.RowHeadersVisible = false;
            dgvCompras.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCompras.Size = new Size(676, 160);
            dgvCompras.TabIndex = 2;
            //
            // colCompraFecha
            //
            colCompraFecha.FillWeight = 60F;
            colCompraFecha.HeaderText = "Fecha";
            colCompraFecha.Name = "colCompraFecha";
            colCompraFecha.ReadOnly = true;
            //
            // colCompraVehiculo
            //
            colCompraVehiculo.FillWeight = 160F;
            colCompraVehiculo.HeaderText = "Vehículo";
            colCompraVehiculo.Name = "colCompraVehiculo";
            colCompraVehiculo.ReadOnly = true;
            //
            // colCompraPrecio
            //
            colCompraPrecio.FillWeight = 70F;
            colCompraPrecio.HeaderText = "Precio";
            colCompraPrecio.Name = "colCompraPrecio";
            colCompraPrecio.ReadOnly = true;
            //
            // colCompraFormaPago
            //
            colCompraFormaPago.FillWeight = 70F;
            colCompraFormaPago.HeaderText = "Forma de pago";
            colCompraFormaPago.Name = "colCompraFormaPago";
            colCompraFormaPago.ReadOnly = true;
            //
            // lblVentas
            //
            lblVentas.AutoSize = true;
            lblVentas.Location = new Point(12, 240);
            lblVentas.Name = "lblVentas";
            lblVentas.Size = new Size(170, 15);
            lblVentas.TabIndex = 3;
            lblVentas.Text = "Ventas (autos que compró):";
            //
            // dgvVentas
            //
            dgvVentas.AllowUserToAddRows = false;
            dgvVentas.AllowUserToDeleteRows = false;
            dgvVentas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVentas.Columns.AddRange(new DataGridViewColumn[] { colVentaFecha, colVentaVehiculo, colVentaAgencia, colVentaPrecio, colVentaFormaPago });
            dgvVentas.Location = new Point(12, 260);
            dgvVentas.Name = "dgvVentas";
            dgvVentas.ReadOnly = true;
            dgvVentas.RowHeadersVisible = false;
            dgvVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentas.Size = new Size(676, 160);
            dgvVentas.TabIndex = 4;
            //
            // colVentaFecha
            //
            colVentaFecha.FillWeight = 60F;
            colVentaFecha.HeaderText = "Fecha";
            colVentaFecha.Name = "colVentaFecha";
            colVentaFecha.ReadOnly = true;
            //
            // colVentaVehiculo
            //
            colVentaVehiculo.FillWeight = 160F;
            colVentaVehiculo.HeaderText = "Vehículo";
            colVentaVehiculo.Name = "colVentaVehiculo";
            colVentaVehiculo.ReadOnly = true;
            //
            // colVentaAgencia
            //
            colVentaAgencia.HeaderText = "Agencia";
            colVentaAgencia.Name = "colVentaAgencia";
            colVentaAgencia.ReadOnly = true;
            //
            // colVentaPrecio
            //
            colVentaPrecio.FillWeight = 70F;
            colVentaPrecio.HeaderText = "Precio";
            colVentaPrecio.Name = "colVentaPrecio";
            colVentaPrecio.ReadOnly = true;
            //
            // colVentaFormaPago
            //
            colVentaFormaPago.FillWeight = 70F;
            colVentaFormaPago.HeaderText = "Forma de pago";
            colVentaFormaPago.Name = "colVentaFormaPago";
            colVentaFormaPago.ReadOnly = true;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(588, 430);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 5;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmClienteHistorial
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(700, 472);
            Controls.Add(lblCliente);
            Controls.Add(lblCompras);
            Controls.Add(dgvCompras);
            Controls.Add(lblVentas);
            Controls.Add(dgvVentas);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmClienteHistorial";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Historial del cliente";
            Load += FrmClienteHistorial_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCompras).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvVentas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCliente;
        private Label lblCompras;
        private DataGridView dgvCompras;
        private DataGridViewTextBoxColumn colCompraFecha;
        private DataGridViewTextBoxColumn colCompraVehiculo;
        private DataGridViewTextBoxColumn colCompraPrecio;
        private DataGridViewTextBoxColumn colCompraFormaPago;
        private Label lblVentas;
        private DataGridView dgvVentas;
        private DataGridViewTextBoxColumn colVentaFecha;
        private DataGridViewTextBoxColumn colVentaVehiculo;
        private DataGridViewTextBoxColumn colVentaAgencia;
        private DataGridViewTextBoxColumn colVentaPrecio;
        private DataGridViewTextBoxColumn colVentaFormaPago;
        private Button btnCerrar;
    }
}
