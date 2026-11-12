namespace PRAS.UI
{
    partial class FrmPedidoEdicion
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
            lblOrden = new Label();
            cboOrden = new ComboBox();
            lblProveedor = new Label();
            cboProveedor = new ComboBox();
            lblRepuesto = new Label();
            txtRepuesto = new TextBox();
            lblCantidad = new Label();
            numCantidad = new NumericUpDown();
            lblPrecio = new Label();
            numPrecio = new NumericUpDown();
            btnAgregarRepuesto = new Button();
            dgvDetalle = new DataGridView();
            colRepuesto = new DataGridViewTextBoxColumn();
            colCantidad = new DataGridViewTextBoxColumn();
            colPrecio = new DataGridViewTextBoxColumn();
            colSubtotal = new DataGridViewTextBoxColumn();
            btnQuitarRepuesto = new Button();
            lblTotal = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)numCantidad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrecio).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            SuspendLayout();
            //
            // lblOrden
            //
            lblOrden.AutoSize = true;
            lblOrden.Location = new Point(12, 15);
            lblOrden.Name = "lblOrden";
            lblOrden.TabIndex = 0;
            lblOrden.Text = "Orden de trabajo *:";
            //
            // cboOrden
            //
            cboOrden.DropDownStyle = ComboBoxStyle.DropDownList;
            cboOrden.FormattingEnabled = true;
            cboOrden.Location = new Point(140, 11);
            cboOrden.Name = "cboOrden";
            cboOrden.Size = new Size(432, 23);
            cboOrden.TabIndex = 1;
            //
            // lblProveedor
            //
            lblProveedor.AutoSize = true;
            lblProveedor.Location = new Point(12, 48);
            lblProveedor.Name = "lblProveedor";
            lblProveedor.TabIndex = 2;
            lblProveedor.Text = "Proveedor *:";
            //
            // cboProveedor
            //
            cboProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProveedor.FormattingEnabled = true;
            cboProveedor.Location = new Point(140, 44);
            cboProveedor.Name = "cboProveedor";
            cboProveedor.Size = new Size(432, 23);
            cboProveedor.TabIndex = 3;
            //
            // lblRepuesto
            //
            lblRepuesto.AutoSize = true;
            lblRepuesto.Location = new Point(12, 85);
            lblRepuesto.Name = "lblRepuesto";
            lblRepuesto.TabIndex = 4;
            lblRepuesto.Text = "Repuesto:";
            //
            // txtRepuesto
            //
            txtRepuesto.Location = new Point(12, 105);
            txtRepuesto.Name = "txtRepuesto";
            txtRepuesto.Size = new Size(260, 23);
            txtRepuesto.TabIndex = 5;
            //
            // lblCantidad
            //
            lblCantidad.AutoSize = true;
            lblCantidad.Location = new Point(280, 85);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.TabIndex = 6;
            lblCantidad.Text = "Cantidad:";
            //
            // numCantidad
            //
            numCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numCantidad.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            numCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numCantidad.Location = new Point(280, 105);
            numCantidad.Name = "numCantidad";
            numCantidad.Size = new Size(70, 23);
            numCantidad.TabIndex = 7;
            //
            // lblPrecio
            //
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(360, 85);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.TabIndex = 8;
            lblPrecio.Text = "Precio unitario:";
            //
            // numPrecio
            //
            numPrecio.DecimalPlaces = 2;
            numPrecio.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numPrecio.ThousandsSeparator = true;
            numPrecio.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            numPrecio.Location = new Point(360, 105);
            numPrecio.Name = "numPrecio";
            numPrecio.Size = new Size(120, 23);
            numPrecio.TabIndex = 9;
            //
            // btnAgregarRepuesto
            //
            btnAgregarRepuesto.Location = new Point(490, 104);
            btnAgregarRepuesto.Name = "btnAgregarRepuesto";
            btnAgregarRepuesto.Size = new Size(82, 25);
            btnAgregarRepuesto.TabIndex = 10;
            btnAgregarRepuesto.Text = "Agregar";
            btnAgregarRepuesto.UseVisualStyleBackColor = true;
            btnAgregarRepuesto.Click += btnAgregarRepuesto_Click;
            //
            // dgvDetalle
            //
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalle.Columns.AddRange(new DataGridViewColumn[] { colRepuesto, colCantidad, colPrecio, colSubtotal });
            dgvDetalle.MultiSelect = false;
            dgvDetalle.ReadOnly = true;
            dgvDetalle.RowHeadersVisible = false;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.Location = new Point(12, 140);
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.Size = new Size(560, 200);
            dgvDetalle.TabIndex = 11;
            //
            // colRepuesto
            //
            colRepuesto.FillWeight = 200F;
            colRepuesto.HeaderText = "Repuesto";
            colRepuesto.Name = "colRepuesto";
            colRepuesto.ReadOnly = true;
            //
            // colCantidad
            //
            colCantidad.FillWeight = 50F;
            colCantidad.HeaderText = "Cantidad";
            colCantidad.Name = "colCantidad";
            colCantidad.ReadOnly = true;
            //
            // colPrecio
            //
            colPrecio.FillWeight = 70F;
            colPrecio.HeaderText = "Precio unitario";
            colPrecio.Name = "colPrecio";
            colPrecio.ReadOnly = true;
            //
            // colSubtotal
            //
            colSubtotal.FillWeight = 70F;
            colSubtotal.HeaderText = "Subtotal";
            colSubtotal.Name = "colSubtotal";
            colSubtotal.ReadOnly = true;
            //
            // btnQuitarRepuesto
            //
            btnQuitarRepuesto.Location = new Point(12, 348);
            btnQuitarRepuesto.Name = "btnQuitarRepuesto";
            btnQuitarRepuesto.Size = new Size(130, 28);
            btnQuitarRepuesto.TabIndex = 12;
            btnQuitarRepuesto.Text = "Quitar repuesto";
            btnQuitarRepuesto.UseVisualStyleBackColor = true;
            btnQuitarRepuesto.Click += btnQuitarRepuesto_Click;
            //
            // lblTotal
            //
            lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotal.TextAlign = ContentAlignment.MiddleRight;
            lblTotal.Location = new Point(300, 350);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(272, 20);
            lblTotal.TabIndex = 13;
            lblTotal.Text = "Total: $ 0,00";
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(352, 390);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 30);
            btnGuardar.TabIndex = 14;
            btnGuardar.Text = "Registrar pedido";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(472, 390);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 30);
            btnCancelar.TabIndex = 15;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmPedidoEdicion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(586, 432);
            Controls.Add(lblOrden);
            Controls.Add(cboOrden);
            Controls.Add(lblProveedor);
            Controls.Add(cboProveedor);
            Controls.Add(lblRepuesto);
            Controls.Add(txtRepuesto);
            Controls.Add(lblCantidad);
            Controls.Add(numCantidad);
            Controls.Add(lblPrecio);
            Controls.Add(numPrecio);
            Controls.Add(btnAgregarRepuesto);
            Controls.Add(dgvDetalle);
            Controls.Add(btnQuitarRepuesto);
            Controls.Add(lblTotal);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPedidoEdicion";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar pedido de repuestos";
            Load += FrmPedidoEdicion_Load;
            ((System.ComponentModel.ISupportInitialize)numCantidad).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrecio).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblOrden;
        private ComboBox cboOrden;
        private Label lblProveedor;
        private ComboBox cboProveedor;
        private Label lblRepuesto;
        private TextBox txtRepuesto;
        private Label lblCantidad;
        private NumericUpDown numCantidad;
        private Label lblPrecio;
        private NumericUpDown numPrecio;
        private Button btnAgregarRepuesto;
        private DataGridView dgvDetalle;
        private DataGridViewTextBoxColumn colRepuesto;
        private DataGridViewTextBoxColumn colCantidad;
        private DataGridViewTextBoxColumn colPrecio;
        private DataGridViewTextBoxColumn colSubtotal;
        private Button btnQuitarRepuesto;
        private Label lblTotal;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
