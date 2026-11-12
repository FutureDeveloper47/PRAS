using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio del taller (tabla Taller) y de las dos operaciones que mueven al auto dentro del taller.</summary>
    public interface ITallerRepository : IGetByIdRepository<Taller>
    {
        /// <summary>El taller de la empresa (el primero cargado). null si no hay ninguno.</summary>
        Taller ObtenerPrincipal();

        /// <summary>
        /// Ingreso al taller en una sola transacción: recepción, diagnóstico, presupuesto vacío,
        /// orden de trabajo y tarea para el mecánico. El auto pasa de Comprado a En reparación.
        /// Devuelve el Id de la orden de trabajo.
        /// </summary>
        int RegistrarIngreso(int idVehiculo, int idMecanico, string diagnostico, string prioridad);

        /// <summary>Termina tareas y orden abiertas del auto, que pasa de En reparación a Reparado.</summary>
        bool FinalizarReparacion(int idVehiculo);
    }
}
