namespace PRAS.UI
{
    partial class FrmIntegridad
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
            lstErrores = new ListBox();
            lblTablas = new Label();
            lblAyuda = new Label();
            btnVerificar = new Button();
            btnRecalcular = new Button();
            btnRestaurar = new Button();
            btnCerrar = new Button();
            SuspendLayout();
            //
            // lblEstado
            //
            lblEstado.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEstado.Location = new Point(12, 12);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(636, 24);
            lblEstado.TabIndex = 0;
            //
            // lstErrores
            //
            lstErrores.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstErrores.HorizontalScrollbar = true;
            lstErrores.ItemHeight = 15;
            lstErrores.Location = new Point(12, 42);
            lstErrores.Name = "lstErrores";
            lstErrores.Size = new Size(636, 184);
            lstErrores.TabIndex = 1;
            //
            // lblTablas
            //
            lblTablas.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblTablas.AutoSize = true;
            lblTablas.ForeColor = SystemColors.GrayText;
            lblTablas.Location = new Point(12, 234);
            lblTablas.Name = "lblTablas";
            lblTablas.Size = new Size(0, 15);
            lblTablas.TabIndex = 2;
            //
            // lblAyuda
            //
            lblAyuda.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblAyuda.Location = new Point(12, 256);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(636, 36);
            lblAyuda.TabIndex = 3;
            lblAyuda.Text = "Si los cambios son correctos, recalcule los dígitos verificadores. Si no, restaure un backup anterior al problema.";
            //
            // btnVerificar
            //
            btnVerificar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnVerificar.Location = new Point(12, 302);
            btnVerificar.Name = "btnVerificar";
            btnVerificar.Size = new Size(150, 30);
            btnVerificar.TabIndex = 4;
            btnVerificar.Text = "Verificar nuevamente";
            btnVerificar.UseVisualStyleBackColor = true;
            btnVerificar.Click += btnVerificar_Click;
            //
            // btnRecalcular
            //
            btnRecalcular.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRecalcular.Location = new Point(168, 302);
            btnRecalcular.Name = "btnRecalcular";
            btnRecalcular.Size = new Size(160, 30);
            btnRecalcular.TabIndex = 5;
            btnRecalcular.Text = "Recalcular dígitos";
            btnRecalcular.UseVisualStyleBackColor = true;
            btnRecalcular.Click += btnRecalcular_Click;
            //
            // btnRestaurar
            //
            btnRestaurar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRestaurar.Location = new Point(334, 302);
            btnRestaurar.Name = "btnRestaurar";
            btnRestaurar.Size = new Size(160, 30);
            btnRestaurar.TabIndex = 6;
            btnRestaurar.Text = "Restaurar backup...";
            btnRestaurar.UseVisualStyleBackColor = true;
            btnRestaurar.Click += btnRestaurar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(548, 302);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 7;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmIntegridad
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(660, 344);
            Controls.Add(lblEstado);
            Controls.Add(lstErrores);
            Controls.Add(lblTablas);
            Controls.Add(lblAyuda);
            Controls.Add(btnVerificar);
            Controls.Add(btnRecalcular);
            Controls.Add(btnRestaurar);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmIntegridad";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Integridad de datos";
            Load += FrmIntegridad_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblEstado;
        private ListBox lstErrores;
        private Label lblTablas;
        private Label lblAyuda;
        private Button btnVerificar;
        private Button btnRecalcular;
        private Button btnRestaurar;
        private Button btnCerrar;
    }
}
