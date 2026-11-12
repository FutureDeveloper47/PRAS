using PRAS.Services.DomainModel;
using PRAS.Services.Exceptions.Base;
using PRAS.Services.Exceptions.DataAccess;
using PRAS.Services.Logic.Infrastructure.ExceptionManagement;

namespace PRAS.Services.Facade.ExtensionsMethods
{
    /// <summary>
    /// Política de excepciones del sistema.
    /// Decide qué hacer con cada tipo de excepción:
    ///   - BusinessRuleException -> es un aviso esperado: se registra como Warn.
    ///   - DataAccessException   -> la DAL ya la registró al envolverla; si viene sin registrar, Error.
    ///   - UIException           -> error de pantalla: se registra como Error.
    ///   - Cualquier otra        -> error no previsto: se registra como Fatal.
    /// Cada excepción se registra UNA sola vez: si la BLL ya le aplicó la política y después
    /// la pantalla se la vuelve a aplicar, la segunda vez no se repite en la bitácora.
    /// También arma el mensaje que la pantalla le muestra al usuario (MensajeParaUsuario).
    /// </summary>
    public static class ExceptionManager
    {
        public static void HandleException(Exception ex, object[] args = null)
        {
            if (ex == null || YaRegistrada(ex))
                return;

            if (ex is BusinessRuleException)
            {
                Registrar(ex, LogLevel.Warn, args);
            }
            else if (ex is DataAccessException)
            {
                // Si tiene InnerException es porque la DAL ya lo envolvió y lo registró.
                if (ex.InnerException == null)
                    Registrar(ex, LogLevel.Error, args);
            }
            else if (ex is UIException)
            {
                Registrar(ex, LogLevel.Error, args);
            }
            else
            {
                Registrar(ex, LogLevel.Fatal, args);
            }
        }

        /// <summary>
        /// Texto que se le muestra al usuario. Las excepciones propias ya traen un mensaje claro;
        /// las desconocidas muestran un mensaje genérico (el detalle técnico queda en la bitácora).
        /// </summary>
        public static string MensajeParaUsuario(Exception ex)
        {
            if (ex is Exceptions.Base.ApplicationException)
                return ex.Message;

            return "Ocurrió un error inesperado. El detalle quedó registrado en la bitácora.";
        }

        /// <summary>true si es un aviso de negocio (se muestra como advertencia, no como error).</summary>
        public static bool EsReglaDeNegocio(Exception ex)
        {
            return ex is BusinessRuleException;
        }

        private const string MarcaRegistrada = "PRAS.Registrada";

        private static bool YaRegistrada(Exception ex)
        {
            return ex.Data.Contains(MarcaRegistrada);
        }

        private static void Registrar(Exception ex, LogLevel nivel, object[] args)
        {
            try
            {
                ex.Data[MarcaRegistrada] = true;
                var origen = ex.TargetSite;
                ExceptionLogger.Log(new ExceptionContext
                {
                    Exception = ex,
                    ClassName = origen?.DeclaringType?.Name,
                    MethodName = origen?.Name,
                    LogLevel = nivel,
                    Arguments = args ?? Array.Empty<object>()
                });
            }
            catch
            {
                // Si falla al registrar, no queremos que eso tire otro error.
            }
        }
    }
}
