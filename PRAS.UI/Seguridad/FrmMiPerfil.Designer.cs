namespace PRAS.UI
{
    partial class FrmMiPerfil
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
            grpDatos = new GroupBox();
            lblUsuario = new Label();
            lblUsuarioValor = new Label();
            lblRol = new Label();
            lblRolValor = new Label();
            lblIdioma = new Label();
            cmbIdioma = new ComboBox();
            btnAplicarIdioma = new Button();
            grpContrasena = new GroupBox();
            lblActual = new Label();
            txtActual = new TextBox();
            lblNueva = new Label();
            txtNueva = new TextBox();
            lblConfirmacion = new Label();
            txtConfirmacion = new TextBox();
            btnGuardarContrasena = new Button();
            grpPregunta = new GroupBox();
            lblPreguntaEstado = new Label();
            lblPregunta = new Label();
            cmbPregunta = new ComboBox();
            lblRespuesta = new Label();
            txtRespuesta = new TextBox();
            btnGuardarPregunta = new Button();
            btnCerrar = new Button();
            grpDatos.SuspendLayout();
            grpContrasena.SuspendLayout();
            grpPregunta.SuspendLayout();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Size = new Size(540, 56);
            lblTitulo.Text = "Mi perfil";
            lblTitulo.Name = "lblTitulo";
            //
            // grpDatos
            //
            grpDatos.Controls.Add(lblUsuario);
            grpDatos.Controls.Add(lblUsuarioValor);
            grpDatos.Controls.Add(lblRol);
            grpDatos.Controls.Add(lblRolValor);
            grpDatos.Controls.Add(lblIdioma);
            grpDatos.Controls.Add(cmbIdioma);
            grpDatos.Controls.Add(btnAplicarIdioma);
            grpDatos.Location = new Point(20, 70);
            grpDatos.Size = new Size(500, 110);
            grpDatos.Text = "Mis datos";
            grpDatos.Name = "grpDatos";
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(16, 26);
            lblUsuario.Text = "Usuario:";
            lblUsuario.Name = "lblUsuario";
            //
            // lblUsuarioValor
            //
            lblUsuarioValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUsuarioValor.Location = new Point(120, 26);
            lblUsuarioValor.Size = new Size(200, 20);
            lblUsuarioValor.Text = "";
            lblUsuarioValor.Name = "lblUsuarioValor";
            //
            // lblRol
            //
            lblRol.AutoSize = true;
            lblRol.Location = new Point(16, 50);
            lblRol.Text = "Rol:";
            lblRol.Name = "lblRol";
            //
            // lblRolValor
            //
            lblRolValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRolValor.Location = new Point(120, 50);
            lblRolValor.Size = new Size(200, 20);
            lblRolValor.Text = "";
            lblRolValor.Name = "lblRolValor";
            //
            // lblIdioma
            //
            lblIdioma.AutoSize = true;
            lblIdioma.Location = new Point(16, 77);
            lblIdioma.Text = "Idioma:";
            lblIdioma.Name = "lblIdioma";
            //
            // cmbIdioma
            //
            cmbIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIdioma.Location = new Point(120, 73);
            cmbIdioma.Size = new Size(220, 23);
            cmbIdioma.Name = "cmbIdioma";
            //
            // btnAplicarIdioma
            //
            btnAplicarIdioma.Location = new Point(350, 70);
            btnAplicarIdioma.Size = new Size(130, 30);
            btnAplicarIdioma.Text = "Aplicar idioma";
            btnAplicarIdioma.UseVisualStyleBackColor = true;
            btnAplicarIdioma.Name = "btnAplicarIdioma";
            btnAplicarIdioma.Click += btnAplicarIdioma_Click;
            //
            // grpContrasena
            //
            grpContrasena.Controls.Add(lblActual);
            grpContrasena.Controls.Add(txtActual);
            grpContrasena.Controls.Add(lblNueva);
            grpContrasena.Controls.Add(txtNueva);
            grpContrasena.Controls.Add(lblConfirmacion);
            grpContrasena.Controls.Add(txtConfirmacion);
            grpContrasena.Controls.Add(btnGuardarContrasena);
            grpContrasena.Location = new Point(20, 190);
            grpContrasena.Size = new Size(500, 150);
            grpContrasena.Text = "Cambiar contraseña";
            grpContrasena.Name = "grpContrasena";
            //
            // lblActual
            //
            lblActual.AutoSize = true;
            lblActual.Location = new Point(16, 28);
            lblActual.Text = "Contraseña actual:";
            lblActual.Name = "lblActual";
            //
            // txtActual
            //
            txtActual.Location = new Point(170, 24);
            txtActual.Size = new Size(310, 23);
            txtActual.MaxLength = 50;
            txtActual.UseSystemPasswordChar = true;
            txtActual.Name = "txtActual";
            //
            // lblNueva
            //
            lblNueva.AutoSize = true;
            lblNueva.Location = new Point(16, 60);
            lblNueva.Text = "Nueva contraseña:";
            lblNueva.Name = "lblNueva";
            //
            // txtNueva
            //
            txtNueva.Location = new Point(170, 56);
            txtNueva.Size = new Size(310, 23);
            txtNueva.MaxLength = 50;
            txtNueva.UseSystemPasswordChar = true;
            txtNueva.Name = "txtNueva";
            //
            // lblConfirmacion
            //
            lblConfirmacion.AutoSize = true;
            lblConfirmacion.Location = new Point(16, 92);
            lblConfirmacion.Text = "Repetir contraseña:";
            lblConfirmacion.Name = "lblConfirmacion";
            //
            // txtConfirmacion
            //
            txtConfirmacion.Location = new Point(170, 88);
            txtConfirmacion.Size = new Size(310, 23);
            txtConfirmacion.MaxLength = 50;
            txtConfirmacion.UseSystemPasswordChar = true;
            txtConfirmacion.Name = "txtConfirmacion";
            //
            // btnGuardarContrasena
            //
            btnGuardarContrasena.Location = new Point(290, 114);
            btnGuardarContrasena.Size = new Size(190, 30);
            btnGuardarContrasena.Text = "Guardar contraseña";
            btnGuardarContrasena.UseVisualStyleBackColor = true;
            btnGuardarContrasena.Name = "btnGuardarContrasena";
            btnGuardarContrasena.Click += btnGuardarContrasena_Click;
            //
            // grpPregunta
            //
            grpPregunta.Controls.Add(lblPreguntaEstado);
            grpPregunta.Controls.Add(lblPregunta);
            grpPregunta.Controls.Add(cmbPregunta);
            grpPregunta.Controls.Add(lblRespuesta);
            grpPregunta.Controls.Add(txtRespuesta);
            grpPregunta.Controls.Add(btnGuardarPregunta);
            grpPregunta.Location = new Point(20, 350);
            grpPregunta.Size = new Size(500, 160);
            grpPregunta.Text = "Pregunta de seguridad (para recuperar la contraseña)";
            grpPregunta.Name = "grpPregunta";
            //
            // lblPreguntaEstado
            //
            lblPreguntaEstado.Location = new Point(16, 24);
            lblPreguntaEstado.Size = new Size(464, 20);
            lblPreguntaEstado.Text = "";
            lblPreguntaEstado.Name = "lblPreguntaEstado";
            //
            // lblPregunta
            //
            lblPregunta.AutoSize = true;
            lblPregunta.Location = new Point(16, 52);
            lblPregunta.Text = "Pregunta:";
            lblPregunta.Name = "lblPregunta";
            //
            // cmbPregunta
            //
            cmbPregunta.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPregunta.Location = new Point(120, 48);
            cmbPregunta.Size = new Size(360, 23);
            cmbPregunta.Name = "cmbPregunta";
            //
            // lblRespuesta
            //
            lblRespuesta.AutoSize = true;
            lblRespuesta.Location = new Point(16, 86);
            lblRespuesta.Text = "Respuesta:";
            lblRespuesta.Name = "lblRespuesta";
            //
            // txtRespuesta
            //
            txtRespuesta.Location = new Point(120, 82);
            txtRespuesta.Size = new Size(360, 23);
            txtRespuesta.MaxLength = 100;
            txtRespuesta.Name = "txtRespuesta";
            //
            // btnGuardarPregunta
            //
            btnGuardarPregunta.Location = new Point(290, 118);
            btnGuardarPregunta.Size = new Size(190, 30);
            btnGuardarPregunta.Text = "Guardar pregunta";
            btnGuardarPregunta.UseVisualStyleBackColor = true;
            btnGuardarPregunta.Name = "btnGuardarPregunta";
            btnGuardarPregunta.Click += btnGuardarPregunta_Click;
            //
            // btnCerrar
            //
            btnCerrar.Location = new Point(420, 522);
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Name = "btnCerrar";
            //
            // FrmMiPerfil
            //
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(540, 566);
            Text = "Mi perfil";
            Controls.Add(lblTitulo);
            Controls.Add(grpDatos);
            Controls.Add(grpContrasena);
            Controls.Add(grpPregunta);
            Controls.Add(btnCerrar);
            Name = "FrmMiPerfil";
            Load += FrmMiPerfil_Load;
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            grpContrasena.ResumeLayout(false);
            grpContrasena.PerformLayout();
            grpPregunta.ResumeLayout(false);
            grpPregunta.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private GroupBox grpDatos;
        private Label lblUsuario;
        private Label lblUsuarioValor;
        private Label lblRol;
        private Label lblRolValor;
        private Label lblIdioma;
        private ComboBox cmbIdioma;
        private Button btnAplicarIdioma;
        private GroupBox grpContrasena;
        private Label lblActual;
        private TextBox txtActual;
        private Label lblNueva;
        private TextBox txtNueva;
        private Label lblConfirmacion;
        private TextBox txtConfirmacion;
        private Button btnGuardarContrasena;
        private GroupBox grpPregunta;
        private Label lblPreguntaEstado;
        private Label lblPregunta;
        private ComboBox cmbPregunta;
        private Label lblRespuesta;
        private TextBox txtRespuesta;
        private Button btnGuardarPregunta;
        private Button btnCerrar;
    }
}
