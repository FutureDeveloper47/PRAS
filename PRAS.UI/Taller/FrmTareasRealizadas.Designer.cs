namespace PRAS.UI
{
    partial class FrmTareasRealizadas
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
            chkMostrarFinalizadas = new CheckBox();
            btnActualizar = new Button();
            dgvTareas = new DataGridView();
            colPrioridad = new DataGridViewTextBoxColumn();
            colOT = new DataGridViewTextBoxColumn();
            colDominio = new DataGridViewTextBoxColumn();
            colVehiculo = new DataGridViewTextBoxColumn();
            colMecanico = new DataGridViewTextBoxColumn();
            colDescripcion = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colInicio = new DataGridViewTextBoxColumn();
            colFin = new DataGridViewTextBoxColumn();
            btnIniciarTarea = new Button();
            btnRegistrarFinalizada = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvTareas).BeginInit();
            SuspendLayout();
            //
            // chkMostrarFinalizadas
            //
            chkMostrarFinalizadas.AutoSize = true;
            chkMostrarFinalizadas.Location = new Point(12, 12);
            chkMostrarFinalizadas.Name = "chkMostrarFinalizadas";
            chkMostrarFinalizadas.TabIndex = 0;
            chkMostrarFinalizadas.Text = "Mostrar también las tareas finalizadas";
            chkMostrarFinalizadas.UseVisualStyleBackColor = true;
            chkMostrarFinalizadas.CheckedChanged += chkMostrarFinalizadas_CheckedChanged;
            //
            // btnActualizar
            //
            btnActualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnActualizar.Location = new Point(848, 8);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(100, 25);
            btnActualizar.TabIndex = 1;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            //
            // dgvTareas
            //
            dgvTareas.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            dgvTareas.AllowUserToAddRows = false;
            dgvTareas.AllowUserToDeleteRows = false;
            dgvTareas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTareas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTareas.Columns.AddRange(new DataGridViewColumn[] { colPrioridad, colOT, colDominio, colVehiculo, colMecanico, colDescripcion, colEstado, colInicio, colFin });
            dgvTareas.MultiSelect = false;
            dgvTareas.ReadOnly = true;
            dgvTareas.RowHeadersVisible = false;
            dgvTareas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTareas.Location = new Point(12, 40);
            dgvTareas.Name = "dgvTareas";
            dgvTareas.Size = new Size(936, 400);
            dgvTareas.TabIndex = 2;
            dgvTareas.SelectionChanged += dgvTareas_SelectionChanged;
            //
            // colPrioridad
            //
            colPrioridad.FillWeight = 50F;
            colPrioridad.HeaderText = "Prioridad";
            colPrioridad.Name = "colPrioridad";
            colPrioridad.ReadOnly = true;
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
            // colVehiculo
            //
            colVehiculo.FillWeight = 110F;
            colVehiculo.HeaderText = "Vehículo";
            colVehiculo.Name = "colVehiculo";
            colVehiculo.ReadOnly = true;
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
            colDescripcion.FillWeight = 200F;
            colDescripcion.HeaderText = "Tarea";
            colDescripcion.Name = "colDescripcion";
            colDescripcion.ReadOnly = true;
            //
            // colEstado
            //
            colEstado.FillWeight = 60F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // colInicio
            //
            colInicio.FillWeight = 70F;
            colInicio.HeaderText = "Inicio";
            colInicio.Name = "colInicio";
            colInicio.ReadOnly = true;
            //
            // colFin
            //
            colFin.FillWeight = 70F;
            colFin.HeaderText = "Fin";
            colFin.Name = "colFin";
            colFin.ReadOnly = true;
            //
            // btnIniciarTarea
            //
            btnIniciarTarea.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnIniciarTarea.Location = new Point(12, 455);
            btnIniciarTarea.Name = "btnIniciarTarea";
            btnIniciarTarea.Size = new Size(150, 32);
            btnIniciarTarea.TabIndex = 3;
            btnIniciarTarea.Text = "Iniciar tarea";
            btnIniciarTarea.UseVisualStyleBackColor = true;
            btnIniciarTarea.Click += btnIniciarTarea_Click;
            //
            // btnRegistrarFinalizada
            //
            btnRegistrarFinalizada.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRegistrarFinalizada.Location = new Point(170, 455);
            btnRegistrarFinalizada.Name = "btnRegistrarFinalizada";
            btnRegistrarFinalizada.Size = new Size(200, 32);
            btnRegistrarFinalizada.TabIndex = 4;
            btnRegistrarFinalizada.Text = "Registrar como finalizada";
            btnRegistrarFinalizada.UseVisualStyleBackColor = true;
            btnRegistrarFinalizada.Click += btnRegistrarFinalizada_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(848, 455);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 5;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmTareasRealizadas
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(960, 500);
            Controls.Add(chkMostrarFinalizadas);
            Controls.Add(btnActualizar);
            Controls.Add(dgvTareas);
            Controls.Add(btnIniciarTarea);
            Controls.Add(btnRegistrarFinalizada);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmTareasRealizadas";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar tareas realizadas";
            Load += FrmTareasRealizadas_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTareas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox chkMostrarFinalizadas;
        private Button btnActualizar;
        private DataGridView dgvTareas;
        private DataGridViewTextBoxColumn colPrioridad;
        private DataGridViewTextBoxColumn colOT;
        private DataGridViewTextBoxColumn colDominio;
        private DataGridViewTextBoxColumn colVehiculo;
        private DataGridViewTextBoxColumn colMecanico;
        private DataGridViewTextBoxColumn colDescripcion;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colInicio;
        private DataGridViewTextBoxColumn colFin;
        private Button btnIniciarTarea;
        private Button btnRegistrarFinalizada;
        private Button btnCerrar;
    }
}
