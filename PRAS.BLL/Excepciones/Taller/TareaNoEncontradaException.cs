namespace PRAS.BLL.Excepciones
{
    /// <summary>La tarea ya no existe.</summary>
    public class TareaNoEncontradaException : TallerBusinessException
    {
        public int IdTarea { get; }

        public TareaNoEncontradaException(int idTarea)
            : base("La tarea ya no existe.", "TAREA_NO_ENCONTRADA")
        {
            IdTarea = idTarea;
        }
    }
}
