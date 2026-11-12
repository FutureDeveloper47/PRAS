using System.Configuration;
using System.Globalization;
using PRAS.Services.Logic.CustomExceptions;

namespace PRAS.Services.DataAccess
{
    /// <summary>
    /// Lee los archivos de idioma. Hay un archivo por idioma:
    ///   I18n\idioma.es-AR   I18n\idioma.en-US   ...
    /// Cada línea es  clave:traducción   (la clave es el texto original en español).
    /// Para agregar un idioma nuevo alcanza con copiar un archivo y traducir la parte derecha.
    /// </summary>
    internal static class IdiomaDal
    {
        private static string folder = ConfigurationManager.AppSettings["I18NFolder"] ?? "I18n";

        private static string fileName = ConfigurationManager.AppSettings["I18NFileName"] ?? "idioma";

        /// <summary>Carpeta de los idiomas. Si es relativa, se toma desde donde está el .exe.</summary>
        private static string Carpeta
        {
            get { return Path.IsPathRooted(folder) ? folder : Path.Combine(AppContext.BaseDirectory, folder); }
        }

        /// <summary>Archivo del idioma actual del hilo. EJ: I18n\idioma.en-US</summary>
        private static string ArchivoIdiomaActual
        {
            get { return Path.Combine(Carpeta, fileName + "." + Thread.CurrentThread.CurrentCulture.Name); }
        }

        /// <summary>
        /// Busca la traducción del texto en el archivo del idioma actual (Thread.CurrentThread.CurrentCulture).
        /// Si no la encuentra, lanza PalabraNoEncontradaException.
        /// </summary>
        public static string Traducir(string texto)
        {
            using (StreamReader sr = new StreamReader(ArchivoIdiomaActual))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(':');

                    if (parts.Length == 2 && parts[0].Trim().ToLower() == texto.Trim().ToLower())
                    {
                        // Si llegamos acá es porque encontramos la clave.
                        return parts[1].Trim();
                    }
                }
            }
            throw new PalabraNoEncontradaException(texto);
        }

        /// <summary>Un idioma por cada archivo de la carpeta I18n (la extensión es el nombre de la cultura).</summary>
        public static List<CultureInfo> ObtenerIdiomas()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Carpeta);

            List<CultureInfo> idiomas = new List<CultureInfo>();

            foreach (FileInfo fo in directoryInfo.GetFiles(fileName + ".*"))
            {
                idiomas.Add(new CultureInfo(fo.Extension.Replace(".", "")));
            }

            return idiomas;
        }

        public static bool ExisteIdioma(string cultura)
        {
            return File.Exists(Path.Combine(Carpeta, fileName + "." + cultura));
        }

        public static void AgregarPalabra(string palabra, string traduccion = "")
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ArchivoIdiomaActual, true))   // Append agrega al final del archivo
                {
                    sw.WriteLine($"{palabra}:{traduccion}");
                }
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                // Si el programa está en una carpeta protegida (por ejemplo Archivos de programa)
                // no podemos escribir el archivo, así que la palabra queda sin anotar.
            }
        }
    }
}
