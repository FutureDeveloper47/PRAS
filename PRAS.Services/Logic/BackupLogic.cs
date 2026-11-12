using System.Configuration;
using PRAS.Services.DataAccess;

namespace PRAS.Services.Logic
{
    /// <summary>
    /// Lógica de Backup y Restore. La carpeta por defecto sale del App.config ("BackupFolder").
    /// </summary>
    internal static class BackupLogic
    {
        /// <summary>Carpeta donde se proponen los backups. Si no existe, se crea.</summary>
        public static string CarpetaBackups()
        {
            string carpeta = ConfigurationManager.AppSettings["BackupFolder"] ?? "Backups";

            if (!Path.IsPathRooted(carpeta))
            {
                carpeta = Path.Combine(AppContext.BaseDirectory, carpeta);
            }

            Directory.CreateDirectory(carpeta);
            return carpeta;
        }

        /// <summary>Nombre sugerido: PRAS_2026-10-14_11-30.bak</summary>
        public static string NombreSugerido()
        {
            return $"{BackupDal.NombreBase()}_{DateTime.Now:yyyy-MM-dd_HH-mm}.bak";
        }

        public static void RealizarBackup(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                throw new ArgumentException("Indique dónde guardar el backup.");
            }

            string carpeta = Path.GetDirectoryName(rutaArchivo);
            if (!string.IsNullOrEmpty(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            BackupDal.RealizarBackup(rutaArchivo);
        }

        public static void RealizarRestore(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException("No se encontró el archivo de backup.", rutaArchivo);
            }

            BackupDal.RealizarRestore(rutaArchivo);
        }
    }
}
