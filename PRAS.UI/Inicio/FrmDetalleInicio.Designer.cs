namespace PRAS.UI
{
    partial class FrmDetalleInicio
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
            DataGridViewCellStyle estiloMonto = new DataGridViewCellStyle();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            dgvDetalle = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colDominio = new DataGridViewTextBoxColumn();
            colVehiculo = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colPersona = new DataGridViewTextBoxColumn();
            colMonto = new DataGridViewTextBoxColumn();
            colExtra = new DataGridViewTextBoxColumn();
            lblTotal = new Label();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            SuspendLayout();
            //
            // lblTitulo  (el Tema lo convierte en la franja azul de arriba)
            //
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(920, 56);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "DETALLE";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblSubtitulo
            //
            lblSubtitulo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSubtitulo.Font = new Font("Segoe UI", 9.75F);
            lblSubtitulo.ForeColor = SystemColors.GrayText;
            lblSubtitulo.Location = new Point(20, 68);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(880, 22);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "";
            //
            // dgvDetalle
            //
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.AllowUserToResizeRows = false;
            dgvDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.Columns.AddRange(new DataGridViewColumn[] { colFecha, colDominio, colVehiculo, colEstado, colPersona, colMonto, colExtra });
            dgvDetalle.Location = new Point(20, 98);
            dgvDetalle.MultiSelect = false;
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.ReadOnly = true;
            dgvDetalle.RowHeadersVisible = false;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.Size = new Size(880, 400);
            dgvDetalle.TabIndex = 2;
            dgvDetalle.DataBindingComplete += dgvDetalle_DataBindingComplete;
            //
            // colFecha
            //
            colFecha.DataPropertyName = "Fecha";
            estiloFecha.Format = "dd/MM/yyyy";
            colFecha.DefaultCellStyle = estiloFecha;
            colFecha.FillWeight = 85F;
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colDominio
            //
            colDominio.DataPropertyName = "Dominio";
            colDominio.FillWeight = 65F;
            colDominio.HeaderText = "Patente";
            colDominio.Name = "colDominio";
            colDominio.ReadOnly = true;
            //
            // colVehiculo
            //
            colVehiculo.DataPropertyName = "Vehiculo";
            colVehiculo.FillWeight = 145F;
            colVehiculo.HeaderText = "Vehículo";
            colVehiculo.Name = "colVehiculo";
            colVehiculo.ReadOnly = true;
            //
            // colEstado
            //
            colEstado.DataPropertyName = "Estado";
            colEstado.FillWeight = 90F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // colPersona
            //
            colPersona.DataPropertyName = "Persona";
            colPersona.FillWeight = 110F;
            colPersona.HeaderText = "Persona";
            colPersona.Name = "colPersona";
            colPersona.ReadOnly = true;
            //
            // colMonto
            //
            colMonto.DataPropertyName = "Monto";
            estiloMonto.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloMonto.Format = "$ #,##0.00";
            colMonto.DefaultCellStyle = estiloMonto;
            colMonto.FillWeight = 95F;
            colMonto.HeaderText = "Monto";
            colMonto.Name = "colMonto";
            colMonto.ReadOnly = true;
            //
            // colExtra
            //
            colExtra.DataPropertyName = "Extra";
            colExtra.FillWeight = 105F;
            colExtra.HeaderText = "Detalle";
            colExtra.Name = "colExtra";
            colExtra.ReadOnly = true;
            //
            // lblTotal
            //
            lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblTotal.Location = new Point(20, 522);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(40, 17);
            lblTotal.TabIndex = 3;
            lblTotal.Text = "Total";
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.Location = new Point(790, 512);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 36);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmDetalleInicio
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(920, 564);
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(dgvDetalle);
            Controls.Add(lblTotal);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            MinimumSize = new Size(760, 450);
            Name = "FrmDetalleInicio";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Detalle";
            Load += FrmDetalleInicio_Load;
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblSubtitulo;
        private DataGridView dgvDetalle;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colDominio;
        private DataGridViewTextBoxColumn colVehiculo;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colPersona;
        private DataGridViewTextBoxColumn colMonto;
        private DataGridViewTextBoxColumn colExtra;
        private Label lblTotal;
        private Button btnCerrar;
    }
}
