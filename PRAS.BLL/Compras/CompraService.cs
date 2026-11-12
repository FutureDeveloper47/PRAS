using PRAS.BLL.Excepciones;
using PRAS.DAL.Factory;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;
using PRAS.Services.Exceptions.DataAccess;
using PRAS.Services.Facade;
using PRAS.Services.Facade.ExtensionsMethods;

namespace PRAS.BLL
{
    /// <summary>
    /// Casos de uso de Compras: .001 Registrar Compra de Vehículo, .002 Consultar Compras.
    /// Registrar Compra es la ÚNICA forma en que un auto entra al sistema.
    /// </summary>
    public class CompraService
    {
        /// <summary>A los particulares siempre les pagamos por transferencia.</summary>
        public const string FormaPagoCompra = "Transferencia";

        private readonly ICompraRepository _repositorio = FactoryDataAccess.CompraRepository;
        private readonly IClienteRepository _clienteRepositorio = FactoryDataAccess.ClienteRepository;
        private readonly VehiculoService _vehiculoService = new VehiculoService();

        // Registrar la compra de un auto (CU Compras.001)

        /// <summary>
        /// Registra la compra y da de alta el auto (en estado Comprado), todo junto.
        /// Devuelve el Id de la compra.
        /// </summary>
        public int RegistrarCompra(Compra compra)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarCompras);

                if (compra.Vehiculo == null)
                    throw new CompraSinVehiculoException();

                ValidarVendedor(compra);
                ValidarVehiculo(compra.Vehiculo);
                ValidarDatosCompra(compra);

                // El estado no lo elige el usuario, cambia solo.
                compra.Vehiculo.EstadoVehiculo = EstadoVehiculo.Comprado;
                compra.FormaPago = FormaPagoCompra;

                _repositorio.Add(compra);
                LoggerService.Info($"Registró la compra {compra.IdCompra} del vehículo {compra.Vehiculo.Dominio} por $ {compra.PrecioCompra:N2}.");
                return compra.IdCompra;
            }
            catch (ReglaNegocioException ex)
            {
                // Aviso de negocio: queda en la bitácora y sigue hasta la pantalla.
                ExceptionManager.HandleException(ex);
                throw;
            }
            catch (DataAccessException)
            {
                // Ya lo registró la DAL.
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(ex);
                throw;
            }
        }

        // Consultar compras (CU Compras.002)

        public List<Compra> Listar(DateTime desde, DateTime hasta, string filtro)
        {
            Sesion.ExigirPermiso(Funcion.GestionarCompras);
            Validar.RangoFechas(desde, hasta);

            return _repositorio.Listar(desde, hasta, (filtro ?? "").Trim());
        }

        // Validaciones

        private void ValidarVendedor(Compra compra)
        {
            Validar.Seleccionado(compra.IdCliente, "el cliente que vende el vehículo");

            Cliente cliente = _clienteRepositorio.GetById(compra.IdCliente)
                ?? throw new ClienteNoEncontradoException(compra.IdCliente);

            if (!cliente.Activo)
                throw new ClienteDeshabilitadoException(cliente.NombreCompleto);
        }

        private void ValidarVehiculo(Vehiculo vehiculo)
        {
            vehiculo.IdVehiculo = 0;   // auto nuevo, todavía no tiene Id
            _vehiculoService.ValidarDatos(vehiculo);
        }

        private static void ValidarDatosCompra(Compra compra)
        {
            Validar.NoFutura(compra.FechaCompra, "compra");
            Validar.MayorACero(compra.PrecioCompra, "El precio de compra");
            compra.Observaciones = Validar.Opcional(compra.Observaciones, "Observaciones", 500);
        }
    }
}
