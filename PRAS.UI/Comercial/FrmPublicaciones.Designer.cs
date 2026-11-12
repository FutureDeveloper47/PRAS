namespace PRAS.UI
{
    partial class FrmPublicaciones
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
            chkSoloActivas = new CheckBox();
            dgvPublicaciones = new DataGridView();
            colFecha = new DataGridViewTextBoxColumn();
            colDominio = new DataGridViewTextBoxColumn();
            colVehiculo = new DataGridViewTextBoxColumn();
            colPlataforma = new DataGridViewTextBoxColumn();
            colPrecio = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colFinalizacion = new DataGridViewTextBoxColumn();
            btnNuevo = new Button();
            btnModificar = new Button();
            btnQuitarPublicacion = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPublicaciones).BeginInit();
            SuspendLayout();
            //
            // chkSoloActivas
            //
            chkSoloActivas.AutoSize = true;
            chkSoloActivas.Checked = true;
            chkSoloActivas.CheckState = CheckState.Checked;
            chkSoloActivas.Location = new Point(12, 12);
            chkSoloActivas.Name = "chkSoloActivas";
            chkSoloActivas.TabIndex = 0;
            chkSoloActivas.Text = "Mostrar solo las publicaciones activas";
            chkSoloActivas.UseVisualStyleBackColor = true;
            chkSoloActivas.CheckedChanged += chkSoloActivas_CheckedChanged;
            //
            // dgvPublicaciones
            //
            dgvPublicaciones.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            dgvPublicaciones.AllowUserToAddRows = false;
            dgvPublicaciones.AllowUserToDeleteRows = false;
            dgvPublicaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPublicaciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPublicaciones.Columns.AddRange(new DataGridViewColumn[] { colFecha, colDominio, colVehiculo, colPlataforma, colPrecio, colEstado, colFinalizacion });
            dgvPublicaciones.MultiSelect = false;
            dgvPublicaciones.ReadOnly = true;
            dgvPublicaciones.RowHeadersVisible = false;
            dgvPublicaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPublicaciones.Location = new Point(12, 40);
            dgvPublicaciones.Name = "dgvPublicaciones";
            dgvPublicaciones.Size = new Size(776, 428);
            dgvPublicaciones.TabIndex = 1;
            dgvPublicaciones.CellDoubleClick += dgvPublicaciones_CellDoubleClick;
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
            // colPlataforma
            //
            colPlataforma.FillWeight = 100F;
            colPlataforma.HeaderText = "Plataforma";
            colPlataforma.Name = "colPlataforma";
            colPlataforma.ReadOnly = true;
            //
            // colPrecio
            //
            colPrecio.FillWeight = 80F;
            colPrecio.HeaderText = "Precio publicado";
            colPrecio.Name = "colPrecio";
            colPrecio.ReadOnly = true;
            //
            // colEstado
            //
            colEstado.FillWeight = 55F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // colFinalizacion
            //
            colFinalizacion.FillWeight = 65F;
            colFinalizacion.HeaderText = "Finalización";
            colFinalizacion.Name = "colFinalizacion";
            colFinalizacion.ReadOnly = true;
            //
            // btnNuevo
            //
            btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevo.Location = new Point(800, 40);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(130, 30);
            btnNuevo.TabIndex = 2;
            btnNuevo.Text = "Nueva publicación";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnModificar
            //
            btnModificar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnModificar.Location = new Point(800, 80);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(130, 30);
            btnModificar.TabIndex = 3;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            //
            // btnQuitarPublicacion
            //
            btnQuitarPublicacion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnQuitarPublicacion.Location = new Point(800, 120);
            btnQuitarPublicacion.Name = "btnQuitarPublicacion";
            btnQuitarPublicacion.Size = new Size(130, 30);
            btnQuitarPublicacion.TabIndex = 4;
            btnQuitarPublicacion.Text = "Dar de baja";
            btnQuitarPublicacion.UseVisualStyleBackColor = true;
            btnQuitarPublicacion.Click += btnQuitarPublicacion_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(830, 438);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 5;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmPublicaciones
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(942, 480);
            Controls.Add(chkSoloActivas);
            Controls.Add(dgvPublicaciones);
            Controls.Add(btnNuevo);
            Controls.Add(btnModificar);
            Controls.Add(btnQuitarPublicacion);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmPublicaciones";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Publicaciones";
            Load += FrmPublicaciones_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPublicaciones).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox chkSoloActivas;
        private DataGridView dgvPublicaciones;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colDominio;
        private DataGridViewTextBoxColumn colVehiculo;
        private DataGridViewTextBoxColumn colPlataforma;
        private DataGridViewTextBoxColumn colPrecio;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colFinalizacion;
        private Button btnNuevo;
        private Button btnModificar;
        private Button btnQuitarPublicacion;
        private Button btnCerrar;
    }
}
