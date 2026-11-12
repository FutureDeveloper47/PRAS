namespace PRAS.Entidades
{
    /// <summary>
    /// Pedido de repuestos a un proveedor para una orden de trabajo.
    /// </summary>
    public class PedidoRepuesto
    {
        public int IdPedido { get; set; }
        public int IdProveedor { get; set; }
        public int IdOrdenTrabajo { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string EstadoPedido { get; set; }
        public decimal MontoTotal { get; set; }
        public Proveedor Proveedor { get; set; }
        public List<DetallePedidoRepuesto> Detalles { get; set; } = new List<DetallePedidoRepuesto>();

        /// <summary>Patente del auto de la orden de trabajo (para mostrar en pantalla).</summary>
        public string DominioVehiculo { get; set; }
    }
}
