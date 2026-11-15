using System.Runtime.InteropServices;

namespace PRAS.UI
{
    /// <summary>
    /// Aspecto visual de TODO el sistema, en un solo lugar.
    /// Cada pantalla no elige
    /// sus colores: los toma de acá. Si mañana querés otro color, lo cambiás una sola vez.
    ///
    /// Se aplica solo, al abrir cada pantalla, porque todas heredan de FormBase.
    ///
    /// MODO OSCURO: hay dos paletas (Claro y Oscuro) con los mismos nombres de colores.
    /// Tema.Primario, Tema.Fondo, etc. devuelven el color de la paleta elegida, así el resto del
    /// programa no cambia. Al cambiar de tema, CambiarModo repinta las ventanas abiertas: recorre
    /// cada control y reemplaza cada color de la paleta vieja por el mismo color de la paleta nueva.
    /// La preferencia se guarda por usuario en un archivo local (PreferenciasLocales), no en la base.
    /// </summary>
    public static class Tema
    {
        /// <summary>Los colores de un tema. Las dos paletas tienen los mismos nombres.</summary>
        private sealed class Paleta
        {
            public Color Primario;         // relleno: franja del título, botón principal, encabezado de grillas
            public Color PrimarioHover;    // cuando pasás el mouse
            public Color PrimarioOscuro;   // cuando hacés clic
            public Color Acento;           // texto "azul" (títulos, links): en oscuro es más claro para que se lea
            public Color Fondo;            // fondo de las pantallas
            public Color Superficie;       // tarjetas y grillas
            public Color Entrada;          // fondo de los cuadros donde se escribe
            public Color Borde;
            public Color Texto;
            public Color TextoApagado;
            public Color Peligro;          // rojo (Eliminar)
            public Color PeligroFondo;
            public Color Exito;            // verde (texto)
            public Color ExitoFondo;
            public Color AdvertenciaFondo; // amarillo suave
            public Color FilaAlterna;
            public Color Seleccion;
            public Color FondoResaltado;   // tarjeta con el mouse encima
        }

        private static readonly Paleta Claro = new Paleta
        {
            Primario = Color.FromArgb(31, 78, 121),
            PrimarioHover = Color.FromArgb(44, 100, 150),
            PrimarioOscuro = Color.FromArgb(22, 58, 92),
            Acento = Color.FromArgb(31, 78, 121),
            Fondo = Color.FromArgb(243, 245, 248),
            Superficie = Color.White,
            Entrada = Color.White,
            Borde = Color.FromArgb(214, 220, 228),
            Texto = Color.FromArgb(31, 41, 51),
            TextoApagado = Color.FromArgb(150, 158, 170),
            Peligro = Color.FromArgb(192, 57, 43),
            PeligroFondo = Color.FromArgb(253, 236, 236),
            Exito = Color.FromArgb(46, 125, 50),
            ExitoFondo = Color.FromArgb(232, 245, 233),
            AdvertenciaFondo = Color.FromArgb(255, 248, 220),
            FilaAlterna = Color.FromArgb(247, 249, 252),
            Seleccion = Color.FromArgb(214, 230, 245),
            FondoResaltado = Color.FromArgb(240, 246, 252)
        };

        /// <summary>
        /// Paleta oscura. Contraste revisado: texto claro (E6E8EB) sobre fondos oscuros (1E1F22 / 2B2D31),
        /// el azul de los títulos es más claro (8AB8F0) y el blanco sobre el azul de relleno (2F6DB5) se lee bien.
        /// </summary>
        private static readonly Paleta Oscura = new Paleta
        {
            Primario = Color.FromArgb(47, 109, 181),
            PrimarioHover = Color.FromArgb(58, 124, 198),
            PrimarioOscuro = Color.FromArgb(37, 90, 150),
            Acento = Color.FromArgb(138, 184, 240),
            Fondo = Color.FromArgb(30, 31, 34),
            Superficie = Color.FromArgb(43, 45, 49),
            Entrada = Color.FromArgb(35, 36, 40),
            Borde = Color.FromArgb(70, 74, 82),
            Texto = Color.FromArgb(230, 232, 235),
            TextoApagado = Color.FromArgb(160, 166, 175),
            Peligro = Color.FromArgb(255, 120, 110),
            PeligroFondo = Color.FromArgb(74, 38, 38),
            Exito = Color.FromArgb(120, 210, 130),
            ExitoFondo = Color.FromArgb(35, 64, 42),
            AdvertenciaFondo = Color.FromArgb(74, 66, 32),
            FilaAlterna = Color.FromArgb(48, 50, 55),
            Seleccion = Color.FromArgb(54, 87, 125),
            FondoResaltado = Color.FromArgb(52, 56, 62)
        };

