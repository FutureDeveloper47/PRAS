using System.Data;
using Microsoft.Data.SqlClient;
using PRAS.Services.Exceptions.DataAccess;
using PRAS.Services.Facade;

namespace PRAS.DAL.Tools
{
    /// <summary>
    /// Ayudante de SQL de la DAL.
    /// Todos los repositorios pasan por acá, así ninguno repite el código de abrir la conexión,
    /// armar el comando y cerrar todo. Siempre usamos parámetros (@nombre), nunca concatenamos valores.
    ///
    /// Manejo de errores: si SQL Server falla, la SqlException se registra en la bitácora
    /// (con LoggerService, que es la fachada de Services) y se envuelve en una DataAccessException
    /// con un mensaje claro. Así la BLL y la UI no dependen de SQL Server.
    /// </summary>
    internal static class SqlHelper
    {
        // ---------------------------- Base del negocio (PRAS) ----------------------------

        /// <summary>SELECT que devuelve filas.</summary>
        public static DataTable ExecuteDataTable(string commandText, params SqlParameter[] parameters)
        {
            return Envolver(nameof(ExecuteDataTable), commandText, () => Consultar(Conexion.Crear(), commandText, parameters));
        }

        /// <summary>INSERT / UPDATE / DELETE. Devuelve la cantidad de filas afectadas.</summary>
        public static int ExecuteNonQuery(string commandText, params SqlParameter[] parameters)
        {
            return Envolver(nameof(ExecuteNonQuery), commandText, () => NoConsulta(Conexion.Crear(), commandText, parameters));
        }

        /// <summary>Consulta que devuelve un solo valor (un COUNT o el Id recién insertado).</summary>
        public static object ExecuteScalar(string commandText, params SqlParameter[] parameters)
        {
            return Envolver(nameof(ExecuteScalar), commandText, () => Escalar(Conexion.Crear(), commandText, parameters));
        }

        // ---------------------------- Base de seguridad (PRAS_Seguridad) ----------------------------

        public static DataTable ExecuteDataTableSeguridad(string commandText, params SqlParameter[] parameters)
        {
            return Envolver(nameof(ExecuteDataTableSeguridad), commandText, () => Consultar(Conexion.CrearSeguridad(), commandText, parameters));
        }

        public static int ExecuteNonQuerySeguridad(string commandText, params SqlParameter[] parameters)
        {
            return Envolver(nameof(ExecuteNonQuerySeguridad), commandText, () => NoConsulta(Conexion.CrearSeguridad(), commandText, parameters));
        }

        public static object ExecuteScalarSeguridad(string commandText, params SqlParameter[] parameters)
        {
            return Envolver(nameof(ExecuteScalarSeguridad), commandText, () => Escalar(Conexion.CrearSeguridad(), commandText, parameters));
        }

        // ---------------------------- Transacciones ----------------------------

        /// <summary>
        /// Ejecuta varias operaciones como una sola: o se guardan todas o ninguna.
        /// Se usa, por ejemplo, en Registrar Compra (inserta el Vehiculo y la Compra).
        /// </summary>
        public static void ExecuteInTransaction(Action<SqlConnection, SqlTransaction> trabajo)
        {
            Envolver<object>(nameof(ExecuteInTransaction), "(transacción)", () =>
            {
                using var cn = Conexion.Crear();
                cn.Open();
                using var tx = cn.BeginTransaction();
                try
                {
                    trabajo(cn, tx);
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
                return null;
            });
        }

        /// <summary>INSERT / UPDATE / DELETE dentro de una transacción ya abierta.</summary>
        public static int ExecuteNonQuery(SqlConnection cn, SqlTransaction tx, string commandText, params SqlParameter[] parameters)
        {
            using var cmd = new SqlCommand(commandText, cn, tx);
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>Consulta de un solo valor dentro de una transacción ya abierta.</summary>
        public static object ExecuteScalar(SqlConnection cn, SqlTransaction tx, string commandText, params SqlParameter[] parameters)
        {
            using var cmd = new SqlCommand(commandText, cn, tx);
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteScalar();
        }

        // ---------------------------- Varios ----------------------------

        /// <summary>Crea un parámetro convirtiendo null en DBNull (que es lo que SQL Server espera).</summary>
        public static SqlParameter Param(string nombre, object valor)
        {
            return new SqlParameter(nombre, valor ?? DBNull.Value);
        }

        /// <summary>Intenta abrir una conexión a la base del negocio.</summary>
        public static bool ProbarConexion(out string error)
        {
            return Probar(Conexion.Crear, out error);
        }

        /// <summary>Intenta abrir una conexión a la base de seguridad.</summary>
        public static bool ProbarConexionSeguridad(out string error)
        {
            return Probar(Conexion.CrearSeguridad, out error);
        }

        private static bool Probar(Func<SqlConnection> crear, out string error)
        {
            try
            {
                using var cn = crear();
                cn.Open();
                error = "";
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private static DataTable Consultar(SqlConnection conexion, string sql, SqlParameter[] parametros)
        {
            using var cn = conexion;
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddRange(parametros);
            using var adaptador = new SqlDataAdapter(cmd);
            var tabla = new DataTable();
            adaptador.Fill(tabla);
            return tabla;
        }

        private static int NoConsulta(SqlConnection conexion, string sql, SqlParameter[] parametros)
        {
            using var cn = conexion;
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddRange(parametros);
            cn.Open();
            return cmd.ExecuteNonQuery();
        }

        private static object Escalar(SqlConnection conexion, string sql, SqlParameter[] parametros)
        {
            using var cn = conexion;
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddRange(parametros);
            cn.Open();
            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// Ejecuta el acceso a la base. Si SQL Server tira una SqlException:
        /// 1) la registramos en la bitácora (LoggerService), y
        /// 2) tiramos una DataAccessException con un mensaje entendible.
        /// Las demás excepciones (por ejemplo una ConcurrencyException de un repositorio) pasan sin cambios.
        /// </summary>
        private static T Envolver<T>(string metodo, string sql, Func<T> accion)
        {
            try
            {
                return accion();
            }
            catch (SqlException sqlEx)
            {
                try
                {
                    LoggerService.Error($"Error de base de datos en SqlHelper.{metodo}: {sqlEx.Message}", sqlEx);
                }
                catch
                {
                    // Si la bitácora también falla (por ejemplo, la base está caída), seguimos igual.
                }

                throw new DataAccessException(MensajeSegunError(sqlEx), sqlEx, CodigoSegunError(sqlEx));
            }
        }

        private static string MensajeSegunError(SqlException ex)
        {
            switch (ex.Number)
            {
                case 2627:
                case 2601:
                    return "Ya existe un registro con esos datos (dato duplicado).";
                case 547:
                    return "No se puede completar la operación porque el dato está relacionado con otros registros.";
                case -2:
                    return "La base de datos tardó demasiado en responder. Intente nuevamente.";
                case 53:
                case 2:
                case -1:
                case 4060:
                case 18456:
                    return "No se pudo conectar con la base de datos. Verifique que SQL Server esté funcionando.";
                default:
                    return "Error al acceder a la base de datos. Intente más tarde o consulte al administrador.";
            }
        }

        private static string CodigoSegunError(SqlException ex)
        {
            switch (ex.Number)
            {
                case 2627:
                case 2601:
                    return "DB_DUPLICADO";
                case 547:
                    return "DB_RELACIONADO";
                case -2:
                    return "DB_TIMEOUT";
                case 53:
                case 2:
                case -1:
                case 4060:
                case 18456:
                    return "DB_UNAVAILABLE";
                default:
                    return "DB_ERROR";
            }
        }
    }
}
