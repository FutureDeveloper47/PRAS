using PRAS.BLL;
using PRAS.Entidades;

namespace PRAS.UI
{
    /// <summary>
    /// CU Proveedores.004 - Listar. Desde acá se llega a Alta (.001), Modificar (.002)
    /// y Deshabilitar (.003). Misma estructura que FrmClientes.
    /// </summary>
    public partial class FrmProveedores : FormBase
    {
        private readonly ProveedorService _proveedorService = new ProveedorService();

        public FrmProveedores()
        {
            InitializeComponent();
            dgvProveedores.AutoGenerateColumns = false;   // usamos solo las columnas del diseñador
        }

        private void FrmProveedores_Load(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                dgvProveedores.DataSource = _proveedorService.Listar(txtBuscar.Text, chkMostrarDeshabilitados.Checked);
                ActualizarBotonDeshabilitar();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Devuelve el proveedor de la fila seleccionada, o null si no hay ninguna.</summary>
        private Proveedor ProveedorSeleccionado()
        {
            return dgvProveedores.CurrentRow?.DataBoundItem as Proveedor;
        }

        /// <summary>El mismo botón sirve para deshabilitar o volver a habilitar, según la fila elegida.</summary>
        private void ActualizarBotonDeshabilitar()
        {
            Proveedor proveedor = ProveedorSeleccionado();
            btnDeshabilitar.Text = (proveedor != null && !proveedor.Activo) ? "Habilitar" : "Deshabilitar";
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
            using var frm = new FrmProveedorEdicion(null);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Proveedor proveedor = ProveedorSeleccionado();
            if (proveedor == null)
            {
                Mensajes.Advertencia("Seleccione un proveedor de la lista.");
                return;
            }

            using var frm = new FrmProveedorEdicion(proveedor);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            Proveedor proveedor = ProveedorSeleccionado();
            if (proveedor == null)
            {
                Mensajes.Advertencia("Seleccione un proveedor de la lista.");
                return;
            }

            try
            {
                if (proveedor.Activo)
                {
                    if (!Mensajes.Confirmar($"¿Deshabilitar al proveedor {proveedor.RazonSocial}?"))
                        return;
                    _proveedorService.Deshabilitar(proveedor.IdProveedor);
                }
                else
                {
                    if (!Mensajes.Confirmar($"¿Volver a habilitar al proveedor {proveedor.RazonSocial}?"))
                        return;
                    _proveedorService.Habilitar(proveedor.IdProveedor);
                }
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void dgvProveedores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                btnModificar_Click(sender, e);
        }

        private void dgvProveedores_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotonDeshabilitar();
        }
    }
}
