namespace PRAS.UI
{
    partial class FrmPedidos
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
            lblEstado = new Label();
            cboEstado = new ComboBox();
            btnActualizar = new Button();
            dgvPedidos = new DataGridView();
            colPedido = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colProveedor = new DataGridViewTextBoxColumn();
            colOT = new DataGridViewTextBoxColumn();
            colDominio = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colEntrega = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            lblDetalleTitulo = new Label();
            dgvDetalle = new DataGridView();
            colRepuesto = new DataGridViewTextBoxColumn();
            colCantidad = new DataGridViewTextBoxColumn();
            colPrecio = new DataGridViewTextBoxColumn();
            colSubtotal = new DataGridViewTextBoxColumn();
            btnNuevo = new Button();
            btnRegistrarEntrega = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPedidos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            SuspendLayout();
            //
            // lblEstado
            //
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(12, 15);
            lblEstado.Name = "lblEstado";
            lblEstado.TabIndex = 0;
            lblEstado.Text = "Estado:";
            //
            // cboEstado
            //
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.FormattingEnabled = true;
            cboEstado.Location = new Point(65, 11);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(130, 23);
            cboEstado.TabIndex = 1;
            cboEstado.SelectedIndexChanged += cboEstado_SelectedIndexChanged;
            //
            // btnActualizar
            //
            btnActualizar.Location = new Point(205, 10);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(90, 25);
            btnActualizar.TabIndex = 2;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            //
            // dgvPedidos
            //
            dgvPedidos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvPedidos.AllowUserToAddRows = false;
            dgvPedidos.AllowUserToDeleteRows = false;
            dgvPedidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPedidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPedidos.Columns.AddRange(new DataGridViewColumn[] { colPedido, colFecha, colProveedor, colOT, colDominio, colEstado, colEntrega, colTotal });
            dgvPedidos.MultiSelect = false;
            dgvPedidos.ReadOnly = true;
            dgvPedidos.RowHeadersVisible = false;
            dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidos.Location = new Point(12, 45);
            dgvPedidos.Name = "dgvPedidos";
            dgvPedidos.Size = new Size(776, 220);
            dgvPedidos.TabIndex = 3;
            dgvPedidos.SelectionChanged += dgvPedidos_SelectionChanged;
            //
            // colPedido
            //
            colPedido.FillWeight = 35F;
            colPedido.HeaderText = "N°";
            colPedido.Name = "colPedido";
            colPedido.ReadOnly = true;
            //
            // colFecha
            //
            colFecha.FillWeight = 60F;
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colProveedor
            //
            colProveedor.FillWeight = 130F;
            colProveedor.HeaderText = "Proveedor";
            colProveedor.Name = "colProveedor";
            colProveedor.ReadOnly = true;
            //
            // colOT
            //
            colOT.FillWeight = 35F;
            colOT.HeaderText = "OT";
            colOT.Name = "colOT";
            colOT.ReadOnly = true;
            //
            // colDominio
            //
            colDominio.FillWeight = 60F;
            colDominio.HeaderText = "Dominio";
            colDominio.Name = "colDominio";
            colDominio.ReadOnly = true;
            //
            // colEstado
            //
            colEstado.FillWeight = 60F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // colEntrega
            //
            colEntrega.FillWeight = 60F;
            colEntrega.HeaderText = "Entrega";
            colEntrega.Name = "colEntrega";
            colEntrega.ReadOnly = true;
            //
            // colTotal
            //
            colTotal.FillWeight = 70F;
            colTotal.HeaderText = "Total";
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            //
            // lblDetalleTitulo
            //
            lblDetalleTitulo.AutoSize = true;
            lblDetalleTitulo.Location = new Point(12, 275);
            lblDetalleTitulo.Name = "lblDetalleTitulo";
            lblDetalleTitulo.TabIndex = 4;
            lblDetalleTitulo.Text = "Repuestos del pedido seleccionado";
            //
            // dgvDetalle
            //
            dgvDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalle.Columns.AddRange(new DataGridViewColumn[] { colRepuesto, colCantidad, colPrecio, colSubtotal });
            dgvDetalle.MultiSelect = false;
            dgvDetalle.ReadOnly = true;
            dgvDetalle.RowHeadersVisible = false;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.Location = new Point(12, 295);
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.Size = new Size(776, 150);
            dgvDetalle.TabIndex = 5;
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
            // btnNuevo
            //
            btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevo.Location = new Point(800, 45);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(130, 30);
            btnNuevo.TabIndex = 6;
            btnNuevo.Text = "Nuevo pedido";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnRegistrarEntrega
            //
            btnRegistrarEntrega.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRegistrarEntrega.Location = new Point(800, 85);
            btnRegistrarEntrega.Name = "btnRegistrarEntrega";
            btnRegistrarEntrega.Size = new Size(130, 30);
            btnRegistrarEntrega.TabIndex = 7;
            btnRegistrarEntrega.Text = "Registrar entrega";
            btnRegistrarEntrega.UseVisualStyleBackColor = true;
            btnRegistrarEntrega.Click += btnRegistrarEntrega_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(830, 458);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 8;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmPedidos
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(942, 500);
            Controls.Add(lblEstado);
            Controls.Add(cboEstado);
            Controls.Add(btnActualizar);
            Controls.Add(dgvPedidos);
            Controls.Add(lblDetalleTitulo);
            Controls.Add(dgvDetalle);
            Controls.Add(btnNuevo);
            Controls.Add(btnRegistrarEntrega);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmPedidos";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Pedidos de repuestos";
            Load += FrmPedidos_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPedidos).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblEstado;
        private ComboBox cboEstado;
        private Button btnActualizar;
        private DataGridView dgvPedidos;
        private DataGridViewTextBoxColumn colPedido;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colProveedor;
        private DataGridViewTextBoxColumn colOT;
        private DataGridViewTextBoxColumn colDominio;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colEntrega;
        private DataGridViewTextBoxColumn colTotal;
        private Label lblDetalleTitulo;
        private DataGridView dgvDetalle;
        private DataGridViewTextBoxColumn colRepuesto;
        private DataGridViewTextBoxColumn colCantidad;
        private DataGridViewTextBoxColumn colPrecio;
        private DataGridViewTextBoxColumn colSubtotal;
        private Button btnNuevo;
        private Button btnRegistrarEntrega;
        private Button btnCerrar;
    }
}
