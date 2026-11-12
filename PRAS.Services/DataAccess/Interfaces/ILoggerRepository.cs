using PRAS.Services.DomainModel;

namespace PRAS.Services.DataAccess.Interfaces
{
    /// <summary>
    /// Contrato de "dónde se guarda la bitácora". Lo cumplen:
    /// - SqlLoggerRepository  -> tabla Bitacora de la base PRAS_Seguridad.
    /// - FileLoggerRepository -> archivo de texto.
    /// La lógica (FileLogger) no sabe cuál de los dos está usando: lo elige la factory.
    /// </summary>
    internal interface ILoggerRepository
    {
        void WriteLog(LogEntry entry);

        /// <summary>Registros entre dos fechas, del más nuevo al más viejo.</summary>
        List<LogEntry> ReadLog(DateTime desde, DateTime hasta);
    }
}
