namespace PRAS.UI
{
    partial class FrmReporte
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
            btnGenerar = new Button();
            btnExportar = new Button();
            dgvReporte = new DataGridView();
            lblTotales = new Label();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
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
            // btnGenerar
            //
            btnGenerar.Location = new Point(342, 10);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(100, 25);
            btnGenerar.TabIndex = 4;
            btnGenerar.Text = "Generar";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            //
            // btnExportar
            //
            btnExportar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportar.Location = new Point(848, 10);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(140, 25);
            btnExportar.TabIndex = 5;
            btnExportar.Text = "Exportar...";
            btnExportar.UseVisualStyleBackColor = true;
            btnExportar.Click += btnExportar_Click;
            //
            // dgvReporte
            //
            dgvReporte.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            dgvReporte.AllowUserToAddRows = false;
            dgvReporte.AllowUserToDeleteRows = false;
            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReporte.MultiSelect = false;
            dgvReporte.ReadOnly = true;
            dgvReporte.RowHeadersVisible = false;
            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.Location = new Point(12, 45);
            dgvReporte.Name = "dgvReporte";
            dgvReporte.Size = new Size(976, 395);
            dgvReporte.TabIndex = 6;
            //
            // lblTotales
            //
            lblTotales.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblTotales.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotales.Location = new Point(12, 448);
            lblTotales.Name = "lblTotales";
            lblTotales.Size = new Size(860, 40);
            lblTotales.TabIndex = 7;
            lblTotales.Text = "";
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(888, 458);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 8;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmReporte
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1000, 500);
            Controls.Add(lblDesde);
            Controls.Add(dtpDesde);
            Controls.Add(lblHasta);
            Controls.Add(dtpHasta);
            Controls.Add(btnGenerar);
            Controls.Add(btnExportar);
            Controls.Add(dgvReporte);
            Controls.Add(lblTotales);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmReporte";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reporte";
            Load += FrmReporte_Load;
            ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnGenerar;
        private Button btnExportar;
        private DataGridView dgvReporte;
        private Label lblTotales;
        private Button btnCerrar;
    }
}
