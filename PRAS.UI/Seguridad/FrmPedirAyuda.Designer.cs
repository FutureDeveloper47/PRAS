namespace PRAS.UI
{
    partial class FrmPedirAyuda
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
            lblTema = new Label();
            cmbTema = new ComboBox();
            lblDetalle = new Label();
            txtDetalle = new TextBox();
            lblCaracteres = new Label();
            btnRegistrarPedido = new Button();
            btnEnviarEmail = new Button();
            btnCerrar = new Button();
            lblUrgenteTitulo = new Label();
            lblContacto = new Label();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Size = new Size(560, 56);
            lblTitulo.Text = "Pedir ayuda";
            lblTitulo.Name = "lblTitulo";
            //
            // lblTema
            //
            lblTema.AutoSize = true;
            lblTema.Location = new Point(24, 72);
            lblTema.Text = "Tema del problema: *";
            lblTema.Name = "lblTema";
            //
            // cmbTema
            //
            cmbTema.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTema.Location = new Point(24, 92);
            cmbTema.Size = new Size(510, 23);
            cmbTema.Name = "cmbTema";
            //
            // lblDetalle
            //
            lblDetalle.AutoSize = true;
            lblDetalle.Location = new Point(24, 126);
            lblDetalle.Text = "Detalle del problema: *";
            lblDetalle.Name = "lblDetalle";
            //
            // txtDetalle
            //
            txtDetalle.Multiline = true;
            txtDetalle.Location = new Point(24, 146);
            txtDetalle.Size = new Size(510, 110);
            txtDetalle.MaxLength = 1000;
            txtDetalle.ScrollBars = ScrollBars.Vertical;
            txtDetalle.Name = "txtDetalle";
            //
            // lblCaracteres
            //
            lblCaracteres.Location = new Point(384, 262);
            lblCaracteres.Size = new Size(150, 18);
            lblCaracteres.TextAlign = ContentAlignment.MiddleRight;
            lblCaracteres.Text = "0 / 1000";
            lblCaracteres.Name = "lblCaracteres";
            //
            // btnRegistrarPedido
            //
            btnRegistrarPedido.Location = new Point(24, 286);
            btnRegistrarPedido.Size = new Size(170, 30);
            btnRegistrarPedido.Text = "Enviar pedido";
            btnRegistrarPedido.UseVisualStyleBackColor = true;
            btnRegistrarPedido.Name = "btnRegistrarPedido";
            btnRegistrarPedido.Click += btnRegistrarPedido_Click;
            //
            // btnEnviarEmail
            //
            btnEnviarEmail.Location = new Point(204, 286);
            btnEnviarEmail.Size = new Size(200, 30);
            btnEnviarEmail.Text = "Enviar también por email";
            btnEnviarEmail.UseVisualStyleBackColor = true;
            btnEnviarEmail.Name = "btnEnviarEmail";
            btnEnviarEmail.Click += btnEnviarEmail_Click;
            //
            // btnCerrar
            //
            btnCerrar.Location = new Point(434, 286);
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Name = "btnCerrar";
            //
            // lblUrgenteTitulo
            //
            lblUrgenteTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUrgenteTitulo.Location = new Point(24, 332);
            lblUrgenteTitulo.Size = new Size(510, 20);
            lblUrgenteTitulo.Text = "Si es urgente, comuníquese con el administrador:";
            lblUrgenteTitulo.Name = "lblUrgenteTitulo";
            //
            // lblContacto
            //
            lblContacto.Location = new Point(24, 354);
            lblContacto.Size = new Size(510, 56);
            lblContacto.Text = "";
            lblContacto.Name = "lblContacto";
            //
            // FrmPedirAyuda
            //
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(560, 420);
            Text = "Pedir ayuda";
            Controls.Add(lblTitulo);
            Controls.Add(lblTema);
            Controls.Add(cmbTema);
            Controls.Add(lblDetalle);
            Controls.Add(txtDetalle);
            Controls.Add(lblCaracteres);
            Controls.Add(btnRegistrarPedido);
            Controls.Add(btnEnviarEmail);
            Controls.Add(btnCerrar);
            Controls.Add(lblUrgenteTitulo);
            Controls.Add(lblContacto);
            Name = "FrmPedirAyuda";
            Load += FrmPedirAyuda_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblTema;
        private ComboBox cmbTema;
        private Label lblDetalle;
        private TextBox txtDetalle;
        private Label lblCaracteres;
        private Button btnRegistrarPedido;
        private Button btnEnviarEmail;
        private Button btnCerrar;
        private Label lblUrgenteTitulo;
        private Label lblContacto;
    }
}
