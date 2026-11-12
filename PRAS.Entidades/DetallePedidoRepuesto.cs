namespace PRAS.Entidades
{
    /// <summary>
    /// Renglón de un pedido de repuestos.
    /// </summary>
    public class DetallePedidoRepuesto
    {
        public int IdDetallePedido { get; set; }
        public int IdPedido { get; set; }
        public string DescripcionRepuesto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
