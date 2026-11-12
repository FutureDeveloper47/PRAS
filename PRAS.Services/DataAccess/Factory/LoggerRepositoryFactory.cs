using PRAS.Services.DataAccess.Implementations;
using PRAS.Services.DataAccess.Interfaces;

namespace PRAS.Services.DataAccess.Factory
{
    /// <summary>
    /// Factory de la bitácora (patrón Factory):
    /// según el App.config ("LoggerType") devuelve dónde se guarda la bitácora.
    ///   "Sql"  -> tabla Bitacora de PRAS_Seguridad
    ///   "File" -> archivo de texto
    /// Así se cambia el destino sin tocar código, solo el App.config.
    /// </summary>
    internal static class LoggerRepositoryFactory
    {
        public static ILoggerRepository Crear(string tipo, string fileName, string minimalLogLevel)
        {
            switch ((tipo ?? "").Trim().ToLower())
            {
                case "file":
                    return new FileLoggerRepository(fileName, minimalLogLevel);

                case "sql":
                default:
                    return new SqlLoggerRepository();
            }
        }
    }
}
