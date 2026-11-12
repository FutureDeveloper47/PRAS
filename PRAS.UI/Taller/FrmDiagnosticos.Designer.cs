namespace PRAS.UI
{
    partial class FrmDiagnosticos
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
            chkMostrarTodos = new CheckBox();
            dgvDiagnosticos = new DataGridView();
            colDominio = new DataGridViewTextBoxColumn();
            colVehiculo = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colEstadoDiagnostico = new DataGridViewTextBoxColumn();
            colEstadoVehiculo = new DataGridViewTextBoxColumn();
            lblVehiculo = new Label();
            lblDanios = new Label();
            txtDanios = new TextBox();
            lblObservaciones = new Label();
            txtObservaciones = new TextBox();
            chkRequiereReparacion = new CheckBox();
            btnGuardar = new Button();
            lblEvidenciasTitulo = new Label();
            dgvEvidencias = new DataGridView();
            colTipo = new DataGridViewTextBoxColumn();
            colDescripcionEvidencia = new DataGridViewTextBoxColumn();
            colFechaCarga = new DataGridViewTextBoxColumn();
            colArchivo = new DataGridViewTextBoxColumn();
            txtDescripcionEvidencia = new TextBox();
            btnAgregarEvidencia = new Button();
            btnVerEvidencia = new Button();
            grpFinalizar = new GroupBox();
            lblMecanico = new Label();
            cboMecanico = new ComboBox();
            lblPrioridad = new Label();
            cboPrioridad = new ComboBox();
            btnRegistrarFinalizacion = new Button();
            btnCerrar = new Button();
            grpFinalizar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDiagnosticos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvEvidencias).BeginInit();
            SuspendLayout();
            //
            // chkMostrarTodos
            //
            chkMostrarTodos.AutoSize = true;
            chkMostrarTodos.Location = new Point(12, 12);
            chkMostrarTodos.Name = "chkMostrarTodos";
            chkMostrarTodos.TabIndex = 0;
            chkMostrarTodos.Text = "Mostrar también los autos que ya salieron del taller";
            chkMostrarTodos.UseVisualStyleBackColor = true;
            chkMostrarTodos.CheckedChanged += chkMostrarTodos_CheckedChanged;
            //
            // dgvDiagnosticos
            //
            dgvDiagnosticos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            dgvDiagnosticos.AllowUserToAddRows = false;
            dgvDiagnosticos.AllowUserToDeleteRows = false;
            dgvDiagnosticos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDiagnosticos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDiagnosticos.Columns.AddRange(new DataGridViewColumn[] { colDominio, colVehiculo, colFecha, colEstadoDiagnostico, colEstadoVehiculo });
            dgvDiagnosticos.MultiSelect = false;
            dgvDiagnosticos.ReadOnly = true;
            dgvDiagnosticos.RowHeadersVisible = false;
            dgvDiagnosticos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDiagnosticos.Location = new Point(12, 40);
            dgvDiagnosticos.Name = "dgvDiagnosticos";
            dgvDiagnosticos.Size = new Size(430, 540);
            dgvDiagnosticos.TabIndex = 1;
            dgvDiagnosticos.SelectionChanged += dgvDiagnosticos_SelectionChanged;
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
            // colFecha
            //
            colFecha.FillWeight = 60F;
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            //
            // colEstadoDiagnostico
            //
            colEstadoDiagnostico.FillWeight = 70F;
            colEstadoDiagnostico.HeaderText = "Diagnóstico";
            colEstadoDiagnostico.Name = "colEstadoDiagnostico";
            colEstadoDiagnostico.ReadOnly = true;
            //
            // colEstadoVehiculo
            //
            colEstadoVehiculo.FillWeight = 80F;
            colEstadoVehiculo.HeaderText = "Estado del auto";
            colEstadoVehiculo.Name = "colEstadoVehiculo";
            colEstadoVehiculo.ReadOnly = true;
            //
            // lblVehiculo
            //
            lblVehiculo.AutoSize = true;
            lblVehiculo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblVehiculo.Location = new Point(460, 42);
            lblVehiculo.Name = "lblVehiculo";
            lblVehiculo.TabIndex = 2;
            lblVehiculo.Text = "Seleccione un vehículo de la lista.";
            //
            // lblDanios
            //
            lblDanios.AutoSize = true;
            lblDanios.Location = new Point(460, 72);
            lblDanios.Name = "lblDanios";
            lblDanios.TabIndex = 3;
            lblDanios.Text = "Descripción de daños *:";
            //
            // txtDanios
            //
            txtDanios.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDanios.Multiline = true;
            txtDanios.ScrollBars = ScrollBars.Vertical;
            txtDanios.Location = new Point(460, 92);
            txtDanios.Name = "txtDanios";
            txtDanios.Size = new Size(520, 90);
            txtDanios.TabIndex = 4;
            //
            // lblObservaciones
            //
            lblObservaciones.AutoSize = true;
            lblObservaciones.Location = new Point(460, 190);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.TabIndex = 5;
            lblObservaciones.Text = "Observaciones:";
            //
            // txtObservaciones
            //
            txtObservaciones.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtObservaciones.Multiline = true;
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Location = new Point(460, 210);
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Size = new Size(520, 50);
            txtObservaciones.TabIndex = 6;
            //
            // chkRequiereReparacion
            //
            chkRequiereReparacion.AutoSize = true;
            chkRequiereReparacion.Checked = true;
            chkRequiereReparacion.CheckState = CheckState.Checked;
            chkRequiereReparacion.Location = new Point(460, 270);
            chkRequiereReparacion.Name = "chkRequiereReparacion";
            chkRequiereReparacion.TabIndex = 7;
            chkRequiereReparacion.Text = "Requiere reparación";
            chkRequiereReparacion.UseVisualStyleBackColor = true;
            //
            // btnGuardar
            //
            btnGuardar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGuardar.Location = new Point(840, 265);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(140, 28);
            btnGuardar.TabIndex = 8;
            btnGuardar.Text = "Guardar cambios";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // lblEvidenciasTitulo
            //
            lblEvidenciasTitulo.AutoSize = true;
            lblEvidenciasTitulo.Location = new Point(460, 305);
            lblEvidenciasTitulo.Name = "lblEvidenciasTitulo";
            lblEvidenciasTitulo.TabIndex = 9;
            lblEvidenciasTitulo.Text = "Evidencias (fotos, videos o documentos)";
            //
            // dgvEvidencias
            //
            dgvEvidencias.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvEvidencias.AllowUserToAddRows = false;
            dgvEvidencias.AllowUserToDeleteRows = false;
            dgvEvidencias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEvidencias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEvidencias.Columns.AddRange(new DataGridViewColumn[] { colTipo, colDescripcionEvidencia, colFechaCarga, colArchivo });
            dgvEvidencias.MultiSelect = false;
            dgvEvidencias.ReadOnly = true;
            dgvEvidencias.RowHeadersVisible = false;
            dgvEvidencias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEvidencias.Location = new Point(460, 325);
            dgvEvidencias.Name = "dgvEvidencias";
            dgvEvidencias.Size = new Size(520, 140);
            dgvEvidencias.TabIndex = 10;
            dgvEvidencias.CellDoubleClick += dgvEvidencias_CellDoubleClick;
            //
            // colTipo
            //
            colTipo.FillWeight = 50F;
            colTipo.HeaderText = "Tipo";
            colTipo.Name = "colTipo";
            colTipo.ReadOnly = true;
            //
            // colDescripcionEvidencia
            //
            colDescripcionEvidencia.FillWeight = 140F;
            colDescripcionEvidencia.HeaderText = "Descripción";
            colDescripcionEvidencia.Name = "colDescripcionEvidencia";
            colDescripcionEvidencia.ReadOnly = true;
            //
            // colFechaCarga
            //
            colFechaCarga.FillWeight = 60F;
            colFechaCarga.HeaderText = "Fecha";
            colFechaCarga.Name = "colFechaCarga";
            colFechaCarga.ReadOnly = true;
            //
            // colArchivo
            //
            colArchivo.FillWeight = 120F;
            colArchivo.HeaderText = "Archivo";
            colArchivo.Name = "colArchivo";
            colArchivo.ReadOnly = true;
            //
            // txtDescripcionEvidencia
            //
            txtDescripcionEvidencia.PlaceholderText = "Descripción de la evidencia (opcional)";
            txtDescripcionEvidencia.Location = new Point(460, 472);
            txtDescripcionEvidencia.Name = "txtDescripcionEvidencia";
            txtDescripcionEvidencia.Size = new Size(250, 23);
            txtDescripcionEvidencia.TabIndex = 11;
            //
            // btnAgregarEvidencia
            //
            btnAgregarEvidencia.Location = new Point(716, 471);
            btnAgregarEvidencia.Name = "btnAgregarEvidencia";
            btnAgregarEvidencia.Size = new Size(140, 27);
            btnAgregarEvidencia.TabIndex = 12;
            btnAgregarEvidencia.Text = "Agregar evidencia...";
            btnAgregarEvidencia.UseVisualStyleBackColor = true;
            btnAgregarEvidencia.Click += btnAgregarEvidencia_Click;
            //
            // btnVerEvidencia
            //
            btnVerEvidencia.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnVerEvidencia.Location = new Point(862, 471);
            btnVerEvidencia.Name = "btnVerEvidencia";
            btnVerEvidencia.Size = new Size(118, 27);
            btnVerEvidencia.TabIndex = 13;
            btnVerEvidencia.Text = "Abrir evidencia";
            btnVerEvidencia.UseVisualStyleBackColor = true;
            btnVerEvidencia.Click += btnVerEvidencia_Click;
            //
            // grpFinalizar
            //
            grpFinalizar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpFinalizar.Controls.Add(lblMecanico);
            grpFinalizar.Controls.Add(cboMecanico);
            grpFinalizar.Controls.Add(lblPrioridad);
            grpFinalizar.Controls.Add(cboPrioridad);
            grpFinalizar.Controls.Add(btnRegistrarFinalizacion);
            grpFinalizar.Location = new Point(460, 510);
            grpFinalizar.Name = "grpFinalizar";
            grpFinalizar.Size = new Size(520, 70);
            grpFinalizar.TabIndex = 14;
            grpFinalizar.Text = "Finalizar diagnóstico";
            //
            // lblMecanico
            //
            lblMecanico.AutoSize = true;
            lblMecanico.Location = new Point(10, 30);
            lblMecanico.Name = "lblMecanico";
            lblMecanico.TabIndex = 0;
            lblMecanico.Text = "Mecánico:";
            //
            // cboMecanico
            //
            cboMecanico.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMecanico.FormattingEnabled = true;
            cboMecanico.Location = new Point(75, 26);
            cboMecanico.Name = "cboMecanico";
            cboMecanico.Size = new Size(190, 23);
            cboMecanico.TabIndex = 1;
            //
            // lblPrioridad
            //
            lblPrioridad.AutoSize = true;
            lblPrioridad.Location = new Point(275, 30);
            lblPrioridad.Name = "lblPrioridad";
            lblPrioridad.TabIndex = 2;
            lblPrioridad.Text = "Prioridad:";
            //
            // cboPrioridad
            //
            cboPrioridad.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPrioridad.FormattingEnabled = true;
            cboPrioridad.Location = new Point(340, 26);
            cboPrioridad.Name = "cboPrioridad";
            cboPrioridad.Size = new Size(75, 23);
            cboPrioridad.TabIndex = 3;
            //
            // btnRegistrarFinalizacion
            //
            btnRegistrarFinalizacion.Location = new Point(425, 24);
            btnRegistrarFinalizacion.Name = "btnRegistrarFinalizacion";
            btnRegistrarFinalizacion.Size = new Size(85, 28);
            btnRegistrarFinalizacion.TabIndex = 4;
            btnRegistrarFinalizacion.Text = "Finalizar";
            btnRegistrarFinalizacion.UseVisualStyleBackColor = true;
            btnRegistrarFinalizacion.Click += btnRegistrarFinalizacion_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(880, 598);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 15;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmDiagnosticos
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1000, 640);
            Controls.Add(chkMostrarTodos);
            Controls.Add(dgvDiagnosticos);
            Controls.Add(lblVehiculo);
            Controls.Add(lblDanios);
            Controls.Add(txtDanios);
            Controls.Add(lblObservaciones);
            Controls.Add(txtObservaciones);
            Controls.Add(chkRequiereReparacion);
            Controls.Add(btnGuardar);
            Controls.Add(lblEvidenciasTitulo);
            Controls.Add(dgvEvidencias);
            Controls.Add(txtDescripcionEvidencia);
            Controls.Add(btnAgregarEvidencia);
            Controls.Add(btnVerEvidencia);
            Controls.Add(grpFinalizar);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmDiagnosticos";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar diagnóstico";
            Load += FrmDiagnosticos_Load;
            ((System.ComponentModel.ISupportInitialize)dgvDiagnosticos).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvEvidencias).EndInit();
            grpFinalizar.ResumeLayout(false);
            grpFinalizar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox chkMostrarTodos;
        private DataGridView dgvDiagnosticos;
        private DataGridViewTextBoxColumn colDominio;
        private DataGridViewTextBoxColumn colVehiculo;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colEstadoDiagnostico;
        private DataGridViewTextBoxColumn colEstadoVehiculo;
        private Label lblVehiculo;
        private Label lblDanios;
        private TextBox txtDanios;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private CheckBox chkRequiereReparacion;
        private Button btnGuardar;
        private Label lblEvidenciasTitulo;
        private DataGridView dgvEvidencias;
        private DataGridViewTextBoxColumn colTipo;
        private DataGridViewTextBoxColumn colDescripcionEvidencia;
        private DataGridViewTextBoxColumn colFechaCarga;
        private DataGridViewTextBoxColumn colArchivo;
        private TextBox txtDescripcionEvidencia;
        private Button btnAgregarEvidencia;
        private Button btnVerEvidencia;
        private GroupBox grpFinalizar;
        private Label lblMecanico;
        private ComboBox cboMecanico;
        private Label lblPrioridad;
        private ComboBox cboPrioridad;
        private Button btnRegistrarFinalizacion;
        private Button btnCerrar;
    }
}
