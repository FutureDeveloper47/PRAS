using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de proveedores (tabla Proveedor).</summary>
    public interface IProveedorRepository : IGenericRepository<Proveedor>
    {
        Proveedor ObtenerPorCUIT(string cuit);

        List<Proveedor> Listar(string filtro, bool incluirInactivos);

        void CambiarEstado(int idProveedor, bool activo);

        bool TienePedidosPendientes(int idProveedor);
    }
}
