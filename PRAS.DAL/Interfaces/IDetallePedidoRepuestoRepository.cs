using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de los renglones de un pedido (tabla DetallePedidoRepuesto).</summary>
    public interface IDetallePedidoRepuestoRepository : IGetByIdRepository<DetallePedidoRepuesto>
    {
        List<DetallePedidoRepuesto> ListarPorPedido(int idPedido);
    }
}
