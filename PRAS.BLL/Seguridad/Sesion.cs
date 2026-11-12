using PRAS.BLL.Excepciones;
using PRAS.Entidades;
using PRAS.Services.DomainModel.Composite;
using PRAS.Services.Facade;

namespace PRAS.BLL
{
    /// <summary>
    /// Guarda quién está usando el sistema en este momento y decide a qué funciones
    /// puede entrar según sus permisos (familias y patentes).
    /// Es static porque en una aplicación de escritorio hay una sola sesión abierta a la vez.
    ///
    /// Los permisos (patrón Composite) se guardan acá y no en la entidad Usuario:
    /// así PRAS.Entidades no depende del proyecto Services (no se cruzan las capas).
    /// </summary>
    public static class Sesion
    {
        private static List<Patente> _patentes = new List<Patente>();

        public static Usuario UsuarioActual { get; private set; }

        public static bool HayUsuarioLogueado => UsuarioActual != null;

        /// <summary>Patentes del usuario logueado (ya recorridas todas sus familias).</summary>
        public static IReadOnlyList<Patente> Patentes => _patentes;

        internal static void Iniciar(Usuario usuario, List<Patente> patentes)
        {
            UsuarioActual = usuario;
            _patentes = patentes ?? new List<Patente>();
            LoggerService.Usuario = usuario.NombreUsuario;   // para que la bitácora sepa quién hizo cada cosa
        }

        public static void Cerrar()
        {
            UsuarioActual = null;
            _patentes = new List<Patente>();
            LoggerService.Usuario = null;
        }

        /// <summary>
        /// ¿El usuario logueado tiene esta función?
        /// Se responde con el patrón COMPOSITE: las familias del usuario
        /// ya se recorrieron hasta llegar a sus patentes, y se busca la patente cuyo DataKey es el
        /// nombre de la función. Los permisos se administran en Seguridad > Permisos.
        /// </summary>
        public static bool TienePermiso(Funcion funcion)
        {
            if (UsuarioActual == null)
                return false;

            string clave = funcion.ToString();
            return _patentes.Exists(p => p.DataKey == clave);
        }

        /// <summary>
        /// Control en la BLL además del menú: aunque alguien llegara a la pantalla,
        /// la operación se rechaza si el usuario no tiene la patente.
        /// </summary>
        public static void ExigirPermiso(Funcion funcion)
        {
            if (!TienePermiso(funcion))
                throw new PermisoDenegadoException(funcion);
        }

        /// <summary>Texto amigable del rol (con ñ) para mostrar en pantalla.</summary>
        public static string TextoRol(Rol rol)
        {
            switch (rol)
            {
                case Rol.Dueno: return "Dueño";
                case Rol.Administrador: return "Administrador";
                case Rol.Mecanico: return "Mecánico";
                default: return rol.ToString();
            }
        }
    }
}
