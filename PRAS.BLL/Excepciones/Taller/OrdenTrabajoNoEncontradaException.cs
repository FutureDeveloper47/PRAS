namespace PRAS.BLL.Excepciones
{
    /// <summary>La orden de trabajo ya no existe.</summary>
    public class OrdenTrabajoNoEncontradaException : TallerBusinessException
    {
        public int IdOrdenTrabajo { get; }

        public OrdenTrabajoNoEncontradaException(int idOrdenTrabajo)
            : base("La orden de trabajo ya no existe.", "ORDEN_NO_ENCONTRADA")
        {
            IdOrdenTrabajo = idOrdenTrabajo;
        }
    }
}
