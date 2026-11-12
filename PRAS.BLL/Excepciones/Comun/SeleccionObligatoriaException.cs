namespace PRAS.BLL.Excepciones
{
    /// <summary>No se eligió un dato obligatorio de una lista (un auto, un cliente, una agencia...).</summary>
    public class SeleccionObligatoriaException : DatoInvalidoException
    {
        public string Que { get; }

        public SeleccionObligatoriaException(string que)
            : base($"Seleccione {que}.", "SELECCION_OBLIGATORIA")
        {
            Que = que;
        }
    }
}
