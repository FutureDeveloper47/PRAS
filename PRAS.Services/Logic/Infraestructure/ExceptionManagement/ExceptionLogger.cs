using PRAS.Services.DomainModel;
using PRAS.Services.Facade;

namespace PRAS.Services.Logic.Infrastructure.ExceptionManagement
{
    /// <summary>
    /// Registra excepciones.
    /// Van a la MISMA bitácora del sistema (tabla Bitacora de PRAS_Seguridad),
    /// así se ven en la pantalla Seguridad > Bitácora junto con todo lo demás.
    /// </summary>
    internal static class ExceptionLogger
    {
        public static void Log(ExceptionContext context)
        {
            var entry = new LogEntry
            {
                TimeStamp = context.Timestamp,
                LogLevel = context.LogLevel,
                Message = context.Exception.Message,
                Exception = context.Exception,
                MethodName = context.MethodName,
                ClassName = context.ClassName,
                Arguments = context.Arguments
            };

            LoggerService.Escribir(entry);
        }

        public static void Log(string message, LogLevel level)
        {
            LoggerService.Escribir(new LogEntry
            {
                TimeStamp = DateTime.Now,
                LogLevel = level,
                Message = message
            });
        }
    }
}
