namespace PRAS.UI
{
    partial class FrmVentas
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
            dgvVentas = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colDominio = new DataGridViewTextBoxColumn();
            colVehiculo = new DataGridViewTextBoxColumn();
            colAgencia = new DataGridViewTextBoxColumn();
            colComprador = new DataGridViewTextBoxColumn();
            colDNI = new DataGridViewTextBoxColumn();
            colPrecio = new DataGridViewTextBoxColumn();
            colComision = new DataGridViewTextBoxColumn();
            colFormaPago = new DataGridViewTextBoxColumn();
            lblResumen = new Label();
            btnNuevo = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvVentas).BeginInit();
            SuspendLayout();
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(12, 15);
            lblDesde.Name = "lblDesde";
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
            txtBuscar.PlaceholderText = "Dominio, marca, apellido, DNI o agencia";
            txtBuscar.Location = new Point(342, 11);
            txtBuscar.Name = "txtBuscar";
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
            // dgvVentas
            //
            dgvVentas.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            dgvVentas.AllowUserToAddRows = false;
            dgvVentas.AllowUserToDeleteRows = false;
            dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVentas.Columns.AddRange(new DataGridViewColumn[] { colFecha, colDominio, colVehiculo, colAgencia, colComprador, colDNI, colPrecio, colComision, colFormaPago });
            dgvVentas.MultiSelect = false;
            dgvVentas.ReadOnly = true;
            dgvVentas.RowHeadersVisible = false;
            dgvVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentas.Location = new Point(12, 45);
            dgvVentas.Name = "dgvVentas";
            dgvVentas.Size = new Size(876, 390);
            dgvVentas.TabIndex = 6;
            //
            // colFecha
            //
            colFecha.FillWeight = 55F;
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colDominio
            //
            colDominio.FillWeight = 55F;
            colDominio.HeaderText = "Dominio";
            colDominio.Name = "colDominio";
            colDominio.ReadOnly = true;
            //
            // colVehiculo
            //
            colVehiculo.FillWeight = 120F;
            colVehiculo.HeaderText = "Vehículo";
            colVehiculo.Name = "colVehiculo";
            colVehiculo.ReadOnly = true;
            //
            // colAgencia
            //
            colAgencia.FillWeight = 110F;
            colAgencia.HeaderText = "Agencia";
            colAgencia.Name = "colAgencia";
            colAgencia.ReadOnly = true;
            //
            // colComprador
            //
            colComprador.FillWeight = 110F;
            colComprador.HeaderText = "Comprador";
            colComprador.Name = "colComprador";
            colComprador.ReadOnly = true;
            //
            // colDNI
            //
            colDNI.FillWeight = 55F;
            colDNI.HeaderText = "DNI";
            colDNI.Name = "colDNI";
            colDNI.ReadOnly = true;
            //
            // colPrecio
            //
            colPrecio.FillWeight = 75F;
            colPrecio.HeaderText = "Precio";
            colPrecio.Name = "colPrecio";
            colPrecio.ReadOnly = true;
            //
            // colComision
            //
            colComision.FillWeight = 65F;
            colComision.HeaderText = "Comisión";
            colComision.Name = "colComision";
            colComision.ReadOnly = true;
            //
            // colFormaPago
            //
            colFormaPago.FillWeight = 65F;
            colFormaPago.HeaderText = "Forma de pago";
            colFormaPago.Name = "colFormaPago";
            colFormaPago.ReadOnly = true;
            //
            // lblResumen
            //
            lblResumen.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblResumen.AutoSize = true;
            lblResumen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblResumen.Location = new Point(12, 445);
            lblResumen.Name = "lblResumen";
            lblResumen.TabIndex = 7;
            lblResumen.Text = "";
            //
            // btnNuevo
            //
            btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevo.Location = new Point(900, 45);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(128, 30);
            btnNuevo.TabIndex = 8;
            btnNuevo.Text = "Nueva venta";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(928, 438);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 9;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmVentas
            //
            AcceptButton = btnBuscar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1040, 480);
            Controls.Add(lblDesde);
            Controls.Add(dtpDesde);
            Controls.Add(lblHasta);
            Controls.Add(dtpHasta);
            Controls.Add(txtBuscar);
            Controls.Add(btnBuscar);
            Controls.Add(dgvVentas);
            Controls.Add(lblResumen);
            Controls.Add(btnNuevo);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmVentas";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Consultar ventas";
            Load += FrmVentas_Load;
            ((System.ComponentModel.ISupportInitialize)dgvVentas).EndInit();
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
        private DataGridView dgvVentas;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colDominio;
        private DataGridViewTextBoxColumn colVehiculo;
        private DataGridViewTextBoxColumn colAgencia;
        private DataGridViewTextBoxColumn colComprador;
        private DataGridViewTextBoxColumn colDNI;
        private DataGridViewTextBoxColumn colPrecio;
        private DataGridViewTextBoxColumn colComision;
        private DataGridViewTextBoxColumn colFormaPago;
        private Label lblResumen;
        private Button btnNuevo;
        private Button btnCerrar;
    }
}
