namespace PRAS.BLL.Excepciones
{
    /// <summary>La tarea no está en el estado que la operación necesita (o cambió mientras tanto).</summary>
    public class TareaEstadoInvalidoException : TallerBusinessException
    {
        public TareaEstadoInvalidoException(string mensaje)
            : base(mensaje, "TAREA_ESTADO_INVALIDO")
        {
        }
    }
}
