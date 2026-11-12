using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de tareas (tabla TareaOrdenTrabajo).</summary>
    public interface ITareaOrdenTrabajoRepository : ICrudRepository<TareaOrdenTrabajo>
    {
        List<TareaOrdenTrabajo> ListarPorOrden(int idOrdenTrabajo);

        /// <summary>Tareas de órdenes abiertas. idMecanico = null trae las de todos.</summary>
        List<TareaOrdenTrabajo> ListarDeMecanico(int? idMecanico, bool incluirFinalizadas);

        /// <summary>Cambia el estado solo si la tarea seguía en estadoAnterior. Devuelve false si no.</summary>
        bool CambiarEstado(int idTarea, string estadoAnterior, string estadoNuevo, DateTime fecha);
    }
}
