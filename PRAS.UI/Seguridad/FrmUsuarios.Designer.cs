namespace PRAS.UI
{
    partial class FrmUsuarios
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
            chkMostrarDeshabilitados = new CheckBox();
            dgvUsuarios = new DataGridView();
            colNombreUsuario = new DataGridViewTextBoxColumn();
            colRol = new DataGridViewTextBoxColumn();
            colMecanico = new DataGridViewTextBoxColumn();
            colActivo = new DataGridViewCheckBoxColumn();
            btnNuevo = new Button();
            btnModificar = new Button();
            btnDeshabilitar = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            //
            // chkMostrarDeshabilitados
            //
            chkMostrarDeshabilitados.AutoSize = true;
            chkMostrarDeshabilitados.Location = new Point(12, 14);
            chkMostrarDeshabilitados.Name = "chkMostrarDeshabilitados";
            chkMostrarDeshabilitados.Size = new Size(152, 19);
            chkMostrarDeshabilitados.TabIndex = 0;
            chkMostrarDeshabilitados.Text = "Mostrar deshabilitados";
            chkMostrarDeshabilitados.UseVisualStyleBackColor = true;
            chkMostrarDeshabilitados.CheckedChanged += chkMostrarDeshabilitados_CheckedChanged;
            //
            // dgvUsuarios
            //
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Columns.AddRange(new DataGridViewColumn[] { colNombreUsuario, colRol, colMecanico, colActivo });
            dgvUsuarios.Location = new Point(12, 42);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(460, 307);
            dgvUsuarios.TabIndex = 1;
            dgvUsuarios.CellDoubleClick += dgvUsuarios_CellDoubleClick;
            dgvUsuarios.CellFormatting += dgvUsuarios_CellFormatting;
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
            //
            // colNombreUsuario
            //
            colNombreUsuario.DataPropertyName = "NombreUsuario";
            colNombreUsuario.HeaderText = "Usuario";
            colNombreUsuario.Name = "colNombreUsuario";
            colNombreUsuario.ReadOnly = true;
            //
            // colRol
            //
            colRol.DataPropertyName = "Rol";
            colRol.HeaderText = "Rol";
            colRol.Name = "colRol";
            colRol.ReadOnly = true;
            // 
            // colMecanico
            // 
            colMecanico.HeaderText = "Mecánico";
            colMecanico.Name = "colMecanico";
            colMecanico.ReadOnly = true;
            //
            // colActivo
            //
            colActivo.DataPropertyName = "Activo";
            colActivo.FillWeight = 40F;
            colActivo.HeaderText = "Activo";
            colActivo.Name = "colActivo";
            colActivo.ReadOnly = true;
            //
            // btnNuevo
            //
            btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevo.Location = new Point(484, 42);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(100, 30);
            btnNuevo.TabIndex = 2;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnModificar
            //
            btnModificar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnModificar.Location = new Point(484, 78);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(100, 30);
            btnModificar.TabIndex = 3;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            //
            // btnDeshabilitar
            //
            btnDeshabilitar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDeshabilitar.Location = new Point(484, 114);
            btnDeshabilitar.Name = "btnDeshabilitar";
            btnDeshabilitar.Size = new Size(100, 30);
            btnDeshabilitar.TabIndex = 4;
            btnDeshabilitar.Text = "Deshabilitar";
            btnDeshabilitar.UseVisualStyleBackColor = true;
            btnDeshabilitar.Click += btnDeshabilitar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(484, 319);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 5;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmUsuarios
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(596, 361);
            Controls.Add(chkMostrarDeshabilitados);
            Controls.Add(dgvUsuarios);
            Controls.Add(btnNuevo);
            Controls.Add(btnModificar);
            Controls.Add(btnDeshabilitar);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmUsuarios";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Usuarios";
            Load += FrmUsuarios_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox chkMostrarDeshabilitados;
        private DataGridView dgvUsuarios;
        private DataGridViewTextBoxColumn colNombreUsuario;
        private DataGridViewTextBoxColumn colRol;
        private DataGridViewTextBoxColumn colMecanico;
        private DataGridViewCheckBoxColumn colActivo;
        private Button btnNuevo;
        private Button btnModificar;
        private Button btnDeshabilitar;
        private Button btnCerrar;
    }
}
