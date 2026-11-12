using PRAS.DAL.Factory;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

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

        /// <summary>Día 1 del mes que viene (NO se incluye: así queda justo el mes actual).</summary>
        private static DateTime InicioMesSiguiente => InicioMes.AddMonths(1);

        public ResumenInicio ObtenerResumen()
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);

            return new ResumenInicio
            {
                // "En taller" es cuando el auto está en diagnóstico o en reparación.
                VehiculosEnTaller = _repositorio.ContarVehiculos(EstadoVehiculo.EnDiagnostico)
                                  + _repositorio.ContarVehiculos(EstadoVehiculo.EnReparacion),

                ChocadosIngresadosMes = _repositorio.ContarCompras(InicioMes, InicioMesSiguiente),
                VentasMes = _repositorio.ContarVentas(InicioMes, InicioMesSiguiente),
                Actividades = _repositorio.ListarActividadReciente(CantidadActividades)
            };
        }

        // Detalle de las tarjetas
        // Cada tarjeta usa el mismo criterio que su número, así si dice 3 hay 3 renglones.

        /// <summary>Tarjeta TALLER: autos en diagnóstico o en reparación.</summary>
        public List<DetalleInicio> ObtenerDetalleTaller()
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);
            return _repositorio.ListarVehiculosEnTaller();
        }

        /// <summary>Tarjeta INGRESOS: chocados comprados en el mes actual.</summary>
        public List<DetalleInicio> ObtenerDetalleIngresos()
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);
            return _repositorio.ListarComprasDetalle(InicioMes, InicioMesSiguiente);
        }

        /// <summary>Tarjeta AGENCIA: ventas del mes actual y autos que la agencia tiene para vender.</summary>
        public List<DetalleInicio> ObtenerDetalleAgencia()
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);
            return _repositorio.ListarAgenciaDetalle(InicioMes, InicioMesSiguiente);
        }
    }
}
