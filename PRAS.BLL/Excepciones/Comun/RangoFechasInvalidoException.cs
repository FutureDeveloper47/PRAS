namespace PRAS.BLL.Excepciones
{
    /// <summary>En una consulta, la fecha "desde" es posterior a la fecha "hasta".</summary>
    public class RangoFechasInvalidoException : DatoInvalidoException
    {
        public DateTime Desde { get; }
        public DateTime Hasta { get; }

        public RangoFechasInvalidoException(DateTime desde, DateTime hasta)
            : base("La fecha \"desde\" no puede ser posterior a la fecha \"hasta\".", "RANGO_FECHAS_INVALIDO")
        {
            Desde = desde;
            Hasta = hasta;
        }
    }
}
