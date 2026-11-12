namespace PRAS.Entidades
{
    /// <summary>
    /// Orden de trabajo generada a partir de una recepción y un presupuesto aprobado.
    /// </summary>
    public class OrdenTrabajo
    {
        public int IdOrdenTrabajo { get; set; }
        public int IdRecepcion { get; set; }
        public int IdPresupuesto { get; set; }
        public int IdVehiculo { get; set; }
        public int IdTaller { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string EstadoOrden { get; set; }
        public string Prioridad { get; set; }
        public string Observaciones { get; set; }
        public List<TareaOrdenTrabajo> Tareas { get; set; } = new List<TareaOrdenTrabajo>();

        // Datos para mostrar en pantalla (vienen de otras tablas)
        public Vehiculo Vehiculo { get; set; }
        /// <summary>Total del presupuesto de la orden (suma de repuestos y mano de obra).</summary>
        public decimal MontoPresupuesto { get; set; }
        /// <summary>Nombres de los mecánicos que tienen tareas en la orden, separados por coma.</summary>
        public string Mecanicos { get; set; }

        /// <summary>Texto para los combos. Ej: "OT 12 - AE530NS - Fiat Cronos (2023)".</summary>
        public string Descripcion => Vehiculo == null ? $"OT {IdOrdenTrabajo}" : $"OT {IdOrdenTrabajo} - {Vehiculo.Descripcion}";
    }
}
