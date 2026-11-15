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

        /// <summary>
        /// Traduce un mensaje con datos (avisos, errores, bitácora). Si no lo encuentra,
        /// lo devuelve igual y no lo anota en el archivo de idioma.
        /// </summary>
        public static string TraducirMensaje(string mensaje)
        {
            return IdiomaLogic.TraducirMensaje(mensaje);
        }

        /// <summary>
        /// Traduce un valor guardado en español (estado, prioridad, forma de pago...) solo para mostrarlo.
        /// Si no es una clave del archivo de idioma (un nombre, una patente), lo devuelve igual.
        /// </summary>
        public static string TraducirValor(string valor)
        {
            return IdiomaLogic.TraducirValor(valor);
        }
    }
}
