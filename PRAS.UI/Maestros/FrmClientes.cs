using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// CU Clientes.005 - Listar. Desde esta pantalla se llega a Alta (.001),
    /// Modificar (.002), Deshabilitar (.003) y Consultar Historial (.004).
    /// MOLDE para las pantallas de Mecánicos, Proveedores y Agencias.
    /// </summary>
    public partial class FrmClientes : FormBase
    {
        private readonly ClienteService _clienteService = new ClienteService();

        public FrmClientes()
        {
            InitializeComponent();
            dgvClientes.AutoGenerateColumns = false;   // usamos solo las columnas del diseñador
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                dgvClientes.DataSource = _clienteService.Listar(txtBuscar.Text, chkMostrarDeshabilitados.Checked);
                ActualizarBotonDeshabilitar();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Devuelve el cliente de la fila seleccionada, o null si no hay ninguna.</summary>
        private Cliente ClienteSeleccionado()
        {
            return dgvClientes.CurrentRow?.DataBoundItem as Cliente;
        }

        /// <summary>El mismo botón sirve para deshabilitar o volver a habilitar, según la fila elegida.</summary>
        private void ActualizarBotonDeshabilitar()
        {
            Cliente cliente = ClienteSeleccionado();
            btnDeshabilitar.Text = ((cliente != null && !cliente.Activo) ? "Habilitar" : "Deshabilitar").Traducir();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void chkMostrarDeshabilitados_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var frm = new FrmClienteEdicion(null);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Cliente cliente = ClienteSeleccionado();
            if (cliente == null)
            {
                Mensajes.Advertencia("Seleccione un cliente de la lista.");
                return;
            }

            using var frm = new FrmClienteEdicion(cliente);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            Cliente cliente = ClienteSeleccionado();
            if (cliente == null)
            {
                Mensajes.Advertencia("Seleccione un cliente de la lista.");
                return;
            }

            try
            {
                if (cliente.Activo)
                {
                    if (!Mensajes.Confirmar($"¿Deshabilitar al cliente {cliente.NombreCompleto}?"))
                        return;
                    _clienteService.Deshabilitar(cliente.IdCliente);
                }
                else
                {
                    if (!Mensajes.Confirmar($"¿Volver a habilitar al cliente {cliente.NombreCompleto}?"))
                        return;
                    _clienteService.Habilitar(cliente.IdCliente);
                }
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            Cliente cliente = ClienteSeleccionado();
            if (cliente == null)
            {
                Mensajes.Advertencia("Seleccione un cliente de la lista.");
                return;
            }

            using var frm = new FrmClienteHistorial(cliente);
            frm.ShowDialog(this);
        }

        private void dgvClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                btnModificar_Click(sender, e);
        }

        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotonDeshabilitar();
        }
    }
}
