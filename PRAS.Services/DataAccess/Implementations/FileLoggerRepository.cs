using System.Globalization;
using System.Text;
using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DomainModel;

namespace PRAS.Services.DataAccess.Implementations
{
    /// <summary>
    /// Escribe (y lee) la bitácora en un archivo de texto. Tiene el método ReadLog para poder MOSTRAR la bitácora en un formulario.
    /// Hoy la bitácora se guarda en la base (SqlLoggerRepository); este archivo se usa
    /// si en el App.config se elige LoggerType = "File", o de respaldo si la base no responde.
    /// </summary>
    internal class FileLoggerRepository : ILoggerRepository
    {
        private string _fileName;
        private LogLevel _minimalLogLevel;

        // Este objeto es para el lock: así dos hilos no escriben el archivo al mismo tiempo.
        private static readonly object _lock = new object();

        public FileLoggerRepository(string fileName, string minimalLogLevel = "Info")
        {
            _fileName = fileName;
            _minimalLogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), minimalLogLevel);
            EnsureDirectoryExists();
        }

        public void WriteLog(LogEntry entry)
        {
            try
            {
                if (entry.LogLevel < _minimalLogLevel)
                {
                    return;
                }

                lock (_lock)
                {
                    File.AppendAllText(_fileName, entry.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al escribir en el archivo de log: " + ex.Message);
                throw;
            }
        }

        public void WriteLog(string message, LogLevel logLevel, string usuario, Exception exception = null)
        {
            var entry = new LogEntry
            {
                TimeStamp = DateTime.Now,
                LogLevel = logLevel,
                Usuario = usuario,
                Message = message,
                Exception = exception
            };

            WriteLog(entry);
        }

        /// <summary>
        /// Lee el archivo completo y lo convierte en una lista de LogEntry.
        /// Cada registro termina con una línea de guiones (LogEntry.Separador).
        /// </summary>
        /// <summary>Registros entre dos fechas, del más nuevo al más viejo.</summary>
        public List<LogEntry> ReadLog(DateTime desde, DateTime hasta)
        {
            List<LogEntry> todos = ReadLog();
            todos.Reverse();   // el archivo va del más viejo al más nuevo

            return todos
                .Where(r => r.TimeStamp.Date >= desde.Date && r.TimeStamp.Date <= hasta.Date)
                .ToList();
        }

        private List<LogEntry> ReadLog()
        {
            var registros = new List<LogEntry>();

            if (!File.Exists(_fileName))
            {
                return registros;   // todavía no se grabó nada
            }

            string[] lineas;
            lock (_lock)
            {
                lineas = File.ReadAllLines(_fileName);
            }

            LogEntry actual = null;
            string campoActual = null;   // para los mensajes de más de una línea

            foreach (string linea in lineas)
            {
                if (linea == LogEntry.Separador)
                {
                    if (actual != null)
                    {
                        registros.Add(actual);
                    }
                    actual = null;
                    continue;
                }

                // Primera línea de cada registro: [2026-10-14 11:20:00] [Info]
                if (actual == null)
                {
                    actual = LeerEncabezado(linea);
                    campoActual = null;
                    continue;
                }

                if (linea.StartsWith("User: "))
                {
                    actual.Usuario = linea.Substring("User: ".Length);
                    campoActual = null;
                }
                else if (linea.StartsWith("Method: "))
                {
                    actual.ClassName = linea.Substring("Method: ".Length);
                    campoActual = null;
                }
                else if (linea.StartsWith("Message: "))
                {
                    actual.Message = linea.Substring("Message: ".Length);
                    campoActual = "Message";
                }
                else if (linea.StartsWith("Exception Type: "))
                {
                    actual.ExceptionType = linea.Substring("Exception Type: ".Length);
                    campoActual = null;
                }
                else if (linea.StartsWith("Exception Message: "))
                {
                    actual.ExceptionMessage = linea.Substring("Exception Message: ".Length);
                    campoActual = "ExceptionMessage";
                }
                else if (linea.StartsWith("StackTrace: ") || linea.StartsWith("InnerException") || linea.StartsWith("Arguments:"))
                {
                    campoActual = null;   // el detalle técnico no se muestra en la grilla
                }
                else if (campoActual == "Message" && linea.Trim() != "")
                {
                    actual.Message += " " + linea.Trim();
                }
                else if (campoActual == "ExceptionMessage" && linea.Trim() != "")
                {
                    actual.ExceptionMessage += " " + linea.Trim();
                }
            }

            return registros;
        }

        /// <summary>Interpreta "[2026-10-14 11:20:00] [Info]". Si la línea no tiene ese formato, devuelve null.</summary>
        private static LogEntry LeerEncabezado(string linea)
        {
            // Posiciones:  [yyyy-MM-dd HH:mm:ss] [Nivel]
            // 0         1         2
            // 0123456789012345678901234
            if (linea.Length < 24 || linea[0] != '[' || linea[20] != ']')
            {
                return null;
            }

            if (!DateTime.TryParseExact(linea.Substring(1, 19), "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
            {
                return null;
            }

            string nivelTexto = linea.Substring(22).Trim('[', ']', ' ');
            if (!Enum.TryParse(nivelTexto, out LogLevel nivel))
            {
                return null;
            }

            return new LogEntry { TimeStamp = fecha, LogLevel = nivel };
        }

        private void EnsureDirectoryExists()
        {
            var directory = Path.GetDirectoryName(_fileName);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
