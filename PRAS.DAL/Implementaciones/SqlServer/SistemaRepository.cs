using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Chequeos generales de la base (singleton): si hay conexión con las dos bases y, la primera
    /// vez que se usa el sistema en una PC, la creación de las bases con el script de instalación.
    /// Lo usa la BLL al arrancar el programa, antes de pedir usuario y contraseña.
    /// </summary>
    internal sealed class SistemaRepository : ISistemaRepository
    {
        #region Singleton
        private static readonly SistemaRepository _instance = new SistemaRepository();

        public static SistemaRepository Current
        {
            get { return _instance; }
        }

        private SistemaRepository()
        {
        }
        #endregion

        public bool ProbarConexion(out string error)
        {
            return SqlHelper.ProbarConexion(out error);
        }

        public bool ProbarConexionSeguridad(out string error)
        {
            return SqlHelper.ProbarConexionSeguridad(out error);
        }

        /// <summary>
        /// Tabla nueva de la 4ta entrega (pregunta de seguridad para recuperar la contraseña).
        /// Va en una tabla aparte y no en Usuario para no cambiar las columnas que protege el DVH.
        /// Al admin le dejamos una pregunta de prueba (respuesta: pras) para poder mostrar la recuperación.
        /// </summary>
        private const string ActualizarEstructuraStatement = @"
            IF OBJECT_ID('UsuarioRecuperacion', 'U') IS NULL
            BEGIN
                CREATE TABLE UsuarioRecuperacion (
                    IdUsuario          INT            NOT NULL PRIMARY KEY
                                       CONSTRAINT FK_UsuarioRecuperacion_Usuario REFERENCES Usuario(IdUsuario),
                    PreguntaSeguridad  NVARCHAR(200)  NOT NULL,
                    RespuestaHash      VARCHAR(64)    NOT NULL
                );

                INSERT INTO UsuarioRecuperacion (IdUsuario, PreguntaSeguridad, RespuestaHash)
                SELECT IdUsuario, N'¿Cuál es el nombre del sistema?', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'pras'), 2)
                FROM Usuario
                WHERE NombreUsuario = N'admin';
            END";

        /// <summary>
        /// Cambios del 15/11 (5ta tanda). Cada bloque se fija antes si ya está hecho (IF NOT EXISTS),
        /// así se puede correr todas las veces que haga falta sin romper ni duplicar nada.
        /// 1) Tabla UsuarioPersona: nombre, apellido, DNI, teléfono y email de cada usuario
        ///    (aparte de Usuario para no tocar las columnas que protege el DVH).
        /// 2) Pregunta de seguridad para los usuarios de prueba que todavía no tienen una
        ///    (respuesta: pras). No pisa las preguntas que ya cargó cada usuario.
        /// 3) Nombre de los usuarios de prueba admin y dueno (los mecánicos toman el de la tabla Mecanico).
        /// </summary>
        private static readonly string[] ActualizacionesSeguridad =
        {
            @"IF OBJECT_ID('UsuarioPersona', 'U') IS NULL
              BEGIN
                  CREATE TABLE UsuarioPersona (
                      IdUsuario  INT            NOT NULL PRIMARY KEY
                                 CONSTRAINT FK_UsuarioPersona_Usuario REFERENCES Usuario(IdUsuario),
                      Nombre     NVARCHAR(50)   NOT NULL,
                      Apellido   NVARCHAR(50)   NOT NULL,
                      DNI        VARCHAR(10)    NULL,
                      Telefono   VARCHAR(200)   NULL,   -- encriptado con AES
                      Email      NVARCHAR(400)  NULL    -- encriptado con AES
                  );
              END",

            @"INSERT INTO UsuarioRecuperacion (IdUsuario, PreguntaSeguridad, RespuestaHash)
              SELECT U.IdUsuario, N'¿Cuál es el nombre del sistema?', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'pras'), 2)
              FROM Usuario U
              WHERE U.NombreUsuario IN (N'admin', N'dueno', N'mecanico', N'juan')
                AND NOT EXISTS (SELECT 1 FROM UsuarioRecuperacion R WHERE R.IdUsuario = U.IdUsuario)",

            @"INSERT INTO UsuarioPersona (IdUsuario, Nombre, Apellido)
              SELECT U.IdUsuario, X.Nombre, X.Apellido
              FROM (VALUES (N'admin', N'Matías', N'Montoto'), (N'dueno', N'Gustavo', N'Ibarra')) AS X (Usuario, Nombre, Apellido)
              INNER JOIN Usuario U ON U.NombreUsuario = X.Usuario
              WHERE NOT EXISTS (SELECT 1 FROM UsuarioPersona P WHERE P.IdUsuario = U.IdUsuario)"
        };

        /// <summary>
        /// Cambios del 15/11 en la base del negocio (PRAS): ficha del auto para venderlo (en Vehiculo)
        /// y observaciones de la publicación (en Publicacion). Cada columna se agrega solo si no existe.
        /// </summary>
        private static readonly string[] ActualizacionesNegocio =
        {
            "IF COL_LENGTH('Vehiculo', 'Kilometraje') IS NULL ALTER TABLE Vehiculo ADD Kilometraje INT NULL CONSTRAINT CK_Vehiculo_Kilometraje CHECK (Kilometraje >= 0)",
            "IF COL_LENGTH('Vehiculo', 'Combustible') IS NULL ALTER TABLE Vehiculo ADD Combustible NVARCHAR(20) NULL",
            "IF COL_LENGTH('Vehiculo', 'TieneGNC') IS NULL ALTER TABLE Vehiculo ADD TieneGNC BIT NULL",
            "IF COL_LENGTH('Vehiculo', 'Transmision') IS NULL ALTER TABLE Vehiculo ADD Transmision NVARCHAR(20) NULL",
            "IF COL_LENGTH('Vehiculo', 'Puertas') IS NULL ALTER TABLE Vehiculo ADD Puertas TINYINT NULL CONSTRAINT CK_Vehiculo_Puertas CHECK (Puertas BETWEEN 2 AND 5)",
            "IF COL_LENGTH('Vehiculo', 'CantidadDuenos') IS NULL ALTER TABLE Vehiculo ADD CantidadDuenos TINYINT NULL CONSTRAINT CK_Vehiculo_Duenos CHECK (CantidadDuenos BETWEEN 1 AND 20)",
            "IF COL_LENGTH('Vehiculo', 'VTVVigente') IS NULL ALTER TABLE Vehiculo ADD VTVVigente BIT NULL",
            "IF COL_LENGTH('Vehiculo', 'ServiceOficial') IS NULL ALTER TABLE Vehiculo ADD ServiceOficial BIT NULL",
            "IF COL_LENGTH('Vehiculo', 'AireAcondicionado') IS NULL ALTER TABLE Vehiculo ADD AireAcondicionado BIT NULL",
            "IF COL_LENGTH('Vehiculo', 'DireccionAsistida') IS NULL ALTER TABLE Vehiculo ADD DireccionAsistida BIT NULL",
            "IF COL_LENGTH('Vehiculo', 'EstadoChapaPintura') IS NULL ALTER TABLE Vehiculo ADD EstadoChapaPintura NVARCHAR(20) NULL",
            "IF COL_LENGTH('Publicacion', 'Observaciones') IS NULL ALTER TABLE Publicacion ADD Observaciones NVARCHAR(500) NULL"
        };

        public void ActualizarEstructura()
        {
            SqlHelper.ExecuteNonQuerySeguridad(ActualizarEstructuraStatement);

            foreach (string actualizacion in ActualizacionesSeguridad)
                SqlHelper.ExecuteNonQuerySeguridad(actualizacion);

            foreach (string actualizacion in ActualizacionesNegocio)
                SqlHelper.ExecuteNonQuery(actualizacion);
        }

        /// <summary>Pregunta a SQL Server (base master) si existen PRAS y PRAS_Seguridad.</summary>
        public bool ExistenLasBases()
        {
            using var cn = Conexion.CrearMaster();
            cn.Open();
            using var cmd = new SqlCommand(
                "SELECT CASE WHEN DB_ID('PRAS') IS NOT NULL AND DB_ID('PRAS_Seguridad') IS NOT NULL THEN 1 ELSE 0 END", cn);
            return (int)cmd.ExecuteScalar() == 1;
        }

        /// <summary>
        /// Ejecuta el script de instalación completo. El script está escrito para SQL Server
        /// Management Studio, que separa los bloques con la palabra GO: acá hacemos lo mismo,
        /// cortamos el texto en cada GO y ejecutamos los bloques uno por uno, en orden.
        /// </summary>
        public void InstalarBases(string rutaScript)
        {
            string script = File.ReadAllText(rutaScript);
            string[] bloques = Regex.Split(script, @"^\s*GO\s*;?\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using var cn = Conexion.CrearMaster();
            cn.Open();
            foreach (string bloque in bloques)
            {
                if (string.IsNullOrWhiteSpace(bloque))
                    continue;
                using var cmd = new SqlCommand(bloque, cn) { CommandTimeout = 300 };
                cmd.ExecuteNonQuery();
            }
        }
    }
}
