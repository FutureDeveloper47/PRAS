namespace PRAS.UI
{
    partial class FrmBitacora
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
            DataGridViewCellStyle estiloFecha = new DataGridViewCellStyle();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            lblNivel = new Label();
            cmbNivel = new ComboBox();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            lblTexto = new Label();
            txtTexto = new TextBox();
            btnBuscar = new Button();
            btnLimpiar = new Button();
            dgvBitacora = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colNivel = new DataGridViewTextBoxColumn();
            colUsuario = new DataGridViewTextBoxColumn();
            colMensaje = new DataGridViewTextBoxColumn();
            colDetalle = new DataGridViewTextBoxColumn();
            lblCantidad = new Label();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvBitacora).BeginInit();
            SuspendLayout();
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(12, 16);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(42, 15);
            lblDesde.TabIndex = 0;
            lblDesde.Text = "Desde:";
            //
            // dtpDesde
            //
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(70, 12);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(110, 23);
            dtpDesde.TabIndex = 1;
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(196, 16);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(40, 15);
            lblHasta.TabIndex = 2;
            lblHasta.Text = "Hasta:";
            //
            // dtpHasta
            //
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(250, 12);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(110, 23);
            dtpHasta.TabIndex = 3;
            //
            // lblNivel
            //
            lblNivel.AutoSize = true;
            lblNivel.Location = new Point(378, 16);
            lblNivel.Name = "lblNivel";
            lblNivel.Size = new Size(37, 15);
            lblNivel.TabIndex = 4;
            lblNivel.Text = "Nivel:";
            //
            // cmbNivel
            //
            cmbNivel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNivel.Location = new Point(428, 12);
            cmbNivel.Name = "cmbNivel";
            cmbNivel.Size = new Size(110, 23);
            cmbNivel.TabIndex = 5;
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(556, 16);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(50, 15);
            lblUsuario.TabIndex = 6;
            lblUsuario.Text = "Usuario:";
            //
            // txtUsuario
            //
            txtUsuario.Location = new Point(620, 12);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(130, 23);
            txtUsuario.TabIndex = 7;
            //
            // lblTexto
            //
            lblTexto.AutoSize = true;
            lblTexto.Location = new Point(12, 50);
            lblTexto.Name = "lblTexto";
            lblTexto.Size = new Size(45, 15);
            lblTexto.TabIndex = 8;
            lblTexto.Text = "Buscar:";
            //
            // txtTexto
            //
            txtTexto.Location = new Point(70, 46);
            txtTexto.Name = "txtTexto";
            txtTexto.Size = new Size(468, 23);
            txtTexto.TabIndex = 9;
            //
            // btnBuscar
            //
            btnBuscar.Location = new Point(556, 43);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(94, 29);
            btnBuscar.TabIndex = 10;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // btnLimpiar
            //
            btnLimpiar.Location = new Point(656, 43);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(94, 29);
            btnLimpiar.TabIndex = 11;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            //
            // dgvBitacora
            //
            dgvBitacora.AllowUserToAddRows = false;
            dgvBitacora.AllowUserToDeleteRows = false;
            dgvBitacora.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvBitacora.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBitacora.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBitacora.Columns.AddRange(new DataGridViewColumn[] { colFecha, colNivel, colUsuario, colMensaje, colDetalle });
            dgvBitacora.Location = new Point(12, 84);
            dgvBitacora.MultiSelect = false;
            dgvBitacora.Name = "dgvBitacora";
            dgvBitacora.ReadOnly = true;
            dgvBitacora.RowHeadersVisible = false;
            dgvBitacora.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBitacora.Size = new Size(916, 380);
            dgvBitacora.TabIndex = 12;
            dgvBitacora.CellDoubleClick += dgvBitacora_CellDoubleClick;
            dgvBitacora.CellFormatting += dgvBitacora_CellFormatting;
            //
            // colFecha
            //
            colFecha.DataPropertyName = "TimeStamp";
            estiloFecha.Format = "dd/MM/yyyy HH:mm:ss";
            colFecha.DefaultCellStyle = estiloFecha;
            colFecha.FillWeight = 22F;
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colNivel
            //
            colNivel.DataPropertyName = "LogLevel";
            colNivel.FillWeight = 10F;
            colNivel.HeaderText = "Nivel";
            colNivel.Name = "colNivel";
            colNivel.ReadOnly = true;
            //
            // colUsuario
            //
            colUsuario.DataPropertyName = "Usuario";
            colUsuario.FillWeight = 13F;
            colUsuario.HeaderText = "Usuario";
            colUsuario.Name = "colUsuario";
            colUsuario.ReadOnly = true;
            //
            // colMensaje
            //
            colMensaje.DataPropertyName = "Message";
            colMensaje.FillWeight = 55F;
            colMensaje.HeaderText = "Mensaje";
            colMensaje.Name = "colMensaje";
            colMensaje.ReadOnly = true;
            //
            // colDetalle
            //
            colDetalle.DataPropertyName = "Detalle";
            colDetalle.FillWeight = 30F;
            colDetalle.HeaderText = "Detalle del error";
            colDetalle.Name = "colDetalle";
            colDetalle.ReadOnly = true;
            //
            // lblCantidad
            //
            lblCantidad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCantidad.AutoSize = true;
            lblCantidad.Location = new Point(12, 481);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(0, 15);
            lblCantidad.TabIndex = 13;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(828, 474);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 14;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmBitacora
            //
            AcceptButton = btnBuscar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(940, 516);
            Controls.Add(lblDesde);
            Controls.Add(dtpDesde);
            Controls.Add(lblHasta);
            Controls.Add(dtpHasta);
            Controls.Add(lblNivel);
            Controls.Add(cmbNivel);
            Controls.Add(lblUsuario);
            Controls.Add(txtUsuario);
            Controls.Add(lblTexto);
            Controls.Add(txtTexto);
            Controls.Add(btnBuscar);
            Controls.Add(btnLimpiar);
            Controls.Add(dgvBitacora);
            Controls.Add(lblCantidad);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmBitacora";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Bitácora";
            Load += FrmBitacora_Load;
            ((System.ComponentModel.ISupportInitialize)dgvBitacora).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Label lblNivel;
        private ComboBox cmbNivel;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblTexto;
        private TextBox txtTexto;
        private Button btnBuscar;
        private Button btnLimpiar;
        private DataGridView dgvBitacora;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colNivel;
        private DataGridViewTextBoxColumn colUsuario;
        private DataGridViewTextBoxColumn colMensaje;
        private DataGridViewTextBoxColumn colDetalle;
        private Label lblCantidad;
        private Button btnCerrar;
    }
}
