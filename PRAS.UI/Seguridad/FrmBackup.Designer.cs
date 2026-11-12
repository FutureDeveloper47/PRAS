namespace PRAS.UI
{
    partial class FrmBackup
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
            grpBackup = new GroupBox();
            lblDestino = new Label();
            txtDestino = new TextBox();
            btnElegirDestino = new Button();
            btnRealizarBackup = new Button();
            grpRestore = new GroupBox();
            lblOrigen = new Label();
            txtOrigen = new TextBox();
            btnElegirOrigen = new Button();
            lblAviso = new Label();
            btnRestaurar = new Button();
            btnCerrar = new Button();
            grpBackup.SuspendLayout();
            grpRestore.SuspendLayout();
            SuspendLayout();
            //
            // grpBackup
            //
            grpBackup.Controls.Add(lblDestino);
            grpBackup.Controls.Add(txtDestino);
            grpBackup.Controls.Add(btnElegirDestino);
            grpBackup.Controls.Add(btnRealizarBackup);
            grpBackup.Location = new Point(12, 12);
            grpBackup.Name = "grpBackup";
            grpBackup.Size = new Size(576, 125);
            grpBackup.TabIndex = 0;
            grpBackup.TabStop = false;
            grpBackup.Text = "Realizar backup";
            //
            // lblDestino
            //
            lblDestino.AutoSize = true;
            lblDestino.Location = new Point(14, 26);
            lblDestino.Name = "lblDestino";
            lblDestino.Size = new Size(108, 15);
            lblDestino.TabIndex = 0;
            lblDestino.Text = "Archivo de destino:";
            //
            // txtDestino
            //
            txtDestino.Location = new Point(14, 46);
            txtDestino.Name = "txtDestino";
            txtDestino.ReadOnly = true;
            txtDestino.Size = new Size(440, 23);
            txtDestino.TabIndex = 1;
            //
            // btnElegirDestino
            //
            btnElegirDestino.Location = new Point(462, 43);
            btnElegirDestino.Name = "btnElegirDestino";
            btnElegirDestino.Size = new Size(100, 29);
            btnElegirDestino.TabIndex = 2;
            btnElegirDestino.Text = "Examinar...";
            btnElegirDestino.UseVisualStyleBackColor = true;
            btnElegirDestino.Click += btnElegirDestino_Click;
            //
            // btnRealizarBackup
            //
            btnRealizarBackup.Location = new Point(402, 82);
            btnRealizarBackup.Name = "btnRealizarBackup";
            btnRealizarBackup.Size = new Size(160, 30);
            btnRealizarBackup.TabIndex = 3;
            btnRealizarBackup.Text = "Realizar backup";
            btnRealizarBackup.UseVisualStyleBackColor = true;
            btnRealizarBackup.Click += btnRealizarBackup_Click;
            //
            // grpRestore
            //
            grpRestore.Controls.Add(lblOrigen);
            grpRestore.Controls.Add(txtOrigen);
            grpRestore.Controls.Add(btnElegirOrigen);
            grpRestore.Controls.Add(lblAviso);
            grpRestore.Controls.Add(btnRestaurar);
            grpRestore.Location = new Point(12, 150);
            grpRestore.Name = "grpRestore";
            grpRestore.Size = new Size(576, 150);
            grpRestore.TabIndex = 1;
            grpRestore.TabStop = false;
            grpRestore.Text = "Restaurar backup";
            //
            // lblOrigen
            //
            lblOrigen.AutoSize = true;
            lblOrigen.Location = new Point(14, 26);
            lblOrigen.Name = "lblOrigen";
            lblOrigen.Size = new Size(105, 15);
            lblOrigen.TabIndex = 0;
            lblOrigen.Text = "Archivo de backup:";
            //
            // txtOrigen
            //
            txtOrigen.Location = new Point(14, 46);
            txtOrigen.Name = "txtOrigen";
            txtOrigen.ReadOnly = true;
            txtOrigen.Size = new Size(440, 23);
            txtOrigen.TabIndex = 1;
            //
            // btnElegirOrigen
            //
            btnElegirOrigen.Location = new Point(462, 43);
            btnElegirOrigen.Name = "btnElegirOrigen";
            btnElegirOrigen.Size = new Size(100, 29);
            btnElegirOrigen.TabIndex = 2;
            btnElegirOrigen.Text = "Examinar...";
            btnElegirOrigen.UseVisualStyleBackColor = true;
            btnElegirOrigen.Click += btnElegirOrigen_Click;
            //
            // lblAviso
            //
            lblAviso.ForeColor = Color.FromArgb(192, 57, 43);
            lblAviso.Location = new Point(14, 78);
            lblAviso.Name = "lblAviso";
            lblAviso.Size = new Size(548, 20);
            lblAviso.TabIndex = 3;
            lblAviso.Text = "¡Atención! Se reemplazan todos los datos actuales por los del backup.";
            //
            // btnRestaurar
            //
            btnRestaurar.Location = new Point(402, 106);
            btnRestaurar.Name = "btnRestaurar";
            btnRestaurar.Size = new Size(160, 30);
            btnRestaurar.TabIndex = 4;
            btnRestaurar.Text = "Restaurar";
            btnRestaurar.UseVisualStyleBackColor = true;
            btnRestaurar.Click += btnRestaurar_Click;
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(488, 312);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 2;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmBackup
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(600, 354);
            Controls.Add(grpBackup);
            Controls.Add(grpRestore);
            Controls.Add(btnCerrar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmBackup";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Backup y restore";
            Load += FrmBackup_Load;
            grpBackup.ResumeLayout(false);
            grpBackup.PerformLayout();
            grpRestore.ResumeLayout(false);
            grpRestore.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpBackup;
        private Label lblDestino;
        private TextBox txtDestino;
        private Button btnElegirDestino;
        private Button btnRealizarBackup;
        private GroupBox grpRestore;
        private Label lblOrigen;
        private TextBox txtOrigen;
        private Button btnElegirOrigen;
        private Label lblAviso;
        private Button btnRestaurar;
        private Button btnCerrar;
    }
}
