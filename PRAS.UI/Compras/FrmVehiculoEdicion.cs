using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// "Ver Detalle" y "Modificar" del Buscador de autos (Pantalla C).
    /// Es la misma pantalla en los dos casos: con soloLectura = true no se puede editar nada.
    /// </summary>
    public partial class FrmVehiculoEdicion : FormBase
    {
        private readonly VehiculoService _vehiculoService = new VehiculoService();
        private readonly Vehiculo _vehiculo;
        private readonly bool _soloLectura;

        public FrmVehiculoEdicion(Vehiculo vehiculo, bool soloLectura)
        {
            InitializeComponent();
            _vehiculo = vehiculo;
            _soloLectura = soloLectura;
        }

        private void FrmVehiculoEdicion_Load(object sender, EventArgs e)
        {
            Text = _soloLectura ? "Detalle del vehículo" : "Modificar vehículo";
            nudAnio.Maximum = DateTime.Today.Year + 1;

            txtDominio.Text = _vehiculo.Dominio;
            txtMarca.Text = _vehiculo.Marca;
            txtModelo.Text = _vehiculo.Modelo;
            nudAnio.Value = Math.Min(_vehiculo.Anio, nudAnio.Maximum);
            txtColor.Text = _vehiculo.Color;
            txtChasis.Text = _vehiculo.NumeroChasis;
            txtMotor.Text = _vehiculo.NumeroMotor;
            txtEstado.Text = VehiculoService.TextoEstado(_vehiculo.EstadoVehiculo);

            MostrarCompra();

            if (_soloLectura)
            {
                // Modo Ver Detalle: todo bloqueado, solo se puede cerrar.
                txtDominio.ReadOnly = true;
                txtMarca.ReadOnly = true;
                txtModelo.ReadOnly = true;
                nudAnio.Enabled = false;
                txtColor.ReadOnly = true;
                txtChasis.ReadOnly = true;
                txtMotor.ReadOnly = true;
                btnGuardar.Visible = false;
                btnCancelar.Text = "CERRAR".Traducir();
            }
        }

        /// <summary>Muestra a quién, cuándo y por cuánto se compró el auto.</summary>
        private void MostrarCompra()
        {
            try
            {
                Compra compra = _vehiculoService.ObtenerCompra(_vehiculo.IdVehiculo);
                if (compra == null)
                {
                    lblCompra.Text = "Sin datos de compra.".Traducir();
                    return;
                }

                lblCompra.Text =
                    $"Vendedor: {compra.Cliente.NombreCompleto} (DNI {compra.Cliente.DNI})\n" +
                    $"Fecha: {compra.FechaCompra:dd/MM/yyyy}   -   Precio: {compra.PrecioCompra:C2}   -   Pago: {compra.FormaPago}";
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Usamos una copia: si la BLL rechaza algo, el original de la lista no se toca.
            var modificado = new Vehiculo
            {
                IdVehiculo = _vehiculo.IdVehiculo,
                Dominio = txtDominio.Text,
                Marca = txtMarca.Text,
                Modelo = txtModelo.Text,
                Anio = (int)nudAnio.Value,
                Color = txtColor.Text,
                NumeroChasis = txtChasis.Text,
                NumeroMotor = txtMotor.Text,
                EstadoVehiculo = _vehiculo.EstadoVehiculo
            };

            try
            {
                _vehiculoService.Modificar(modificado);
                Mensajes.Info("Los datos del vehículo se guardaron correctamente.");
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
