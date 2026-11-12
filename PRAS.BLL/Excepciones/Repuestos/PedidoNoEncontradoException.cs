namespace PRAS.BLL.Excepciones
{
    /// <summary>El pedido ya no existe.</summary>
    public class PedidoNoEncontradoException : PedidoRepuestoBusinessException
    {
        public int IdPedido { get; }

        public PedidoNoEncontradoException(int idPedido)
            : base("El pedido ya no existe.", "PEDIDO_NO_ENCONTRADO")
        {
            IdPedido = idPedido;
        }
    }
}
