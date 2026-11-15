using PRAS.Services.DomainModel;
using PRAS.Services.Logic;

namespace PRAS.Services.Facade
{
    /// <summary>
    /// FACHADA de los dígitos verificadores (DVH y DVV).
    /// Uso desde la BLL, después de guardar un cliente:
    ///     DigitoVerificadorService.RecalcularRegistro("Cliente", cliente.IdCliente);
    /// </summary>
    public static class DigitoVerificadorService
    {
        public static void RecalcularRegistro(string tabla, int id)
        {
            DigitoVerificadorLogic.RecalcularRegistro(tabla, id);
        }

        public static void RecalcularTodo()
        {
            DigitoVerificadorLogic.RecalcularTodo();
        }

        /// <summary>Lista de problemas. Vacía = base íntegra.</summary>
        public static List<string> VerificarIntegridad()
        {
            return DigitoVerificadorLogic.VerificarIntegridad();
        }

        /// <summary>Igual que la anterior, pero también devuelve el resultado de cada tabla.</summary>
        public static List<string> VerificarIntegridad(List<ResultadoIntegridadTabla> detalle)
        {
            return DigitoVerificadorLogic.VerificarIntegridad(detalle);
        }

        public static List<string> TablasProtegidas()
        {
            return DigitoVerificadorLogic.TablasProtegidas();
        }
    }
}
