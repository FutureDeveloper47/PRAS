namespace PRAS.UI
{
    partial class FrmPublicacionEdicion
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
            lblVehiculo = new Label();
            cboConsignacion = new ComboBox();
            lblDatosConsignacion = new Label();
            lblPlataforma = new Label();
            cboPlataforma = new ComboBox();
            lblPrecio = new Label();
            numPrecio = new NumericUpDown();
            lblDescripcion = new Label();
            txtDescripcion = new TextBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)numPrecio).BeginInit();
            SuspendLayout();
            //
            // lblVehiculo
            //
            lblVehiculo.AutoSize = true;
            lblVehiculo.Location = new Point(12, 15);
            lblVehiculo.Name = "lblVehiculo";
            lblVehiculo.TabIndex = 0;
            lblVehiculo.Text = "Vehículo en agencia *:";
            //
            // cboConsignacion
            //
            cboConsignacion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboConsignacion.FormattingEnabled = true;
            cboConsignacion.Location = new Point(150, 11);
            cboConsignacion.Name = "cboConsignacion";
            cboConsignacion.Size = new Size(396, 23);
            cboConsignacion.TabIndex = 1;
            cboConsignacion.SelectedIndexChanged += cboConsignacion_SelectedIndexChanged;
            //
            // lblDatosConsignacion
            //
            lblDatosConsignacion.Location = new Point(150, 40);
            lblDatosConsignacion.Name = "lblDatosConsignacion";
            lblDatosConsignacion.Size = new Size(396, 20);
            lblDatosConsignacion.TabIndex = 2;
            lblDatosConsignacion.Text = "";
            //
            // lblPlataforma
            //
            lblPlataforma.AutoSize = true;
            lblPlataforma.Location = new Point(12, 70);
            lblPlataforma.Name = "lblPlataforma";
            lblPlataforma.TabIndex = 3;
            lblPlataforma.Text = "Plataforma *:";
            //
            // cboPlataforma
            //
            cboPlataforma.Location = new Point(150, 66);
            cboPlataforma.Name = "cboPlataforma";
            cboPlataforma.Size = new Size(250, 23);
            cboPlataforma.TabIndex = 4;
            //
            // lblPrecio
            //
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(12, 103);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.TabIndex = 5;
            lblPrecio.Text = "Precio publicado *:";
            //
            // numPrecio
            //
            numPrecio.DecimalPlaces = 2;
            numPrecio.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numPrecio.ThousandsSeparator = true;
            numPrecio.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            numPrecio.Location = new Point(150, 99);
            numPrecio.Name = "numPrecio";
            numPrecio.Size = new Size(150, 23);
            numPrecio.TabIndex = 6;
            //
            // lblDescripcion
            //
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(12, 136);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.TabIndex = 7;
            lblDescripcion.Text = "Descripción:";
            //
            // txtDescripcion
            //
            txtDescripcion.Multiline = true;
            txtDescripcion.ScrollBars = ScrollBars.Vertical;
            txtDescripcion.Location = new Point(150, 133);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(396, 140);
            txtDescripcion.TabIndex = 8;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(340, 295);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(100, 30);
            btnGuardar.TabIndex = 9;
            btnGuardar.Text = "Publicar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(446, 295);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 30);
            btnCancelar.TabIndex = 10;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmPublicacionEdicion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(560, 340);
            Controls.Add(lblVehiculo);
            Controls.Add(cboConsignacion);
            Controls.Add(lblDatosConsignacion);
            Controls.Add(lblPlataforma);
            Controls.Add(cboPlataforma);
            Controls.Add(lblPrecio);
            Controls.Add(numPrecio);
            Controls.Add(lblDescripcion);
            Controls.Add(txtDescripcion);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPublicacionEdicion";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Publicar auto";
            Load += FrmPublicacionEdicion_Load;
            ((System.ComponentModel.ISupportInitialize)numPrecio).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblVehiculo;
        private ComboBox cboConsignacion;
        private Label lblDatosConsignacion;
        private Label lblPlataforma;
        private ComboBox cboPlataforma;
        private Label lblPrecio;
        private NumericUpDown numPrecio;
        private Label lblDescripcion;
        private TextBox txtDescripcion;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
