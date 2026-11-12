using PRAS.Services.DomainModel;
using PRAS.Services.Logic;

namespace PRAS.Services.Facade
{
    /// <summary>
    /// FACHADA de la bitácora: es lo único que ven las demás capas.
    /// Ejemplo de uso desde la BLL:  LoggerService.Info("Alta de cliente DNI 30111222");
    /// </summary>
    public static class LoggerService
    {
        // FileLogger tendría que llamarse LoggerLogic y llamar a la factory desde ahí
        private static readonly FileLogger fileLogger = new FileLogger();

        /// <summary>
        /// Usuario que queda registrado en cada línea de la bitácora.
        /// La BLL (Sesion) lo completa al iniciar sesión y lo borra al cerrarla.
        /// </summary>
        public static string Usuario
        {
            get { return fileLogger.Usuario; }
            set { fileLogger.Usuario = value; }
        }

        public static void Debug(string message) { fileLogger.Debug(message); }
        public static void Debug(string message, Exception exception) { fileLogger.Debug(message, exception); }

        public static void Info(string message) { fileLogger.Info(message); }
        public static void Info(string message, Exception exception) { fileLogger.Info(message, exception); }

        public static void Warn(string message) { fileLogger.Warn(message); }
        public static void Warn(string message, Exception exception) { fileLogger.Warn(message, exception); }

        public static void Error(string message) { fileLogger.Error(message); }
        public static void Error(string message, Exception exception) { fileLogger.Error(message, exception); }

        public static void Fatal(string message) { fileLogger.Fatal(message); }
        public static void Fatal(string message, Exception exception) { fileLogger.Fatal(message, exception); }

        /// <summary>Graba un registro completo (lo usa el ExceptionLogger del manejo de excepciones).</summary>
        internal static void Escribir(LogEntry entry) { fileLogger.Escribir(entry); }

        /// <summary>Lee la bitácora con filtros (la usa la pantalla "Bitácora").</summary>
        public static List<LogEntry> Leer(DateTime desde, DateTime hasta, LogLevel? nivel, string usuario, string texto)
        {
            return fileLogger.Leer(desde, hasta, nivel, usuario, texto);
        }
    }
}
