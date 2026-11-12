using System.Globalization;
using PRAS.Services.Logic;

namespace PRAS.Services.Facade
{
    /// <summary>FACHADA del multi idioma.</summary>
    public static class IdiomaService
    {
        public static List<CultureInfo> ObtenerIdiomas()
        {
            return IdiomaLogic.ObtenerIdiomas();
        }

        /// <summary>Cambia el idioma de todo el sistema. Ej: CambiarIdioma("en-US").</summary>
        public static void CambiarIdioma(string cultura)
        {
            IdiomaLogic.CambiarIdioma(cultura);
        }

        public static CultureInfo IdiomaActual()
        {
            return IdiomaLogic.IdiomaActual();
        }

        public static string Traducir(string texto)
        {
            return IdiomaLogic.Traducir(texto);
        }
    }
}
