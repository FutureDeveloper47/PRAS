using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Comercial - Gestionar consignaciones: autos entregados a agencias externas para su venta.
    /// Desde acá se entrega un auto reparado a una agencia o se lo retira sin vender.
    /// </summary>
    public partial class FrmConsignaciones : FormBase
    {
        private readonly ConsignacionService _consignacionService = new ConsignacionService();

        public FrmConsignaciones()
        {
            InitializeComponent();
        }

        private void FrmConsignaciones_Load(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                dgvConsignaciones.Rows.Clear();
                foreach (Consignacion c in _consignacionService.Listar(chkSoloVigentes.Checked))
                {
                    int fila = dgvConsignaciones.Rows.Add(c.FechaEntrega.ToString("dd/MM/yyyy"), c.Vehiculo.Dominio,
                        $"{c.Vehiculo.Marca} {c.Vehiculo.Modelo} ({c.Vehiculo.Anio})", c.Agencia.RazonSocial,
                        c.PrecioMinimoVenta.ToString("C2"), c.PorcentajeComision.ToString("0.##") + " %",
                        c.EstadoConsignacion.Traducir(), c.FechaRetiro?.ToString("dd/MM/yyyy"));
                    dgvConsignaciones.Rows[fila].Tag = c;
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void chkSoloVigentes_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var frm = new FrmConsignacionEdicion();
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnRegistrarRetiro_Click(object sender, EventArgs e)
        {
            if (!(dgvConsignaciones.CurrentRow?.Tag is Consignacion consignacion))
            {
                Mensajes.Advertencia("Seleccione una consignación de la lista.".Traducir());
                return;
            }

            if (!Mensajes.Confirmar($"¿Retirar el vehículo {consignacion.Vehiculo.Dominio} de {consignacion.Agencia.RazonSocial}? " +
                                    "Si está publicado, la publicación se da de baja y el auto vuelve a \"Reparado\"."))
                return;

            try
            {
                _consignacionService.RegistrarRetiro(consignacion.IdConsignacion);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
