namespace PRAS.BLL.Excepciones
{
    /// <summary>La consignación ya no existe.</summary>
    public class ConsignacionNoEncontradaException : ComercialBusinessException
    {
        public int IdConsignacion { get; }

        public ConsignacionNoEncontradaException(int idConsignacion)
            : base("La consignación ya no existe.", "CONSIGNACION_NO_ENCONTRADA")
        {
            IdConsignacion = idConsignacion;
        }
    }
}
