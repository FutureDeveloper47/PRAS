namespace PRAS.BLL.Excepciones
{
    /// <summary>El costo estimado de una tarea no puede ser negativo.</summary>
    public class CostoNegativoException : TallerBusinessException
    {
        public CostoNegativoException()
            : base("El costo estimado no puede ser negativo.", "COSTO_NEGATIVO")
        {
        }
    }
}
