namespace PRAS.Entidades
{
    /// <summary>
    /// Presupuesto interno de reparación: lo aprueba el Dueño o el Administrador.
    /// </summary>
    public class Presupuesto
    {
        public int IdPresupuesto { get; set; }
        public int IdVehiculo { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string EstadoPresupuesto { get; set; }
        public decimal MontoTotal { get; set; }
        public string Observaciones { get; set; }
        public List<DetallePresupuesto> Detalles { get; set; } = new List<DetallePresupuesto>();
        public Vehiculo Vehiculo { get; set; }

        /// <summary>Texto para el combo "SELECCIONAR AUTO" de la Carga de costos.</summary>
        public string DescripcionVehiculo => Vehiculo == null ? "" : Vehiculo.Descripcion;
    }
}
