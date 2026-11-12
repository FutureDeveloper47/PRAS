using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de clientes (tabla Cliente).</summary>
    public interface IClienteRepository : IGenericRepository<Cliente>
    {
        Cliente ObtenerPorDNI(string dni);

        List<Cliente> Listar(string filtro, bool incluirInactivos);

        void CambiarEstado(int idCliente, bool activo);

        /// <summary>
        /// Ids de los clientes que todavía tienen el teléfono o el email sin encriptar
        /// (datos cargados antes de que existiera el encriptado AES).
        /// </summary>
        List<int> ListarIdsSinEncriptar();
    }
}
