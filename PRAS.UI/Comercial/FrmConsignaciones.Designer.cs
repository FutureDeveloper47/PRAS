namespace PRAS.UI
{
    partial class FrmConsignaciones
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
            chkSoloVigentes = new CheckBox();
            dgvConsignaciones = new DataGridView();
            colFechaEntrega = new DataGridViewTextBoxColumn();
            colDominio = new DataGridViewTextBoxColumn();
            colVehiculo = new DataGridViewTextBoxColumn();
            colAgencia = new DataGridViewTextBoxColumn();
            colMinimo = new DataGridViewTextBoxColumn();
            colComision = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colRetiro = new DataGridViewTextBoxColumn();
            btnNuevo = new Button();
            btnRegistrarRetiro = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvConsignaciones).BeginInit();
            SuspendLayout();
            //
            // chkSoloVigentes
            //
            chkSoloVigentes.AutoSize = true;
            chkSoloVigentes.Checked = true;
            chkSoloVigentes.CheckState = CheckState.Checked;
            chkSoloVigentes.Location = new Point(12, 12);
            chkSoloVigentes.Name = "chkSoloVigentes";
            chkSoloVigentes.TabIndex = 0;
            chkSoloVigentes.Text = "Mostrar solo los autos que están en las agencias";
            chkSoloVigentes.UseVisualStyleBackColor = true;
            chkSoloVigentes.CheckedChanged += chkSoloVigentes_CheckedChanged;
            //
            // dgvConsignaciones
            //
            dgvConsignaciones.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            dgvConsignaciones.AllowUserToAddRows = false;
            dgvConsignaciones.AllowUserToDeleteRows = false;
            dgvConsignaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvConsignaciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvConsignaciones.Columns.AddRange(new DataGridViewColumn[] { colFechaEntrega, colDominio, colVehiculo, colAgencia, colMinimo, colComision, colEstado, colRetiro });
            dgvConsignaciones.MultiSelect = false;
            dgvConsignaciones.ReadOnly = true;
            dgvConsignaciones.RowHeadersVisible = false;
            dgvConsignaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvConsignaciones.Location = new Point(12, 40);
            dgvConsignaciones.Name = "dgvConsignaciones";
            dgvConsignaciones.Size = new Size(776, 428);
            dgvConsignaciones.TabIndex = 1;
            //
            // colFechaEntrega
            //
            colFechaEntrega.FillWeight = 60F;
            colFechaEntrega.HeaderText = "Entrega";
            colFechaEntrega.Name = "colFechaEntrega";
            colFechaEntrega.ReadOnly = true;
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
            // colAgencia
            //
            colAgencia.FillWeight = 120F;
            colAgencia.HeaderText = "Agencia";
            colAgencia.Name = "colAgencia";
            colAgencia.ReadOnly = true;
            //
            // colMinimo
            //
            colMinimo.FillWeight = 80F;
            colMinimo.HeaderText = "Precio mínimo";
            colMinimo.Name = "colMinimo";
            colMinimo.ReadOnly = true;
            //
            // colComision
            //
            colComision.FillWeight = 50F;
            colComision.HeaderText = "Comisión %";
            colComision.Name = "colComision";
            colComision.ReadOnly = true;
            //
            // colEstado
            //
            colEstado.FillWeight = 55F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // colRetiro
            //
            colRetiro.FillWeight = 65F;
            colRetiro.HeaderText = "Retiro / venta";
            colRetiro.Name = "colRetiro";
            colRetiro.ReadOnly = true;
            //
            // btnNuevo
            //
            btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevo.Location = new Point(800, 40);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(130, 30);
            btnNuevo.TabIndex = 2;
            btnNuevo.Text = "Nueva consignación";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnRegistrarRetiro
            //
            btnRegistrarRetiro.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRegistrarRetiro.Location = new Point(800, 80);
            btnRegistrarRetiro.Name = "btnRegistrarRetiro";
            btnRegistrarRetiro.Size = new Size(130, 30);
            btnRegistrarRetiro.TabIndex = 3;
            btnRegistrarRetiro.Text = "Registrar retiro";
            btnRegistrarRetiro.UseVisualStyleBackColor = true;
            btnRegistrarRetiro.Click += btnRegistrarRetiro_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(830, 438);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmConsignaciones
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(942, 480);
            Controls.Add(chkSoloVigentes);
            Controls.Add(dgvConsignaciones);
            Controls.Add(btnNuevo);
            Controls.Add(btnRegistrarRetiro);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmConsignaciones";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Consignaciones (autos en agencias)";
            Load += FrmConsignaciones_Load;
            ((System.ComponentModel.ISupportInitialize)dgvConsignaciones).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox chkSoloVigentes;
        private DataGridView dgvConsignaciones;
        private DataGridViewTextBoxColumn colFechaEntrega;
        private DataGridViewTextBoxColumn colDominio;
        private DataGridViewTextBoxColumn colVehiculo;
        private DataGridViewTextBoxColumn colAgencia;
        private DataGridViewTextBoxColumn colMinimo;
        private DataGridViewTextBoxColumn colComision;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colRetiro;
        private Button btnNuevo;
        private Button btnRegistrarRetiro;
        private Button btnCerrar;
    }
}
