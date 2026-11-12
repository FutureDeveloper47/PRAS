namespace PRAS.Services.Exceptions.DataAccess
{
    /// <summary>
    /// Otro usuario cambió el registro mientras se hacía la operación.
    /// Ej: dos personas intentan vender el mismo auto al mismo tiempo.
    /// </summary>
    public class ConcurrencyException : DataAccessException
    {
        public ConcurrencyException(string message = "El registro fue modificado por otro usuario. Recargue los datos e intente nuevamente.")
            : base(message, code: "CONCURRENCY_ERROR")
        {
        }
    }
}
