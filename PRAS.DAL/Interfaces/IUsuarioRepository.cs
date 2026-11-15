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

        void CambiarContrasena(int idUsuario, string contrasenaHash);

        void GuardarPreguntaSeguridad(int idUsuario, string pregunta, string respuestaHash);

        /// <summary>Guarda nombre, apellido, DNI, teléfono y email del usuario (tabla UsuarioPersona).</summary>
        void GuardarPersona(Usuario usuario);
    }
}
