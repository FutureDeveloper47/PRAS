using PRAS.DAL.Factory;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;
using PRAS.Services.Facade;

namespace PRAS.BLL
{
    /// <summary>
    /// Pantalla A de EA - Inicio: arma el resumen que se ve al entrar al sistema
    /// y el detalle que se abre al tocar cada tarjeta (TALLER, INGRESOS, AGENCIA).
    /// Las reglas de "qué se cuenta" están acá (BLL), no en la pantalla ni en la base.
    /// </summary>
    public class InicioService
    {
        /// <summary>Cuántos renglones muestra la lista "Actividad reciente".</summary>
        private const int CantidadActividades = 10;

        private readonly IInicioRepository _repositorio = FactoryDataAccess.InicioRepository;

        /// <summary>
        /// El resumen tiene números del negocio, así que lo ven los que pueden ver reportes
        /// (Dueño y Administrador). El Mecánico no.
        /// </summary>
        public bool PuedeVerResumen()
        {
            return Sesion.TienePermiso(Funcion.VerReportes);
        }

        /// <summary>Día 1 del mes actual (se incluye en las consultas).</summary>
        private static DateTime InicioMes => new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        /// <summary>Día 1 del mes que viene (NO se incluye: así queda justo hasta fin del mes actual).</summary>
        private static DateTime InicioMesSiguiente => InicioMes.AddMonths(1);

        /// <summary>
        /// Día 1 del mes de hace 11 meses: desde acá hasta InicioMesSiguiente son 12 meses completos
        /// (contando el actual). Antes las tarjetas mostraban solo el mes actual y casi siempre
        /// quedaban con 0 o 1 auto; con un año se ve el movimiento real del negocio.
        /// </summary>
        public static DateTime InicioUltimoAnio => InicioMes.AddMonths(-11);

        public ResumenInicio ObtenerResumen()
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);

