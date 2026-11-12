namespace PRAS.BLL.Excepciones
{
    /// <summary>Se cargó una fecha posterior a hoy en una operación que ya ocurrió.</summary>
    public class FechaFuturaException : DatoInvalidoException
    {
        public string Que { get; }

        public FechaFuturaException(string que)
            : base($"La fecha de {que} no puede ser posterior a hoy.", "FECHA_FUTURA")
        {
            Que = que;
        }
    }
}
