using System.Globalization;
using PRAS.BLL;
using PRAS.Entidades;

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

        /// <summary>Ej: "septiembre de 2026".</summary>
        private static string MesActual()
        {
            return DateTime.Today.ToString("MMMM 'de' yyyy", Argentina);
        }

        private static string Plata(decimal monto)
        {
            return monto.ToString(FormatoPlata, Argentina);
        }

        // Taller

        private void MostrarTaller()
        {
            List<DetalleInicio> filas = _inicioService.ObtenerDetalleTaller();

            Text = "Detalle - Taller";
            lblTitulo.Text = "TALLER: vehículos en el taller";
            lblSubtitulo.Text = "Autos que están ahora en diagnóstico o en reparación, con su mecánico y prioridad.";

            colFecha.HeaderText = "Ingreso al taller";
            colPersona.HeaderText = "Mecánico";
            colExtra.HeaderText = "Prioridad";
            colMonto.Visible = false;   // en el taller todavía no hay precio

            dgvDetalle.DataSource = filas;
            lblTotal.Text = filas.Count == 0
                ? "No hay vehículos en el taller."
                : $"{filas.Count} vehículo(s) en el taller";
        }

        // Ingresos

        private void MostrarIngresos()
        {
            List<DetalleInicio> filas = _inicioService.ObtenerDetalleIngresos();

            Text = "Detalle - Ingresos";
            lblTitulo.Text = "INGRESOS: chocados comprados";
            lblSubtitulo.Text = $"Compras de autos chocados registradas en {MesActual()}.";

            colFecha.HeaderText = "Fecha de compra";
            colPersona.HeaderText = "Vendedor";
            colMonto.HeaderText = "Precio de compra";
            colExtra.HeaderText = "Forma de pago";

            dgvDetalle.DataSource = filas;

            decimal total = filas.Sum(f => f.Monto ?? 0);
            lblTotal.Text = filas.Count == 0
                ? "No hay compras en el mes."
                : $"{filas.Count} compra(s)  -  Total invertido: {Plata(total)}";
        }

        // Agencia

        private void MostrarAgencia()
        {
            List<DetalleInicio> filas = _inicioService.ObtenerDetalleAgencia();

            Text = "Detalle - Agencia";
            lblTitulo.Text = "AGENCIA: ventas y autos en agencia";
            lblSubtitulo.Text = $"Arriba (en verde) las ventas de {MesActual()}; abajo los autos que la agencia tiene para vender.";

            colFecha.HeaderText = "Fecha";
            colPersona.HeaderText = "Comprador";
            colMonto.HeaderText = "Precio";
            colExtra.HeaderText = "Agencia";

            dgvDetalle.DataSource = filas;

            List<DetalleInicio> ventas = filas.Where(f => f.EsVenta).ToList();
            decimal totalVendido = ventas.Sum(f => f.Monto ?? 0);
            int enAgencia = filas.Count - ventas.Count;

            lblTotal.Text = $"{ventas.Count} venta(s) en el mes por {Plata(totalVendido)}  -  {enAgencia} auto(s) en agencia";
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
