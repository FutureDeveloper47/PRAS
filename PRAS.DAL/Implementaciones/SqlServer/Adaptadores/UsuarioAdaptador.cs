using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la tabla Usuario (base PRAS_Seguridad) en un objeto Usuario.
    /// </summary>
    internal sealed class UsuarioAdaptador : IAdaptador<Usuario>
    {
        #region Singleton
        private static readonly UsuarioAdaptador _instance = new UsuarioAdaptador();

        public static UsuarioAdaptador Current
        {
            get { return _instance; }
        }

        private UsuarioAdaptador()
        {
        }
        #endregion

        public Usuario Fill(DataRow fila)
        {
            return new Usuario
            {
                IdUsuario = (int)fila["IdUsuario"],
                NombreUsuario = (string)fila["NombreUsuario"],
                ContrasenaHash = (string)fila["ContrasenaHash"],
                Rol = (Rol)Convert.ToInt32(fila["Rol"]),   // en la base es TINYINT
                IdMecanico = fila["IdMecanico"] as int?,   // "as int?" deja null si en la base hay NULL
                Activo = (bool)fila["Activo"],
                Idioma = fila["Idioma"] as string   // null si nunca eligió idioma
            };
        }
    }
}