        private static Paleta _actual = Claro;

        /// <summary>true si está activo el modo oscuro.</summary>
        public static bool Oscuro => _actual == Oscura;

        // Colores (los usan todas las pantallas). Devuelven el color de la paleta activa.
        public static Color Primario => _actual.Primario;
        public static Color PrimarioHover => _actual.PrimarioHover;
        public static Color PrimarioOscuro => _actual.PrimarioOscuro;
        public static Color Acento => _actual.Acento;
        public static Color Fondo => _actual.Fondo;
        public static Color Superficie => _actual.Superficie;
        public static Color Entrada => _actual.Entrada;
        public static Color Borde => _actual.Borde;
        public static Color Texto => _actual.Texto;
        public static Color TextoApagado => _actual.TextoApagado;
        public static Color Peligro => _actual.Peligro;
        public static Color PeligroFondo => _actual.PeligroFondo;
        public static Color Exito => _actual.Exito;
        public static Color ExitoFondo => _actual.ExitoFondo;
        public static Color AdvertenciaFondo => _actual.AdvertenciaFondo;
        public static Color FilaAlterna => _actual.FilaAlterna;
        public static Color Seleccion => _actual.Seleccion;
        public static Color FondoResaltado => _actual.FondoResaltado;

        private const int AltoEncabezado = 56;
        private const int AltoFranjaTarjeta = 4;

        /// <summary>Aplica el tema a una pantalla y a todos sus controles.</summary>
        public static void Aplicar(Form form)
        {
            form.BackColor = Fondo;
            form.ForeColor = Texto;   // los controles toman este color si no tienen uno propio

            AplicarAControles(form.Controls);
            ArmarEncabezado(form);

            // Lo que vino del diseñador con colores "de Windows" (o del tema claro) pasa al tema activo.
            if (Oscuro)
                Recolorear(form, Claro, Oscura);
            AplicarBarraDeTitulo(form);
        }

        // Cambiar de tema

        /// <summary>
        /// Cambia entre claro y oscuro y repinta TODAS las ventanas abiertas, sin reiniciar.
        /// </summary>
        public static void CambiarModo(bool oscuro)
        {
            Paleta anterior = _actual;
            _actual = oscuro ? Oscura : Claro;
            if (anterior == _actual)
                return;

            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                // Las ventanas que no son del sistema (ej: la presentación) se repintan solas al dibujarse.
                if (form is FormBase)
                {
                    form.SuspendLayout();
                    Recolorear(form, anterior, _actual);
                    AplicarBarraDeTitulo(form);
                    form.ResumeLayout(true);
                }
                form.Invalidate(true);
            }
        }

        /// <summary>
        /// Recorre la ventana y cambia cada color de la paleta "desde" por el mismo color de la paleta "hacia".
        /// Así también cambian los colores que las pantallas pusieron por código (Tema.Peligro, Tema.ExitoFondo...).
        /// </summary>
        private static void Recolorear(Control raiz, Paleta desde, Paleta hacia)
        {
            Dictionary<int, Color> fondos = MapaDeFondos(desde, hacia);
            Dictionary<int, Color> textos = MapaDeTextos(desde, hacia);
            RecolorearControl(raiz, fondos, textos);
        }

