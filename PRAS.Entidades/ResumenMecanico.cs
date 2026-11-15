namespace PRAS.Entidades
{
    /// <summary>
    /// Números del Inicio del mecánico (sus tarjetas). No es una tabla: lo arma la DAL contando.
    /// "Mis" = lo que tiene asignado el mecánico logueado (tareas de órdenes de trabajo a su nombre).
    /// </summary>
    public class ResumenMecanico
    {
        /// <summary>Autos del taller esperando diagnóstico (el diagnóstico no se asigna a un mecánico en particular).</summary>
        public int AutosEnDiagnostico { get; set; }

        /// <summary>Órdenes de trabajo abiertas donde tiene alguna tarea.</summary>
        public int OrdenesAbiertas { get; set; }

        /// <summary>Pedidos de repuestos pendientes de entrega de sus órdenes.</summary>
        public int PedidosPendientes { get; set; }

        /// <summary>Tareas que terminó en el mes actual.</summary>
        public int TareasRealizadasMes { get; set; }
    }
}
