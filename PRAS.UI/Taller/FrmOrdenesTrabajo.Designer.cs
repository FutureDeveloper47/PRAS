namespace PRAS.UI
{
    partial class FrmOrdenesTrabajo
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
            dgvOrdenes = new DataGridView();
            colOT = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colDominio = new DataGridViewTextBoxColumn();
            colVehiculo = new DataGridViewTextBoxColumn();
            colPrioridad = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colMecanicos = new DataGridViewTextBoxColumn();
            colPresupuesto = new DataGridViewTextBoxColumn();
            lblTareasTitulo = new Label();
            dgvTareas = new DataGridView();
            colTarea = new DataGridViewTextBoxColumn();
            colMecanico = new DataGridViewTextBoxColumn();
            colDescripcion = new DataGridViewTextBoxColumn();
            colCosto = new DataGridViewTextBoxColumn();
            colEstadoTarea = new DataGridViewTextBoxColumn();
            colInicio = new DataGridViewTextBoxColumn();
            colFin = new DataGridViewTextBoxColumn();
            grpGestion = new GroupBox();
            lblNuevaPrioridad = new Label();
            cboPrioridad = new ComboBox();
            btnCambiarPrioridad = new Button();
            btnRegistrarFinOrden = new Button();
            lblMecanicoTarea = new Label();
            cboMecanico = new ComboBox();
            lblDescripcionTarea = new Label();
            txtDescripcionTarea = new TextBox();
            lblCosto = new Label();
            numCosto = new NumericUpDown();
            btnAgregarTarea = new Button();
            btnCerrar = new Button();
            grpGestion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvTareas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCosto).BeginInit();
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
            cboEstado.Size = new Size(120, 23);
            cboEstado.TabIndex = 1;
            cboEstado.SelectedIndexChanged += cboEstado_SelectedIndexChanged;
            //
            // btnActualizar
            //
            btnActualizar.Location = new Point(195, 10);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(90, 25);
            btnActualizar.TabIndex = 2;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            //
            // dgvOrdenes
            //
            dgvOrdenes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrdenes.AllowUserToAddRows = false;
            dgvOrdenes.AllowUserToDeleteRows = false;
            dgvOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrdenes.Columns.AddRange(new DataGridViewColumn[] { colOT, colFecha, colDominio, colVehiculo, colPrioridad, colEstado, colMecanicos, colPresupuesto });
            dgvOrdenes.MultiSelect = false;
            dgvOrdenes.ReadOnly = true;
            dgvOrdenes.RowHeadersVisible = false;
            dgvOrdenes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrdenes.Location = new Point(12, 45);
            dgvOrdenes.Name = "dgvOrdenes";
            dgvOrdenes.Size = new Size(976, 230);
            dgvOrdenes.TabIndex = 3;
            dgvOrdenes.SelectionChanged += dgvOrdenes_SelectionChanged;
            //
            // colOT
            //
            colOT.FillWeight = 35F;
            colOT.HeaderText = "OT";
            colOT.Name = "colOT";
            colOT.ReadOnly = true;
            //
            // colFecha
            //
            colFecha.FillWeight = 60F;
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
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
            // colPrioridad
            //
            colPrioridad.FillWeight = 50F;
            colPrioridad.HeaderText = "Prioridad";
            colPrioridad.Name = "colPrioridad";
            colPrioridad.ReadOnly = true;
            //
            // colEstado
            //
            colEstado.FillWeight = 60F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // colMecanicos
            //
            colMecanicos.FillWeight = 130F;
            colMecanicos.HeaderText = "Mecánicos";
            colMecanicos.Name = "colMecanicos";
            colMecanicos.ReadOnly = true;
            //
            // colPresupuesto
            //
            colPresupuesto.FillWeight = 70F;
            colPresupuesto.HeaderText = "Presupuesto";
            colPresupuesto.Name = "colPresupuesto";
            colPresupuesto.ReadOnly = true;
            //
            // lblTareasTitulo
            //
            lblTareasTitulo.AutoSize = true;
            lblTareasTitulo.Location = new Point(12, 285);
            lblTareasTitulo.Name = "lblTareasTitulo";
            lblTareasTitulo.TabIndex = 4;
            lblTareasTitulo.Text = "Tareas de la orden seleccionada";
            //
            // dgvTareas
            //
            dgvTareas.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            dgvTareas.AllowUserToAddRows = false;
            dgvTareas.AllowUserToDeleteRows = false;
            dgvTareas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTareas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTareas.Columns.AddRange(new DataGridViewColumn[] { colTarea, colMecanico, colDescripcion, colCosto, colEstadoTarea, colInicio, colFin });
            dgvTareas.MultiSelect = false;
            dgvTareas.ReadOnly = true;
            dgvTareas.RowHeadersVisible = false;
            dgvTareas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTareas.Location = new Point(12, 305);
            dgvTareas.Name = "dgvTareas";
            dgvTareas.Size = new Size(976, 170);
            dgvTareas.TabIndex = 5;
            //
            // colTarea
            //
            colTarea.FillWeight = 30F;
            colTarea.HeaderText = "N°";
            colTarea.Name = "colTarea";
            colTarea.ReadOnly = true;
            //
            // colMecanico
            //
            colMecanico.FillWeight = 90F;
            colMecanico.HeaderText = "Mecánico";
            colMecanico.Name = "colMecanico";
            colMecanico.ReadOnly = true;
            //
            // colDescripcion
            //
            colDescripcion.FillWeight = 220F;
            colDescripcion.HeaderText = "Tarea";
            colDescripcion.Name = "colDescripcion";
            colDescripcion.ReadOnly = true;
            //
            // colCosto
            //
            colCosto.FillWeight = 70F;
            colCosto.HeaderText = "Costo estimado";
            colCosto.Name = "colCosto";
            colCosto.ReadOnly = true;
            //
            // colEstadoTarea
            //
            colEstadoTarea.FillWeight = 60F;
            colEstadoTarea.HeaderText = "Estado";
            colEstadoTarea.Name = "colEstadoTarea";
            colEstadoTarea.ReadOnly = true;
            //
            // colInicio
            //
            colInicio.FillWeight = 60F;
            colInicio.HeaderText = "Inicio";
            colInicio.Name = "colInicio";
            colInicio.ReadOnly = true;
            //
            // colFin
            //
            colFin.FillWeight = 60F;
            colFin.HeaderText = "Fin";
            colFin.Name = "colFin";
            colFin.ReadOnly = true;
            //
            // grpGestion
            //
            grpGestion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpGestion.Controls.Add(lblNuevaPrioridad);
            grpGestion.Controls.Add(cboPrioridad);
            grpGestion.Controls.Add(btnCambiarPrioridad);
            grpGestion.Controls.Add(btnRegistrarFinOrden);
            grpGestion.Controls.Add(lblMecanicoTarea);
            grpGestion.Controls.Add(cboMecanico);
            grpGestion.Controls.Add(lblDescripcionTarea);
            grpGestion.Controls.Add(txtDescripcionTarea);
            grpGestion.Controls.Add(lblCosto);
            grpGestion.Controls.Add(numCosto);
            grpGestion.Controls.Add(btnAgregarTarea);
            grpGestion.Location = new Point(12, 485);
            grpGestion.Name = "grpGestion";
            grpGestion.Size = new Size(976, 105);
            grpGestion.TabIndex = 6;
            grpGestion.Text = "Gestión de la orden seleccionada";
            //
            // lblNuevaPrioridad
            //
            lblNuevaPrioridad.AutoSize = true;
            lblNuevaPrioridad.Location = new Point(10, 29);
            lblNuevaPrioridad.Name = "lblNuevaPrioridad";
            lblNuevaPrioridad.TabIndex = 0;
            lblNuevaPrioridad.Text = "Prioridad:";
            //
            // cboPrioridad
            //
            cboPrioridad.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPrioridad.FormattingEnabled = true;
            cboPrioridad.Location = new Point(80, 25);
            cboPrioridad.Name = "cboPrioridad";
            cboPrioridad.Size = new Size(80, 23);
            cboPrioridad.TabIndex = 1;
            //
            // btnCambiarPrioridad
            //
            btnCambiarPrioridad.Location = new Point(170, 23);
            btnCambiarPrioridad.Name = "btnCambiarPrioridad";
            btnCambiarPrioridad.Size = new Size(130, 27);
            btnCambiarPrioridad.TabIndex = 2;
            btnCambiarPrioridad.Text = "Cambiar prioridad";
            btnCambiarPrioridad.UseVisualStyleBackColor = true;
            btnCambiarPrioridad.Click += btnCambiarPrioridad_Click;
            //
            // btnRegistrarFinOrden
            //
            btnRegistrarFinOrden.Location = new Point(310, 23);
            btnRegistrarFinOrden.Name = "btnRegistrarFinOrden";
            btnRegistrarFinOrden.Size = new Size(230, 27);
            btnRegistrarFinOrden.TabIndex = 3;
            btnRegistrarFinOrden.Text = "Finalizar orden (auto reparado)";
            btnRegistrarFinOrden.UseVisualStyleBackColor = true;
            btnRegistrarFinOrden.Click += btnRegistrarFinOrden_Click;
            //
            // lblMecanicoTarea
            //
            lblMecanicoTarea.AutoSize = true;
            lblMecanicoTarea.Location = new Point(10, 69);
            lblMecanicoTarea.Name = "lblMecanicoTarea";
            lblMecanicoTarea.TabIndex = 4;
            lblMecanicoTarea.Text = "Mecánico:";
            //
            // cboMecanico
            //
            cboMecanico.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMecanico.FormattingEnabled = true;
            cboMecanico.Location = new Point(80, 65);
            cboMecanico.Name = "cboMecanico";
            cboMecanico.Size = new Size(180, 23);
            cboMecanico.TabIndex = 5;
            //
            // lblDescripcionTarea
            //
            lblDescripcionTarea.AutoSize = true;
            lblDescripcionTarea.Location = new Point(270, 69);
            lblDescripcionTarea.Name = "lblDescripcionTarea";
            lblDescripcionTarea.TabIndex = 6;
            lblDescripcionTarea.Text = "Tarea:";
            //
            // txtDescripcionTarea
            //
            txtDescripcionTarea.Location = new Point(315, 65);
            txtDescripcionTarea.Name = "txtDescripcionTarea";
            txtDescripcionTarea.Size = new Size(330, 23);
            txtDescripcionTarea.TabIndex = 7;
            //
            // lblCosto
            //
            lblCosto.AutoSize = true;
            lblCosto.Location = new Point(655, 69);
            lblCosto.Name = "lblCosto";
            lblCosto.TabIndex = 8;
            lblCosto.Text = "Costo estimado:";
            //
            // numCosto
            //
            numCosto.DecimalPlaces = 2;
            numCosto.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numCosto.ThousandsSeparator = true;
            numCosto.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            numCosto.Location = new Point(750, 65);
            numCosto.Name = "numCosto";
            numCosto.Size = new Size(100, 23);
            numCosto.TabIndex = 9;
            //
            // btnAgregarTarea
            //
            btnAgregarTarea.Location = new Point(860, 63);
            btnAgregarTarea.Name = "btnAgregarTarea";
            btnAgregarTarea.Size = new Size(105, 27);
            btnAgregarTarea.TabIndex = 10;
            btnAgregarTarea.Text = "Agregar tarea";
            btnAgregarTarea.UseVisualStyleBackColor = true;
            btnAgregarTarea.Click += btnAgregarTarea_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(888, 600);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 7;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmOrdenesTrabajo
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1000, 642);
            Controls.Add(lblEstado);
            Controls.Add(cboEstado);
            Controls.Add(btnActualizar);
            Controls.Add(dgvOrdenes);
            Controls.Add(lblTareasTitulo);
            Controls.Add(dgvTareas);
            Controls.Add(grpGestion);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmOrdenesTrabajo";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Órdenes de trabajo";
            Load += FrmOrdenesTrabajo_Load;
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvTareas).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCosto).EndInit();
            grpGestion.ResumeLayout(false);
            grpGestion.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblEstado;
        private ComboBox cboEstado;
        private Button btnActualizar;
        private DataGridView dgvOrdenes;
        private DataGridViewTextBoxColumn colOT;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colDominio;
        private DataGridViewTextBoxColumn colVehiculo;
        private DataGridViewTextBoxColumn colPrioridad;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colMecanicos;
        private DataGridViewTextBoxColumn colPresupuesto;
        private Label lblTareasTitulo;
        private DataGridView dgvTareas;
        private DataGridViewTextBoxColumn colTarea;
        private DataGridViewTextBoxColumn colMecanico;
        private DataGridViewTextBoxColumn colDescripcion;
        private DataGridViewTextBoxColumn colCosto;
        private DataGridViewTextBoxColumn colEstadoTarea;
        private DataGridViewTextBoxColumn colInicio;
        private DataGridViewTextBoxColumn colFin;
        private GroupBox grpGestion;
        private Label lblNuevaPrioridad;
        private ComboBox cboPrioridad;
        private Button btnCambiarPrioridad;
        private Button btnRegistrarFinOrden;
        private Label lblMecanicoTarea;
        private ComboBox cboMecanico;
        private Label lblDescripcionTarea;
        private TextBox txtDescripcionTarea;
        private Label lblCosto;
        private NumericUpDown numCosto;
        private Button btnAgregarTarea;
        private Button btnCerrar;
    }
}
