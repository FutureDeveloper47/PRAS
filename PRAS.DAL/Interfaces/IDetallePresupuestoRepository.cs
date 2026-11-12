using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de los renglones de un presupuesto (tabla DetallePresupuesto).</summary>
    public interface IDetallePresupuestoRepository : IGetByIdRepository<DetallePresupuesto>
    {
        List<DetallePresupuesto> ListarPorPresupuesto(int idPresupuesto);
    }
}
