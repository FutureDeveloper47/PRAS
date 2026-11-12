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
    /// Caso de uso Comercial - Gestionar consignaciones. Un auto REPARADO se le deja a una agencia
    /// para que lo venda. Se pacta un precio mínimo de venta y el porcentaje de comisión de la agencia.
    /// </summary>
    public class ConsignacionService
    {
        private readonly IConsignacionRepository _repositorio = FactoryDataAccess.ConsignacionRepository;
        private readonly IVehiculoRepository _vehiculoRepositorio = FactoryDataAccess.VehiculoRepository;
        private readonly IAgenciaRepository _agenciaRepositorio = FactoryDataAccess.AgenciaRepository;

        public List<Consignacion> Listar(bool soloVigentes)
        {
            Sesion.ExigirPermiso(Funcion.GestionarConsignaciones);
            return _repositorio.Listar(soloVigentes);
        }

        /// <summary>Autos que se pueden entregar a una agencia (los Reparados).</summary>
        public List<Vehiculo> ListarVehiculosReparados()
        {
            Sesion.ExigirPermiso(Funcion.GestionarConsignaciones);
            return _vehiculoRepositorio.Listar(EstadoVehiculo.Reparado, "");
        }

        public List<Agencia> ListarAgencias()
        {
            Sesion.ExigirPermiso(Funcion.GestionarConsignaciones);
            return _agenciaRepositorio.Listar("", false);
        }

        /// <summary>Entrega el auto a la agencia: pasa de Reparado a En agencia. Devuelve el Id de la consignación.</summary>
        public int Registrar(Consignacion consignacion)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarConsignaciones);

                Validar.Seleccionado(consignacion.IdVehiculo, "el vehículo que se entrega a la agencia");
                Vehiculo vehiculo = _vehiculoRepositorio.GetById(consignacion.IdVehiculo)
                    ?? throw new VehiculoNoEncontradoException(consignacion.IdVehiculo);
                if (vehiculo.EstadoVehiculo != EstadoVehiculo.Reparado)
                    throw new EstadoVehiculoInvalidoException(vehiculo, "entregarlo a una agencia (tiene que estar Reparado)");

                Validar.Seleccionado(consignacion.IdAgencia, "la agencia");
                Agencia agencia = _agenciaRepositorio.GetById(consignacion.IdAgencia)
                    ?? throw new AgenciaNoEncontradaException(consignacion.IdAgencia);
                if (!agencia.Activo)
                    throw new AgenciaDeshabilitadaException(agencia.RazonSocial);

                Validar.MayorACero(consignacion.PrecioMinimoVenta, "El precio mínimo de venta");
                if (consignacion.PorcentajeComision <= 0 || consignacion.PorcentajeComision > 100)
                    throw new ComisionInvalidaException(consignacion.PorcentajeComision);
                Validar.NoFutura(consignacion.FechaEntrega, "entrega");

                _repositorio.Add(consignacion);
                LoggerService.Info($"Consignación {consignacion.IdConsignacion}: entregó el vehículo {vehiculo.Dominio} a {agencia.RazonSocial} " +
                                   $"(mínimo {consignacion.PrecioMinimoVenta:C2}, comisión {consignacion.PorcentajeComision}%).");
                return consignacion.IdConsignacion;
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

        /// <summary>Retiramos el auto de la agencia sin venderlo: vuelve a Reparado.</summary>
        public void RegistrarRetiro(int idConsignacion)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarConsignaciones);

                Consignacion consignacion = _repositorio.GetById(idConsignacion)
                    ?? throw new ConsignacionNoEncontradaException(idConsignacion);
                if (consignacion.EstadoConsignacion != Estados.ConsignacionVigente)
                    throw new ConsignacionNoVigenteException($"La consignación ya está {consignacion.EstadoConsignacion.ToLower()}.");

                _repositorio.RegistrarRetiro(idConsignacion, DateTime.Now);
                LoggerService.Info($"Retiró el vehículo {consignacion.Vehiculo.Dominio} de la agencia {consignacion.Agencia.RazonSocial}.");
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
