namespace PRAS.UI
{
    partial class FrmConsignacionEdicion
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
            cboVehiculo = new ComboBox();
            lblAgencia = new Label();
            cboAgencia = new ComboBox();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            lblPrecioMinimo = new Label();
            numPrecioMinimo = new NumericUpDown();
            lblComision = new Label();
            numComision = new NumericUpDown();
            btnGuardar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)numPrecioMinimo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numComision).BeginInit();
            SuspendLayout();
            //
            // lblVehiculo
            //
            lblVehiculo.AutoSize = true;
            lblVehiculo.Location = new Point(12, 15);
            lblVehiculo.Name = "lblVehiculo";
            lblVehiculo.TabIndex = 0;
            lblVehiculo.Text = "Vehículo reparado *:";
            //
            // cboVehiculo
            //
            cboVehiculo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVehiculo.FormattingEnabled = true;
            cboVehiculo.Location = new Point(170, 11);
            cboVehiculo.Name = "cboVehiculo";
            cboVehiculo.Size = new Size(336, 23);
            cboVehiculo.TabIndex = 1;
            //
            // lblAgencia
            //
            lblAgencia.AutoSize = true;
            lblAgencia.Location = new Point(12, 48);
            lblAgencia.Name = "lblAgencia";
            lblAgencia.TabIndex = 2;
            lblAgencia.Text = "Agencia *:";
            //
            // cboAgencia
            //
            cboAgencia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAgencia.FormattingEnabled = true;
            cboAgencia.Location = new Point(170, 44);
            cboAgencia.Name = "cboAgencia";
            cboAgencia.Size = new Size(336, 23);
            cboAgencia.TabIndex = 3;
            //
            // lblFecha
            //
            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(12, 81);
            lblFecha.Name = "lblFecha";
            lblFecha.TabIndex = 4;
            lblFecha.Text = "Fecha de entrega *:";
            //
            // dtpFecha
            //
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(170, 77);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(110, 23);
            dtpFecha.TabIndex = 5;
            //
            // lblPrecioMinimo
            //
            lblPrecioMinimo.AutoSize = true;
            lblPrecioMinimo.Location = new Point(12, 114);
            lblPrecioMinimo.Name = "lblPrecioMinimo";
            lblPrecioMinimo.TabIndex = 6;
            lblPrecioMinimo.Text = "Precio mínimo de venta *:";
            //
            // numPrecioMinimo
            //
            numPrecioMinimo.DecimalPlaces = 2;
            numPrecioMinimo.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numPrecioMinimo.ThousandsSeparator = true;
            numPrecioMinimo.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            numPrecioMinimo.Location = new Point(170, 110);
            numPrecioMinimo.Name = "numPrecioMinimo";
            numPrecioMinimo.Size = new Size(150, 23);
            numPrecioMinimo.TabIndex = 7;
            //
            // lblComision
            //
            lblComision.AutoSize = true;
            lblComision.Location = new Point(12, 147);
            lblComision.Name = "lblComision";
            lblComision.TabIndex = 8;
            lblComision.Text = "Comisión de la agencia (%) *:";
            //
            // numComision
            //
            numComision.DecimalPlaces = 2;
            numComision.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            numComision.Value = new decimal(new int[] { 5, 0, 0, 0 });
            numComision.Location = new Point(170, 143);
            numComision.Name = "numComision";
            numComision.Size = new Size(80, 23);
            numComision.TabIndex = 9;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(276, 215);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(130, 30);
            btnGuardar.TabIndex = 10;
            btnGuardar.Text = "Registrar consignación";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(412, 215);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 30);
            btnCancelar.TabIndex = 11;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmConsignacionEdicion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(520, 260);
            Controls.Add(lblVehiculo);
            Controls.Add(cboVehiculo);
            Controls.Add(lblAgencia);
            Controls.Add(cboAgencia);
            Controls.Add(lblFecha);
            Controls.Add(dtpFecha);
            Controls.Add(lblPrecioMinimo);
            Controls.Add(numPrecioMinimo);
            Controls.Add(lblComision);
            Controls.Add(numComision);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmConsignacionEdicion";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Entregar auto a una agencia";
            Load += FrmConsignacionEdicion_Load;
            ((System.ComponentModel.ISupportInitialize)numPrecioMinimo).EndInit();
            ((System.ComponentModel.ISupportInitialize)numComision).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblVehiculo;
        private ComboBox cboVehiculo;
        private Label lblAgencia;
        private ComboBox cboAgencia;
        private Label lblFecha;
        private DateTimePicker dtpFecha;
        private Label lblPrecioMinimo;
        private NumericUpDown numPrecioMinimo;
        private Label lblComision;
        private NumericUpDown numComision;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