        private static void RecolorearControl(Control control, Dictionary<int, Color> fondos, Dictionary<int, Color> textos)
        {
            control.BackColor = Cambiar(control.BackColor, fondos);
            control.ForeColor = Cambiar(control.ForeColor, textos);

            switch (control)
            {
                case Button boton:
                    boton.FlatAppearance.BorderColor = Cambiar(boton.FlatAppearance.BorderColor, fondos);
                    boton.FlatAppearance.MouseOverBackColor = Cambiar(boton.FlatAppearance.MouseOverBackColor, fondos);
                    boton.FlatAppearance.MouseDownBackColor = Cambiar(boton.FlatAppearance.MouseDownBackColor, fondos);
                    break;
                case CheckBox casilla when casilla.Appearance == Appearance.Button:
                case RadioButton opcion when opcion.Appearance == Appearance.Button:
                    var boton2 = (ButtonBase)control;
                    boton2.FlatAppearance.BorderColor = Cambiar(boton2.FlatAppearance.BorderColor, fondos);
                    boton2.FlatAppearance.CheckedBackColor = Cambiar(boton2.FlatAppearance.CheckedBackColor, fondos);
                    boton2.FlatAppearance.MouseOverBackColor = Cambiar(boton2.FlatAppearance.MouseOverBackColor, fondos);
                    break;
                case DataGridView grilla:
                    RecolorearGrilla(grilla, fondos, textos);
                    return;   // las partes internas de la grilla no se recorren
                case ToolStrip barra:
                    foreach (ToolStripItem item in barra.Items)
                        RecolorearItem(item, textos);
                    break;
                case TabControl pestanias:
                    pestanias.Invalidate();
                    break;
                case TabPage pagina:
                    pagina.UseVisualStyleBackColor = false;
                    break;
                case LinkLabel link:
                    link.LinkColor = Oscuro ? Acento : Color.FromArgb(0, 0, 255);
                    link.ActiveLinkColor = link.LinkColor;
                    link.VisitedLinkColor = link.LinkColor;
                    break;
            }

            BarrasDeDesplazamiento(control);

            foreach (Control hijo in control.Controls)
                RecolorearControl(hijo, fondos, textos);
        }

        private static void RecolorearItem(ToolStripItem item, Dictionary<int, Color> textos)
        {
            // La barra de estado de abajo es azul con letra blanca: el blanco no se toca.
            item.ForeColor = Cambiar(item.ForeColor, textos);
            if (item is ToolStripMenuItem menu)
                foreach (ToolStripItem hijo in menu.DropDownItems)
                    RecolorearItem(hijo, textos);
        }

        private static void RecolorearGrilla(DataGridView grilla, Dictionary<int, Color> fondos, Dictionary<int, Color> textos)
        {
            grilla.BackgroundColor = Cambiar(grilla.BackgroundColor, fondos);
            grilla.GridColor = Cambiar(grilla.GridColor, fondos);
            foreach (DataGridViewCellStyle estilo in new[] { grilla.DefaultCellStyle, grilla.AlternatingRowsDefaultCellStyle,
                                                              grilla.ColumnHeadersDefaultCellStyle, grilla.RowHeadersDefaultCellStyle })
            {
                estilo.BackColor = Cambiar(estilo.BackColor, fondos);
                estilo.SelectionBackColor = Cambiar(estilo.SelectionBackColor, fondos);
                estilo.ForeColor = Cambiar(estilo.ForeColor, textos);
                estilo.SelectionForeColor = Cambiar(estilo.SelectionForeColor, textos);
            }
            foreach (DataGridViewRow fila in grilla.Rows)
            {
                if (fila.HasDefaultCellStyle)
                {
                    fila.DefaultCellStyle.BackColor = Cambiar(fila.DefaultCellStyle.BackColor, fondos);
                    fila.DefaultCellStyle.ForeColor = Cambiar(fila.DefaultCellStyle.ForeColor, textos);
                }
            }
            BarrasDeDesplazamiento(grilla);
            grilla.Invalidate();
        }

        private static Color Cambiar(Color color, Dictionary<int, Color> mapa)
        {
            if (color.IsEmpty)
                return color;
            return mapa.TryGetValue(color.ToArgb(), out Color nuevo) ? nuevo : color;
        }

        /// <summary>Colores de FONDO: cada uno de la paleta vieja con su equivalente nuevo (más los de Windows).</summary>
        private static Dictionary<int, Color> MapaDeFondos(Paleta desde, Paleta hacia)
        {
            var mapa = new Dictionary<int, Color>();
            void Par(Color de, Color a) => mapa.TryAdd(de.ToArgb(), a);

            Par(desde.Primario, hacia.Primario);
            Par(desde.PrimarioHover, hacia.PrimarioHover);
            Par(desde.PrimarioOscuro, hacia.PrimarioOscuro);
            Par(desde.Fondo, hacia.Fondo);
            Par(desde.Superficie, hacia.Superficie);
            Par(desde.Entrada, hacia.Entrada);
            Par(desde.Borde, hacia.Borde);
            Par(desde.PeligroFondo, hacia.PeligroFondo);
            Par(desde.Peligro, hacia.Peligro);
            Par(desde.ExitoFondo, hacia.ExitoFondo);
            Par(desde.AdvertenciaFondo, hacia.AdvertenciaFondo);
            Par(desde.FilaAlterna, hacia.FilaAlterna);
            Par(desde.Seleccion, hacia.Seleccion);
            Par(desde.FondoResaltado, hacia.FondoResaltado);

            // Colores "de Windows" que traen los diseñadores (solo al pasar a oscuro).
            if (hacia == Oscura)
            {
                Par(SystemColors.Control, hacia.Fondo);
                Par(SystemColors.Window, hacia.Entrada);
                Par(SystemColors.ControlLight, hacia.Superficie);
                Par(SystemColors.ButtonFace, hacia.Superficie);
            }
            return mapa;
        }

