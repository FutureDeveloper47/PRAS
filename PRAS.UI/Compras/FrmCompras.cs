using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// CU Compras.002 - Consultar Compras: lista las compras entre dos fechas,
    /// con un filtro opcional. Desde acá también se puede registrar una compra nueva.
    /// </summary>
    public partial class FrmCompras : FormBase
    {
        private readonly CompraService _compraService = new CompraService();

        public FrmCompras()
        {
            InitializeComponent();
        }

        private void FrmCompras_Load(object sender, EventArgs e)
        {
            // Por defecto, el último mes.
            dtpHasta.Value = DateTime.Today;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                List<Compra> compras = _compraService.Listar(dtpDesde.Value, dtpHasta.Value, txtBuscar.Text);

                // Cargamos las filas a mano porque mostramos datos del auto y del cliente.
                dgvCompras.Rows.Clear();
                decimal total = 0;
                foreach (Compra compra in compras)
                {
                    dgvCompras.Rows.Add(
                        compra.FechaCompra.ToString("dd/MM/yyyy"),
                        compra.Vehiculo.Dominio,
                        $"{compra.Vehiculo.Marca} {compra.Vehiculo.Modelo} ({compra.Vehiculo.Anio})",
                        compra.Cliente.NombreCompleto,
                        compra.Cliente.DNI,
                        compra.PrecioCompra.ToString("C2"),
                        compra.FormaPago);
                    total += compra.PrecioCompra;
                }

                lblResumen.Text = $"{compras.Count} {"compra(s)".Traducir()}  -  {"Total invertido".Traducir()}: {total:C2}";
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

        private void btnNuevaCompra_Click(object sender, EventArgs e)
        {
            using var frm = new FrmRegistrarCompra();
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }
    }
}
