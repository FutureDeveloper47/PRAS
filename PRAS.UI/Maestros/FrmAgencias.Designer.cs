namespace PRAS.UI
{
    partial class FrmAgencias
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
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            chkMostrarDeshabilitadas = new CheckBox();
            dgvAgencias = new DataGridView();
            colRazonSocial = new DataGridViewTextBoxColumn();
            colCUIT = new DataGridViewTextBoxColumn();
            colLocalidad = new DataGridViewTextBoxColumn();
            colTelefono = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colActivo = new DataGridViewCheckBoxColumn();
            btnNuevo = new Button();
            btnModificar = new Button();
            btnDeshabilitar = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvAgencias).BeginInit();
            SuspendLayout();
            //
            // lblBuscar
            //
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(12, 15);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(45, 15);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar:";
            //
            // txtBuscar
            //
            txtBuscar.Location = new Point(65, 12);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Razón social, CUIT o localidad";
            txtBuscar.Size = new Size(250, 23);
            txtBuscar.TabIndex = 1;
            //
            // btnBuscar
            //
            btnBuscar.Location = new Point(321, 11);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(80, 25);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // chkMostrarDeshabilitadas
            //
            chkMostrarDeshabilitadas.AutoSize = true;
            chkMostrarDeshabilitadas.Location = new Point(420, 14);
            chkMostrarDeshabilitadas.Name = "chkMostrarDeshabilitadas";
            chkMostrarDeshabilitadas.Size = new Size(152, 19);
            chkMostrarDeshabilitadas.TabIndex = 3;
            chkMostrarDeshabilitadas.Text = "Mostrar deshabilitadas";
            chkMostrarDeshabilitadas.UseVisualStyleBackColor = true;
            chkMostrarDeshabilitadas.CheckedChanged += chkMostrarDeshabilitadas_CheckedChanged;
            //
            // dgvAgencias
            //
            dgvAgencias.AllowUserToAddRows = false;
            dgvAgencias.AllowUserToDeleteRows = false;
            dgvAgencias.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAgencias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAgencias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAgencias.Columns.AddRange(new DataGridViewColumn[] { colRazonSocial, colCUIT, colLocalidad, colTelefono, colEmail, colActivo });
            dgvAgencias.Location = new Point(12, 45);
            dgvAgencias.MultiSelect = false;
            dgvAgencias.Name = "dgvAgencias";
            dgvAgencias.ReadOnly = true;
            dgvAgencias.RowHeadersVisible = false;
            dgvAgencias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAgencias.Size = new Size(760, 390);
            dgvAgencias.TabIndex = 4;
            dgvAgencias.CellDoubleClick += dgvAgencias_CellDoubleClick;
            dgvAgencias.SelectionChanged += dgvAgencias_SelectionChanged;
            //
            // colRazonSocial
            //
            colRazonSocial.DataPropertyName = "RazonSocial";
            colRazonSocial.FillWeight = 140F;
            colRazonSocial.HeaderText = "Razón social";
            colRazonSocial.Name = "colRazonSocial";
            colRazonSocial.ReadOnly = true;
            //
            // colCUIT
            //
            colCUIT.DataPropertyName = "CUIT";
            colCUIT.FillWeight = 80F;
            colCUIT.HeaderText = "CUIT";
            colCUIT.Name = "colCUIT";
            colCUIT.ReadOnly = true;
            //
            // colLocalidad
            //
            colLocalidad.DataPropertyName = "Localidad";
            colLocalidad.HeaderText = "Localidad";
            colLocalidad.Name = "colLocalidad";
            colLocalidad.ReadOnly = true;
            //
            // colTelefono
            //
            colTelefono.DataPropertyName = "Telefono";
            colTelefono.FillWeight = 80F;
            colTelefono.HeaderText = "Teléfono";
            colTelefono.Name = "colTelefono";
            colTelefono.ReadOnly = true;
            //
            // colEmail
            //
            colEmail.DataPropertyName = "Email";
            colEmail.FillWeight = 120F;
            colEmail.HeaderText = "Email";
            colEmail.Name = "colEmail";
            colEmail.ReadOnly = true;
            //
            // colActivo
            //
            colActivo.DataPropertyName = "Activo";
            colActivo.FillWeight = 45F;
            colActivo.HeaderText = "Activa";
            colActivo.Name = "colActivo";
            colActivo.ReadOnly = true;
            //
            // btnNuevo
            //
            btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevo.Location = new Point(784, 45);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(100, 30);
            btnNuevo.TabIndex = 5;
            btnNuevo.Text = "Nueva";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnModificar
            //
            btnModificar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnModificar.Location = new Point(784, 81);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(100, 30);
            btnModificar.TabIndex = 6;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            //
            // btnDeshabilitar
            //
            btnDeshabilitar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDeshabilitar.Location = new Point(784, 117);
            btnDeshabilitar.Name = "btnDeshabilitar";
            btnDeshabilitar.Size = new Size(100, 30);
            btnDeshabilitar.TabIndex = 7;
            btnDeshabilitar.Text = "Deshabilitar";
            btnDeshabilitar.UseVisualStyleBackColor = true;
            btnDeshabilitar.Click += btnDeshabilitar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(784, 405);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 8;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmAgencias
            //
            AcceptButton = btnBuscar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(896, 447);
            Controls.Add(lblBuscar);
            Controls.Add(txtBuscar);
            Controls.Add(btnBuscar);
            Controls.Add(chkMostrarDeshabilitadas);
            Controls.Add(dgvAgencias);
            Controls.Add(btnNuevo);
            Controls.Add(btnModificar);
            Controls.Add(btnDeshabilitar);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmAgencias";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Agencias";
            Load += FrmAgencias_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAgencias).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBuscar;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private CheckBox chkMostrarDeshabilitadas;
        private DataGridView dgvAgencias;
        private DataGridViewTextBoxColumn colRazonSocial;
        private DataGridViewTextBoxColumn colCUIT;
        private DataGridViewTextBoxColumn colLocalidad;
        private DataGridViewTextBoxColumn colTelefono;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewCheckBoxColumn colActivo;
        private Button btnNuevo;
        private Button btnModificar;
        private Button btnDeshabilitar;
        private Button btnCerrar;
    }
}
