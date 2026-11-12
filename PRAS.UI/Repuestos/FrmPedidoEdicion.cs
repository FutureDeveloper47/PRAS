using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Repuestos - Registrar pedido: se elige la orden de trabajo (el auto), el proveedor
    /// y se cargan los repuestos. El pedido queda "Pendiente" hasta que el proveedor lo entrega.
    /// </summary>
    public partial class FrmPedidoEdicion : FormBase
    {
        private readonly PedidoRepuestoService _pedidoService = new PedidoRepuestoService();
        private readonly List<DetallePedidoRepuesto> _detalles = new List<DetallePedidoRepuesto>();

        public FrmPedidoEdicion()
        {
            InitializeComponent();
        }

        private void FrmPedidoEdicion_Load(object sender, EventArgs e)
        {
            try
            {
                cboOrden.DisplayMember = nameof(OrdenTrabajo.Descripcion);
                cboOrden.ValueMember = nameof(OrdenTrabajo.IdOrdenTrabajo);
                cboOrden.DataSource = _pedidoService.ListarOrdenesAbiertas();

                cboProveedor.DisplayMember = nameof(Proveedor.RazonSocial);
                cboProveedor.ValueMember = nameof(Proveedor.IdProveedor);
                cboProveedor.DataSource = _pedidoService.ListarProveedores();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void MostrarDetalles()
        {
            dgvDetalle.Rows.Clear();
            foreach (DetallePedidoRepuesto d in _detalles)
                dgvDetalle.Rows.Add(d.DescripcionRepuesto, d.Cantidad, d.PrecioUnitario.ToString("C2"), d.Subtotal.ToString("C2"));

            lblTotal.Text = $"{"Total".Traducir()}: {_detalles.Sum(d => d.Subtotal):C2}";
        }

        private void btnAgregarRepuesto_Click(object sender, EventArgs e)
        {
            try
            {
                _detalles.Add(_pedidoService.CrearDetalle(txtRepuesto.Text, (int)numCantidad.Value, numPrecio.Value));
                MostrarDetalles();
                txtRepuesto.Clear();
                numCantidad.Value = 1;
                numPrecio.Value = 0;
                txtRepuesto.Focus();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnQuitarRepuesto_Click(object sender, EventArgs e)
        {
            int fila = dgvDetalle.CurrentRow?.Index ?? -1;
            if (fila < 0)
                return;

            _detalles.RemoveAt(fila);
            MostrarDetalles();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var pedido = new PedidoRepuesto
                {
                    IdOrdenTrabajo = cboOrden.SelectedValue as int? ?? 0,
                    IdProveedor = cboProveedor.SelectedValue as int? ?? 0,
                    Detalles = new List<DetallePedidoRepuesto>(_detalles)
                };
                int id = _pedidoService.Registrar(pedido);

                Mensajes.Info($"{"Pedido registrado con el número".Traducir()} {id}.");
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
