namespace PRAS.Entidades
{
    /// <summary>
    /// Entrega de un vehículo reparado a una agencia para su venta.
    /// </summary>
    public class Consignacion
    {
        public int IdConsignacion { get; set; }
        public int IdVehiculo { get; set; }
        public int IdAgencia { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime? FechaRetiro { get; set; }
        public decimal PrecioMinimoVenta { get; set; }
        public decimal PorcentajeComision { get; set; }
        public string EstadoConsignacion { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public Agencia Agencia { get; set; }
    }
}
