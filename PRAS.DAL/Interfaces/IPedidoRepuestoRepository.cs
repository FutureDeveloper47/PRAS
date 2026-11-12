using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de pedidos de repuestos (tabla PedidoRepuesto). El Add guarda también los renglones.</summary>
    public interface IPedidoRepuestoRepository : ICrudRepository<PedidoRepuesto>
    {
        /// <summary>estado = null trae todos.</summary>
        List<PedidoRepuesto> Listar(string estado);

        /// <summary>
        /// El proveedor entregó: el pedido pasa a Entregado y sus repuestos se suman al presupuesto
        /// de la orden de trabajo. Devuelve false si el pedido ya no estaba pendiente.
        /// </summary>
        bool RegistrarEntrega(int idPedido, DateTime fechaEntrega);
    }
}
