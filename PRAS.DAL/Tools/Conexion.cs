using System.Configuration;
using Microsoft.Data.SqlClient;

namespace PRAS.DAL.Tools
{
    /// <summary>
    /// Lee las cadenas de conexión del App.config (proyecto PRAS.UI) y crea conexiones nuevas.
    /// Trabajamos con dos bases:
    /// - "PRAS"           -> datos del negocio (clientes, autos, compras, taller, ventas...).
    /// - "PRAS_Seguridad" -> usuarios, permisos, dígitos verificadores y bitácora.
    /// Es internal: fuera de la DAL nadie necesita conocer la conexión.
    /// </summary>
    internal static class Conexion
    {
        private const string NombreCadena = "PRAS";
        private const string NombreCadenaSeguridad = "PRAS_Seguridad";

        /// <summary>Conexión a la base del negocio (PRAS).</summary>
        public static SqlConnection Crear()
        {
            return new SqlConnection(LeerCadena(NombreCadena));
        }

        /// <summary>Conexión a la base de seguridad (PRAS_Seguridad).</summary>
        public static SqlConnection CrearSeguridad()
        {
            return new SqlConnection(LeerCadena(NombreCadenaSeguridad));
        }

        /// <summary>
        /// Conexión a la base "master" del mismo servidor. Se usa una sola vez: para ver si las
        /// bases de PRAS existen y, si no, crearlas (primera vez que se usa el sistema en una PC).
        /// </summary>
        public static SqlConnection CrearMaster()
        {
            var datos = new SqlConnectionStringBuilder(LeerCadena(NombreCadena)) { InitialCatalog = "master" };
            return new SqlConnection(datos.ConnectionString);
        }

        private static string LeerCadena(string nombre)
        {
            var cs = ConfigurationManager.ConnectionStrings[nombre];
            if (cs == null)
                throw new InvalidOperationException($"No se encontró la cadena de conexión '{nombre}' en App.config.");
            return cs.ConnectionString;
        }
    }
}
