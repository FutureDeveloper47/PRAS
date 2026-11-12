using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de ventas (tabla Venta).</summary>
    public interface IVentaRepository : IGetByIdRepository<Venta>
    {
        List<Venta> Listar(DateTime desde, DateTime hasta, string filtro);

        List<Venta> ListarPorCliente(int idCliente);

        /// <summary>
        /// Registra la venta en una transacción: la consignación pasa a Vendida, se finaliza la
        /// publicación activa y el auto pasa a Vendido. Devuelve el Id de la venta.
        /// </summary>
        int Registrar(Venta venta, int idVehiculo);
    }
}
