namespace PRAS.UI
{
    partial class FrmCargaCostos
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
            lblAuto = new Label();
            cboAuto = new ComboBox();
            lblTipoGasto = new Label();
            cboTipoGasto = new ComboBox();
            lblDescripcion = new Label();
            txtDescripcion = new TextBox();
            lblCantidad = new Label();
            nudCantidad = new NumericUpDown();
            lblPrecio = new Label();
            nudPrecio = new NumericUpDown();
            btnAgregar = new Button();
            dgvGastos = new DataGridView();
            colDescripcion = new DataGridViewTextBoxColumn();
            colCantidad = new DataGridViewTextBoxColumn();
            colPrecio = new DataGridViewTextBoxColumn();
            colSubtotal = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            btnQuitar = new Button();
            lblTotal = new Label();
            btnFinalizar = new Button();
            btnCancelar = new Button();
            btnGuardar = new Button();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvGastos).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(880, 45);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "PRAS: Programa de Registro Automotor Superior";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblAuto
            //
            lblAuto.AutoSize = true;
            lblAuto.Location = new Point(60, 85);
            lblAuto.Name = "lblAuto";
            lblAuto.TabIndex = 1;
            lblAuto.Text = "SELECCIONAR AUTO";
            //
            // cboAuto
            //
            cboAuto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAuto.FormattingEnabled = true;
            cboAuto.Location = new Point(230, 82);
            cboAuto.Name = "cboAuto";
            cboAuto.Size = new Size(420, 23);
            cboAuto.TabIndex = 2;
            cboAuto.SelectionChangeCommitted += cboAuto_SelectionChangeCommitted;
            //
            // lblTipoGasto
            //
            lblTipoGasto.AutoSize = true;
            lblTipoGasto.Location = new Point(60, 125);
            lblTipoGasto.Name = "lblTipoGasto";
            lblTipoGasto.TabIndex = 3;
            lblTipoGasto.Text = "Tipo de Gasto";
            //
            // cboTipoGasto
            //
            cboTipoGasto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoGasto.FormattingEnabled = true;
            cboTipoGasto.Location = new Point(230, 122);
            cboTipoGasto.Name = "cboTipoGasto";
            cboTipoGasto.Size = new Size(160, 23);
            cboTipoGasto.TabIndex = 4;
            //
            // lblDescripcion
            //
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(60, 165);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.TabIndex = 5;
            lblDescripcion.Text = "Descripción";
            //
            // txtDescripcion
            //
            txtDescripcion.Location = new Point(230, 162);
            txtDescripcion.MaxLength = 185;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.PlaceholderText = "Ej: paragolpe delantero / chapa y pintura";
            txtDescripcion.Size = new Size(420, 23);
            txtDescripcion.TabIndex = 6;
            //
            // lblCantidad
            //
            lblCantidad.AutoSize = true;
            lblCantidad.Location = new Point(60, 205);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.TabIndex = 7;
            lblCantidad.Text = "Cantidad";
            //
            // nudCantidad
            //
            nudCantidad.Location = new Point(230, 202);
            nudCantidad.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCantidad.Name = "nudCantidad";
            nudCantidad.Size = new Size(70, 23);
            nudCantidad.TabIndex = 8;
            nudCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lblPrecio
            //
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(330, 205);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.TabIndex = 9;
            lblPrecio.Text = "Precio unitario $";
            //
            // nudPrecio
            //
            nudPrecio.DecimalPlaces = 2;
            nudPrecio.Location = new Point(460, 202);
            nudPrecio.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            nudPrecio.Name = "nudPrecio";
            nudPrecio.Size = new Size(120, 23);
            nudPrecio.TabIndex = 10;
            nudPrecio.ThousandsSeparator = true;
            //
            // btnAgregar
            //
            btnAgregar.Location = new Point(600, 201);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(110, 25);
            btnAgregar.TabIndex = 11;
            btnAgregar.Text = "Agregar gasto";
            btnAgregar.UseVisualStyleBackColor = true;
            btnAgregar.Click += btnAgregar_Click;
            //
            // dgvGastos
            //
            dgvGastos.AllowUserToAddRows = false;
            dgvGastos.AllowUserToDeleteRows = false;
            dgvGastos.AllowUserToResizeRows = false;
            dgvGastos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGastos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGastos.Columns.AddRange(new DataGridViewColumn[] { colDescripcion, colCantidad, colPrecio, colSubtotal, colEstado });
            dgvGastos.Location = new Point(60, 245);
            dgvGastos.MultiSelect = false;
            dgvGastos.Name = "dgvGastos";
            dgvGastos.ReadOnly = true;
            dgvGastos.RowHeadersVisible = false;
            dgvGastos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGastos.Size = new Size(760, 180);
            dgvGastos.TabIndex = 12;
            //
            // colDescripcion
            //
            colDescripcion.FillWeight = 45F;
            colDescripcion.HeaderText = "Descripción";
            colDescripcion.Name = "colDescripcion";
            colDescripcion.ReadOnly = true;
            //
            // colCantidad
            //
            colCantidad.FillWeight = 10F;
            colCantidad.HeaderText = "Cant.";
            colCantidad.Name = "colCantidad";
            colCantidad.ReadOnly = true;
            //
            // colPrecio
            //
            colPrecio.FillWeight = 15F;
            colPrecio.HeaderText = "Precio unit.";
            colPrecio.Name = "colPrecio";
            colPrecio.ReadOnly = true;
            //
            // colSubtotal
            //
            colSubtotal.FillWeight = 15F;
            colSubtotal.HeaderText = "Subtotal";
            colSubtotal.Name = "colSubtotal";
            colSubtotal.ReadOnly = true;
            //
            // colEstado
            //
            colEstado.FillWeight = 15F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // btnQuitar
            //
            btnQuitar.Location = new Point(60, 432);
            btnQuitar.Name = "btnQuitar";
            btnQuitar.Size = new Size(170, 25);
            btnQuitar.TabIndex = 13;
            btnQuitar.Text = "Quitar gasto nuevo";
            btnQuitar.UseVisualStyleBackColor = true;
            btnQuitar.Click += btnQuitar_Click;
            //
            // lblTotal
            //
            lblTotal.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTotal.Location = new Point(60, 470);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(700, 38);
            lblTotal.TabIndex = 14;
            lblTotal.Text = "TOTAL ACUMULADO = $ 0,00";
            //
            // btnFinalizar
            //
            btnFinalizar.Location = new Point(60, 530);
            btnFinalizar.Name = "btnFinalizar";
            btnFinalizar.Size = new Size(190, 28);
            btnFinalizar.TabIndex = 15;
            btnFinalizar.Text = "Finalizar reparación";
            btnFinalizar.UseVisualStyleBackColor = true;
            btnFinalizar.Click += btnFinalizar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(600, 530);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(105, 28);
            btnCancelar.TabIndex = 17;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(715, 530);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(105, 28);
            btnGuardar.TabIndex = 16;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // FrmCargaCostos
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(880, 575);
            Controls.Add(lblTitulo);
            Controls.Add(lblAuto);
            Controls.Add(cboAuto);
            Controls.Add(lblTipoGasto);
            Controls.Add(cboTipoGasto);
            Controls.Add(lblDescripcion);
            Controls.Add(txtDescripcion);
            Controls.Add(lblCantidad);
            Controls.Add(nudCantidad);
            Controls.Add(lblPrecio);
            Controls.Add(nudPrecio);
            Controls.Add(btnAgregar);
            Controls.Add(dgvGastos);
            Controls.Add(btnQuitar);
            Controls.Add(lblTotal);
            Controls.Add(btnFinalizar);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCargaCostos";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Carga de Costos";
            FormClosing += FrmCargaCostos_FormClosing;
            Load += FrmCargaCostos_Load;
            ((System.ComponentModel.ISupportInitialize)nudCantidad).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvGastos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblAuto;
        private ComboBox cboAuto;
        private Label lblTipoGasto;
        private ComboBox cboTipoGasto;
        private Label lblDescripcion;
        private TextBox txtDescripcion;
        private Label lblCantidad;
        private NumericUpDown nudCantidad;
        private Label lblPrecio;
        private NumericUpDown nudPrecio;
        private Button btnAgregar;
        private DataGridView dgvGastos;
        private DataGridViewTextBoxColumn colDescripcion;
        private DataGridViewTextBoxColumn colCantidad;
        private DataGridViewTextBoxColumn colPrecio;
        private DataGridViewTextBoxColumn colSubtotal;
        private DataGridViewTextBoxColumn colEstado;
        private Button btnQuitar;
        private Label lblTotal;
        private Button btnFinalizar;
        private Button btnCancelar;
        private Button btnGuardar;
    }
}
