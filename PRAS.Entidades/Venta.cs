namespace PRAS.Entidades
{
    /// <summary>
    /// Venta de un vehículo en consignación a un comprador.
    /// </summary>
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdConsignacion { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal MontoComision { get; set; }
        public string FormaPago { get; set; }
        public Consignacion Consignacion { get; set; }
        public Cliente Cliente { get; set; }
    }
}
