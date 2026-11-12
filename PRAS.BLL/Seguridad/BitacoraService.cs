using PRAS.Services.DomainModel;
using PRAS.Services.Facade;

namespace PRAS.BLL
{
    /// <summary>
    /// Caso de uso Seguridad - Consultar Bitácora.
    /// La bitácora la graba el servicio LoggerService (proyecto PRAS.Services);
    /// esta clase solo controla el permiso y valida los filtros antes de leerla.
    /// </summary>
    public class BitacoraService
    {
        public List<LogEntry> Listar(DateTime desde, DateTime hasta, LogLevel? nivel, string usuario, string texto)
        {
            Sesion.ExigirPermiso(Funcion.VerBitacora);

            Validar.RangoFechas(desde, hasta);

            return LoggerService.Leer(desde, hasta, nivel, usuario, texto);
        }
    }
}
