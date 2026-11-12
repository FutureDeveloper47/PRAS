namespace PRAS.Services.Exceptions.Base
{
    /// <summary>
    /// Error ocurrido en una pantalla.
    /// Ej: no se pudo abrir el archivo de una evidencia o exportar un reporte.
    /// </summary>
    public class UIException : ApplicationException
    {
        public UIException()
            : base("Ocurrió un error inesperado en la pantalla.", "UI_ERROR")
        {
        }

        public UIException(string message, Exception innerException = null)
            : base(message, innerException, "UI_ERROR")
        {
        }
    }
}
