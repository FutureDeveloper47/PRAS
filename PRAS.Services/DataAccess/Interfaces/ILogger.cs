namespace PRAS.Services.DataAccess.Interfaces
{
    /// <summary>
    /// Contrato de cualquier "logger".
    /// Hoy lo implementa FileLogger (graba en un archivo); mañana podría haber un SqlLogger
    /// que grabe en una tabla, y el resto del sistema no se entera del cambio.
    /// </summary>
    internal interface ILogger
    {
        void Debug(string message);
        void Debug(string message, Exception exception);
        void Info(string message);
        void Info(string message, Exception exception);
        void Warn(string message);
        void Warn(string message, Exception exception);
        void Error(string message);
        void Error(string message, Exception exception);
        void Fatal(string message);
        void Fatal(string message, Exception exception);
    }
}
