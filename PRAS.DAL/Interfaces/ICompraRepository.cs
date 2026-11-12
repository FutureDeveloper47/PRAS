using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de compras (tabla Compra). El Add también da de alta el vehículo, en la misma transacción.</summary>
    public interface ICompraRepository : ICrudRepository<Compra>
    {
        List<Compra> Listar(DateTime desde, DateTime hasta, string filtro);

        Compra ObtenerPorVehiculo(int idVehiculo);

        List<Compra> ListarPorCliente(int idCliente);
    }
}