        /// <summary>Colores de TEXTO. El blanco no se toca: es el texto sobre el azul (franjas y botones).</summary>
        private static Dictionary<int, Color> MapaDeTextos(Paleta desde, Paleta hacia)
        {
            var mapa = new Dictionary<int, Color>();
            void Par(Color de, Color a) => mapa.TryAdd(de.ToArgb(), a);

            Par(desde.Texto, hacia.Texto);
            Par(desde.TextoApagado, hacia.TextoApagado);
            Par(desde.Acento, hacia.Acento);
            Par(desde.Primario, hacia.Acento);   // un texto "azul" sobre la tarjeta usa el azul que se lee en cada tema
            Par(desde.Peligro, hacia.Peligro);
            Par(desde.Exito, hacia.Exito);

            if (hacia == Oscura)
            {
                Par(SystemColors.ControlText, hacia.Texto);
                Par(SystemColors.WindowText, hacia.Texto);
                Par(Color.Black, hacia.Texto);
                Par(SystemColors.GrayText, hacia.TextoApagado);
            }
            return mapa;
        }

        // Barra de título de Windows (la de arriba, con la X): en Windows 10/11 se puede pedir oscura.

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int atributo, ref int valor, int tamanio);

        private const int DwmaModoOscuro = 20;   // DWMWA_USE_IMMERSIVE_DARK_MODE

        private static void AplicarBarraDeTitulo(Form form)
        {
            if (!form.IsHandleCreated)
                return;
            try
            {
                int valor = Oscuro ? 1 : 0;
                DwmSetWindowAttribute(form.Handle, DwmaModoOscuro, ref valor, sizeof(int));
            }
            catch
            {
                // En versiones de Windows que no lo soportan, la barra queda como está.
            }
        }

        /// <summary>
        /// Recorre los controles (y los que están adentro de paneles) y le da estilo a cada uno
        /// según su tipo. "switch" con tipos = "si es un Button, hacé esto; si es una grilla, esto otro".
        /// </summary>
        private static void AplicarAControles(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                switch (control)
                {
                    case Button boton:
                        EstilizarBoton(boton);
                        break;
                    case RadioButton opcion when opcion.Appearance == Appearance.Button:
                        EstilizarBotonOpcion(opcion);
                        break;
                    case DataGridView grilla:
                        EstilizarGrilla(grilla);
                        break;
                    case TextBox texto:
                        texto.BorderStyle = BorderStyle.FixedSingle;
                        // Los de solo lectura quedan con el fondo de la pantalla (así se nota que no se escriben).
                        texto.BackColor = texto.ReadOnly ? Fondo : Entrada;
                        texto.ForeColor = Texto;
                        break;
                    case NumericUpDown numero:
                        numero.BorderStyle = BorderStyle.FixedSingle;
                        numero.BackColor = Entrada;
                        numero.ForeColor = Texto;
                        break;
                    case ComboBox combo:
                        combo.FlatStyle = FlatStyle.Flat;
                        combo.BackColor = Entrada;
                        combo.ForeColor = Texto;
                        // Los combos "de elegir" (DropDownList) los pinta Windows siempre en blanco:
                        // los dibujamos nosotros para que respeten el tema (también en modo oscuro).
                        if (combo.DropDownStyle == ComboBoxStyle.DropDownList && combo.DrawMode == DrawMode.Normal)
                        {
                            combo.DrawMode = DrawMode.OwnerDrawFixed;
                            combo.DrawItem += DibujarOpcionCombo;
                        }
                        break;
                    case ListBox lista:
                        lista.BorderStyle = BorderStyle.FixedSingle;
                        lista.BackColor = Entrada;
                        lista.ForeColor = Texto;
                        break;
                    case GroupBox grupo:
                        grupo.ForeColor = Texto;
                        break;
                    case TabControl pestanias:
                        EstilizarPestanias(pestanias);
                        break;
                    case TabPage pagina:
                        // Con UseVisualStyleBackColor Windows la pinta siempre clara (en oscuro quedaba blanca).
                        pagina.UseVisualStyleBackColor = false;
                        pagina.BackColor = Fondo;
                        pagina.ForeColor = Texto;
                        break;
                    case TreeView arbol:
                        arbol.BackColor = Entrada;
                        arbol.ForeColor = Texto;
                        arbol.LineColor = TextoApagado;
                        arbol.BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case StatusStrip barra:
                        barra.BackColor = Primario;
                        barra.ForeColor = Color.White;
                        barra.SizingGrip = false;
                        barra.Renderer = new RendererMenu(esBarraDeEstado: true);
                        break;
                    case MenuStrip menu:
                        menu.BackColor = Superficie;
                        menu.Renderer = new RendererMenu(esBarraDeEstado: false);
                        menu.Padding = new Padding(6, 4, 0, 4);
                        break;
                    case Panel panel:
                        EstilizarPanel(panel);
                        break;
                    case Label etiqueta:
                        EstilizarEtiqueta(etiqueta);
                        break;
                }

                BarrasDeDesplazamiento(control);

                // Si un control tiene otros adentro (por ejemplo un Panel), los recorremos también.
                // Grillas, combos y números tienen partes internas que no hay que tocar.
                bool recorrerHijos = control.HasChildren
                    && !(control is DataGridView) && !(control is NumericUpDown) && !(control is ComboBox);
                if (recorrerHijos)
                    AplicarAControles(control.Controls);
            }
        }

        /// <summary>
        /// Dibuja cada opción de un combo con los colores del tema. El texto sale de GetItemText,
        /// que respeta DisplayMember y el evento Format (o sea, los valores traducidos).
        /// </summary>
        private static void DibujarOpcionCombo(object sender, DrawItemEventArgs e)
        {
            var combo = (ComboBox)sender;
            bool resaltada = (e.State & DrawItemState.Selected) != 0 && (e.State & DrawItemState.ComboBoxEdit) == 0;

            using (var fondo = new SolidBrush(resaltada ? Seleccion : combo.BackColor))
                e.Graphics.FillRectangle(fondo, e.Bounds);

            if (e.Index >= 0 && e.Index < combo.Items.Count)
            {
                string texto = combo.GetItemText(combo.Items[e.Index]);
                Color color = combo.Enabled ? (resaltada ? Texto : combo.ForeColor) : TextoApagado;
                var area = new Rectangle(e.Bounds.X + 2, e.Bounds.Y, e.Bounds.Width - 2, e.Bounds.Height);
                TextRenderer.DrawText(e.Graphics, texto, combo.Font, area, color,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
            }
        }

        // Pestañas (TabControl)

        /// <summary>
        /// Las pestañas las dibujamos nosotros: Windows las pinta siempre claras.
        /// La elegida va con el fondo de la página y el texto en azul (Acento); las otras, más apagadas.
        /// </summary>
        private static void EstilizarPestanias(TabControl pestanias)
        {
            if (pestanias.DrawMode == TabDrawMode.OwnerDrawFixed)
                return;   // ya estaba preparado
            pestanias.DrawMode = TabDrawMode.OwnerDrawFixed;
            pestanias.SizeMode = TabSizeMode.Fixed;
            pestanias.ItemSize = new Size(Math.Max(pestanias.ItemSize.Width, 140), Math.Max(pestanias.ItemSize.Height, 30));
            pestanias.DrawItem += DibujarPestania;
        }

        private static void DibujarPestania(object sender, DrawItemEventArgs e)
        {
            var pestanias = (TabControl)sender;
            bool elegida = e.Index == pestanias.SelectedIndex;
            Rectangle area = e.Bounds;

            using (var fondo = new SolidBrush(elegida ? Fondo : Superficie))
                e.Graphics.FillRectangle(fondo, area);
            if (elegida)
            {
                using var franja = new SolidBrush(Primario);
                e.Graphics.FillRectangle(franja, area.X, area.Y, area.Width, 3);
            }

            string texto = pestanias.TabPages[e.Index].Text;
            using var fuente = new Font(pestanias.Font, elegida ? FontStyle.Bold : FontStyle.Regular);
            TextRenderer.DrawText(e.Graphics, texto, fuente, area, elegida ? Acento : TextoApagado,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        // Barras de desplazamiento

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string tema, string lista);

        /// <summary>
        /// Las barras de desplazamiento las dibuja Windows. En Windows 10/11 existe el tema "DarkMode_Explorer",
        /// que las pinta oscuras: se lo pedimos a las listas, árboles, cuadros de texto de varias líneas y grillas.
        /// En el tema claro se vuelve al normal ("Explorer"). Si Windows no lo tiene, no pasa nada.
        /// </summary>
        private static void BarrasDeDesplazamiento(Control control)
        {
            bool tieneBarras = control is ListBox || control is TreeView || control is DataGridView
                               || control is ListView || (control is ScrollableControl { AutoScroll: true } && !(control is Form))
                               || (control is TextBox texto && texto.Multiline);
            if (!tieneBarras)
                return;

            if (control.IsHandleCreated)
                PonerTemaDeBarras(control);
            else
            {
                control.HandleCreated -= Control_HandleCreated;
                control.HandleCreated += Control_HandleCreated;
            }

            // Las grillas tienen sus barras como controles hijos.
            if (control is DataGridView grilla)
                foreach (Control barra in grilla.Controls)
                    if (barra is ScrollBar && barra.IsHandleCreated)
                        PonerTemaDeBarras(barra);
        }

        private static void Control_HandleCreated(object sender, EventArgs e)
        {
            PonerTemaDeBarras((Control)sender);
        }

        private static void PonerTemaDeBarras(Control control)
        {
            try
            {
                SetWindowTheme(control.Handle, Oscuro ? "DarkMode_Explorer" : "Explorer", null);
            }
            catch
            {
                // Windows viejo: las barras quedan como estaban.
            }
        }

        // Encabezado

        /// <summary>
        /// Franja azul arriba de cada pantalla con el título.
        /// - Si la pantalla ya tiene un lblTitulo (las de tus diseños de EA), se lo convierte en franja.
        /// - Si no tiene (Clientes, Mecánicos, etc.), se agrega una franja con el texto de la ventana
        ///   y se corre el contenido hacia abajo.
        /// - La pantalla principal no lleva franja (ya tiene el menú); su título queda en azul.
        /// </summary>
        private static void ArmarEncabezado(Form form)
        {
            if (form.MainMenuStrip != null)
            {
                foreach (Control titulo in form.Controls.Find("lblTitulo", true))
                    titulo.ForeColor = Acento;
                return;
            }

            Control existente = form.Controls["lblTitulo"];   // busca solo en el primer nivel
            if (existente is Label lblTitulo)
            {
                lblTitulo.AutoSize = false;
                lblTitulo.Location = new Point(0, 0);
                lblTitulo.Size = new Size(form.ClientSize.Width, AltoEncabezado);
                lblTitulo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                DarEstiloDeFranja(lblTitulo);
                return;
            }

            // Si la pantalla no tiene título, pasamos todo a un panel debajo de la franja.
            Size tamanioOriginal = form.ClientSize;
            var contenido = new Panel { Size = tamanioOriginal, BackColor = Fondo };

            form.SuspendLayout();
            var controlesOriginales = new List<Control>();
            foreach (Control control in form.Controls)
                controlesOriginales.Add(control);
            foreach (Control control in controlesOriginales)
                contenido.Controls.Add(control);   // al meterlo en el panel, sale del formulario

            form.ClientSize = new Size(tamanioOriginal.Width, tamanioOriginal.Height + AltoEncabezado);

            var franja = new Label { Text = form.Text, Dock = DockStyle.Top, Height = AltoEncabezado };
            DarEstiloDeFranja(franja);

            contenido.Dock = DockStyle.Fill;
            form.Controls.Add(contenido);
            form.Controls.Add(franja);   // lo agregamos último para que quede arriba de todo
            form.ResumeLayout(true);
        }

        private static void DarEstiloDeFranja(Label franja)
        {
            franja.BackColor = Primario;
            franja.ForeColor = Color.White;
            franja.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            franja.TextAlign = ContentAlignment.MiddleCenter;
        }

        // Botones

        private enum TipoBoton { Principal, Secundario, Peligro }

        /// <summary>
        /// Decide el estilo por el NOMBRE del botón:
        /// Guardar/Registrar/Agregar/Ingresar/Nuevo = azul lleno (la acción principal);
        /// Eliminar/Deshabilitar/Quitar = borde rojo; el resto = blanco con borde.
        /// </summary>
        private static TipoBoton Clasificar(Button boton)
        {
            string nombre = boton.Name.ToLower();

            if (nombre.Contains("eliminar") || nombre.Contains("deshabilitar") || nombre.Contains("quitar"))
                return TipoBoton.Peligro;

            if (nombre.Contains("guardar") || nombre.Contains("registrar") || nombre.Contains("agregar")
                || nombre.Contains("ingresar") || nombre == "btnnuevo" || nombre == "btnnuevacompra")
                return TipoBoton.Principal;

            return TipoBoton.Secundario;
        }

        private static void EstilizarBoton(Button boton)
        {
            boton.FlatStyle = FlatStyle.Flat;
            boton.UseVisualStyleBackColor = false;
            boton.Cursor = Cursors.Hand;

            switch (Clasificar(boton))
            {
                case TipoBoton.Principal:
                    boton.FlatAppearance.BorderSize = 0;
                    boton.FlatAppearance.MouseOverBackColor = PrimarioHover;
                    boton.FlatAppearance.MouseDownBackColor = PrimarioOscuro;
                    ColoresBotonPrincipal(boton);
                    // Un botón azul apagado se ve mal, así que lo pasamos a gris mientras esté apagado.
                    boton.EnabledChanged += (sender, e) => ColoresBotonPrincipal(boton);
                    break;

                case TipoBoton.Peligro:
                    boton.BackColor = Superficie;
                    boton.ForeColor = Peligro;
                    boton.FlatAppearance.BorderColor = Peligro;
                    boton.FlatAppearance.BorderSize = 1;
                    boton.FlatAppearance.MouseOverBackColor = PeligroFondo;
                    boton.FlatAppearance.MouseDownBackColor = PeligroFondo;
                    break;

                default:
                    boton.BackColor = Superficie;
                    boton.ForeColor = Acento;
                    boton.FlatAppearance.BorderColor = Borde;
                    boton.FlatAppearance.BorderSize = 1;
                    boton.FlatAppearance.MouseOverBackColor = Fondo;
                    boton.FlatAppearance.MouseDownBackColor = Seleccion;
                    break;
            }
        }

        private static void ColoresBotonPrincipal(Button boton)
        {
            boton.BackColor = boton.Enabled ? Primario : Borde;
            boton.ForeColor = boton.Enabled ? Color.White : TextoApagado;
        }

        /// <summary>Botones de opción (Baja / Alta / Media): el elegido queda azul.</summary>
        private static void EstilizarBotonOpcion(RadioButton opcion)
        {
            opcion.FlatStyle = FlatStyle.Flat;
            opcion.UseVisualStyleBackColor = false;
            opcion.Cursor = Cursors.Hand;
            opcion.BackColor = Superficie;
            opcion.FlatAppearance.BorderColor = Borde;
            opcion.FlatAppearance.CheckedBackColor = Primario;
            opcion.FlatAppearance.MouseOverBackColor = Fondo;

            opcion.ForeColor = opcion.Checked ? Color.White : Texto;
            opcion.CheckedChanged += (sender, e) => opcion.ForeColor = opcion.Checked ? Color.White : Texto;
        }

        // Grillas

        private static void EstilizarGrilla(DataGridView grilla)
        {
            grilla.BackgroundColor = Superficie;
            grilla.BorderStyle = BorderStyle.None;
            grilla.GridColor = Borde;
            grilla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;   // solo líneas horizontales

            // Encabezado de columnas azul.
            grilla.EnableHeadersVisualStyles = false;   // sin esto Windows no usa nuestros colores
            grilla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grilla.ColumnHeadersHeight = 34;
            grilla.ColumnHeadersDefaultCellStyle.BackColor = Primario;
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = Primario;
            grilla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grilla.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 0, 0);

            // Filas más altas, con color alternado y selección celeste.
            grilla.RowTemplate.Height = 30;
            grilla.DefaultCellStyle.BackColor = Superficie;
            grilla.DefaultCellStyle.ForeColor = Texto;
            grilla.DefaultCellStyle.SelectionBackColor = Seleccion;
            grilla.DefaultCellStyle.SelectionForeColor = Texto;
            grilla.DefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
            grilla.AlternatingRowsDefaultCellStyle.BackColor = FilaAlterna;
        }

        // Paneles y etiquetas

        /// <summary>
        /// Paneles con borde = "tarjetas" blancas (ej: TALLER / INGRESOS / AGENCIA del Inicio).
        /// Los carteles de la Pantalla B llevan fondo rojo claro o verde claro.
        /// Los paneles sin borde son solo contenedores y quedan transparentes.
        /// </summary>
        private static void EstilizarPanel(Panel panel)
        {
            bool esCartel = panel.Name == "pnlFaltanDatos" || panel.Name == "pnlExito";
            if (!esCartel && panel.BorderStyle != BorderStyle.FixedSingle)
                return;

            if (panel.Name == "pnlFaltanDatos")
                panel.BackColor = PeligroFondo;
            else if (panel.Name == "pnlExito")
                panel.BackColor = ExitoFondo;
            else
                panel.BackColor = Superficie;

            // El borde de Windows es negro, lo cambiamos por uno gris dibujado a mano.
            panel.BorderStyle = BorderStyle.None;
            panel.Paint += DibujarBordeTarjeta;
        }

        private static void DibujarBordeTarjeta(object sender, PaintEventArgs e)
        {
            var panel = (Panel)sender;
            using (var lapiz = new Pen(Borde))
                e.Graphics.DrawRectangle(lapiz, 0, 0, panel.Width - 1, panel.Height - 1);

            // Las tarjetas (no los carteles) llevan una franja azul arriba.
            // Se decide por el nombre y no por el color, porque una tarjeta cambia de color al pasar el mouse.
            bool esCartel = panel.Name == "pnlFaltanDatos" || panel.Name == "pnlExito";
            if (!esCartel)
            {
                using var pincel = new SolidBrush(Primario);
                e.Graphics.FillRectangle(pincel, 0, 0, panel.Width, AltoFranjaTarjeta);
            }
        }

        /// <summary>Los títulos de sección (lblXxxTitulo) y el total van en azul.</summary>
        private static void EstilizarEtiqueta(Label etiqueta)
        {
            bool esTituloDeSeccion = etiqueta.Name.EndsWith("Titulo") && etiqueta.Name != "lblTitulo";
            if (esTituloDeSeccion || etiqueta.Name == "lblTotal")
                etiqueta.ForeColor = Acento;
        }

        // Menú

        /// <summary>Colores del menú principal y de la barra de estado de abajo.</summary>
        /// <summary>
        /// Dibuja los menús con los colores del tema. El texto de cada opción se pinta acá
        /// (y no con el ForeColor de cada ítem): así también se leen las opciones que se crean
        /// por código, como la lista de idiomas, y las flechas de los submenús.
        /// En la barra de estado de abajo (azul) el texto queda blanco.
        /// </summary>
        private class RendererMenu : ToolStripProfessionalRenderer
        {
            private readonly bool _esBarraDeEstado;

            public RendererMenu(bool esBarraDeEstado) : base(new ColoresMenu())
            {
                _esBarraDeEstado = esBarraDeEstado;
                RoundedEdges = false;
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                if (_esBarraDeEstado)
                    e.TextColor = Color.White;
                else
                    e.TextColor = e.Item.Enabled ? Texto : TextoApagado;
                base.OnRenderItemText(e);
            }

            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                e.ArrowColor = _esBarraDeEstado ? Color.White : Texto;
                base.OnRenderArrow(e);
            }
        }

        private class ColoresMenu : ProfessionalColorTable
        {
            public override Color CheckBackground => Seleccion;
            public override Color CheckSelectedBackground => Seleccion;
            public override Color CheckPressedBackground => Seleccion;
            public override Color SeparatorDark => Borde;
            public override Color SeparatorLight => Borde;
            public override Color MenuStripGradientBegin => Superficie;
            public override Color MenuStripGradientEnd => Superficie;
            public override Color MenuItemSelected => Seleccion;
            public override Color MenuItemBorder => Seleccion;
            public override Color MenuItemSelectedGradientBegin => Seleccion;
            public override Color MenuItemSelectedGradientEnd => Seleccion;
            public override Color MenuItemPressedGradientBegin => Seleccion;
            public override Color MenuItemPressedGradientEnd => Seleccion;
            public override Color MenuBorder => Borde;
            public override Color ToolStripDropDownBackground => Superficie;
            public override Color ImageMarginGradientBegin => Superficie;
            public override Color ImageMarginGradientMiddle => Superficie;
            public override Color ImageMarginGradientEnd => Superficie;
            public override Color StatusStripGradientBegin => Primario;
            public override Color StatusStripGradientEnd => Primario;
        }
    }
}
