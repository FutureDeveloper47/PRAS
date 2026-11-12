using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de órdenes de trabajo (tabla OrdenTrabajo).</summary>
    public interface IOrdenTrabajoRepository : IGetByIdRepository<OrdenTrabajo>
    {
        /// <summary>estado = null trae todas. idMecanico = null trae las de todos.</summary>
        List<OrdenTrabajo> Listar(string estado, int? idMecanico);

        void CambiarPrioridad(int idOrdenTrabajo, string prioridad);

        /// <summary>Tareas sin terminar y pedidos de repuestos sin entregar de la orden.</summary>
        void ContarPendientes(int idOrdenTrabajo, out int tareasPendientes, out int pedidosPendientes);
    }
}
