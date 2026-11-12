using System.Configuration;
using PRAS.Services.DataAccess.Factory;
using PRAS.Services.DataAccess.Implementations;
using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DomainModel;

namespace PRAS.Services.Logic
{
    /// <summary>
    /// Lógica de la bitácora.
    /// El repositorio se elige con una factory (LoggerRepositoryFactory):
    /// la factory decide si la bitácora va a la base o a un archivo.
    ///
    /// Configuración del App.config:
    ///   LoggerType      -> "Sql" (tabla Bitacora de PRAS_Seguridad) o "File" (archivo)
    ///   FileName        -> archivo de texto (ej: Logs\bitacora.log)
    ///   MinimalLogLevel -> desde qué nivel se graba (ej: Info no graba los Debug)
    ///
    /// Si la base no responde, el registro se graba igual en el archivo (respaldo),
    /// así nunca se pierde lo que pasó (ej: "no se pudo conectar a la base").
    /// </summary>
    internal class FileLogger : ILogger
    {
        private readonly ILoggerRepository loggerRepository;   // el que eligió la factory
        private readonly FileLoggerRepository respaldoArchivo;   // por si falla la base
        private readonly LogLevel minimalLogLevel;

        /// <summary>Usuario logueado. Lo completa la BLL al iniciar/cerrar sesión.</summary>
        public string Usuario { get; set; }

        public FileLogger()
        {
            string _fileName = ConfigurationManager.AppSettings["FileName"] ?? @"Logs\bitacora.log";
            string minimalLogLevelString = ConfigurationManager.AppSettings["MinimalLogLevel"] ?? "Info";
            string loggerType = ConfigurationManager.AppSettings["LoggerType"] ?? "Sql";
            minimalLogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), minimalLogLevelString);

            // Si la ruta es relativa, la tomamos desde la carpeta del .exe
            if (!Path.IsPathRooted(_fileName))
            {
                _fileName = Path.Combine(AppContext.BaseDirectory, _fileName);
            }

            loggerRepository = LoggerRepositoryFactory.Crear(loggerType, _fileName, minimalLogLevel.ToString());
            respaldoArchivo = new FileLoggerRepository(_fileName, minimalLogLevel.ToString());
        }

        public void Debug(string message) { Escribir(message, LogLevel.Debug, null); }
        public void Debug(string message, Exception exception) { Escribir(message, LogLevel.Debug, exception); }

        public void Info(string message) { Escribir(message, LogLevel.Info, null); }
        public void Info(string message, Exception exception) { Escribir(message, LogLevel.Info, exception); }

        public void Warn(string message) { Escribir(message, LogLevel.Warn, null); }
        public void Warn(string message, Exception exception) { Escribir(message, LogLevel.Warn, exception); }

        public void Error(string message) { Escribir(message, LogLevel.Error, null); }
        public void Error(string message, Exception exception) { Escribir(message, LogLevel.Error, exception); }

        public void Fatal(string message) { Escribir(message, LogLevel.Fatal, null); }
        public void Fatal(string message, Exception exception) { Escribir(message, LogLevel.Fatal, exception); }

        private void Escribir(string message, LogLevel logLevel, Exception exception)
        {
            if (logLevel < minimalLogLevel)
            {
                return;
            }

            Escribir(new LogEntry
            {
                TimeStamp = DateTime.Now,
                LogLevel = logLevel,
                Message = message,
                Exception = exception
            });
        }

        /// <summary>
        /// Graba un registro ya armado (lo usa el ExceptionLogger, que además completa
        /// la clase y el método donde ocurrió el error).
        /// </summary>
        public void Escribir(LogEntry entry)
        {
            if (entry.LogLevel < minimalLogLevel)
            {
                return;
            }

            entry.Usuario ??= Usuario;

            try
            {
                loggerRepository.WriteLog(entry);
            }
            catch (Exception ex)
            {
                // La base no respondió, lo guardamos en el archivo para no perderlo.
                try
                {
                    respaldoArchivo.WriteLog(entry);
                    respaldoArchivo.WriteLog(new LogEntry
                    {
                        TimeStamp = DateTime.Now,
                        LogLevel = LogLevel.Error,
                        Usuario = Usuario,
                        Message = "No se pudo grabar la bitácora en la base; se guardó en este archivo.",
                        Exception = ex
                    });
                }
                catch
                {
                    // Si tampoco podemos escribir el archivo, seguimos igual.
                }
            }
        }

        /// <summary>
        /// Devuelve los registros de la bitácora que cumplen los filtros, del más nuevo al más viejo.
        /// nivel = null -> todos los niveles. usuario/texto vacíos -> sin filtrar por eso.
        /// </summary>
        public List<LogEntry> Leer(DateTime desde, DateTime hasta, LogLevel? nivel, string usuario, string texto)
        {
            usuario = (usuario ?? "").Trim();
            texto = (texto ?? "").Trim();

            return loggerRepository.ReadLog(desde, hasta)
                .Where(r => nivel == null || r.LogLevel == nivel.Value)
                .Where(r => usuario == "" || (r.Usuario ?? "").Contains(usuario, StringComparison.OrdinalIgnoreCase))
                .Where(r => texto == "" || (r.Message ?? "").Contains(texto, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
