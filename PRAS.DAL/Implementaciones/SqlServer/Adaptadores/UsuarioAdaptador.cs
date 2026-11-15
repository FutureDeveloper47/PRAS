using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;
using PRAS.Services.Facade;

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

        /// <summary>Lee una columna de texto que puede no venir en la consulta o estar en NULL.</summary>
        private static string Columna(DataRow fila, string nombre)
        {
            return fila.Table.Columns.Contains(nombre) ? fila[nombre] as string : null;
        }

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
                Idioma = fila["Idioma"] as string,   // null si nunca eligió idioma
                PreguntaSeguridad = fila.Table.Columns.Contains("PreguntaSeguridad") ? fila["PreguntaSeguridad"] as string : null,
                RespuestaSeguridadHash = fila.Table.Columns.Contains("RespuestaHash") ? fila["RespuestaHash"] as string : null,

                // Datos personales (tabla UsuarioPersona). Teléfono y email vienen encriptados con AES.
                Nombre = Columna(fila, "PersonaNombre"),
                Apellido = Columna(fila, "PersonaApellido"),
                DNI = Columna(fila, "PersonaDNI"),
                Telefono = CryptographyService.Desencriptar(Columna(fila, "PersonaTelefono")),
                Email = CryptographyService.Desencriptar(Columna(fila, "PersonaEmail")),
                UltimoIngreso = fila.Table.Columns.Contains("UltimoIngreso") ? fila["UltimoIngreso"] as DateTime? : null
            };
        }
    }
}
