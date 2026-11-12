using PRAS.Services.Exceptions.Base;

namespace PRAS.Services.Exceptions.DataAccess
{
    /// <summary>
    /// Error al acceder a la base de datos.
    /// La DAL atrapa la SqlException original y la "envuelve" en esta excepción,
    /// así las capas de arriba no dependen de SQL Server y el usuario ve un mensaje claro.
    /// </summary>
    public class DataAccessException : Exceptions.Base.ApplicationException
    {
        public DataAccessException(string message, string code = "DATA_ACCESS_ERROR")
            : base(message, code)
        {
        }

        public DataAccessException(string message, Exception innerException, string code = "DATA_ACCESS_ERROR")
            : base(message, innerException, code)
        {
        }

        public DataAccessException(Exception innerException, string code = "DATA_ACCESS_ERROR")
            : base(innerException.Message, innerException, code)
        {
        }
    }
}