            return new ResumenInicio
            {
                // "En taller" es cuando el auto está en diagnóstico o en reparación.
                VehiculosEnTaller = _repositorio.ContarVehiculos(EstadoVehiculo.EnDiagnostico)
                                  + _repositorio.ContarVehiculos(EstadoVehiculo.EnReparacion),

                ChocadosIngresadosUltimoAnio = _repositorio.ContarCompras(InicioUltimoAnio, InicioMesSiguiente),
                VentasUltimoAnio = _repositorio.ContarVentas(InicioUltimoAnio, InicioMesSiguiente),
                OrdenesAbiertas = _repositorio.ContarOrdenesAbiertas(),
                PresupuestosPendientes = _repositorio.ContarPresupuestosPendientes(),
                AutosSinIngresar = _repositorio.ContarVehiculos(EstadoVehiculo.Comprado),
                OrdenesDemoradas = _repositorio.ContarOrdenesAbiertasDesde(DateTime.Today.AddDays(-DiasDemora)),
                Actividades = _repositorio.ListarActividadReciente(CantidadActividades)
            };
        }

        // Detalle de las tarjetas
        // Cada tarjeta usa el mismo criterio que su número, así si dice 3 hay 3 renglones.

        /// <summary>Una orden abierta hace más de estos días se muestra como demorada en el inicio del dueño.</summary>
        public const int DiasDemora = 15;

        // Inicio del dueño: panel MECÁNICOS

        /// <summary>
        /// El panel MECÁNICOS lo ve quien ve el resumen del negocio y además gestiona las órdenes de trabajo
        /// y los mecánicos (hoy, la familia Dueño). Se decide por permisos (Composite), no por el rol.
        /// </summary>
        public bool PuedeVerMecanicos()
        {
            return PuedeVerResumen() && Sesion.TienePermiso(Funcion.GestionarOrdenesTrabajo) && Sesion.TienePermiso(Funcion.GestionarMecanicos);
        }

        /// <summary>Un renglón por mecánico activo con su trabajo actual.</summary>
        public List<ResumenMecanico> ObtenerResumenPorMecanico()
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);
            Sesion.ExigirPermiso(Funcion.GestionarMecanicos);
            return _repositorio.ListarResumenPorMecanico(InicioMes, InicioMesSiguiente);
        }

        /// <summary>
        /// Trabajo de UN mecánico elegido por el dueño (solo consulta): sus órdenes abiertas, las tareas
        /// que terminó este mes o sus pedidos pendientes. Queda en la bitácora quién consultó a qué mecánico.
        /// </summary>
        public List<DetalleInicio> ObtenerDetalleDeMecanico(int idMecanico, string nombreMecanico, string que)
        {
            Sesion.ExigirPermiso(Funcion.GestionarOrdenesTrabajo);
            Sesion.ExigirPermiso(Funcion.GestionarMecanicos);

            List<DetalleInicio> filas;
            switch (que)
            {
                case "Tareas":
                    filas = _repositorio.ListarTareasRealizadasDeMecanico(idMecanico, InicioMes, InicioMesSiguiente);
                    break;
                case "Pedidos":
                    filas = _repositorio.ListarPedidosPendientesDeMecanico(idMecanico);
                    break;
                default:
                    filas = _repositorio.ListarOrdenesDeMecanico(idMecanico);
                    break;
            }

            LoggerService.Info($"Consultó el trabajo del mecánico {nombreMecanico}.");
            return filas;
        }

        // Inicio del mecánico

        /// <summary>
        /// El Inicio del mecánico se muestra a quien NO ve el resumen del negocio y tiene un mecánico vinculado.
        /// Cada tarjeta y cada botón se muestran según sus permisos (patentes), no por el rol.
        /// </summary>
        public bool EsInicioDeMecanico()
        {
            return !PuedeVerResumen() && Sesion.UsuarioActual?.IdMecanico != null;
        }

        /// <summary>Los números de las tarjetas del mecánico logueado.</summary>
        public ResumenMecanico ObtenerResumenMecanico()
        {
            int idMecanico = MecanicoLogueado();
            return _repositorio.ObtenerResumenMecanico(idMecanico, InicioMes, InicioMesSiguiente);
        }

        public List<DetalleInicio> ObtenerDetalleAutosEnDiagnostico()
        {
            Sesion.ExigirPermiso(Funcion.ConsultarDiagnostico);
            return _repositorio.ListarAutosEnDiagnostico();
        }

        public List<DetalleInicio> ObtenerDetalleMisOrdenes()
        {
            Sesion.ExigirPermiso(Funcion.ConsultarOrdenesTrabajo);
            return _repositorio.ListarOrdenesDeMecanico(MecanicoLogueado());
        }

        public List<DetalleInicio> ObtenerDetalleMisPedidos()
        {
            Sesion.ExigirPermiso(Funcion.ConsultarPedidosRepuestos);
            return _repositorio.ListarPedidosPendientesDeMecanico(MecanicoLogueado());
        }

        public List<DetalleInicio> ObtenerDetalleMisTareas()
        {
            Sesion.ExigirPermiso(Funcion.RegistrarTareasRealizadas);
            return _repositorio.ListarTareasRealizadasDeMecanico(MecanicoLogueado(), InicioMes, InicioMesSiguiente);
        }

        /// <summary>Mecánico vinculado al usuario logueado. Si no tiene, no puede ver "lo suyo".</summary>
        private static int MecanicoLogueado()
        {
            int? idMecanico = Sesion.UsuarioActual?.IdMecanico;
            if (idMecanico == null)
                Sesion.ExigirPermiso(Funcion.GestionarOrdenesTrabajo);   // sin mecánico vinculado: PermisoDenegadoException
            return idMecanico ?? 0;
        }

        /// <summary>Tarjeta TALLER: autos en diagnóstico o en reparación.</summary>
        public List<DetalleInicio> ObtenerDetalleTaller()
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);
            return _repositorio.ListarVehiculosEnTaller();
        }

        /// <summary>Tarjeta INGRESOS: chocados comprados en los últimos 12 meses.</summary>
        public List<DetalleInicio> ObtenerDetalleIngresos()
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);
            return _repositorio.ListarComprasDetalle(InicioUltimoAnio, InicioMesSiguiente);
        }

        /// <summary>
        /// Tarjeta AGENCIA: ventas de los últimos 12 meses y, debajo, los autos que la agencia
        /// tiene ahora para vender (entregados o publicados).
        /// </summary>
        public List<DetalleInicio> ObtenerDetalleAgencia()
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);
            return _repositorio.ListarAgenciaDetalle(InicioUltimoAnio, InicioMesSiguiente);
        }
    }
}
