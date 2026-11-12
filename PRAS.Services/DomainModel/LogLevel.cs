namespace PRAS.Services.DomainModel
{
    /// <summary>
    /// Nivel de gravedad de cada registro de la bitácora.
    /// El orden importa: si el nivel mínimo configurado es Info, los Debug no se graban.
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}
