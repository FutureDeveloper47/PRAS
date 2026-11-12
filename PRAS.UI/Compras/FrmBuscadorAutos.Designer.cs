namespace PRAS.UI
{
    partial class FrmBuscadorAutos
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
            lblTitulo = new Label();
            lblEstado = new Label();
            cboEstado = new ComboBox();
            txtPatente = new TextBox();
            lblBuscarPatente = new Label();
            dgvAutos = new DataGridView();
            colPatente = new DataGridViewTextBoxColumn();
            colMarca = new DataGridViewTextBoxColumn();
            colModelo = new DataGridViewTextBoxColumn();
            colAnio = new DataGridViewTextBoxColumn();
            colColor = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            lblCantidad = new Label();
            btnVerDetalle = new Button();
            btnModificar = new Button();
            btnEliminar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvAutos).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(860, 45);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "PRAS: Programa de Registro Automotor Superior";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblEstado
            //
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(262, 83);
            lblEstado.Name = "lblEstado";
            lblEstado.TabIndex = 1;
            lblEstado.Text = "ESTADO:";
            //
            // cboEstado
            //
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.FormattingEnabled = true;
            cboEstado.Location = new Point(330, 80);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(200, 23);
            cboEstado.TabIndex = 2;
            cboEstado.SelectedIndexChanged += cboEstado_SelectedIndexChanged;
            //
            // txtPatente
            //
            txtPatente.CharacterCasing = CharacterCasing.Upper;
            txtPatente.Location = new Point(330, 115);
            txtPatente.MaxLength = 10;
            txtPatente.Name = "txtPatente";
            txtPatente.PlaceholderText = "Ej: AB123CD";
            txtPatente.Size = new Size(200, 23);
            txtPatente.TabIndex = 3;
            txtPatente.TextChanged += txtPatente_TextChanged;
            //
            // lblBuscarPatente
            //
            lblBuscarPatente.AutoSize = true;
            lblBuscarPatente.Location = new Point(545, 118);
            lblBuscarPatente.Name = "lblBuscarPatente";
            lblBuscarPatente.TabIndex = 4;
            lblBuscarPatente.Text = "Buscar Patente";
            //
            // dgvAutos
            //
            dgvAutos.AllowUserToAddRows = false;
            dgvAutos.AllowUserToDeleteRows = false;
            dgvAutos.AllowUserToResizeRows = false;
            dgvAutos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAutos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAutos.Columns.AddRange(new DataGridViewColumn[] { colPatente, colMarca, colModelo, colAnio, colColor, colEstado });
            dgvAutos.Location = new Point(30, 155);
            dgvAutos.MultiSelect = false;
            dgvAutos.Name = "dgvAutos";
            dgvAutos.ReadOnly = true;
            dgvAutos.RowHeadersVisible = false;
            dgvAutos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAutos.Size = new Size(800, 300);
            dgvAutos.TabIndex = 5;
            dgvAutos.CellDoubleClick += dgvAutos_CellDoubleClick;
            //
            // colPatente
            //
            colPatente.FillWeight = 15F;
            colPatente.HeaderText = "Patente";
            colPatente.Name = "colPatente";
            colPatente.ReadOnly = true;
            //
            // colMarca
            //
            colMarca.FillWeight = 20F;
            colMarca.HeaderText = "Marca";
            colMarca.Name = "colMarca";
            colMarca.ReadOnly = true;
            //
            // colModelo
            //
            colModelo.FillWeight = 20F;
            colModelo.HeaderText = "Modelo";
            colModelo.Name = "colModelo";
            colModelo.ReadOnly = true;
            //
            // colAnio
            //
            colAnio.FillWeight = 10F;
            colAnio.HeaderText = "Año";
            colAnio.Name = "colAnio";
            colAnio.ReadOnly = true;
            //
            // colColor
            //
            colColor.FillWeight = 15F;
            colColor.HeaderText = "Color";
            colColor.Name = "colColor";
            colColor.ReadOnly = true;
            //
            // colEstado
            //
            colEstado.FillWeight = 20F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            //
            // lblCantidad
            //
            lblCantidad.AutoSize = true;
            lblCantidad.ForeColor = SystemColors.GrayText;
            lblCantidad.Location = new Point(30, 462);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.TabIndex = 6;
            lblCantidad.Text = "0 vehículo(s)";
            //
            // btnVerDetalle
            //
            btnVerDetalle.Location = new Point(240, 490);
            btnVerDetalle.Name = "btnVerDetalle";
            btnVerDetalle.Size = new Size(105, 27);
            btnVerDetalle.TabIndex = 7;
            btnVerDetalle.Text = "Ver Detalle";
            btnVerDetalle.UseVisualStyleBackColor = true;
            btnVerDetalle.Click += btnVerDetalle_Click;
            //
            // btnModificar
            //
            btnModificar.Location = new Point(378, 490);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(105, 27);
            btnModificar.TabIndex = 8;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            //
            // btnEliminar
            //
            btnEliminar.Location = new Point(516, 490);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(105, 27);
            btnEliminar.TabIndex = 9;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            //
            // FrmBuscadorAutos
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(860, 540);
            Controls.Add(lblTitulo);
            Controls.Add(lblEstado);
            Controls.Add(cboEstado);
            Controls.Add(txtPatente);
            Controls.Add(lblBuscarPatente);
            Controls.Add(dgvAutos);
            Controls.Add(lblCantidad);
            Controls.Add(btnVerDetalle);
            Controls.Add(btnModificar);
            Controls.Add(btnEliminar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmBuscadorAutos";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "BUSCADOR DE AUTOS";
            Load += FrmBuscadorAutos_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAutos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblEstado;
        private ComboBox cboEstado;
        private TextBox txtPatente;
        private Label lblBuscarPatente;
        private DataGridView dgvAutos;
        private DataGridViewTextBoxColumn colPatente;
        private DataGridViewTextBoxColumn colMarca;
        private DataGridViewTextBoxColumn colModelo;
        private DataGridViewTextBoxColumn colAnio;
        private DataGridViewTextBoxColumn colColor;
        private DataGridViewTextBoxColumn colEstado;
        private Label lblCantidad;
        private Button btnVerDetalle;
        private Button btnModificar;
        private Button btnEliminar;
    }
}
