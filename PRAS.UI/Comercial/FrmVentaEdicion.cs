using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Comercial - Registrar venta. Se elige el auto (que está en una agencia), se busca al
    /// comprador por DNI (o se lo da de alta) y se carga el precio. La comisión de la agencia
    /// se calcula sola con el porcentaje de la consignación.
    /// </summary>
    public partial class FrmVentaEdicion : FormBase
    {
        private readonly VentaService _ventaService = new VentaService();
        private Cliente _cliente;

        public FrmVentaEdicion()
        {
            InitializeComponent();
        }

        private void FrmVentaEdicion_Load(object sender, EventArgs e)
        {
            dtpFecha.Value = DateTime.Today;
            dtpFecha.MaxDate = DateTime.Today;
            cboFormaPago.Items.AddRange(VentaService.FormasPago);
            cboFormaPago.SelectedIndex = 1;   // Transferencia

            try
            {
                List<Consignacion> consignaciones = _ventaService.ListarConsignacionesVigentes();
                cboConsignacion.DisplayMember = "Texto";
                cboConsignacion.DataSource = consignaciones
                    .Select(c => new { Texto = $"{c.Vehiculo.Descripcion} - {c.Agencia.RazonSocial}", Consignacion = c })
                    .ToList();

                if (consignaciones.Count == 0)
                    Mensajes.Info("No hay autos en agencias para vender.".Traducir());
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private Consignacion ConsignacionElegida()
        {
            object item = cboConsignacion.SelectedItem;
            return item?.GetType().GetProperty("Consignacion")?.GetValue(item) as Consignacion;
        }

        private void cboConsignacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            Consignacion consignacion = ConsignacionElegida();
            if (consignacion == null)
            {
                lblDatosConsignacion.Text = "";
                return;
            }

            lblDatosConsignacion.Text =
                $"{"Precio mínimo".Traducir()}: {consignacion.PrecioMinimoVenta:C2}   |   " +
                $"{"Comisión".Traducir()}: {consignacion.PorcentajeComision:0.##} %   |   " +
                $"{"Estado".Traducir()}: {VehiculoService.TextoEstado(consignacion.Vehiculo.EstadoVehiculo).Traducir()}";

            if (numPrecio.Value < consignacion.PrecioMinimoVenta)
                numPrecio.Value = Math.Min(consignacion.PrecioMinimoVenta, numPrecio.Maximum);

            ActualizarComision();
        }

        private void numPrecio_ValueChanged(object sender, EventArgs e)
        {
            ActualizarComision();
        }

        /// <summary>Muestra la comisión que se le paga a la agencia con el precio cargado.</summary>
        private void ActualizarComision()
        {
            Consignacion consignacion = ConsignacionElegida();
            decimal comision = consignacion == null ? 0 : VentaService.CalcularComision(numPrecio.Value, consignacion.PorcentajeComision);
            lblMontoComision.Text = comision.ToString("C2");
        }

        // Comprador

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                _cliente = _ventaService.BuscarCliente(txtDNI.Text);
                if (_cliente == null)
                {
                    lblCliente.Text = "(sin cliente)".Traducir();
                    if (Mensajes.Confirmar("No hay ningún cliente con ese DNI. ¿Desea darlo de alta?".Traducir()))
                        btnNuevoCliente_Click(sender, e);
                    return;
                }

                MostrarCliente();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnNuevoCliente_Click(object sender, EventArgs e)
        {
            using var frm = new FrmClienteEdicion(null);
            if (frm.ShowDialog(this) == DialogResult.OK && frm.ClienteGuardado != null)
            {
                _cliente = frm.ClienteGuardado;
                txtDNI.Text = _cliente.DNI;
                MostrarCliente();
            }
        }

        private void MostrarCliente()
        {
            lblCliente.Text = _cliente.Activo
                ? $"{_cliente.NombreCompleto} (DNI {_cliente.DNI})"
                : $"{_cliente.NombreCompleto} (DNI {_cliente.DNI}) - " + "DESHABILITADO".Traducir();
        }

        // Guardar

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Consignacion consignacion = ConsignacionElegida();
            if (consignacion != null && _cliente != null &&
                !Mensajes.Confirmar($"¿Registrar la venta de {consignacion.Vehiculo.Dominio} a {_cliente.NombreCompleto} por {numPrecio.Value:C2}?"))
                return;

            try
            {
                int id = _ventaService.Registrar(consignacion?.IdConsignacion ?? 0, _cliente?.IdCliente ?? 0,
                    numPrecio.Value, cboFormaPago.SelectedItem as string, dtpFecha.Value);

                Mensajes.Info($"{"Venta registrada con el número".Traducir()} {id}. " + "El auto quedó Vendido.".Traducir());
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
