using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>Desde qué opción del menú se abrió la pantalla de pedidos.</summary>
    public enum ModoPedidos
    {
        Registrar,   // Repuestos > Registrar pedido
        Entrega,   // Repuestos > Registrar entrega de pedido
        Consulta   // Repuestos > Consultar pedidos
    }

    /// <summary>
    /// Repuestos - Registrar pedido, Registrar entrega y Consultar pedidos (una sola pantalla).
    /// Los botones aparecen según los permisos del usuario.
    /// </summary>
    public partial class FrmPedidos : FormBase
    {
        private readonly PedidoRepuestoService _pedidoService = new PedidoRepuestoService();
        private readonly ModoPedidos _modo;

        public FrmPedidos(ModoPedidos modo)
        {
            InitializeComponent();
            _modo = modo;

            // El título va acá y no en el Load porque FormBase lo traduce y arma
            // la franja azul antes de que corra el Load.
            switch (modo)
            {
                case ModoPedidos.Registrar: Text = "Registrar pedido de repuestos"; break;
                case ModoPedidos.Entrega: Text = "Registrar entrega de pedido"; break;
                default: Text = "Consultar pedidos de repuestos"; break;
            }
        }

        private void FrmPedidos_Load(object sender, EventArgs e)
        {
            btnNuevo.Visible = _modo != ModoPedidos.Entrega && _pedidoService.PuedeRegistrarPedido();
            btnRegistrarEntrega.Visible = _modo != ModoPedidos.Registrar && _pedidoService.PuedeRegistrarEntrega();

            cboEstado.Items.AddRange(new object[] { "Pendientes".Traducir(), "Entregados".Traducir(), "Todos".Traducir() });
            cboEstado.SelectedIndex = _modo == ModoPedidos.Consulta ? 2 : 0;
        }

        private string EstadoElegido()
        {
            switch (cboEstado.SelectedIndex)
            {
                case 0: return Estados.PedidoPendiente;
                case 1: return Estados.PedidoEntregado;
                default: return null;
            }
        }

        private void CargarGrilla()
        {
            try
            {
                dgvPedidos.Rows.Clear();
                dgvDetalle.Rows.Clear();
                foreach (PedidoRepuesto p in _pedidoService.Listar(EstadoElegido()))
                {
                    int fila = dgvPedidos.Rows.Add(p.IdPedido, p.FechaPedido.ToString("dd/MM/yyyy"), p.Proveedor.RazonSocial,
                        p.IdOrdenTrabajo, p.DominioVehiculo, p.EstadoPedido.Traducir(),
                        p.FechaEntrega?.ToString("dd/MM/yyyy"), p.MontoTotal.ToString("C2"));
                    dgvPedidos.Rows[fila].Tag = p;
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private PedidoRepuesto PedidoSeleccionado => dgvPedidos.CurrentRow?.Tag as PedidoRepuesto;

        private void cboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void dgvPedidos_SelectionChanged(object sender, EventArgs e)
        {
            dgvDetalle.Rows.Clear();
            PedidoRepuesto pedido = PedidoSeleccionado;
            if (pedido == null)
                return;

            btnRegistrarEntrega.Enabled = pedido.EstadoPedido == Estados.PedidoPendiente;

            try
            {
                foreach (DetallePedidoRepuesto d in _pedidoService.ListarDetalles(pedido.IdPedido))
                    dgvDetalle.Rows.Add(d.DescripcionRepuesto, d.Cantidad, d.PrecioUnitario.ToString("C2"), d.Subtotal.ToString("C2"));
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var frm = new FrmPedidoEdicion();
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnRegistrarEntrega_Click(object sender, EventArgs e)
        {
            PedidoRepuesto pedido = PedidoSeleccionado;
            if (pedido == null)
            {
                Mensajes.Advertencia("Seleccione un pedido de la lista.".Traducir());
                return;
            }

            if (!Mensajes.Confirmar($"¿Registrar que {pedido.Proveedor.RazonSocial} entregó el pedido {pedido.IdPedido}? " +
                                    $"Su total ({pedido.MontoTotal:C2}) se suma al presupuesto de la OT {pedido.IdOrdenTrabajo}."))
                return;

            try
            {
                _pedidoService.RegistrarEntrega(pedido.IdPedido);
                Mensajes.Info("Entrega registrada. El costo se sumó al presupuesto de la reparación.".Traducir());
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
