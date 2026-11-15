namespace PRAS.UI
{
    /// <summary>
    /// Aspecto visual de TODO el sistema, en un solo lugar.
    /// Cada pantalla no elige
    /// sus colores: los toma de acá. Si mañana querés otro color, lo cambiás una sola vez.
    ///
    /// Se aplica solo, al abrir cada pantalla, porque todas heredan de FormBase.
    /// </summary>
    public static class Tema
    {
        // Colores
        public static readonly Color Primario = Color.FromArgb(31, 78, 121);   // azul marino
        public static readonly Color PrimarioHover = Color.FromArgb(44, 100, 150);   // cuando pasás el mouse
        public static readonly Color PrimarioOscuro = Color.FromArgb(22, 58, 92);   // cuando hacés clic
        public static readonly Color Fondo = Color.FromArgb(243, 245, 248);   // gris muy claro
        public static readonly Color Superficie = Color.White;   // tarjetas y grillas
        public static readonly Color Borde = Color.FromArgb(214, 220, 228);
        public static readonly Color Texto = Color.FromArgb(31, 41, 51);
        public static readonly Color TextoApagado = Color.FromArgb(150, 158, 170);
        public static readonly Color Peligro = Color.FromArgb(192, 57, 43);   // rojo (Eliminar)
        public static readonly Color PeligroFondo = Color.FromArgb(253, 236, 236);
        public static readonly Color ExitoFondo = Color.FromArgb(232, 245, 233);
        public static readonly Color FilaAlterna = Color.FromArgb(247, 249, 252);
        public static readonly Color Seleccion = Color.FromArgb(214, 230, 245);

        private const int AltoEncabezado = 56;
        private const int AltoFranjaTarjeta = 4;

        /// <summary>Aplica el tema a una pantalla y a todos sus controles.</summary>
        public static void Aplicar(Form form)
        {
            form.BackColor = Fondo;
            form.ForeColor = Texto;   // los controles toman este color si no tienen uno propio

            AplicarAControles(form.Controls);
            ArmarEncabezado(form);
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
                        break;
                    case NumericUpDown numero:
                        numero.BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case ComboBox combo:
                        combo.FlatStyle = FlatStyle.Flat;
                        break;
                    case ListBox lista:
                        lista.BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case StatusStrip barra:
                        barra.BackColor = Primario;
                        barra.ForeColor = Color.White;
                        barra.SizingGrip = false;
                        barra.Renderer = new ToolStripProfessionalRenderer(new ColoresMenu());
                        break;
                    case MenuStrip menu:
                        menu.BackColor = Superficie;
                        menu.Renderer = new ToolStripProfessionalRenderer(new ColoresMenu());
                        menu.Padding = new Padding(6, 4, 0, 4);
                        break;
                    case Panel panel:
                        EstilizarPanel(panel);
                        break;
                    case Label etiqueta:
                        EstilizarEtiqueta(etiqueta);
                        break;
                }

                // Si un control tiene otros adentro (por ejemplo un Panel), los recorremos también.
                // Grillas, combos y números tienen partes internas que no hay que tocar.
                bool recorrerHijos = control.HasChildren
                    && !(control is DataGridView) && !(control is NumericUpDown) && !(control is ComboBox);
                if (recorrerHijos)
                    AplicarAControles(control.Controls);
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
                    titulo.ForeColor = Primario;
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
                    boton.ForeColor = Primario;
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
                etiqueta.ForeColor = Primario;
        }

        // Menú

        /// <summary>Colores del menú principal y de la barra de estado de abajo.</summary>
        private class ColoresMenu : ProfessionalColorTable
        {
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
