using System.Data;
using Microsoft.Data.SqlClient;
using PRAS.Services.DataAccess.Tools;

namespace PRAS.Services.DataAccess
{
    /// <summary>
    /// Backup y Restore de LAS DOS bases con los comandos propios de SQL Server
    /// (BACKUP DATABASE / RESTORE DATABASE).
    ///
    /// Las dos bases van al MISMO archivo .bak (uno detrás del otro):
    ///   posición 1 = PRAS (negocio), posición 2 = PRAS_Seguridad.
    /// Se guardan juntas porque dependen una de la otra: por ejemplo, el dígito verificador
    /// vertical de la tabla Cliente (que está en PRAS) se guarda en PRAS_Seguridad.
    ///
    /// Se conecta a la base "master": no se puede pisar (restaurar) una base
    /// mientras uno mismo está conectado adentro de ella.
    /// </summary>
    internal static class BackupDal
    {
        /// <summary>Nombre de la base del negocio (Initial Catalog de la cadena "PRAS").</summary>
        public static string NombreBase()
        {
            return new SqlConnectionStringBuilder(SqlHelper.ConnectionStringNegocio).InitialCatalog;
        }

        /// <summary>Nombre de la base de seguridad (Initial Catalog de la cadena "PRAS_Seguridad").</summary>
        public static string NombreBaseSeguridad()
        {
            return new SqlConnectionStringBuilder(SqlHelper.ConnectionString).InitialCatalog;
        }

        private static List<string> BasesDelSistema()
        {
            return new List<string> { NombreBase(), NombreBaseSeguridad() };
        }

        /// <summary>Misma cadena de conexión pero apuntando a master.</summary>
        private static string CadenaMaster()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(SqlHelper.ConnectionString);
            builder.InitialCatalog = "master";
            return builder.ConnectionString;
        }

        public static void RealizarBackup(string rutaArchivo)
        {
            using (SqlConnection conn = new SqlConnection(CadenaMaster()))
            {
                conn.Open();

                bool primera = true;
                foreach (string nombreBase in BasesDelSistema())
                {
                    // INIT arranca el archivo de cero y NOINIT agrega al final.
                    string modo = primera ? "INIT" : "NOINIT";

                    using (SqlCommand cmd = new SqlCommand(
                        $"BACKUP DATABASE @Base TO DISK = @Ruta WITH {modo}, NAME = @Nombre", conn))
                    {
                        cmd.CommandTimeout = 0;   // sin límite de tiempo, una base grande puede tardar
                        cmd.Parameters.AddWithValue("@Base", nombreBase);
                        cmd.Parameters.AddWithValue("@Ruta", rutaArchivo);
                        cmd.Parameters.AddWithValue("@Nombre", $"{nombreBase} - Backup {DateTime.Now:dd/MM/yyyy HH:mm}");
                        cmd.ExecuteNonQuery();
                    }

                    primera = false;
                }
            }
        }

        public static void RealizarRestore(string rutaArchivo)
        {
            // Cerramos las conexiones que el sistema tiene guardadas en el pool.
            SqlConnection.ClearAllPools();

            using (SqlConnection conn = new SqlConnection(CadenaMaster()))
            {
                conn.Open();

                // 1) Vemos qué bases trae el archivo y en qué posición está cada una.
                Dictionary<string, int> contenido = LeerContenidoDelArchivo(conn, rutaArchivo);

                List<string> aRestaurar = BasesDelSistema().Where(b => contenido.ContainsKey(b)).ToList();
                if (aRestaurar.Count == 0)
                {
                    throw new InvalidOperationException("El archivo no contiene un backup de las bases del sistema PRAS.");
                }

                // 2) Restauramos cada una.
                foreach (string nombreBase in aRestaurar)
                {
                    RestaurarBase(conn, nombreBase, rutaArchivo, contenido[nombreBase]);
                }
            }

            // Las conexiones viejas del pool quedaron cortadas, las tiramos.
            SqlConnection.ClearAllPools();
        }

        /// <summary>RESTORE HEADERONLY: lista los backups que hay dentro del archivo (base y posición).</summary>
        private static Dictionary<string, int> LeerContenidoDelArchivo(SqlConnection conn, string rutaArchivo)
        {
            Dictionary<string, int> contenido = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            using (SqlCommand cmd = new SqlCommand("RESTORE HEADERONLY FROM DISK = @Ruta", conn))
            {
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@Ruta", rutaArchivo);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Si la misma base aparece más de una vez, nos quedamos con la última.
                        contenido[reader["DatabaseName"].ToString()] = Convert.ToInt32(reader["Position"]);
                    }
                }
            }

            return contenido;
        }

        private static void RestaurarBase(SqlConnection conn, string nombreBase, string rutaArchivo, int posicion)
        {
            // El nombre va entre corchetes porque ALTER DATABASE no acepta parámetros para el nombre.
            string baseEntreCorchetes = "[" + nombreBase.Replace("]", "]]") + "]";
            string baseEntreComillas = nombreBase.Replace("'", "''");

            try
            {
                // a) Sacamos a todos los que estén usando la base, si no el RESTORE falla.
                EjecutarEnMaster(conn, $"IF DB_ID('{baseEntreComillas}') IS NOT NULL ALTER DATABASE {baseEntreCorchetes} SET SINGLE_USER WITH ROLLBACK IMMEDIATE");

                // b) Pisamos la base con lo que hay en esa posición del .bak.
                using (SqlCommand cmd = new SqlCommand("RESTORE DATABASE @Base FROM DISK = @Ruta WITH FILE = @Posicion, REPLACE", conn))
                {
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.AddWithValue("@Base", nombreBase);
                    cmd.Parameters.AddWithValue("@Ruta", rutaArchivo);
                    cmd.Parameters.AddWithValue("@Posicion", posicion);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                // c) Pase lo que pase, la base vuelve a quedar abierta para todos.
                EjecutarEnMaster(conn, $"IF DB_ID('{baseEntreComillas}') IS NOT NULL ALTER DATABASE {baseEntreCorchetes} SET MULTI_USER");
            }
        }

        private static void EjecutarEnMaster(SqlConnection conn, string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
