namespace PRAS.Entidades
{
    /// <summary>
    /// Compra de un vehículo chocado a un particular. Es por donde el auto entra al sistema.
    /// </summary>
    public class Compra
    {
        public int IdCompra { get; set; }
        public int IdVehiculo { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal PrecioCompra { get; set; }
        public string FormaPago { get; set; }
        public string Observaciones { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public Cliente Cliente { get; set; }
    }
}
