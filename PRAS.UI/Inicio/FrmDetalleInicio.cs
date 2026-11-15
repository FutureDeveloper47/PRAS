using System.Globalization;
using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>Qué tarjeta del Inicio se tocó.</summary>
    public enum TipoDetalleInicio
    {
        Taller,
        Ingresos,
        Agencia
    }

    /// <summary>
    /// Detalle que se abre al tocar una tarjeta del Inicio (TALLER, INGRESOS o AGENCIA).
    /// Es UNA sola pantalla para las tres: según la tarjeta cambian el título,
    /// el nombre de algunas columnas y el total de abajo.
    /// </summary>
    public partial class FrmDetalleInicio : FormBase
    {
        private static readonly CultureInfo Argentina = new CultureInfo("es-AR");
        private const string FormatoPlata = "$ #,##0.00";

        private readonly InicioService _inicioService = new InicioService();
        private readonly TipoDetalleInicio _tipo;

        public FrmDetalleInicio(TipoDetalleInicio tipo)
        {
            InitializeComponent();
            _tipo = tipo;

            // Doble clic en un renglón: se abre la ficha completa de ese auto.
            dgvDetalle.CellDoubleClick += dgvDetalle_CellDoubleClick;
        }

        private void dgvDetalle_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;   // doble clic en el encabezado

            if (dgvDetalle.Rows[e.RowIndex].DataBoundItem is DetalleInicio detalle)
            {
                using (var ficha = new FrmFichaVehiculo(detalle.IdVehiculo))
                    ficha.ShowDialog(this);
            }
        }

        private void FrmDetalleInicio_Load(object sender, EventArgs e)
        {
            // Las columnas ya están armadas en el diseñador, que no invente otras.
            dgvDetalle.AutoGenerateColumns = false;

            try
            {
                switch (_tipo)
                {
                    case TipoDetalleInicio.Taller:
                        MostrarTaller();
                        break;
                    case TipoDetalleInicio.Ingresos:
                        MostrarIngresos();
                        break;
                    default:
                        MostrarAgencia();
                        break;
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>
        /// Período que muestran INGRESOS y AGENCIA, en el idioma elegido.
        /// Ej: "noviembre de 2025 - noviembre de 2026" (los últimos 12 meses, contando el actual).
        /// </summary>
        private static string UltimoAnio()
        {
            string desde = InicioService.InicioUltimoAnio.ToString("Y", CultureInfo.CurrentCulture);
            string hasta = DateTime.Today.ToString("Y", CultureInfo.CurrentCulture);
            return $"{desde} - {hasta}";
        }

        private static string Plata(decimal monto)
        {
            return monto.ToString(FormatoPlata, Argentina);
        }

        // Taller

        private void MostrarTaller()
        {
            List<DetalleInicio> filas = _inicioService.ObtenerDetalleTaller();

            Text = "Detalle - Taller".Traducir();
            lblTitulo.Text = "TALLER".Traducir() + ": " + "vehículos en el taller".Traducir();
            lblSubtitulo.Text = "Autos que están ahora en diagnóstico o en reparación, con su mecánico y prioridad. Doble clic para ver la ficha del auto.".Traducir();

            colFecha.HeaderText = "Ingreso al taller".Traducir();
            colPersona.HeaderText = "Mecánico".Traducir();
            colExtra.HeaderText = "Prioridad".Traducir();
            colMonto.Visible = false;   // en el taller todavía no hay precio

            dgvDetalle.DataSource = filas;
            lblTotal.Text = filas.Count == 0
                ? "No hay vehículos en el taller.".Traducir()
                : $"{filas.Count} vehículo(s) en el taller".TraducirMensaje();
        }

        // Ingresos

        private void MostrarIngresos()
        {
            List<DetalleInicio> filas = _inicioService.ObtenerDetalleIngresos();

            Text = "Detalle - Ingresos".Traducir();
            lblTitulo.Text = "INGRESOS".Traducir() + ": " + "chocados comprados".Traducir();
            lblSubtitulo.Text = $"Compras de autos chocados de los últimos 12 meses ({UltimoAnio()}). Doble clic para ver la ficha del auto.".TraducirMensaje();

            colFecha.HeaderText = "Fecha de compra".Traducir();
            colPersona.HeaderText = "Vendedor".Traducir();
            colMonto.HeaderText = "Precio de compra".Traducir();
            colExtra.HeaderText = "Forma de pago".Traducir();

            dgvDetalle.DataSource = filas;

            decimal total = filas.Sum(f => f.Monto ?? 0);
            lblTotal.Text = filas.Count == 0
                ? "No hay compras en los últimos 12 meses.".Traducir()
                : $"{filas.Count} {"compra(s)".Traducir()}  -  {"Total invertido".Traducir()}: {Plata(total)}";
        }

        // Agencia

        private void MostrarAgencia()
        {
            List<DetalleInicio> filas = _inicioService.ObtenerDetalleAgencia();

            Text = "Detalle - Agencia".Traducir();
            lblTitulo.Text = "AGENCIA".Traducir() + ": " + "ventas y autos en agencia".Traducir();
            lblSubtitulo.Text = $"Arriba (en verde) las ventas de los últimos 12 meses ({UltimoAnio()}); abajo los autos que la agencia tiene para vender.".TraducirMensaje();

            colFecha.HeaderText = "Fecha".Traducir();
            colPersona.HeaderText = "Comprador".Traducir();
            colMonto.HeaderText = "Precio".Traducir();
            colExtra.HeaderText = "Agencia".Traducir();

            dgvDetalle.DataSource = filas;

            List<DetalleInicio> ventas = filas.Where(f => f.EsVenta).ToList();
            decimal totalVendido = ventas.Sum(f => f.Monto ?? 0);
            int enAgencia = filas.Count - ventas.Count;

            lblTotal.Text = $"{ventas.Count} venta(s) en los últimos 12 meses por {Plata(totalVendido)}".TraducirMensaje() + "  -  " + $"{enAgencia} auto(s) en agencia".TraducirMensaje();
        }

        /// <summary>
        /// Cuando la grilla termina de cargarse, pinta de verde claro las filas que son ventas
        /// (así se distinguen de los autos que todavía están en la agencia).
        /// </summary>
        private void dgvDetalle_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvDetalle.Rows)
            {
                if (fila.DataBoundItem is DetalleInicio detalle && detalle.EsVenta)
                    fila.DefaultCellStyle.BackColor = Tema.ExitoFondo;
            }
            dgvDetalle.ClearSelection();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
