using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de usuarios (tabla Usuario de la base PRAS_Seguridad).</summary>
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Usuario ObtenerPorNombreUsuario(string nombreUsuario);

        Usuario ObtenerPorIdMecanico(int idMecanico);

        List<Usuario> Listar(bool incluirInactivos);

        void CambiarEstado(int idUsuario, bool activo);

        void CambiarIdioma(int idUsuario, string idioma);

        int ContarAdministradoresActivos(int idUsuarioExcluido);
    }
}
