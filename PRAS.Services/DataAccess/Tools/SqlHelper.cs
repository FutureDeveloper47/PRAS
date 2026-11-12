using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PRAS.Services.DataAccess.Tools
{
    /// <summary>
    /// Ayudante de SQL del proyecto Services.
    ///
    /// El proyecto Services trabaja con la base
    /// de SEGURIDAD: PRAS_Seguridad (usuarios, dígitos verificadores y bitácora).
    /// Además conoce la cadena de la base del NEGOCIO (PRAS), porque los dígitos verificadores
    /// también protegen la tabla Cliente y el backup copia las dos bases.
    /// </summary>
    internal static class SqlHelper
    {
        readonly static string conString;
        readonly static string conStringNegocio;

        static SqlHelper()
        {
            conString = ConfigurationManager.ConnectionStrings["PRAS_Seguridad"].ConnectionString;
            conStringNegocio = ConfigurationManager.ConnectionStrings["PRAS"].ConnectionString;
        }

        /// <summary>Cadena de conexión a la base de SEGURIDAD (PRAS_Seguridad).</summary>
        public static string ConnectionString
        {
            get { return conString; }
        }

        /// <summary>Cadena de conexión a la base del NEGOCIO (PRAS).</summary>
        public static string ConnectionStringNegocio
        {
            get { return conStringNegocio; }
        }

        // Base de seguridad (la de siempre)

        public static Int32 ExecuteNonQuery(String commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(conString, commandText, commandType, parameters);
        }

        public static Object ExecuteScalar(String commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            return ExecuteScalar(conString, commandText, commandType, parameters);
        }

        public static SqlDataReader ExecuteReader(String commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            return ExecuteReader(conString, commandText, commandType, parameters);
        }

        // Cualquier base (le pasamos la cadena de conexión)

        public static Int32 ExecuteNonQuery(String connectionString, String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            CheckNullables(parameters);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    // Hay tres tipos de comando: StoredProcedure, Text y TableDirect.
                    // TableDirect es solo para OLE DB.
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Set the connection, command, and then execute the command and only return one value.
        /// </summary>
        public static Object ExecuteScalar(String connectionString, String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            CheckNullables(parameters);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Set the connection, command, and then execute the command with query and return the reader.
        /// </summary>
        public static SqlDataReader ExecuteReader(String connectionString, String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            CheckNullables(parameters);

            SqlConnection conn = new SqlConnection(connectionString);

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);

                conn.Open();
                // Con CommandBehavior.CloseConnection la conexión se cierra
                // cuando se cierra el lector.
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
        }

        private static void CheckNullables(SqlParameter[] parameters)
        {
            foreach (SqlParameter item in parameters)
            {
                if (item.Value == null)
                {
                    item.Value = DBNull.Value;
                }
            }
        }
    }
}
