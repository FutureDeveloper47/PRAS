using PRAS.Services.DomainModel;
using PRAS.Services.Facade;

namespace PRAS.BLL
{
    /// <summary>
    /// Caso de uso Seguridad - Verificar Integridad (dígitos verificadores DVH y DVV).
    /// Se verifica automáticamente al iniciar sesión y también desde el menú Seguridad.
    /// </summary>
    public class IntegridadService
    {
        /// <summary>
        /// Revisa las tablas protegidas. Devuelve la lista de problemas (vacía = todo bien).
        /// No pide permiso: se ejecuta en cada inicio de sesión, para cualquier rol.
        /// </summary>
        public List<string> Verificar()
        {
            List<string> errores = DigitoVerificadorService.VerificarIntegridad();

            foreach (string error in errores)
                LoggerService.Fatal("Integridad de datos: " + error);

            return errores;
        }

        /// <summary>
        /// Igual que Verificar(), pero también devuelve el resultado de cada tabla protegida
        /// (registros revisados, registros alterados y si el DVV coincide). Lo usa la pantalla
        /// "Integridad de datos" para mostrar qué se controló, no solo "está todo bien".
        /// </summary>
        public List<string> Verificar(List<ResultadoIntegridadTabla> detalle)
        {
            List<string> errores = DigitoVerificadorService.VerificarIntegridad(detalle);

            foreach (string error in errores)
                LoggerService.Fatal("Integridad de datos: " + error);

            LoggerService.Info(errores.Count == 0
                ? "Verificó la integridad de los datos: sin cambios externos."
                : $"Verificó la integridad de los datos: {errores.Count} problema(s) encontrado(s).");

            return errores;
        }

        /// <summary>Tablas que tienen dígitos verificadores (para mostrar en pantalla).</summary>
        public List<string> TablasProtegidas()
        {
            return DigitoVerificadorService.TablasProtegidas();
        }

        /// <summary>
        /// Vuelve a calcular todos los dígitos con los datos ACTUALES.
        /// Solo el Administrador, y solo si está seguro de que los cambios son correctos
        /// (si no, lo que corresponde es restaurar un backup).
        /// </summary>
        public void Recalcular()
        {
            Sesion.ExigirPermiso(Funcion.VerificarIntegridad);

            DigitoVerificadorService.RecalcularTodo();
            LoggerService.Warn("Recalculó los dígitos verificadores de todas las tablas protegidas.");
        }
    }
}
