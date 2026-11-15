using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Comercial - Consultar ventas (entre dos fechas, con filtro) y acceso a Registrar venta.
    /// Abajo se muestra el total vendido y el total de comisiones pagadas a las agencias.
    /// </summary>
    public partial class FrmVentas : FormBase
    {
        private readonly VentaService _ventaService = new VentaService();

        public FrmVentas()
        {
            InitializeComponent();
        }

        private void FrmVentas_Load(object sender, EventArgs e)
        {
            dtpHasta.Value = FiltroFechas.HastaPorDefecto;
            dtpDesde.Value = FiltroFechas.DesdePorDefecto;   // desde 2024, así se ven todas las ventas
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                List<Venta> ventas = _ventaService.Listar(dtpDesde.Value, dtpHasta.Value, txtBuscar.Text);

                dgvVentas.Rows.Clear();
                foreach (Venta v in ventas)
                {
                    Vehiculo vehiculo = v.Consignacion.Vehiculo;
                    dgvVentas.Rows.Add(v.FechaVenta.ToString("dd/MM/yyyy"), vehiculo.Dominio,
                        $"{vehiculo.Marca} {vehiculo.Modelo} ({vehiculo.Anio})", v.Consignacion.Agencia.RazonSocial,
                        v.Cliente.NombreCompleto, v.Cliente.DNI, v.PrecioVenta.ToString("C2"),
                        v.MontoComision.ToString("C2"), v.FormaPago.Traducir());
                }

                lblResumen.Text = $"{ventas.Count} {"venta(s)".Traducir()}  -  {"Total vendido".Traducir()}: {ventas.Sum(v => v.PrecioVenta):C2}" +
                                  $"  -  {"Comisiones".Traducir()}: {ventas.Sum(v => v.MontoComision):C2}";
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var frm = new FrmVentaEdicion();
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }
    }
}
