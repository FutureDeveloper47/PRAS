namespace PRAS.Entidades
{
    /// <summary>
    /// Ingreso de un vehículo propio al taller.
    /// </summary>
    public class RecepcionVehiculo
    {
        public int IdRecepcion { get; set; }
        public int IdVehiculo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string MotivoIngreso { get; set; }
        public string EstadoRecepcion { get; set; }
        public string Observaciones { get; set; }
        public Vehiculo Vehiculo { get; set; }
    }
}
