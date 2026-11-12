using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de presupuestos (tabla Presupuesto).</summary>
    public interface IPresupuestoRepository : IGetByIdRepository<Presupuesto>
    {
        /// <summary>Último presupuesto de cada auto que está En reparación.</summary>
        List<Presupuesto> ListarDeAutosEnReparacion();

        /// <summary>Agrega gastos al presupuesto y recalcula el total, en una transacción.</summary>
        void AgregarDetalles(int idPresupuesto, List<DetallePresupuesto> detalles);
    }
}
