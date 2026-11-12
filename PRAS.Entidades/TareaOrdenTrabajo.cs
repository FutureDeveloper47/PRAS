namespace PRAS.Entidades
{
    /// <summary>
    /// Tarea de una orden de trabajo asignada a un mecánico.
    /// </summary>
    public class TareaOrdenTrabajo
    {
        public int IdTarea { get; set; }
        public int IdOrdenTrabajo { get; set; }
        public int IdMecanico { get; set; }
        public string DescripcionTarea { get; set; }
        public decimal CostoEstimado { get; set; }
        public string EstadoTarea { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public Mecanico Mecanico { get; set; }

        // Datos para mostrar en pantalla (vienen de la orden)
        public Vehiculo Vehiculo { get; set; }
        public string PrioridadOrden { get; set; }
    }
}
