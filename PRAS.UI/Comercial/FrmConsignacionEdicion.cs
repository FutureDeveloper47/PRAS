using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Comercial - Registrar consignación: se entrega un auto REPARADO a una agencia,
    /// con el precio mínimo de venta y el porcentaje de comisión pactados.
    /// </summary>
    public partial class FrmConsignacionEdicion : FormBase
    {
        private readonly ConsignacionService _consignacionService = new ConsignacionService();

        public FrmConsignacionEdicion()
        {
            InitializeComponent();
        }

        private void FrmConsignacionEdicion_Load(object sender, EventArgs e)
        {
            dtpFecha.Value = DateTime.Today;
            try
            {
                cboVehiculo.DisplayMember = nameof(Vehiculo.Descripcion);
                cboVehiculo.ValueMember = nameof(Vehiculo.IdVehiculo);
                cboVehiculo.DataSource = _consignacionService.ListarVehiculosReparados();

                cboAgencia.DisplayMember = nameof(Agencia.RazonSocial);
                cboAgencia.ValueMember = nameof(Agencia.IdAgencia);
                cboAgencia.DataSource = _consignacionService.ListarAgencias();

                if (cboVehiculo.Items.Count == 0)
                    Mensajes.Info("No hay autos en estado Reparado para entregar a una agencia.".Traducir());
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                _consignacionService.Registrar(new Consignacion
                {
                    IdVehiculo = cboVehiculo.SelectedValue as int? ?? 0,
                    IdAgencia = cboAgencia.SelectedValue as int? ?? 0,
                    FechaEntrega = dtpFecha.Value.Date,
                    PrecioMinimoVenta = numPrecioMinimo.Value,
                    PorcentajeComision = numComision.Value
                });

                Mensajes.Info("Consignación registrada. El auto quedó En agencia.".Traducir());
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
