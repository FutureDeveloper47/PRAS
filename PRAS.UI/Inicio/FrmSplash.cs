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

        // Tamaño "de diseño" de la pantalla, pensado para escala 100 % (96 ppp).
        // En pantallas al 125 % o 150 % todo se agranda en la misma proporción
        // (ver Escala), así nada se corre ni se encima.
        private const int AnchoDiseno = 640;
        private const int AltoDiseno = 380;

        // Zona prohibida para los textos de arriba: a la derecha están la llave y el auto.
        private const float AnchoMaximoTitulos = 440f;

        public FrmSplash()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = true;
            TopMost = true;
            Text = "PRAS";
            DoubleBuffered = true;   // dibuja sin parpadeo
            AutoScaleMode = AutoScaleMode.None;   // la escala la manejamos a mano con Escala
            ClientSize = new Size((int)(AnchoDiseno * Escala), (int)(AltoDiseno * Escala));

            try
            {
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch
            {
                // si no se puede leer el ícono, sigue con el de Windows
            }
        }

        /// <summary>
        /// Factor de escala de la pantalla (1 = 100 %, 1,25 = 125 %, 1,5 = 150 %).
        /// Se usa para agrandar el dibujo entero en vez de mover cada cosa por separado.
        /// </summary>
        private float Escala => DeviceDpi / 96f;

        /// <summary>Cambia el texto de abajo y avanza la barra (se redibuja enseguida).</summary>
        public void Avanzar(string estado, int progreso)
        {
            _estado = estado;
            _progreso = Math.Max(0, Math.Min(100, progreso));
            Invalidate();
            Update();   // dibuja ahora mismo, sin esperar
        }

        /// <summary>Si la ventana se mueve a un monitor con otra escala, se ajusta el tamaño.</summary>
        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            ClientSize = new Size((int)(AnchoDiseno * Escala), (int)(AltoDiseno * Escala));
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            // Desde acá dibujamos siempre en el espacio de diseño de 640 x 380:
            // la transformación agranda todo según la escala de Windows.
            g.ScaleTransform(Escala, Escala);
            var area = new Rectangle(0, 0, AnchoDiseno, AltoDiseno);

            // 1) Fondo: degradé del azul del sistema a un azul más oscuro.
            using (var fondo = new LinearGradientBrush(area, Tema.PrimarioHover, Tema.PrimarioOscuro, 60f))
                g.FillRectangle(fondo, area);

            // 2) "Ruta": una línea punteada como la de una calle.
            using (var ruta = new Pen(Color.FromArgb(70, Color.White), 3) { DashStyle = DashStyle.Dash })
                g.DrawLine(ruta, 40, 262, 600, 262);

            // 3) Auto y llave de taller (a la derecha, lejos de los textos).
            DibujarAuto(g, 330, 156);
            DibujarLlave(g, 570, 64);

            // 4) Textos. Las fuentes van en píxeles "de diseño" porque la escala ya la pone ScaleTransform
            //    (si fueran en puntos, Windows las agrandaría dos veces).
            using var formato = (StringFormat)StringFormat.GenericTypographic.Clone();
            formato.FormatFlags |= StringFormatFlags.NoWrap;
            using (var blanco = new SolidBrush(Color.White))
            using (var gris = new SolidBrush(Color.FromArgb(200, 220, 235)))
            using (var chico = new Font("Segoe UI", 12F, GraphicsUnit.Pixel))
            {
                // Cada renglón se ubica debajo del anterior según lo que mide de verdad,
                // así no se pisan aunque cambie la fuente o la escala.
                float y = 30f;
                y = DibujarRenglon(g, "PRAS", "Segoe UI", 54F, FontStyle.Bold, blanco, 36f, y, formato) + 8f;
                y = DibujarRenglon(g, "Programa de Registro Automotor Superior", "Segoe UI", 16F, FontStyle.Regular, gris, 40f, y, formato) + 6f;
                DibujarRenglon(g, "Compra de autos chocados  ·  Taller  ·  Agencia", "Segoe UI", 12F, FontStyle.Regular, gris, 40f, y, formato);

                // 5) Barra de progreso y, bien separado arriba, el texto de estado.
                var barra = new RectangleF(40, 330, 560, 8);
                float altoEstado = g.MeasureString(_estado, chico, AnchoDiseno, formato).Height;
                float yEstado = barra.Y - 14f - altoEstado;   // 14 px de aire entre el texto y la barra
                g.DrawString(_estado, chico, blanco, new RectangleF(40, yEstado, 560, altoEstado + 2), formato);

                using (var vacio = new SolidBrush(Color.FromArgb(60, Color.White)))
                    g.FillRectangle(vacio, barra);
                using (var lleno = new SolidBrush(Color.White))
                    g.FillRectangle(lleno, barra.X, barra.Y, barra.Width * _progreso / 100f, barra.Height);

                g.DrawString("Matías Montoto  ·  UAI  ·  Prácticas Profesionalizantes III", chico, gris, 40, 352, formato);
            }

            // 6) Borde fino.
            using (var borde = new Pen(Tema.PrimarioOscuro))
                g.DrawRectangle(borde, 0, 0, AnchoDiseno - 1, AltoDiseno - 1);
        }

        /// <summary>
        /// Dibuja un renglón de texto y devuelve dónde termina (coordenada Y de abajo).
        /// Si el texto no entra en el ancho permitido, achica la fuente hasta que entre,
        /// para que nunca llegue a la llave ni al auto.
        /// </summary>
        private static float DibujarRenglon(Graphics g, string texto, string familia, float tamano, FontStyle estilo,
                                            Brush pincel, float x, float y, StringFormat formato)
        {
            float tam = tamano;
            Font fuente = new Font(familia, tam, estilo, GraphicsUnit.Pixel);
            SizeF medida = g.MeasureString(texto, fuente, int.MaxValue, formato);
            while (medida.Width > AnchoMaximoTitulos && tam > 8f)
            {
                fuente.Dispose();
                tam -= 1f;
                fuente = new Font(familia, tam, estilo, GraphicsUnit.Pixel);
                medida = g.MeasureString(texto, fuente, int.MaxValue, formato);
            }

            using (fuente)
                g.DrawString(texto, fuente, pincel, x, y, formato);

            // Usamos el alto de línea de la fuente (no solo la tinta) para que entre
            // renglones haya siempre el mismo aire.
            return y + medida.Height;
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
