namespace PRAS.UI
{
    partial class FrmCompras
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
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            dgvCompras = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colDominio = new DataGridViewTextBoxColumn();
            colVehiculo = new DataGridViewTextBoxColumn();
            colVendedor = new DataGridViewTextBoxColumn();
            colDNI = new DataGridViewTextBoxColumn();
            colPrecio = new DataGridViewTextBoxColumn();
            colFormaPago = new DataGridViewTextBoxColumn();
            lblResumen = new Label();
            btnNuevaCompra = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCompras).BeginInit();
            SuspendLayout();
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(12, 15);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(42, 15);
            lblDesde.TabIndex = 0;
            lblDesde.Text = "Desde:";
            //
            // dtpDesde
            //
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(60, 11);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(105, 23);
            dtpDesde.TabIndex = 1;
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(177, 15);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(40, 15);
            lblHasta.TabIndex = 2;
            lblHasta.Text = "Hasta:";
            //
            // dtpHasta
            //
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(223, 11);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(105, 23);
            dtpHasta.TabIndex = 3;
            //
            // txtBuscar
            //
            txtBuscar.Location = new Point(342, 11);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Dominio, marca, modelo, apellido o DNI";
            txtBuscar.Size = new Size(250, 23);
            txtBuscar.TabIndex = 4;
            //
            // btnBuscar
            //
            btnBuscar.Location = new Point(598, 10);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(80, 25);
            btnBuscar.TabIndex = 5;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // dgvCompras
            //
            dgvCompras.AllowUserToAddRows = false;
            dgvCompras.AllowUserToDeleteRows = false;
            dgvCompras.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCompras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCompras.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCompras.Columns.AddRange(new DataGridViewColumn[] { colFecha, colDominio, colVehiculo, colVendedor, colDNI, colPrecio, colFormaPago });
            dgvCompras.Location = new Point(12, 45);
            dgvCompras.MultiSelect = false;
            dgvCompras.Name = "dgvCompras";
            dgvCompras.ReadOnly = true;
            dgvCompras.RowHeadersVisible = false;
            dgvCompras.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCompras.Size = new Size(760, 362);
            dgvCompras.TabIndex = 6;
            //
            // colFecha
            //
            colFecha.FillWeight = 60F;
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colDominio
            //
            colDominio.FillWeight = 60F;
            colDominio.HeaderText = "Dominio";
            colDominio.Name = "colDominio";
            colDominio.ReadOnly = true;
            //
            // colVehiculo
            //
            colVehiculo.FillWeight = 130F;
            colVehiculo.HeaderText = "Vehículo";
            colVehiculo.Name = "colVehiculo";
            colVehiculo.ReadOnly = true;
            //
            // colVendedor
            //
            colVendedor.FillWeight = 120F;
            colVendedor.HeaderText = "Vendedor";
            colVendedor.Name = "colVendedor";
            colVendedor.ReadOnly = true;
            //
            // colDNI
            //
            colDNI.FillWeight = 60F;
            colDNI.HeaderText = "DNI";
            colDNI.Name = "colDNI";
            colDNI.ReadOnly = true;
            //
            // colPrecio
            //
            colPrecio.FillWeight = 80F;
            colPrecio.HeaderText = "Precio";
            colPrecio.Name = "colPrecio";
            colPrecio.ReadOnly = true;
            //
            // colFormaPago
            //
            colFormaPago.FillWeight = 70F;
            colFormaPago.HeaderText = "Forma de pago";
            colFormaPago.Name = "colFormaPago";
            colFormaPago.ReadOnly = true;
            //
            // lblResumen
            //
            lblResumen.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblResumen.AutoSize = true;
            lblResumen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblResumen.Location = new Point(12, 416);
            lblResumen.Name = "lblResumen";
            lblResumen.Size = new Size(0, 15);
            lblResumen.TabIndex = 7;
            //
            // btnNuevaCompra
            //
            btnNuevaCompra.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevaCompra.Location = new Point(784, 45);
            btnNuevaCompra.Name = "btnNuevaCompra";
            btnNuevaCompra.Size = new Size(100, 30);
            btnNuevaCompra.TabIndex = 8;
            btnNuevaCompra.Text = "Nueva compra";
            btnNuevaCompra.UseVisualStyleBackColor = true;
            btnNuevaCompra.Click += btnNuevaCompra_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(784, 405);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 9;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmCompras
            //
            AcceptButton = btnBuscar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(896, 447);
            Controls.Add(lblDesde);
            Controls.Add(dtpDesde);
            Controls.Add(lblHasta);
            Controls.Add(dtpHasta);
            Controls.Add(txtBuscar);
            Controls.Add(btnBuscar);
            Controls.Add(dgvCompras);
            Controls.Add(lblResumen);
            Controls.Add(btnNuevaCompra);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmCompras";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Consultar compras";
            Load += FrmCompras_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCompras).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private DataGridView dgvCompras;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colDominio;
        private DataGridViewTextBoxColumn colVehiculo;
        private DataGridViewTextBoxColumn colVendedor;
        private DataGridViewTextBoxColumn colDNI;
        private DataGridViewTextBoxColumn colPrecio;
        private DataGridViewTextBoxColumn colFormaPago;
        private Label lblResumen;
        private Button btnNuevaCompra;
        private Button btnCerrar;
    }
}
