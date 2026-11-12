using System.Data;
using PRAS.BLL;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Reportes: Vehículos, Reparaciones, Presupuestos y Ventas (una pantalla para los cuatro).
    /// Se elige el período, se genera el reporte y se puede exportar a PDF (para imprimir o mandar)
    /// o a CSV (para abrirlo con Excel). El formato se elige en el cuadro de "Guardar como".
    /// </summary>
    public partial class FrmReporte : FormBase
    {
        /// <summary>Encabezado amigable de cada columna que devuelve la consulta.</summary>
        private static readonly Dictionary<string, string> Encabezados = new Dictionary<string, string>
        {
            ["Dominio"] = "Dominio", ["Vehiculo"] = "Vehículo", ["Estado"] = "Estado",
            ["FechaCompra"] = "Fecha de compra", ["PrecioCompra"] = "Precio de compra",
            ["CostoReparacion"] = "Costo de reparación", ["InversionTotal"] = "Inversión total",
            ["OT"] = "OT", ["FechaCreacion"] = "Fecha", ["Prioridad"] = "Prioridad", ["Mecanicos"] = "Mecánicos",
            ["Tareas"] = "Tareas", ["Costo"] = "Costo", ["DiasEnTaller"] = "Días en taller",
            ["Presupuesto"] = "Presupuesto", ["FechaEmision"] = "Fecha", ["Items"] = "Ítems",
            ["Repuestos"] = "Repuestos", ["ManoDeObraYOtros"] = "Mano de obra y otros", ["MontoTotal"] = "Total",
            ["FechaVenta"] = "Fecha", ["Agencia"] = "Agencia", ["Comprador"] = "Comprador",
            ["PrecioVenta"] = "Precio de venta", ["Comision"] = "Comisión", ["Ganancia"] = "Ganancia"
        };

        /// <summary>Columnas que se suman en el pie del reporte.</summary>
        private static readonly string[] ColumnasTotal =
            { "PrecioCompra", "CostoReparacion", "InversionTotal", "Costo", "MontoTotal", "PrecioVenta", "Comision", "Ganancia" };

        private readonly ReporteService _reporteService = new ReporteService();
        private readonly TipoReporte _tipo;
        private DataTable _datos;

        public FrmReporte(TipoReporte tipo)
        {
            InitializeComponent();
            _tipo = tipo;
            Text = ReporteService.Titulo(tipo);   // va en el constructor para que FormBase lo traduzca y arme el encabezado
            dgvReporte.AutoGenerateColumns = true;
        }

        private void FrmReporte_Load(object sender, EventArgs e)
        {
            bool usaFechas = ReporteService.UsaFechas(_tipo);
            lblDesde.Visible = dtpDesde.Visible = lblHasta.Visible = dtpHasta.Visible = usaFechas;

            dtpHasta.Value = DateTime.Today;
            dtpDesde.Value = new DateTime(DateTime.Today.Year, 1, 1);   // por defecto, el año actual
            Generar();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            Generar();
        }

        private void Generar()
        {
            try
            {
                _datos = _reporteService.Generar(_tipo, dtpDesde.Value, dtpHasta.Value);
                dgvReporte.DataSource = _datos;
                DarFormato();
                MostrarTotales();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Encabezados traducidos y formato de fechas y montos.</summary>
        private void DarFormato()
        {
            foreach (DataGridViewColumn columna in dgvReporte.Columns)
            {
                string nombre = columna.DataPropertyName;
                if (Encabezados.TryGetValue(nombre, out string encabezado))
                    columna.HeaderText = encabezado.Traducir();

                Type tipo = _datos.Columns[nombre].DataType;
                if (tipo == typeof(DateTime))
                    columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                else if (tipo == typeof(decimal))
                {
                    columna.DefaultCellStyle.Format = "C2";
                    columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        private void MostrarTotales()
        {
            var partes = new List<string> { $"{_datos.Rows.Count} {"registro(s)".Traducir()}" };
            foreach (string columna in ColumnasTotal)
            {
                if (_datos.Columns.Contains(columna))
                    partes.Add($"{Encabezados[columna].Traducir()}: {ReporteService.Sumar(_datos, columna):C2}");
            }

            // En el reporte de vehículos agregamos el stock por estado.
            if (_tipo == TipoReporte.Vehiculos)
            {
                DataTable stock = _reporteService.StockPorEstado();
                string detalle = string.Join("   ", stock.AsEnumerable()
                    .Select(f => $"{Convert.ToString(f["Estado"]).Traducir()}: {f["Cantidad"]}"));
                lblTotales.Text = string.Join("   |   ", partes) + Environment.NewLine + detalle;
                return;
            }

            lblTotales.Text = string.Join("   |   ", partes);
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (_datos == null || _datos.Rows.Count == 0)
            {
                Mensajes.Advertencia("No hay datos para exportar.".Traducir());
                return;
            }

            // PDF es la primera opción; también se puede elegir CSV para abrirlo con Excel
            using var dialogo = new SaveFileDialog
            {
                Filter = "Documento PDF|*.pdf|Archivo CSV (Excel)|*.csv",
                FileName = $"{ReporteService.Titulo(_tipo).Traducir()} {DateTime.Now:yyyy-MM-dd}.pdf"
            };
            if (dialogo.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                List<string> encabezados = dgvReporte.Columns.Cast<DataGridViewColumn>()
                    .OrderBy(c => c.Index).Select(c => c.HeaderText).ToList();

                if (Path.GetExtension(dialogo.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    _reporteService.ExportarCsv(_datos, encabezados, dialogo.FileName);
                }
                else
                {
                    string periodo = ReporteService.UsaFechas(_tipo)
                        ? $"{"Período".Traducir()}: {dtpDesde.Value:dd/MM/yyyy} - {dtpHasta.Value:dd/MM/yyyy}"
                        : $"{"Stock al".Traducir()} {DateTime.Today:dd/MM/yyyy}";
                    string totales = lblTotales.Text.Replace(Environment.NewLine, "   |   ");
                    _reporteService.ExportarPdf(_datos, encabezados, Text, periodo, totales, dialogo.FileName);
                }

                Mensajes.Info("Reporte exportado.".Traducir());
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
