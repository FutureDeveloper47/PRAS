namespace PRAS.UI
{
    partial class FrmAsignacionPresupuesto
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
            lblVehiculo = new Label();
            cboVehiculo = new ComboBox();
            lblMecanico = new Label();
            cboMecanico = new ComboBox();
            lblDiagnostico = new Label();
            txtDiagnostico = new TextBox();
            lblPrioridad = new Label();
            rbBaja = new RadioButton();
            rbAlta = new RadioButton();
            rbMedia = new RadioButton();
            lblObligatorios = new Label();
            btnCancelar = new Button();
            btnGuardar = new Button();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(880, 45);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "PRAS: Programa de Registro Automotor Superior";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblVehiculo
            //
            lblVehiculo.AutoSize = true;
            lblVehiculo.Location = new Point(115, 90);
            lblVehiculo.Name = "lblVehiculo";
            lblVehiculo.TabIndex = 1;
            lblVehiculo.Text = "VEHÍCULO *";
            //
            // cboVehiculo
            //
            cboVehiculo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVehiculo.FormattingEnabled = true;
            cboVehiculo.Location = new Point(115, 110);
            cboVehiculo.Name = "cboVehiculo";
            cboVehiculo.Size = new Size(425, 23);
            cboVehiculo.TabIndex = 2;
            //
            // lblMecanico
            //
            lblMecanico.AutoSize = true;
            lblMecanico.Location = new Point(115, 165);
            lblMecanico.Name = "lblMecanico";
            lblMecanico.TabIndex = 3;
            lblMecanico.Text = "Seleccionar mecánico *";
            //
            // cboMecanico
            //
            cboMecanico.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMecanico.FormattingEnabled = true;
            cboMecanico.Location = new Point(115, 185);
            cboMecanico.Name = "cboMecanico";
            cboMecanico.Size = new Size(160, 23);
            cboMecanico.TabIndex = 4;
            //
            // lblDiagnostico
            //
            lblDiagnostico.AutoSize = true;
            lblDiagnostico.Location = new Point(300, 165);
            lblDiagnostico.Name = "lblDiagnostico";
            lblDiagnostico.TabIndex = 5;
            lblDiagnostico.Text = "Diagnóstico inicial *";
            //
            // txtDiagnostico
            //
            txtDiagnostico.Location = new Point(300, 185);
            txtDiagnostico.MaxLength = 1000;
            txtDiagnostico.Multiline = true;
            txtDiagnostico.Name = "txtDiagnostico";
            txtDiagnostico.PlaceholderText = "Daños detectados...";
            txtDiagnostico.ScrollBars = ScrollBars.Vertical;
            txtDiagnostico.Size = new Size(240, 190);
            txtDiagnostico.TabIndex = 6;
            //
            // lblPrioridad
            //
            lblPrioridad.AutoSize = true;
            lblPrioridad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPrioridad.Location = new Point(640, 190);
            lblPrioridad.Name = "lblPrioridad";
            lblPrioridad.TabIndex = 7;
            lblPrioridad.Text = "Prioridad";
            //
            // rbBaja
            //
            rbBaja.Appearance = Appearance.Button;
            rbBaja.Location = new Point(635, 220);
            rbBaja.Name = "rbBaja";
            rbBaja.Size = new Size(95, 24);
            rbBaja.TabIndex = 8;
            rbBaja.Text = "Baja";
            rbBaja.TextAlign = ContentAlignment.MiddleCenter;
            rbBaja.UseVisualStyleBackColor = true;
            //
            // rbAlta
            //
            rbAlta.Appearance = Appearance.Button;
            rbAlta.Location = new Point(635, 262);
            rbAlta.Name = "rbAlta";
            rbAlta.Size = new Size(95, 24);
            rbAlta.TabIndex = 9;
            rbAlta.Text = "Alta";
            rbAlta.TextAlign = ContentAlignment.MiddleCenter;
            rbAlta.UseVisualStyleBackColor = true;
            //
            // rbMedia
            //
            rbMedia.Appearance = Appearance.Button;
            rbMedia.Checked = true;
            rbMedia.Location = new Point(635, 304);
            rbMedia.Name = "rbMedia";
            rbMedia.Size = new Size(95, 24);
            rbMedia.TabIndex = 10;
            rbMedia.TabStop = true;
            rbMedia.Text = "Media";
            rbMedia.TextAlign = ContentAlignment.MiddleCenter;
            rbMedia.UseVisualStyleBackColor = true;
            //
            // lblObligatorios
            //
            lblObligatorios.AutoSize = true;
            lblObligatorios.ForeColor = SystemColors.GrayText;
            lblObligatorios.Location = new Point(115, 400);
            lblObligatorios.Name = "lblObligatorios";
            lblObligatorios.TabIndex = 11;
            lblObligatorios.Text = "(*) Campo obligatorio";
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(615, 465);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(105, 27);
            btnCancelar.TabIndex = 13;
            btnCancelar.Text = "CANCELAR";
            btnCancelar.UseVisualStyleBackColor = true;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(730, 465);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(125, 27);
            btnGuardar.TabIndex = 12;
            btnGuardar.Text = "GUARDAR Y ENVIAR";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // FrmAsignacionPresupuesto
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(880, 515);
            Controls.Add(lblTitulo);
            Controls.Add(lblVehiculo);
            Controls.Add(cboVehiculo);
            Controls.Add(lblMecanico);
            Controls.Add(cboMecanico);
            Controls.Add(lblDiagnostico);
            Controls.Add(txtDiagnostico);
            Controls.Add(lblPrioridad);
            Controls.Add(rbBaja);
            Controls.Add(rbAlta);
            Controls.Add(rbMedia);
            Controls.Add(lblObligatorios);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAsignacionPresupuesto";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Asignación de Presupuesto";
            Load += FrmAsignacionPresupuesto_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblVehiculo;
        private ComboBox cboVehiculo;
        private Label lblMecanico;
        private ComboBox cboMecanico;
        private Label lblDiagnostico;
        private TextBox txtDiagnostico;
        private Label lblPrioridad;
        private RadioButton rbBaja;
        private RadioButton rbAlta;
        private RadioButton rbMedia;
        private Label lblObligatorios;
        private Button btnCancelar;
        private Button btnGuardar;
    }
}
