using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.Facade
{
    /// <summary>
    /// FACHADA de los permisos de cada usuario.
    /// Se llama UsuarioPermisoService para no confundirse con el UsuarioService de la BLL
    /// (alta, modificación y baja de usuarios).
    /// </summary>
    public static class UsuarioPermisoService
    {
        public static List<Component> GetPrivilegios(int idUsuario)
        {
            return Logic.UsuarioPermisoLogic.GetPrivilegios(idUsuario);
        }

        /// <summary>
        /// Todas las patentes del usuario, recorriendo sus familias de forma recursiva (sin repetidas).
        /// Es lo que usa la BLL para saber a qué opciones puede entrar.
        /// </summary>
        public static List<Patente> GetPatentes(int idUsuario)
        {
            return Logic.UsuarioPermisoLogic.GetPatentes(idUsuario);
        }

        /// <summary>Recorre una lista de familias y patentes y devuelve todas las patentes (sin repetidas).</summary>
        public static List<Patente> ObtenerPatentes(List<Component> componentes)
        {
            return Logic.UsuarioPermisoLogic.RecorrerPatentes(componentes);
        }

        public static void AgregarFamilia(Familia familia, int idUsuario)
        {
            Logic.UsuarioPermisoLogic.AgregarFamilia(familia, idUsuario);
        }

        public static void AgregarPatente(Patente patente, int idUsuario)
        {
            Logic.UsuarioPermisoLogic.AgregarPatente(patente, idUsuario);
        }

        public static void Quitar(Component componente, int idUsuario)
        {
            Logic.UsuarioPermisoLogic.Quitar(componente, idUsuario);
        }
    }
}
