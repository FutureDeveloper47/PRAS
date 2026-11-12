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
    /// Casos de uso del taller:
    ///   - Gestionar órdenes de trabajo (prioridad, agregar tareas, finalizar la orden).
    ///   - Consultar órdenes de trabajo (el mecánico ve solo las suyas).
    ///   - Registrar tareas realizadas (el mecánico inicia y finaliza sus tareas).
    /// </summary>
    public class OrdenTrabajoService
    {
        public static readonly string[] Prioridades = { TallerService.PrioridadAlta, TallerService.PrioridadMedia, TallerService.PrioridadBaja };

        private const int LargoDescripcionTarea = 300;

        private readonly IOrdenTrabajoRepository _repositorio = FactoryDataAccess.OrdenTrabajoRepository;
        private readonly ITareaOrdenTrabajoRepository _tareaRepositorio = FactoryDataAccess.TareaOrdenTrabajoRepository;
        private readonly ITallerRepository _tallerRepositorio = FactoryDataAccess.TallerRepository;
        private readonly IVehiculoRepository _vehiculoRepositorio = FactoryDataAccess.VehiculoRepository;
        private readonly IMecanicoRepository _mecanicoRepositorio = FactoryDataAccess.MecanicoRepository;

        // Consultar

        /// <summary>
        /// Órdenes de trabajo. estado = null trae todas.
        /// Quien gestiona las órdenes ve todas; el mecánico ve solo aquellas donde tiene tareas.
        /// </summary>
        public List<OrdenTrabajo> Listar(string estado)
        {
            return _repositorio.Listar(estado, MecanicoFiltro(Funcion.ConsultarOrdenesTrabajo));
        }

        public List<TareaOrdenTrabajo> ListarTareas(int idOrdenTrabajo)
        {
            MecanicoFiltro(Funcion.ConsultarOrdenesTrabajo);
            return _tareaRepositorio.ListarPorOrden(idOrdenTrabajo);
        }

        public bool PuedeGestionar()
        {
            return Sesion.TienePermiso(Funcion.GestionarOrdenesTrabajo);
        }

        public List<Mecanico> ListarMecanicos()
        {
            Sesion.ExigirPermiso(Funcion.GestionarOrdenesTrabajo);
            return _mecanicoRepositorio.Listar("", false);
        }

        // Gestionar

        public void CambiarPrioridad(int idOrdenTrabajo, string prioridad)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarOrdenesTrabajo);
                TallerService.ValidarPrioridad(prioridad);
                OrdenTrabajo orden = ObtenerAbierta(idOrdenTrabajo);

                _repositorio.CambiarPrioridad(idOrdenTrabajo, prioridad);
                LoggerService.Info($"Cambió la prioridad de la OT {orden.IdOrdenTrabajo} de {orden.Prioridad} a {prioridad}.");
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

        /// <summary>Agrega una tarea a la orden, asignada a un mecánico.</summary>
        public void AgregarTarea(int idOrdenTrabajo, int idMecanico, string descripcion, decimal costoEstimado)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarOrdenesTrabajo);
                ObtenerAbierta(idOrdenTrabajo);

                Mecanico mecanico = TallerService.ObtenerMecanicoActivo(idMecanico);
                descripcion = Validar.Requerido(descripcion, "Descripción de la tarea", LargoDescripcionTarea);
                if (costoEstimado < 0)
                    throw new CostoNegativoException();

                _tareaRepositorio.Add(new TareaOrdenTrabajo
                {
                    IdOrdenTrabajo = idOrdenTrabajo,
                    IdMecanico = idMecanico,
                    DescripcionTarea = descripcion,
                    CostoEstimado = costoEstimado
                });
                LoggerService.Info($"Agregó la tarea \"{descripcion}\" a la OT {idOrdenTrabajo} (mecánico {mecanico.NombreCompleto}).");
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

        /// <summary>
        /// Finaliza la orden de trabajo: el auto queda Reparado.
        /// Regla: no puede tener tareas sin terminar ni pedidos de repuestos sin entregar.
        /// </summary>
        public void Finalizar(int idOrdenTrabajo)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarOrdenesTrabajo);
                OrdenTrabajo orden = ObtenerAbierta(idOrdenTrabajo);

                _repositorio.ContarPendientes(idOrdenTrabajo, out int tareas, out int pedidos);
                if (tareas > 0 || pedidos > 0)
                    throw new OrdenConPendientesException(idOrdenTrabajo, tareas, pedidos);

                Vehiculo vehiculo = _vehiculoRepositorio.GetById(orden.IdVehiculo)
                    ?? throw new VehiculoNoEncontradoException(orden.IdVehiculo);
                if (vehiculo.EstadoVehiculo != EstadoVehiculo.EnReparacion)
                    throw new EstadoVehiculoInvalidoException(vehiculo, "finalizar su reparación");

                _tallerRepositorio.FinalizarReparacion(orden.IdVehiculo);
                LoggerService.Info($"Finalizó la OT {idOrdenTrabajo}: el vehículo {vehiculo.Dominio} queda Reparado.");
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

        // Registrar tareas realizadas

        /// <summary>
        /// Tareas de las órdenes abiertas, de la más urgente a la menos urgente.
        /// Si el usuario es un mecánico, solo las suyas.
        /// </summary>
        public List<TareaOrdenTrabajo> ListarTareasParaRegistrar(bool incluirFinalizadas)
        {
            int? idMecanico = MecanicoFiltro(Funcion.RegistrarTareasRealizadas);
            return _tareaRepositorio.ListarDeMecanico(idMecanico, incluirFinalizadas);
        }

        /// <summary>El mecánico empieza la tarea: pasa de Pendiente a En curso.</summary>
        public void IniciarTarea(int idTarea)
        {
            try
            {
                TareaOrdenTrabajo tarea = ObtenerTareaPropia(idTarea);
                if (tarea.EstadoTarea != Estados.TareaPendiente)
                    throw new TareaEstadoInvalidoException($"Solo se puede iniciar una tarea Pendiente (esta está \"{tarea.EstadoTarea}\").");

                if (!_tareaRepositorio.CambiarEstado(idTarea, Estados.TareaPendiente, Estados.TareaEnCurso, DateTime.Now))
                    throw new TareaEstadoInvalidoException("La tarea cambió de estado. Actualice la lista.");

                LoggerService.Info($"Inició la tarea {idTarea} (OT {tarea.IdOrdenTrabajo}, vehículo {tarea.Vehiculo.Dominio}).");
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

        /// <summary>El mecánico termina la tarea (si nunca la había empezado, se guarda también la fecha de inicio).</summary>
        public void FinalizarTarea(int idTarea)
        {
            try
            {
                TareaOrdenTrabajo tarea = ObtenerTareaPropia(idTarea);
                if (tarea.EstadoTarea == Estados.TareaFinalizada)
                    throw new TareaEstadoInvalidoException("La tarea ya está finalizada.");

                if (!_tareaRepositorio.CambiarEstado(idTarea, tarea.EstadoTarea, Estados.TareaFinalizada, DateTime.Now))
                    throw new TareaEstadoInvalidoException("La tarea cambió de estado. Actualice la lista.");

                LoggerService.Info($"Finalizó la tarea {idTarea} (OT {tarea.IdOrdenTrabajo}, vehículo {tarea.Vehiculo.Dominio}).");
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

        // Apoyo

        private OrdenTrabajo ObtenerAbierta(int idOrdenTrabajo)
        {
            OrdenTrabajo orden = _repositorio.GetById(idOrdenTrabajo)
                ?? throw new OrdenTrabajoNoEncontradaException(idOrdenTrabajo);
            if (orden.EstadoOrden != Estados.OrdenAbierta)
                throw new OrdenTrabajoCerradaException($"La OT {idOrdenTrabajo} ya está {orden.EstadoOrden.ToLower()}: no se puede modificar.");
            return orden;
        }

        /// <summary>La tarea tiene que existir, ser del mecánico logueado (si es mecánico) y su orden tiene que seguir abierta.</summary>
        private TareaOrdenTrabajo ObtenerTareaPropia(int idTarea)
        {
            int? idMecanico = MecanicoFiltro(Funcion.RegistrarTareasRealizadas);

            TareaOrdenTrabajo tarea = _tareaRepositorio.GetById(idTarea)
                ?? throw new TareaNoEncontradaException(idTarea);

            if (idMecanico != null && tarea.IdMecanico != idMecanico)
                throw new TareaAjenaException();

            OrdenTrabajo orden = _repositorio.GetById(tarea.IdOrdenTrabajo)
                ?? throw new OrdenTrabajoNoEncontradaException(tarea.IdOrdenTrabajo);
            if (orden.EstadoOrden != Estados.OrdenAbierta)
                throw new OrdenTrabajoCerradaException("La orden de trabajo de esta tarea ya está cerrada.");

            return tarea;
        }

        /// <summary>
        /// Quien gestiona las órdenes ve todo (null = sin filtro).
        /// El mecánico necesita el permiso pedido y ve solo lo suyo (su IdMecanico).
        /// </summary>
        private static int? MecanicoFiltro(Funcion funcion)
        {
            if (Sesion.TienePermiso(Funcion.GestionarOrdenesTrabajo))
                return null;

            Sesion.ExigirPermiso(funcion);
            return Sesion.UsuarioActual.IdMecanico;
        }
    }
}
