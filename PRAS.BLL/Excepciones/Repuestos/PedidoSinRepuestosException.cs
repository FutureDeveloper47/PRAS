namespace PRAS.BLL.Excepciones
{
    /// <summary>Se quiso guardar un pedido sin ningún repuesto.</summary>
    public class PedidoSinRepuestosException : PedidoRepuestoBusinessException
    {
        public PedidoSinRepuestosException()
            : base("Agregue al menos un repuesto al pedido.", "PEDIDO_SIN_REPUESTOS")
        {
        }
    }
}
