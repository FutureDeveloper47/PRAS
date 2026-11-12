using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de agencias (tabla Agencia).</summary>
    public interface IAgenciaRepository : IGenericRepository<Agencia>
    {
        Agencia ObtenerPorCUIT(string cuit);

        List<Agencia> Listar(string filtro, bool incluirInactivos);

        void CambiarEstado(int idAgencia, bool activo);

        bool TieneConsignacionesVigentes(int idAgencia);
    }
}
