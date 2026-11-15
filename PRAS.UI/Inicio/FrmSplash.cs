using System.Drawing.Drawing2D;

namespace PRAS.UI
{
    /// <summary>
    /// Pantalla de presentación que aparece al abrir el programa, mientras se revisa la base de datos.
    /// No tiene archivo .Designer: todo se dibuja por código (OnPaint) con GDI+:
    /// fondo azul con degradé, un auto y una llave de taller, el nombre del sistema
    /// y una barra de progreso con el paso que se está haciendo.
    /// Hereda de Form (no de FormBase) porque no lleva el tema ni la franja de título.
    /// </summary>
    public class FrmSplash : Form
    {
        private string _estado = "Iniciando...";
        private int _progreso;   // de 0 a 100

        public FrmSplash()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(640, 380);
            ShowInTaskbar = true;
            TopMost = true;
            Text = "PRAS";
            DoubleBuffered = true;   // dibuja sin parpadeo

            try
            {
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch
            {
                // si no se puede leer el ícono, sigue con el de Windows
            }
        }

        /// <summary>Cambia el texto de abajo y avanza la barra (se redibuja enseguida).</summary>
        public void Avanzar(string estado, int progreso)
        {
            _estado = estado;
            _progreso = Math.Max(0, Math.Min(100, progreso));
            Invalidate();
            Update();   // dibuja ahora mismo, sin esperar
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle area = ClientRectangle;

            // 1) Fondo: degradé del azul del sistema a un azul más oscuro.
            using (var fondo = new LinearGradientBrush(area, Tema.PrimarioHover, Tema.PrimarioOscuro, 60f))
                g.FillRectangle(fondo, area);

            // 2) "Ruta": una línea punteada como la de una calle.
            using (var ruta = new Pen(Color.FromArgb(70, Color.White), 3) { DashStyle = DashStyle.Dash })
                g.DrawLine(ruta, 40, 250, 600, 250);

            // 3) Auto y llave de taller.
            DibujarAuto(g, 330, 140);
            DibujarLlave(g, 560, 70);

            // 4) Textos.
            using (var titulo = new Font("Segoe UI", 40F, FontStyle.Bold))
            using (var subtitulo = new Font("Segoe UI", 12F))
            using (var chico = new Font("Segoe UI", 9F))
            using (var blanco = new SolidBrush(Color.White))
            using (var gris = new SolidBrush(Color.FromArgb(200, 220, 235)))
            {
                g.DrawString("PRAS", titulo, blanco, 36, 40);
                g.DrawString("Programa de Registro Automotor Superior", subtitulo, gris, 42, 115);
                g.DrawString("Compra de autos chocados  ·  Taller  ·  Agencia", chico, gris, 42, 142);

                g.DrawString(_estado, chico, blanco, 40, 300);
                g.DrawString("Matías Montoto  ·  UAI  ·  Prácticas Profesionalizantes III", chico, gris, 40, 350);
            }

            // 5) Barra de progreso.
            var barra = new Rectangle(40, 322, 560, 8);
            using (var vacio = new SolidBrush(Color.FromArgb(60, Color.White)))
                g.FillRectangle(vacio, barra);
            using (var lleno = new SolidBrush(Color.White))
                g.FillRectangle(lleno, barra.X, barra.Y, barra.Width * _progreso / 100, barra.Height);

            // 6) Borde fino.
            using (var borde = new Pen(Tema.PrimarioOscuro))
                g.DrawRectangle(borde, 0, 0, area.Width - 1, area.Height - 1);
        }

        /// <summary>Silueta de un auto (carrocería, ventanillas y ruedas) con su esquina superior izquierda en (x, y).</summary>
        private static void DibujarAuto(Graphics g, int x, int y)
        {
            using var carroceria = new GraphicsPath();
            carroceria.AddLines(new[]
            {
                new Point(x, y + 75), new Point(x + 10, y + 50), new Point(x + 70, y + 42),
                new Point(x + 100, y + 10), new Point(x + 180, y + 8), new Point(x + 220, y + 42),
                new Point(x + 265, y + 52), new Point(x + 272, y + 78), new Point(x, y + 82)
            });
            carroceria.CloseFigure();

            using (var pintura = new SolidBrush(Color.FromArgb(235, Color.White)))
                g.FillPath(pintura, carroceria);

            // Ventanillas
            using (var vidrio = new SolidBrush(Tema.Primario))
            {
                g.FillPolygon(vidrio, new[] { new Point(x + 82, y + 42), new Point(x + 106, y + 17), new Point(x + 140, y + 16), new Point(x + 140, y + 42) });
                g.FillPolygon(vidrio, new[] { new Point(x + 148, y + 16), new Point(x + 176, y + 16), new Point(x + 206, y + 42), new Point(x + 148, y + 42) });
            }

            // Ruedas: neumático oscuro y llanta clara.
            foreach (int centro in new[] { x + 62, x + 212 })
            {
                using (var neumatico = new SolidBrush(Tema.PrimarioOscuro))
                    g.FillEllipse(neumatico, centro - 24, y + 58, 48, 48);
                using (var llanta = new SolidBrush(Color.FromArgb(220, 230, 240)))
                    g.FillEllipse(llanta, centro - 11, y + 71, 22, 22);
            }
        }

        /// <summary>Llave fija de taller, inclinada.</summary>
        private static void DibujarLlave(Graphics g, int x, int y)
        {
            GraphicsState estado = g.Save();
            g.TranslateTransform(x, y);
            g.RotateTransform(40);

            using (var metal = new SolidBrush(Color.FromArgb(150, Color.White)))
            {
                g.FillRectangle(metal, -6, 0, 12, 70);         // mango
                g.FillEllipse(metal, -16, -22, 32, 32);         // cabeza
            }
            using (var hueco = new SolidBrush(Tema.PrimarioHover))
                g.FillRectangle(hueco, -6, -26, 12, 18);        // boca de la llave

            g.Restore(estado);
        }
    }
}
