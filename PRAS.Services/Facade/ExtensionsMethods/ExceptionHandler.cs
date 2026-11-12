namespace PRAS.Services.Facade.ExtensionsMethods
{
    /// <summary>
    /// Método de extensión: en cualquier catch se escribe
    ///     catch (Exception ex) { ex.Handle(); }
    /// y el ExceptionManager aplica la política que corresponde según el tipo de excepción.
    /// </summary>
    public static class ExceptionHandler
    {
        public static void Handle(this Exception ex)
        {
            ExceptionManager.HandleException(ex);
        }
    }
}
