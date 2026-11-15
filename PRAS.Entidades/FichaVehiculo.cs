namespace PRAS.Entidades
{
    /// <summary>
    /// Toda la historia de un auto junta, para la pantalla "Ficha del vehículo" (solo lectura):
    /// sus datos, la compra, el diagnóstico, la orden de trabajo con sus tareas, la agencia,
    /// la publicación y la venta. No es una tabla: la arma la DAL consultando varias tablas.
    /// Los datos que el auto todavía no tiene (por ejemplo la venta) quedan en null.
    /// </summary>
    public class FichaVehiculo
    {
        public Vehiculo Vehiculo { get; set; }

        // Compra (el auto chocado que compramos)
        public DateTime? FechaCompra { get; set; }
        public decimal? PrecioCompra { get; set; }
        public string FormaPagoCompra { get; set; }
        public string Vendedor { get; set; }
        public string ObservacionesCompra { get; set; }

        // Taller: último diagnóstico
        public DateTime? FechaDiagnostico { get; set; }
        public string DescripcionDanios { get; set; }
        public string EstadoDiagnostico { get; set; }

        // Taller: presupuesto y última orden de trabajo
        public decimal? MontoPresupuesto { get; set; }
        public DateTime? FechaOrden { get; set; }
        public string EstadoOrden { get; set; }
        public string Prioridad { get; set; }
        public List<TareaFicha> Tareas { get; set; } = new List<TareaFicha>();

        // Agencia: última consignación y última publicación
        public string Agencia { get; set; }
        public DateTime? FechaEntregaAgencia { get; set; }
        public string EstadoConsignacion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public string Plataforma { get; set; }
        public decimal? PrecioPublicado { get; set; }
        public string EstadoPublicacion { get; set; }

        // Venta
        public DateTime? FechaVenta { get; set; }
        public decimal? PrecioVenta { get; set; }
        public string Comprador { get; set; }
        public string FormaPagoVenta { get; set; }
    }

    /// <summary>Una tarea de la orden de trabajo, tal como se muestra en la ficha del vehículo.</summary>
    public class TareaFicha
    {
        public string Descripcion { get; set; }
        public string Mecanico { get; set; }
        public string Estado { get; set; }
        public decimal CostoEstimado { get; set; }
    }
}
