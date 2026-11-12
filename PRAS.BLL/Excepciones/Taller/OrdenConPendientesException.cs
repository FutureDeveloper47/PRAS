namespace PRAS.BLL.Excepciones
{
    /// <summary>No se puede cerrar la orden: todavía tiene tareas sin terminar o pedidos de repuestos sin entregar.</summary>
    public class OrdenConPendientesException : TallerBusinessException
    {
        public int IdOrdenTrabajo { get; }
        public int TareasPendientes { get; }
        public int PedidosPendientes { get; }

        public OrdenConPendientesException(int idOrdenTrabajo, int tareasPendientes, int pedidosPendientes)
            : base(ArmarMensaje(idOrdenTrabajo, tareasPendientes, pedidosPendientes), "ORDEN_CON_PENDIENTES")
        {
            IdOrdenTrabajo = idOrdenTrabajo;
            TareasPendientes = tareasPendientes;
            PedidosPendientes = pedidosPendientes;
        }

        private static string ArmarMensaje(int idOrden, int tareas, int pedidos)
        {
            var motivos = new List<string>();
            if (tareas > 0) motivos.Add($"{tareas} tarea(s) sin finalizar");
            if (pedidos > 0) motivos.Add($"{pedidos} pedido(s) de repuestos sin entregar");
            return $"No se puede finalizar la OT {idOrden}: tiene {string.Join(" y ", motivos)}.";
        }
    }
}
