namespace PRAS.BLL.Excepciones
{
    /// <summary>El pedido ya no está pendiente (ya se entregó o cambió mientras tanto).</summary>
    public class PedidoYaProcesadoException : PedidoRepuestoBusinessException
    {
        public PedidoYaProcesadoException(string mensaje)
            : base(mensaje, "PEDIDO_YA_PROCESADO")
        {
        }
    }
}
