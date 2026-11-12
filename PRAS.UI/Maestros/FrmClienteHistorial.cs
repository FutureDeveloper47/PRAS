using PRAS.BLL;
using PRAS.Entidades;

namespace PRAS.UI
{
    /// <summary>
    /// CU Clientes.004 - Consultar Historial.
    /// Como el Cliente tiene dos roles, se muestran dos listas:
    /// las compras (fue vendedor) y las ventas (fue comprador).
    /// </summary>
    public partial class FrmClienteHistorial : FormBase
    {
        private readonly ClienteService _clienteService = new ClienteService();
        private readonly Cliente _cliente;

        public FrmClienteHistorial(Cliente cliente)
        {
            InitializeComponent();
            _cliente = cliente;
        }

        private void FrmClienteHistorial_Load(object sender, EventArgs e)
        {
            lblCliente.Text = $"{_cliente.NombreCompleto}  -  DNI {_cliente.DNI}";

            try
            {
                // Cargamos las filas a mano porque mostramos datos de otros objetos
                // (por ejemplo la descripción del auto) que la grilla no puede leer sola.
                foreach (Compra compra in _clienteService.ListarCompras(_cliente.IdCliente))
                {
                    dgvCompras.Rows.Add(
                        compra.FechaCompra.ToString("dd/MM/yyyy"),
                        compra.Vehiculo.Descripcion,
                        compra.PrecioCompra.ToString("C2"),
                        compra.FormaPago);
                }

                foreach (Venta venta in _clienteService.ListarVentas(_cliente.IdCliente))
                {
                    dgvVentas.Rows.Add(
                        venta.FechaVenta.ToString("dd/MM/yyyy"),
                        venta.Consignacion.Vehiculo.Descripcion,
                        venta.Consignacion.Agencia.RazonSocial,
                        venta.PrecioVenta.ToString("C2"),
                        venta.FormaPago);
                }

                lblCompras.Text += $"  {dgvCompras.Rows.Count}";
                lblVentas.Text += $"  {dgvVentas.Rows.Count}";
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
