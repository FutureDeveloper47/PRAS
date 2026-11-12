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
    /// Casos de uso Comercial - Registrar venta y Consultar ventas. La agencia le vende el auto a un cliente.
    /// Reglas:
    ///   - el auto tiene que estar en la agencia (consignación vigente), publicado o no;
    ///   - el precio de venta no puede ser menor al precio mínimo pactado;
    ///   - la comisión de la agencia se calcula sola con el porcentaje de la consignación.
    /// </summary>
    public class VentaService
    {
        public static readonly string[] FormasPago = { "Efectivo", "Transferencia", "Cheque", "Financiado" };

        private readonly IVentaRepository _repositorio = FactoryDataAccess.VentaRepository;
        private readonly IConsignacionRepository _consignacionRepositorio = FactoryDataAccess.ConsignacionRepository;
        private readonly IClienteRepository _clienteRepositorio = FactoryDataAccess.ClienteRepository;

        // Consultar ventas

        public List<Venta> Listar(DateTime desde, DateTime hasta, string filtro)
        {
            Sesion.ExigirPermiso(Funcion.GestionarVentas);
            Validar.RangoFechas(desde, hasta);
            return _repositorio.Listar(desde, hasta, (filtro ?? "").Trim());
        }

        // Registrar venta

        /// <summary>Autos que se pueden vender (los que están en una agencia).</summary>
        public List<Consignacion> ListarConsignacionesVigentes()
        {
            Sesion.ExigirPermiso(Funcion.GestionarVentas);
            return _consignacionRepositorio.Listar(true);
        }

        /// <summary>Busca al comprador por DNI. null si no existe (la pantalla ofrece darlo de alta).</summary>
        public Cliente BuscarCliente(string dni)
        {
            Sesion.ExigirPermiso(Funcion.GestionarVentas);
            return _clienteRepositorio.ObtenerPorDNI(Validar.DNI(dni));
        }

        /// <summary>Comisión de la agencia = precio de venta x porcentaje pactado.</summary>
        public static decimal CalcularComision(decimal precioVenta, decimal porcentajeComision)
        {
            return Math.Round(precioVenta * porcentajeComision / 100, 2);
        }

        /// <summary>Registra la venta: la consignación pasa a Vendida y el auto a Vendido. Devuelve el Id de la venta.</summary>
        public int Registrar(int idConsignacion, int idCliente, decimal precioVenta, string formaPago, DateTime fechaVenta)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarVentas);

                Validar.Seleccionado(idConsignacion, "el vehículo vendido");
                Consignacion consignacion = _consignacionRepositorio.GetById(idConsignacion)
                    ?? throw new ConsignacionNoEncontradaException(idConsignacion);
                if (consignacion.EstadoConsignacion != Estados.ConsignacionVigente)
                    throw new ConsignacionNoVigenteException("El vehículo ya no está en la agencia (la consignación no está vigente).");

                Vehiculo vehiculo = consignacion.Vehiculo;
                if (vehiculo.EstadoVehiculo != EstadoVehiculo.EnAgencia && vehiculo.EstadoVehiculo != EstadoVehiculo.Publicado)
                    throw new EstadoVehiculoInvalidoException(vehiculo, "venderlo");

                Validar.Seleccionado(idCliente, "o cargue el cliente comprador");
                Cliente cliente = _clienteRepositorio.GetById(idCliente)
                    ?? throw new ClienteNoEncontradoException(idCliente);
                if (!cliente.Activo)
                    throw new ClienteDeshabilitadoException(cliente.NombreCompleto);

                Validar.MayorACero(precioVenta, "El precio de venta");
                if (precioVenta < consignacion.PrecioMinimoVenta)
                    throw new PrecioMenorAlMinimoException(precioVenta, consignacion.PrecioMinimoVenta);

                if (!FormasPago.Contains(formaPago))
                    throw new SeleccionObligatoriaException("la forma de pago");

                Validar.NoFutura(fechaVenta, "venta");
                if (fechaVenta.Date < consignacion.FechaEntrega.Date)
                    throw new FechaVentaInvalidaException();

                var venta = new Venta
                {
                    IdConsignacion = idConsignacion,
                    IdCliente = idCliente,
                    FechaVenta = fechaVenta.Date,
                    PrecioVenta = precioVenta,
                    MontoComision = CalcularComision(precioVenta, consignacion.PorcentajeComision),
                    FormaPago = formaPago
                };
                _repositorio.Registrar(venta, vehiculo.IdVehiculo);

                LoggerService.Info($"Venta {venta.IdVenta}: vehículo {vehiculo.Dominio} a {cliente.NombreCompleto} por {precioVenta:C2} " +
                                   $"(agencia {consignacion.Agencia.RazonSocial}, comisión {venta.MontoComision:C2}).");
                return venta.IdVenta;
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
    }
}
