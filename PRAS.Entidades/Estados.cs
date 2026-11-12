namespace PRAS.Entidades
{
    /// <summary>
    /// Textos de estado que se guardan en las columnas VARCHAR de la base.
    /// Están todos juntos para no escribirlos "a mano" en cada lugar (y no equivocarse con una letra).
    /// El estado del VEHÍCULO no está acá: es el enum EstadoVehiculo (se guarda como número).
    /// </summary>
    public static class Estados
    {
        // Orden de trabajo
        public const string OrdenAbierta = "Abierta";
        public const string OrdenFinalizada = "Finalizada";

        // Tareas de la orden
        public const string TareaPendiente = "Pendiente";
        public const string TareaEnCurso = "En curso";
        public const string TareaFinalizada = "Finalizada";

        // Diagnóstico
        public const string DiagnosticoPendiente = "Pendiente";
        public const string DiagnosticoFinalizado = "Finalizado";

        // Pedido de repuestos
        public const string PedidoPendiente = "Pendiente";
        public const string PedidoEntregado = "Entregado";

        // Consignación (auto en una agencia)
        public const string ConsignacionVigente = "Vigente";
        public const string ConsignacionRetirada = "Retirada";
        public const string ConsignacionVendida = "Vendida";

        // Publicación (aviso de venta)
        public const string PublicacionActiva = "Activa";
        public const string PublicacionFinalizada = "Finalizada";
    }
}
