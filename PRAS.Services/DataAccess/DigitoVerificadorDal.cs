using System.Data;
using Microsoft.Data.SqlClient;
using PRAS.Services.DataAccess.Tools;

namespace PRAS.Services.DataAccess
{
    /// <summary>
    /// SQL de los dígitos verificadores.
    /// - DVH (horizontal): columna "DVH" en cada fila de las tablas protegidas.
    ///   Esas tablas pueden estar en cualquiera de las dos bases, por eso se recibe
    ///   la cadena de conexión de la base donde está la tabla.
    /// - DVV (vertical): tabla "DigitoVerificador" de la base de SEGURIDAD (PRAS_Seguridad),
    ///   una fila por tabla protegida.
    /// Los nombres de tabla/columna NO vienen del usuario: salen de una lista fija
    /// en DigitoVerificadorLogic, por eso se pueden poner dentro del SQL.
    /// </summary>
    internal static class DigitoVerificadorDal
    {
        /// <summary>Todas las filas de la tabla, ordenadas por su Id.</summary>
        public static DataTable ObtenerRegistros(string cadenaConexion, string tabla, string columnaId)
        {
            using (SqlDataReader reader = SqlHelper.ExecuteReader(cadenaConexion,
                $"SELECT * FROM [{tabla}] ORDER BY [{columnaId}]", CommandType.Text))
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }

        /// <summary>Una sola fila (por su Id).</summary>
        public static DataTable ObtenerRegistro(string cadenaConexion, string tabla, string columnaId, int id)
        {
            using (SqlDataReader reader = SqlHelper.ExecuteReader(cadenaConexion,
                $"SELECT * FROM [{tabla}] WHERE [{columnaId}] = @Id", CommandType.Text,
                new SqlParameter("@Id", id)))
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }

        public static void ActualizarDVH(string cadenaConexion, string tabla, string columnaId, int id, string dvh)
        {
            SqlHelper.ExecuteNonQuery(cadenaConexion,
                $"UPDATE [{tabla}] SET DVH = @DVH WHERE [{columnaId}] = @Id", CommandType.Text,
                new SqlParameter("@DVH", dvh),
                new SqlParameter("@Id", id));
        }

        /// <summary>DVV guardado de la tabla (base de seguridad). Devuelve null si todavía no se calculó nunca.</summary>
        public static string ObtenerDVV(string tabla)
        {
            object dvv = SqlHelper.ExecuteScalar(
                "SELECT DVV FROM DigitoVerificador WHERE Tabla = @Tabla", CommandType.Text,
                new SqlParameter("@Tabla", tabla));

            return dvv == null || dvv == DBNull.Value ? null : (string)dvv;
        }

        /// <summary>Guarda el DVV (base de seguridad): si la tabla ya tiene fila la actualiza, si no la inserta.</summary>
        public static void GuardarDVV(string tabla, string dvv)
        {
            SqlHelper.ExecuteNonQuery(
                @"IF EXISTS (SELECT 1 FROM DigitoVerificador WHERE Tabla = @Tabla)
                      UPDATE DigitoVerificador SET DVV = @DVV WHERE Tabla = @Tabla
                  ELSE
                      INSERT INTO DigitoVerificador (Tabla, DVV) VALUES (@Tabla, @DVV)",
                CommandType.Text,
                new SqlParameter("@Tabla", tabla),
                new SqlParameter("@DVV", dvv));
        }
    }
}
