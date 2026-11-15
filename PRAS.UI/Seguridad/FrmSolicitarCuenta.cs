using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// CU.Seguridad.016 - Solicitar cuenta (link "Solicitar acceso" de la pantalla de ingreso).
    /// La persona carga sus datos, el usuario y la contraseña (dos veces), una pregunta de seguridad
    /// y el rol que pide. Si pide Mecánico, además su especialidad, qué repara y su matrícula o experiencia.
    /// La cuenta queda DESHABILITADA hasta que un administrador la habilite (Mi perfil > Administrar usuarios).
    /// Todas las reglas están en la BLL (UsuarioService.SolicitarCuenta); esta pantalla solo junta los datos.
    /// No tiene .Designer: se arma por código.
    /// </summary>
    public class FrmSolicitarCuenta : FormBase
    {
        /// <summary>Especialidades sugeridas (se puede escribir otra). Se guardan en español.</summary>
        private static readonly string[] Especialidades =
            { "Mecánica general", "Chapa y pintura", "Electricidad", "Sacabollos", "Gomería", "Tapicería" };

        private readonly UsuarioService _usuarioService = new UsuarioService();

        private readonly Label lblTitulo = new Label();
        private readonly Label lblAviso = new Label();
        private readonly TextBox txtNombre = new TextBox();
        private readonly TextBox txtApellido = new TextBox();
        private readonly TextBox txtDNI = new TextBox();
        private readonly TextBox txtTelefono = new TextBox();
        private readonly TextBox txtEmail = new TextBox();
        private readonly TextBox txtUsuario = new TextBox();
        private readonly TextBox txtContrasena = new TextBox();
        private readonly TextBox txtConfirmacion = new TextBox();
        private readonly ComboBox cboPregunta = new ComboBox();
        private readonly TextBox txtRespuesta = new TextBox();
        private readonly ComboBox cboRol = new ComboBox();
        private readonly GroupBox grpMecanico = new GroupBox();
        private readonly ComboBox cboEspecialidad = new ComboBox();
        private readonly TextBox txtQueRepara = new TextBox();
        private readonly TextBox txtMatricula = new TextBox();
        private readonly Button btnEnviar = new Button();
        private readonly Button btnCancelar = new Button();

        /// <summary>Usuario pedido (el ingreso lo deja escrito en la casilla "Usuario").</summary>
        public string UsuarioSolicitado { get; private set; }

        public FrmSolicitarCuenta()
        {
            // SuspendLayout/ResumeLayout: al cerrar el bloque, WinForms agranda todo según la escala
            // de Windows (AutoScaleMode.Dpi), igual que en las pantallas hechas con el diseñador.
            SuspendLayout();
            Name = "FrmSolicitarCuenta";
            Text = "Solicitar acceso";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 9.75F);
            ClientSize = new Size(760, 650);

            lblTitulo.Name = "lblTitulo";
            lblTitulo.Text = "Solicitar acceso";

            lblAviso.Name = "lblAviso";
            lblAviso.Text = "La cuenta queda deshabilitada hasta que un administrador la revise y la habilite.";
            lblAviso.SetBounds(24, 66, 712, 24);
            lblAviso.ForeColor = Tema.Peligro;

            // Columna izquierda: datos personales. Columna derecha: datos de la cuenta.
            int y = 100;
            Campo("Nombre: *", txtNombre, 24, ref y);
            Campo("Apellido: *", txtApellido, 24, ref y);
            txtDNI.Name = "txtDNI";   // Entradas: solo números
            Campo("DNI: *", txtDNI, 24, ref y);
            txtTelefono.Name = "txtTelefono";
            Campo("Teléfono: *", txtTelefono, 24, ref y);
            Campo("Email: *", txtEmail, 24, ref y);
            int finIzquierda = y;

            y = 100;
            Campo("Usuario: *", txtUsuario, 392, ref y);
            txtContrasena.UseSystemPasswordChar = true;
            Campo("Contraseña: *", txtContrasena, 392, ref y);
            txtConfirmacion.UseSystemPasswordChar = true;
            Campo("Repetir contraseña: *", txtConfirmacion, 392, ref y);
            cboPregunta.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPregunta.Items.AddRange(_usuarioService.PreguntasDeSeguridad().ToArray());   // se ven traducidas
            Campo("Pregunta de seguridad: *", cboPregunta, 392, ref y);
            Campo("Respuesta: *", txtRespuesta, 392, ref y);

            y = Math.Max(y, finIzquierda);
            cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRol.Items.AddRange(new object[] { "Mecánico", "Dueño", "Administrador" });   // se ven traducidos
            cboRol.SelectedIndex = 0;
            cboRol.SelectedIndexChanged += (s, e) => grpMecanico.Visible = RolElegido() == Rol.Mecanico;
            Campo("Rol que solicita: *", cboRol, 24, ref y);

            // Datos que pide solo el mecánico (se crea también su registro en la tabla Mecanico).
            grpMecanico.Text = "Datos del mecánico";
            grpMecanico.SetBounds(24, y, 712, 150);
            int yg = 26;
            var lblEspecialidad = new Label { Text = "Especialidad: *", AutoSize = true, Location = new Point(14, yg + 4) };
            cboEspecialidad.DropDownStyle = ComboBoxStyle.DropDown;   // se puede escribir otra
            cboEspecialidad.Items.AddRange(Especialidades);
            cboEspecialidad.SetBounds(220, yg, 470, 26);
            var lblQueRepara = new Label { Text = "Qué repara: *", AutoSize = true, Location = new Point(14, yg + 40) };
            txtQueRepara.SetBounds(220, yg + 36, 470, 26);
            var lblMatricula = new Label { Text = "Matrícula / experiencia:", AutoSize = true, Location = new Point(14, yg + 76) };
            txtMatricula.SetBounds(220, yg + 72, 470, 26);
            grpMecanico.Controls.AddRange(new Control[] { lblEspecialidad, cboEspecialidad, lblQueRepara, txtQueRepara, lblMatricula, txtMatricula });

            btnEnviar.Name = "btnGuardar";   // estilo de botón principal (azul)
            btnEnviar.Text = "Enviar solicitud";
            btnEnviar.SetBounds(476, 600, 150, 34);
            btnEnviar.Click += btnEnviar_Click;
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Text = "Cancelar";
            btnCancelar.SetBounds(636, 600, 100, 34);
            btnCancelar.Click += (s, e) => Close();
            CancelButton = btnCancelar;

            Controls.AddRange(new Control[] { lblTitulo, lblAviso, grpMecanico, btnEnviar, btnCancelar });
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>Agrega "etiqueta" arriba y el control abajo, en la columna x, y baja y.</summary>
        private void Campo(string etiqueta, Control control, int x, ref int y)
        {
            var lbl = new Label { Text = etiqueta, AutoSize = true, Location = new Point(x, y) };
            control.SetBounds(x, y + 20, 344, 26);
            Controls.Add(lbl);
            Controls.Add(control);
            y += 56;
        }

        private Rol RolElegido()
        {
            switch (cboRol.SelectedIndex)
            {
                case 1: return Rol.Dueno;
                case 2: return Rol.Administrador;
                default: return Rol.Mecanico;
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                Rol rol = RolElegido();
                var solicitud = new SolicitudCuenta
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    DNI = txtDNI.Text,
                    Telefono = txtTelefono.Text,
                    Email = txtEmail.Text,
                    NombreUsuario = txtUsuario.Text,
                    RolSolicitado = rol,
                    PreguntaSeguridad = cboPregunta.SelectedItem as string,   // en español
                    Especialidad = rol == Rol.Mecanico ? EspecialidadElegida() : null,
                    QueRepara = rol == Rol.Mecanico ? txtQueRepara.Text : null,
                    MatriculaExperiencia = rol == Rol.Mecanico ? txtMatricula.Text : null
                };

                _usuarioService.SolicitarCuenta(solicitud, txtContrasena.Text, txtConfirmacion.Text, txtRespuesta.Text);

                Mensajes.Info("Su solicitud fue registrada. La cuenta queda deshabilitada hasta que un administrador la habilite.".Traducir());
                UsuarioSolicitado = txtUsuario.Text.Trim();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Si eligió una de la lista, el valor en español (lo que se ve está traducido); si escribió otra, lo escrito.</summary>
        private string EspecialidadElegida()
        {
            bool deLaLista = cboEspecialidad.SelectedIndex >= 0
                && cboEspecialidad.Text == cboEspecialidad.GetItemText(cboEspecialidad.SelectedItem);
            return deLaLista ? (string)cboEspecialidad.SelectedItem : cboEspecialidad.Text;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ValoresTraducidos.ConectarEditable(cboEspecialidad);
        }
    }
}
