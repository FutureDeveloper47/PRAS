using System.Globalization;
using System.Text;

namespace PRAS.Services.DomainModel
{
    /// <summary>
    /// Un registro de la bitácora. Tiene dos datos extra:
    /// - Usuario: quién estaba logueado cuando pasó (una bitácora sin usuario sirve poco).
    /// - ExceptionType / ExceptionMessage: se completan al LEER el archivo, para poder
    ///   mostrar el error en la grilla de la pantalla Bitácora.
    /// </summary>
    public class LogEntry
    {
        public DateTime TimeStamp { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Usuario { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public object[] Arguments { get; set; }

        // Se usan al leer la bitácora (en el archivo el error quedó como texto).
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }

        /// <summary>Texto corto del error para mostrar en la grilla (vacío si no hubo excepción).</summary>
        public string Detalle
        {
            get
            {
                if (string.IsNullOrEmpty(ExceptionType))
                    return "";
                return $"{ExceptionType}: {ExceptionMessage}";
            }
        }

        /// <summary>Separador entre un registro y el siguiente dentro del archivo.</summary>
        public static readonly string Separador = new string('-', 80);

        /// <summary>
        /// Así se escribe cada registro en el archivo de la bitácora.
        /// La fecha va con InvariantCulture para que se lea igual en cualquier idioma.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"[{TimeStamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}] [{LogLevel}]");

            if (!string.IsNullOrEmpty(Usuario))
            {
                sb.AppendLine($"User: {Usuario}");
            }

            if (!string.IsNullOrEmpty(ClassName) && !string.IsNullOrEmpty(MethodName))
            {
                sb.AppendLine($"Method: {ClassName}.{MethodName}");
            }

            sb.AppendLine($"Message: {Message}");

            if (Exception != null)
            {
                sb.AppendLine($"Exception Type: {Exception.GetType().Name}");
                sb.AppendLine($"Exception Message: {Exception.Message}");

                if (Arguments != null && Arguments.Length > 0)
                {
                    sb.AppendLine("Arguments:");
                    for (int i = 0; i < Arguments.Length; i++)
                    {
                        sb.AppendLine($"  [{i}]: {Arguments[i] ?? "null"}");
                    }
                }

                if (Exception.InnerException != null)
                {
                    sb.AppendLine($"InnerException: {Exception.InnerException.Message}");
                    sb.AppendLine($"InnerException StackTrace: {Exception.InnerException.StackTrace}");
                }

                sb.AppendLine($"StackTrace: {Exception.StackTrace}");
            }

            sb.AppendLine(Separador);

            return sb.ToString();
        }
    }
}
