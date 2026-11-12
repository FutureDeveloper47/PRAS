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
