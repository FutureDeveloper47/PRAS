namespace PRAS.BLL.Excepciones
{
    /// <summary>Un mecánico quiso registrar una tarea de otro mecánico.</summary>
    public class TareaAjenaException : TallerBusinessException
    {
        public TareaAjenaException()
            : base("Solo puede registrar sus propias tareas.", "TAREA_AJENA")
        {
        }
    }
}
