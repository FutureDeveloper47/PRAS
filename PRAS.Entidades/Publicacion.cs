namespace PRAS.Entidades
{
    /// <summary>
    /// Aviso de venta de un vehículo.
    /// </summary>
    public class Publicacion
    {
        public int IdPublicacion { get; set; }
        public int IdVehiculo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public decimal PrecioPublicado { get; set; }
        public string Plataforma { get; set; }
        public string Descripcion { get; set; }
        public string EstadoPublicacion { get; set; }

        /// <summary>Auto publicado (para mostrar en pantalla).</summary>
        public Vehiculo Vehiculo { get; set; }
    }
}
