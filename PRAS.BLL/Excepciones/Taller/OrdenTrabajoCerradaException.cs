namespace PRAS.BLL.Excepciones
{
    /// <summary>La orden de trabajo ya está cerrada y no se puede modificar.</summary>
    public class OrdenTrabajoCerradaException : TallerBusinessException
    {
        public OrdenTrabajoCerradaException(string mensaje)
            : base(mensaje, "ORDEN_CERRADA")
        {
        }
    }
}
