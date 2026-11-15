using System.Globalization;
using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Ficha del vehículo (solo lectura): toda la historia de un auto en una sola pantalla.
    /// Se abre al hacer clic en un renglón de "Actividad reciente" del Inicio o con doble clic
    /// en el detalle de las tarjetas TALLER / INGRESOS / AGENCIA.
    ///
    /// No tiene archivo .Designer: las secciones se arman por código según lo que el auto tenga
    /// (un auto recién comprado no muestra taller ni venta). Los datos los trae la BLL
    /// (VehiculoService.ObtenerFicha); acá solo se muestran.
    /// Los valores van en cuadros de texto de solo lectura: así el traductor no los toca
    /// (son datos, no textos de la pantalla) y además se pueden copiar.
    /// </summary>
    public class FrmFichaVehiculo : FormBase
    {
        private static readonly CultureInfo Argentina = new CultureInfo("es-AR");
        private const string FormatoPlata = "$ #,##0.00";
        private const int AnchoEtiqueta = 190;

        private readonly VehiculoService _vehiculoService = new VehiculoService();
        private readonly int _idVehiculo;

        private readonly Label lblTitulo = new Label();
        private readonly FlowLayoutPanel pnlSecciones = new FlowLayoutPanel();
        private readonly Panel pnlAbajo = new Panel();
        private readonly Button btnCerrar = new Button();

        /// <summary>Si la BLL no pudo traer la ficha, se guarda el error y se muestra al abrir.</summary>
        private Exception _error;

        public FrmFichaVehiculo(int idVehiculo)
        {
            _idVehiculo = idVehiculo;
            Name = "FrmFichaVehiculo";
            Text = "Ficha del vehículo";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = false;
            ClientSize = new Size(760, 640);
            MinimumSize = new Size(640, 480);
            Font = new Font("Segoe UI", 9.75F);

            ArmarEstructura();

            // Las secciones se arman ACÁ (antes de mostrarse) para que FormBase
            // les aplique el tema y la traducción como a cualquier otra pantalla.
            try
            {
                FichaVehiculo ficha = _vehiculoService.ObtenerFicha(_idVehiculo);
                ArmarSecciones(ficha);
            }
            catch (Exception ex)
            {
                _error = ex;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // El tema le pone borde a todos los cuadros de texto; acá los valores son de solo lectura,
            // así que los dejamos sin borde y con el fondo de la tarjeta (se leen como texto común).
            foreach (Control tarjeta in pnlSecciones.Controls)
                foreach (Control tabla in tarjeta.Controls)
                    foreach (Control valor in tabla.Controls)
                        if (valor is TextBox txt)
                        {
                            txt.BorderStyle = BorderStyle.None;
                            txt.BackColor = Tema.Superficie;
                            txt.ForeColor = Tema.Texto;
                        }

            if (_error != null)
            {
                Mensajes.Error(_error);
                BeginInvoke(new Action(Close));   // cerramos después de que termine de abrirse
            }
        }

        // Estructura fija: franja de título, secciones con barra de desplazamiento y botón Cerrar.

        private void ArmarEstructura()
        {
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Text = "FICHA DEL VEHÍCULO";
            lblTitulo.SetBounds(0, 0, ClientSize.Width, 56);

            pnlAbajo.Dock = DockStyle.Bottom;
            pnlAbajo.Height = 56;
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Text = "Cerrar";
            btnCerrar.Size = new Size(120, 34);
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.Location = new Point(pnlAbajo.Width - 140, 11);
            btnCerrar.Click += (s, e) => Close();
            pnlAbajo.Controls.Add(btnCerrar);
            pnlAbajo.Resize += (s, e) => btnCerrar.Left = pnlAbajo.Width - btnCerrar.Width - 20;
            CancelButton = btnCerrar;

            pnlSecciones.Name = "pnlSecciones";
            pnlSecciones.FlowDirection = FlowDirection.TopDown;
            pnlSecciones.WrapContents = false;
            pnlSecciones.AutoScroll = true;
            pnlSecciones.Padding = new Padding(16, 8, 16, 8);
            pnlSecciones.SetBounds(0, 56, ClientSize.Width, ClientSize.Height - 56 - pnlAbajo.Height);
            pnlSecciones.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlSecciones.Resize += (s, e) => AjustarAnchoSecciones();

            Controls.Add(pnlSecciones);
            Controls.Add(pnlAbajo);
            Controls.Add(lblTitulo);
        }

        // Secciones según lo que tenga el auto

        private void ArmarSecciones(FichaVehiculo ficha)
        {
            Vehiculo v = ficha.Vehiculo;
            Text = $"{"Ficha del vehículo".Traducir()} - {v.Dominio}";

            var auto = NuevaSeccion("Vehículo");
            Renglon(auto, "Patente", v.Dominio);
            Renglon(auto, "Marca y modelo", $"{v.Marca} {v.Modelo}");
            Renglon(auto, "Año", v.Anio.ToString());
            Renglon(auto, "Color", v.Color);
            Renglon(auto, "Nº de chasis", v.NumeroChasis);
            Renglon(auto, "Nº de motor", v.NumeroMotor);
            Renglon(auto, "Estado actual", VehiculoService.TextoEstado(v.EstadoVehiculo).Traducir());

            // Ficha para la venta (se carga al publicarlo): solo si ya se cargó.
            if (v.Kilometraje.HasValue)
            {
                Renglon(auto, "Kilometraje", v.Kilometraje.Value.ToString("N0", CultureInfo.CurrentCulture) + " km");
                Renglon(auto, "Combustible", Valor(v.Combustible) + (v.TieneGNC == true && v.Combustible != "GNC" ? " + " + "GNC".Traducir() : ""));
                Renglon(auto, "Transmisión", Valor(v.Transmision));
                Renglon(auto, "Puertas", v.Puertas?.ToString());
                Renglon(auto, "Cantidad de dueños", v.CantidadDuenos?.ToString());
                Renglon(auto, "Chapa y pintura", Valor(v.EstadoChapaPintura));
                Renglon(auto, "VTV vigente", SiNo(v.VTVVigente));
                Renglon(auto, "Service oficial", SiNo(v.ServiceOficial));
                Renglon(auto, "Aire acondicionado", SiNo(v.AireAcondicionado));
                Renglon(auto, "Dirección asistida", SiNo(v.DireccionAsistida));
            }

            if (ficha.FechaCompra.HasValue)
            {
                var compra = NuevaSeccion("Compra");
                Renglon(compra, "Fecha de compra", Fecha(ficha.FechaCompra));
                Renglon(compra, "Vendedor", ficha.Vendedor);
                Renglon(compra, "Precio de compra", Plata(ficha.PrecioCompra));
                Renglon(compra, "Forma de pago", Valor(ficha.FormaPagoCompra));
                Renglon(compra, "Observaciones", ficha.ObservacionesCompra, multilinea: true);
            }

            if (ficha.FechaDiagnostico.HasValue)
            {
                var diagnostico = NuevaSeccion("Diagnóstico");
                Renglon(diagnostico, "Fecha", Fecha(ficha.FechaDiagnostico));
                Renglon(diagnostico, "Estado", Valor(ficha.EstadoDiagnostico));
                Renglon(diagnostico, "Daños encontrados", ficha.DescripcionDanios, multilinea: true);
            }

            if (ficha.FechaOrden.HasValue || ficha.MontoPresupuesto.HasValue)
            {
                var orden = NuevaSeccion("Orden de trabajo");
                Renglon(orden, "Presupuesto", Plata(ficha.MontoPresupuesto));
                Renglon(orden, "Fecha de la orden", Fecha(ficha.FechaOrden));
                Renglon(orden, "Estado", Valor(ficha.EstadoOrden));
                Renglon(orden, "Prioridad", Valor(ficha.Prioridad));
                if (ficha.Tareas.Count > 0)
                    GrillaTareas(orden, ficha.Tareas);
            }

            if (ficha.Agencia != null || ficha.FechaPublicacion.HasValue)
            {
                var agencia = NuevaSeccion("Agencia y publicación");
                Renglon(agencia, "Agencia", ficha.Agencia);
                Renglon(agencia, "Entregado a la agencia", Fecha(ficha.FechaEntregaAgencia));
                Renglon(agencia, "Estado de la consignación", Valor(ficha.EstadoConsignacion));
                Renglon(agencia, "Plataforma", Valor(ficha.Plataforma));
                Renglon(agencia, "Fecha de publicación", Fecha(ficha.FechaPublicacion));
                Renglon(agencia, "Precio publicado", Plata(ficha.PrecioPublicado));
                Renglon(agencia, "Estado de la publicación", Valor(ficha.EstadoPublicacion));
            }

            if (ficha.FechaVenta.HasValue)
            {
                var venta = NuevaSeccion("Venta");
                Renglon(venta, "Fecha de venta", Fecha(ficha.FechaVenta));
                Renglon(venta, "Comprador", ficha.Comprador);
                Renglon(venta, "Precio de venta", Plata(ficha.PrecioVenta));
                Renglon(venta, "Forma de pago", Valor(ficha.FormaPagoVenta));
            }

            AjustarAnchoSecciones();
        }

        /// <summary>
        /// Una "tarjeta" con su título. Adentro, una tabla de dos columnas: etiqueta | valor.
        /// El panel lleva borde para que Tema lo dibuje como tarjeta (fondo blanco y franja azul).
        /// </summary>
        private TableLayoutPanel NuevaSeccion(string titulo)
        {
            var tarjeta = new Panel
            {
                Name = "pnlSeccion" + pnlSecciones.Controls.Count,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(14, 12, 14, 12),
                Margin = new Padding(0, 0, 0, 12)
            };

            var tabla = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top
            };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, AnchoEtiqueta));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // El nombre termina en "Titulo": Tema lo pinta de azul como los demás títulos de sección.
            var lblSeccion = new Label
            {
                Name = "lblSeccionTitulo",
                Text = titulo.ToUpper(),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Margin = new Padding(0, 4, 0, 8)
            };
            tabla.RowCount = 1;   // la fila 0 es el título; los renglones empiezan en la 1
            tabla.Controls.Add(lblSeccion, 0, 0);
            tabla.SetColumnSpan(lblSeccion, 2);

            tarjeta.Controls.Add(tabla);
            pnlSecciones.Controls.Add(tarjeta);
            return tabla;
        }

        /// <summary>Agrega "Etiqueta: valor". Si el valor está vacío se muestra un guion.</summary>
        private static void Renglon(TableLayoutPanel tabla, string etiqueta, string valor, bool multilinea = false)
        {
            var lbl = new Label
            {
                Text = etiqueta + ":",
                AutoSize = true,
                ForeColor = Tema.TextoApagado,
                Margin = new Padding(0, 4, 8, 4)
            };

            string texto = string.IsNullOrWhiteSpace(valor) ? "-" : valor.Trim();
            var txt = new TextBox
            {
                Name = "txtValor",
                Text = texto,
                ReadOnly = true,
                TabStop = false,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                Multiline = multilinea,
                WordWrap = true,
                Margin = new Padding(0, 6, 0, 4)
            };

            if (multilinea)
            {
                // Altura aproximada según el largo del texto (unas 70 letras por renglón).
                int renglones = Math.Max(1, Math.Min(6, (texto.Length / 70) + 1));
                txt.Height = renglones * 20 + 4;
            }

            int fila = tabla.RowCount++;
            tabla.Controls.Add(lbl, 0, fila);
            tabla.Controls.Add(txt, 1, fila);
        }

        /// <summary>Tareas de la orden: descripción, mecánico, estado y costo.</summary>
        private static void GrillaTareas(TableLayoutPanel tabla, List<TareaFicha> tareas)
        {
            var grilla = new DataGridView
            {
                Name = "dgvTareas",
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 8, 0, 4),
                Height = 34 + 30 * Math.Min(tareas.Count, 5) + 4
            };

            grilla.Columns.Add("colTarea", "Tarea");
            grilla.Columns.Add("colMecanico", "Mecánico");
            grilla.Columns.Add("colEstadoTarea", "Estado");
            grilla.Columns.Add("colCosto", "Costo estimado");
            grilla.Columns["colTarea"].FillWeight = 220;

            foreach (TareaFicha tarea in tareas)
                grilla.Rows.Add(tarea.Descripcion, tarea.Mecanico, Valor(tarea.Estado), Plata(tarea.CostoEstimado));

            int fila = tabla.RowCount++;
            tabla.Controls.Add(grilla, 0, fila);
            tabla.SetColumnSpan(grilla, 2);
        }

        /// <summary>Las tarjetas ocupan todo el ancho disponible (menos la barra de desplazamiento).</summary>
        private void AjustarAnchoSecciones()
        {
            int ancho = pnlSecciones.ClientSize.Width - pnlSecciones.Padding.Horizontal - SystemInformation.VerticalScrollBarWidth;
            foreach (Control tarjeta in pnlSecciones.Controls)
            {
                tarjeta.MinimumSize = new Size(ancho, 0);
                tarjeta.MaximumSize = new Size(ancho, 0);
                foreach (Control tabla in tarjeta.Controls)
                    tabla.MaximumSize = new Size(ancho - tarjeta.Padding.Horizontal - 2, 0);
            }
        }

        // Formatos

        private static string Fecha(DateTime? fecha) => fecha?.ToString("dd/MM/yyyy");

        private static string Plata(decimal? monto) => monto?.ToString(FormatoPlata, Argentina);

        private static string SiNo(bool? valor) => valor == null ? null : (valor.Value ? "Sí" : "No").Traducir();

        /// <summary>
        /// Valores que vienen de la base en español (estados, prioridad, forma de pago):
        /// se guardan igual y se traducen solo para mostrarlos.
        /// </summary>
        private static string Valor(string texto) => string.IsNullOrWhiteSpace(texto) ? null : texto.Traducir();
    }
}
