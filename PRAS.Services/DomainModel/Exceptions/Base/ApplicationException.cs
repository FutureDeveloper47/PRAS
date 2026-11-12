namespace PRAS.Services.Exceptions.Base
{
    /// <summary>
    /// Excepción base de TODO el sistema.
    /// Cada excepción propia lleva un código (ej: "DB_ERROR") y la fecha y hora en que ocurrió.
    /// De esta clase heredan las de reglas de negocio, las de acceso a datos y las de pantalla.
    /// </summary>
    public abstract class ApplicationException : Exception
    {
        public string Code { get; protected set; }
        public DateTime Timestamp { get; }

        protected ApplicationException(string message, string code = "UNKNOWN_ERROR")
            : base(message)
        {
            Code = code;
            Timestamp = DateTime.Now;
        }

        protected ApplicationException(string message, Exception innerException, string code = "UNKNOWN_ERROR")
            : base(message, innerException)
        {
            Code = code;
            Timestamp = DateTime.Now;
        }
    }
}
