using System.Data;
using Microsoft.Data.SqlClient;
using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DataAccess.Tools;
using PRAS.Services.DomainModel;

namespace PRAS.Services.DataAccess.Implementations
{
    /// <summary>
    /// Guarda y lee la bitácora en la tabla Bitacora de la base de SEGURIDAD (PRAS_Seguridad).
    /// </summary>
    internal class SqlLoggerRepository : ILoggerRepository
    {
        public void WriteLog(LogEntry entry)
        {
            string metodo = null;
            if (!string.IsNullOrEmpty(entry.ClassName) && !string.IsNullOrEmpty(entry.MethodName))
            {
                metodo = $"{entry.ClassName}.{entry.MethodName}";
            }

            Exception ex = entry.Exception;

            SqlHelper.ExecuteNonQuery(
                @"INSERT INTO Bitacora (Fecha, Nivel, Usuario, Mensaje, Metodo, TipoExcepcion, MensajeExcepcion, StackTrace)
                  VALUES (@Fecha, @Nivel, @Usuario, @Mensaje, @Metodo, @TipoExcepcion, @MensajeExcepcion, @StackTrace)",
                CommandType.Text,
                new SqlParameter("@Fecha", entry.TimeStamp),
                new SqlParameter("@Nivel", entry.LogLevel.ToString()),
                new SqlParameter("@Usuario", (object)entry.Usuario),
                new SqlParameter("@Mensaje", entry.Message ?? ""),
                new SqlParameter("@Metodo", (object)metodo),
                new SqlParameter("@TipoExcepcion", (object)ex?.GetType().Name),
                new SqlParameter("@MensajeExcepcion", (object)ex?.Message),
                new SqlParameter("@StackTrace", (object)ex?.ToString()));   // ToString incluye la InnerException
        }

        public List<LogEntry> ReadLog(DateTime desde, DateTime hasta)
        {
            List<LogEntry> registros = new List<LogEntry>();

            // Con "menor a hasta + 1 día" entra todo el último día.
            using (SqlDataReader reader = SqlHelper.ExecuteReader(
                @"SELECT Fecha, Nivel, Usuario, Mensaje, Metodo, TipoExcepcion, MensajeExcepcion
                  FROM Bitacora
                  WHERE Fecha >= @Desde AND Fecha < @HastaMasUnDia
                  ORDER BY Fecha DESC, IdBitacora DESC",
                CommandType.Text,
                new SqlParameter("@Desde", desde.Date),
                new SqlParameter("@HastaMasUnDia", hasta.Date.AddDays(1))))
            {
                while (reader.Read())
                {
                    LogLevel nivel;
                    Enum.TryParse(reader["Nivel"].ToString(), out nivel);

                    registros.Add(new LogEntry
                    {
                        TimeStamp = (DateTime)reader["Fecha"],
                        LogLevel = nivel,
                        Usuario = reader["Usuario"] as string,
                        Message = reader["Mensaje"] as string,
                        ClassName = reader["Metodo"] as string,
                        ExceptionType = reader["TipoExcepcion"] as string,
                        ExceptionMessage = reader["MensajeExcepcion"] as string
                    });
                }
            }

            return registros;
        }
    }
}
