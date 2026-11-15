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

        /// <summary>
        /// Traduce un MENSAJE armado por el sistema (avisos, errores, bitácora, actividad reciente).
        /// A diferencia de Traducir, NO anota en el archivo las frases que no encuentra,
        /// porque un mensaje trae datos (una patente, un nombre) y llenaría el archivo de basura.
        ///
        /// Cómo traduce:
        /// 1) Si el mensaje completo es una clave del archivo, devuelve su traducción.
        /// 2) Si no, prueba con las claves que tienen "huecos" {0}, {1}...
        ///    Ej: la clave  "El mecánico {0} ya está deshabilitado."  coincide con
        ///    "El mecánico Juan Perez ya está deshabilitado." y {0} = "Juan Perez".
        ///    Después pone ese dato en el mismo hueco de la traducción.
        /// 3) Si es un mensaje de varias líneas, traduce cada línea por separado.
        /// Si no encuentra nada, devuelve el mensaje tal cual (en español).
        /// </summary>
        public static string TraducirMensaje(string mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
            {
                return mensaje;
            }

            Dictionary<string, string> diccionario;
            try
            {
                diccionario = IdiomaDal.ObtenerDiccionario();
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                return mensaje;
            }

            string[] lineas = mensaje.Replace("\r\n", "\n").Split('\n');
            for (int i = 0; i < lineas.Length; i++)
            {
                lineas[i] = TraducirLinea(lineas[i], diccionario);
            }

            return string.Join("\n", lineas);
        }

        private static string TraducirLinea(string linea, Dictionary<string, string> diccionario)
        {
            string limpia = linea.Trim();
            if (limpia == "")
            {
                return linea;
            }

            // 1) La frase completa es una clave.
            if (diccionario.TryGetValue(limpia, out string exacta) && exacta != "")
            {
                return exacta;
            }

            // 2) Las claves no pueden tener ":" (el ":" separa clave y traducción en el archivo).
            //    Si la frase tiene ": " la partimos en dos y traducimos cada parte.
            //    Ej: "El vehículo AB123CD está "Vendido": no se puede venderlo."
            //    Va ANTES de las plantillas: si no, una plantilla como "Ya existe un cliente con el DNI {0}"
            //    se "comería" todo el resto de la frase dentro del {0}.
            int dosPuntos = limpia.IndexOf(": ", StringComparison.Ordinal);
            if (dosPuntos > 0)
            {
                string antes = TraducirLinea(limpia.Substring(0, dosPuntos), diccionario);
                string despues = TraducirLinea(limpia.Substring(dosPuntos + 2), diccionario);
                return antes + ": " + despues;
            }

            // 3) Claves con huecos {0}, {1}... Probamos primero las más largas (las más específicas),
            //    así "Base {0}, tabla {1}, registro {2} = {3}" gana sobre "Base {0}, tabla {1}".
            IEnumerable<KeyValuePair<string, string>> plantillas = diccionario
                .Where(par => par.Value != "" && par.Key.Contains("{0}"))
                .OrderByDescending(par => par.Key.Length);

            foreach (KeyValuePair<string, string> par in plantillas)
            {
                // Convertimos la clave en un patrón: el texto fijo tiene que coincidir y cada {n} es "cualquier dato".
                string patron = "^" + System.Text.RegularExpressions.Regex.Escape(par.Key) + "$";
                patron = System.Text.RegularExpressions.Regex.Replace(patron, @"\\\{(\d+)}", "(?<p$1>.+?)");

                System.Text.RegularExpressions.Match coincidencia =
                    System.Text.RegularExpressions.Regex.Match(limpia, patron);
                if (!coincidencia.Success)
                {
                    continue;
                }

                string traduccion = par.Value;
                foreach (System.Text.RegularExpressions.Group grupo in coincidencia.Groups)
                {
                    if (grupo.Name.StartsWith("p"))
                    {
                        // El dato también se traduce si es una palabra del archivo
                        // (ej: el campo "Usuario" -> "User"); si es un nombre o un número queda igual.
                        string dato = diccionario.TryGetValue(grupo.Value.Trim(), out string datoTraducido) && datoTraducido != ""
                            ? datoTraducido
                            : grupo.Value;
                        traduccion = traduccion.Replace("{" + grupo.Name.Substring(1) + "}", dato);
                    }
                }

                return traduccion;
            }

            return linea;
        }

        /// <summary>
        /// Traduce un VALOR que viene de la base o de un enum (estado, prioridad, forma de pago, rol...).
        /// Solo lo cambia si el texto completo es una clave del archivo de idioma; si no (un nombre,
        /// una patente, una descripción escrita por el usuario) lo deja igual.
        /// A diferencia de Traducir, no anota nada en el archivo y no usa plantillas {0}:
        /// así nunca "adivina" una traducción para un dato.
        /// </summary>
        public static string TraducirValor(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return valor;
            }

            try
            {
                Dictionary<string, string> diccionario = IdiomaDal.ObtenerDiccionario();
                return diccionario.TryGetValue(valor.Trim(), out string traduccion) && traduccion != ""
                    ? traduccion
                    : valor;
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                return valor;
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
