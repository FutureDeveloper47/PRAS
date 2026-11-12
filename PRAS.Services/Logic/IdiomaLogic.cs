using System.Globalization;
using PRAS.Services.DataAccess;
using PRAS.Services.Logic.CustomExceptions;

namespace PRAS.Services.Logic
{
    /// <summary>
    /// Lógica del multi idioma.
    /// El idioma "actual" es el del hilo: Thread.CurrentThread.CurrentCulture.
    /// </summary>
    internal static class IdiomaLogic
    {
        /// <summary>Idioma con el que arranca el sistema.</summary>
        public const string IdiomaPorDefecto = "es-AR";

        public static string Traducir(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return texto;
            }

            try
            {
                string traduccion = IdiomaDal.Traducir(texto);

                // Si la clave está pero sin traducción ("palabra:"), mostramos el original.
                return traduccion == "" ? texto : traduccion;
            }
            catch (PalabraNoEncontradaException ex)
            {
                // ¿Qué podemos hacer?
                // 1) Agregar la clave en el idioma actual, vacía
                // 2) Agregarla en el idioma padre o en los idiomas que no la tengan
                // 3) No hacer nada
                // 4) Pedirle la traducción a un modelo de IA y agregarla como en el paso 1 o 2

                // Hacemos la opción 1:
                // la palabra queda anotada en el archivo para traducirla después.
                IdiomaDal.AgregarPalabra(ex.Palabra.Trim());

                return texto;   // si no se pudo traducir, devolvemos la misma palabra
            }
            catch (FileNotFoundException)
            {
                // No está el archivo del idioma actual, mostramos el texto original.
                return texto;
            }
            catch (DirectoryNotFoundException)
            {
                return texto;
            }
        }

        public static List<CultureInfo> ObtenerIdiomas()
        {
            return IdiomaDal.ObtenerIdiomas();
        }

        /// <summary>
        /// Cambia el idioma del hilo actual (y de los que se creen después).
        /// Si no existe el archivo de ese idioma, queda el idioma por defecto.
        /// </summary>
        public static void CambiarIdioma(string cultura)
        {
            if (string.IsNullOrWhiteSpace(cultura) || !IdiomaDal.ExisteIdioma(cultura))
            {
                cultura = IdiomaPorDefecto;
            }

            CultureInfo idioma = new CultureInfo(cultura);

            Thread.CurrentThread.CurrentCulture = idioma;
            Thread.CurrentThread.CurrentUICulture = idioma;
            CultureInfo.DefaultThreadCurrentCulture = idioma;
            CultureInfo.DefaultThreadCurrentUICulture = idioma;
        }

        public static CultureInfo IdiomaActual()
        {
            return Thread.CurrentThread.CurrentCulture;
        }
    }
}
