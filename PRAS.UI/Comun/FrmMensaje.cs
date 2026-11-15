namespace PRAS.UI
{
    /// <summary>
    /// Cuadro de mensaje propio (reemplaza a MessageBox en Mensajes): el MessageBox de Windows no se
    /// puede pintar con los colores del sistema, así que en modo oscuro quedaba blanco. Este hereda de
    /// FormBase, así que toma el tema (claro u oscuro) y traduce sus botones como cualquier pantalla.
    /// No tiene .Designer: se arma por código.
    /// </summary>
    public class FrmMensaje : FormBase
    {
        public enum Tipo { Informacion, Advertencia, Error, Pregunta }

        private readonly string _texto;
        private readonly Label lblTexto = new Label();
        private readonly PictureBox picIcono = new PictureBox();
        private readonly Button btnAceptar = new Button();
        private readonly Button btnCancelar = new Button();

        /// <param name="texto">Ya traducido (lo traduce Mensajes antes de llamar).</param>
        public FrmMensaje(string texto, Tipo tipo)
        {
            _texto = texto ?? "";

            Name = "FrmMensaje";
            Text = "PRAS";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);

            picIcono.Image = IconoDe(tipo).ToBitmap();
            picIcono.SizeMode = PictureBoxSizeMode.CenterImage;
            picIcono.SetBounds(20, 22, 40, 40);

            // El tamaño del texto se calcula con lo que mide de verdad (así no se corta un mensaje largo).
            Size medida = TextRenderer.MeasureText(_texto, Font, new Size(420, 0), TextFormatFlags.WordBreak);
            int altoTexto = Math.Max(40, medida.Height + 8);
            lblTexto.Name = "txtMensaje";   // el nombre no termina en "Titulo": el tema no lo pinta de azul
            lblTexto.SetBounds(74, 22, 430, altoTexto);

            int arribaBotones = 22 + altoTexto + 20;
            bool esPregunta = tipo == Tipo.Pregunta;

            // "btnIngresar" se pinta como botón principal (azul); los textos se traducen solos (FormBase).
            btnAceptar.Name = "btnIngresar";
            btnAceptar.Text = esPregunta ? "Sí" : "Aceptar";
            btnAceptar.DialogResult = esPregunta ? DialogResult.Yes : DialogResult.OK;
            btnAceptar.SetBounds(esPregunta ? 284 : 404, arribaBotones, 110, 32);

            btnCancelar.Name = "btnNo";
            btnCancelar.Text = "No";
            btnCancelar.DialogResult = DialogResult.No;
            btnCancelar.SetBounds(404, arribaBotones, 110, 32);
            btnCancelar.Visible = esPregunta;

            AcceptButton = btnAceptar;
            CancelButton = esPregunta ? btnCancelar : btnAceptar;
            ClientSize = new Size(530, arribaBotones + 32 + 18);

            Controls.AddRange(new Control[] { picIcono, lblTexto, btnAceptar, btnCancelar });

            if (tipo == Tipo.Error)
                System.Media.SystemSounds.Hand.Play();
            else if (tipo == Tipo.Advertencia)
                System.Media.SystemSounds.Exclamation.Play();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // El texto se pone DESPUÉS de que FormBase tradujo la pantalla: ya viene traducido
            // y no hay que volver a buscarlo en el archivo de idioma.
            lblTexto.Text = _texto;
            lblTexto.ForeColor = Tema.Texto;
        }

        private static Icon IconoDe(Tipo tipo)
        {
            switch (tipo)
            {
                case Tipo.Advertencia: return SystemIcons.Warning;
                case Tipo.Error: return SystemIcons.Error;
                case Tipo.Pregunta: return SystemIcons.Question;
                default: return SystemIcons.Information;
            }
        }

        /// <summary>Muestra el mensaje sobre la ventana activa y devuelve el botón elegido.</summary>
        public static DialogResult Mostrar(string texto, Tipo tipo)
        {
            using (var frm = new FrmMensaje(texto, tipo))
            {
                Form duenio = Form.ActiveForm;
                return duenio != null && duenio.Visible ? frm.ShowDialog(duenio) : frm.ShowDialog();
            }
        }
    }
}
