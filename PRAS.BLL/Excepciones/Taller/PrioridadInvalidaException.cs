namespace PRAS.BLL.Excepciones
{
    /// <summary>La prioridad no es Alta, Media ni Baja.</summary>
    public class PrioridadInvalidaException : TallerBusinessException
    {
        public PrioridadInvalidaException()
            : base("Elija la prioridad: Alta, Media o Baja.", "PRIORIDAD_INVALIDA")
        {
        }
    }
}
