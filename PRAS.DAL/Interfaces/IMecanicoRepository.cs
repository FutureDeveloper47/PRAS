using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de mecánicos (tabla Mecanico).</summary>
    public interface IMecanicoRepository : IGenericRepository<Mecanico>
    {
        List<Mecanico> Listar(string filtro, bool incluirInactivos);

        void CambiarEstado(int idMecanico, bool activo);

        bool TieneTareasPendientes(int idMecanico);
    }
}
