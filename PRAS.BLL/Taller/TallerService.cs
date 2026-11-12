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
    /// Paso del auto por el taller (una pantalla por etapa):
    /// - Ingreso al taller (Asignación de presupuesto): el auto entra con su mecánico, diagnóstico y prioridad.
    /// - Carga de costos: se cargan los gastos (repuestos y mano de obra) y se termina la reparación.
    /// </summary>
    public class TallerService
    {
        // Prioridades de la orden (los tres botones de la pantalla).
        public const string PrioridadBaja = "Baja";
        public const string PrioridadMedia = "Media";
        public const string PrioridadAlta = "Alta";

        // Tipos de gasto (el combo de la pantalla).
        public const string GastoRepuesto = "Repuesto";
        public const string GastoManoDeObra = "Mano de obra";

        private const int LargoDiagnostico = 1000;   // largo de la columna Diagnostico.DescripcionDanios
        private const int LargoDescripcionGasto = 200;   // largo de la columna DetallePresupuesto.DescripcionItem

        private readonly ITallerRepository _repositorio = FactoryDataAccess.TallerRepository;
        private readonly IPresupuestoRepository _presupuestoRepositorio = FactoryDataAccess.PresupuestoRepository;
        private readonly IDetallePresupuestoRepository _detallePresupuestoRepositorio = FactoryDataAccess.DetallePresupuestoRepository;
        private readonly IVehiculoRepository _vehiculoRepositorio = FactoryDataAccess.VehiculoRepository;
        private readonly IMecanicoRepository _mecanicoRepositorio = FactoryDataAccess.MecanicoRepository;

        // Ingreso del auto al taller

        /// <summary>Autos que pueden entrar al taller (los que están Comprados).</summary>
        public List<Vehiculo> ListarVehiculosParaIngreso()
        {
            Sesion.ExigirPermiso(Funcion.RegistrarRecepcion);
            return _vehiculoRepositorio.Listar(EstadoVehiculo.Comprado, "");
        }

        public List<Mecanico> ListarMecanicos()
        {
            Sesion.ExigirPermiso(Funcion.RegistrarRecepcion);
            return _mecanicoRepositorio.Listar("", false);
        }

        /// <summary>
        /// El auto entra al taller: se crean la recepción, el diagnóstico, un presupuesto vacío,
        /// la orden de trabajo y la tarea del mecánico. El auto pasa a En reparación.
        /// </summary>
        public void RegistrarIngreso(int idVehiculo, int idMecanico, string diagnostico, string prioridad)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.RegistrarRecepcion);

                Validar.Seleccionado(idVehiculo, "el vehículo que ingresa al taller");
                Vehiculo vehiculo = _vehiculoRepositorio.GetById(idVehiculo)
                    ?? throw new VehiculoNoEncontradoException(idVehiculo);
                if (vehiculo.EstadoVehiculo != EstadoVehiculo.Comprado)
                    throw new EstadoVehiculoInvalidoException(vehiculo, "ingresarlo al taller (tiene que estar Comprado)");

                Mecanico mecanico = ObtenerMecanicoActivo(idMecanico);
                diagnostico = Validar.Requerido(diagnostico, "Diagnóstico inicial", LargoDiagnostico);
                ValidarPrioridad(prioridad);

                _repositorio.RegistrarIngreso(idVehiculo, idMecanico, diagnostico, prioridad);
                LoggerService.Info($"Ingreso a taller del vehículo {vehiculo.Dominio} (mecánico {mecanico.NombreCompleto}, prioridad {prioridad}).");
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

        // Carga de costos

        /// <summary>Último presupuesto de cada auto que está en reparación.</summary>
        public List<Presupuesto> ListarPresupuestosEnReparacion()
        {
            Sesion.ExigirPermiso(Funcion.GestionarPresupuestos);
            return _presupuestoRepositorio.ListarDeAutosEnReparacion();
        }

        public List<DetallePresupuesto> ListarGastos(int idPresupuesto)
        {
            Sesion.ExigirPermiso(Funcion.GestionarPresupuestos);
            return _detallePresupuestoRepositorio.ListarPorPresupuesto(idPresupuesto);
        }

        /// <summary>Arma y valida un gasto. Todavía NO se guarda en la base (se guarda con GuardarGastos).</summary>
        public DetallePresupuesto CrearGasto(string tipo, string descripcion, int cantidad, decimal precioUnitario)
        {
            if (tipo != GastoRepuesto && tipo != GastoManoDeObra)
                throw new TipoGastoInvalidoException();

            string texto = $"{tipo}: {Validar.Requerido(descripcion, "Descripción", LargoDescripcionGasto)}";
            if (texto.Length > LargoDescripcionGasto)
                throw new LargoMaximoException("Descripción", LargoDescripcionGasto - tipo.Length - 2);

            Validar.MayorACero(cantidad, "La cantidad");
            Validar.MayorACero(precioUnitario, "El precio");

            return new DetallePresupuesto
            {
                DescripcionItem = texto,
                Cantidad = cantidad,
                PrecioUnitario = precioUnitario,
                Subtotal = cantidad * precioUnitario,
                Aprobado = true   // si el auto es nuestro, el presupuesto se aprueba al cargarlo
            };
        }

        public void GuardarGastos(int idPresupuesto, List<DetallePresupuesto> gastosNuevos)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarPresupuestos);

                Validar.Seleccionado(idPresupuesto, "el auto");
                if (gastosNuevos == null || gastosNuevos.Count == 0)
                    throw new SinGastosNuevosException();

                _presupuestoRepositorio.AgregarDetalles(idPresupuesto, gastosNuevos);
                LoggerService.Info($"Carga de {gastosNuevos.Count} gasto(s) en el presupuesto {idPresupuesto}.");
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

        /// <summary>Termina la reparación: el auto pasa de En reparación a Reparado.</summary>
        public void FinalizarReparacion(int idVehiculo)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarPresupuestos);

                Vehiculo vehiculo = _vehiculoRepositorio.GetById(idVehiculo)
                    ?? throw new VehiculoNoEncontradoException(idVehiculo);
                if (vehiculo.EstadoVehiculo != EstadoVehiculo.EnReparacion)
                    throw new EstadoVehiculoInvalidoException(vehiculo, "finalizar su reparación (no está en reparación)");

                _repositorio.FinalizarReparacion(idVehiculo);
                LoggerService.Info($"Finalizó la reparación del vehículo {vehiculo.Dominio}.");
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

        // ---------------------------- Reglas que usan otros servicios del taller ----------------------------

        /// <summary>La prioridad tiene que ser Alta, Media o Baja.</summary>
        internal static void ValidarPrioridad(string prioridad)
        {
            if (prioridad != PrioridadBaja && prioridad != PrioridadMedia && prioridad != PrioridadAlta)
                throw new PrioridadInvalidaException();
        }

        /// <summary>El mecánico elegido tiene que existir y estar habilitado.</summary>
        internal static Mecanico ObtenerMecanicoActivo(int idMecanico)
        {
            Validar.Seleccionado(idMecanico, "el mecánico");

            Mecanico mecanico = FactoryDataAccess.MecanicoRepository.GetById(idMecanico)
                ?? throw new MecanicoNoEncontradoException(idMecanico);
            if (!mecanico.Activo)
                throw new MecanicoDeshabilitadoException(mecanico.NombreCompleto);
            return mecanico;
        }
    }
}
