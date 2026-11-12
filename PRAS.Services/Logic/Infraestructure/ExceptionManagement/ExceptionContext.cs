using PRAS.Services.DomainModel;

namespace PRAS.Services.Logic.Infrastructure.ExceptionManagement
{
    /// <summary>
    /// Datos de una excepción para registrarla:
    /// qué pasó, en qué clase y método, con qué argumentos y con qué nivel de gravedad.
    /// </summary>
    internal class ExceptionContext
    {
        public Exception Exception { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public object[] Arguments { get; set; }
        public DateTime Timestamp { get; set; }
        public LogLevel LogLevel { get; set; }

        public ExceptionContext()
        {
            Timestamp = DateTime.Now;
            Arguments = Array.Empty<object>();
        }
    }
}
