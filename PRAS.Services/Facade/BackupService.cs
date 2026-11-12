using PRAS.Services.Logic;

namespace PRAS.Services.Facade
{
    /// <summary>FACHADA de Backup y Restore de la base de datos.</summary>
    public static class BackupService
    {
        public static string CarpetaBackups()
        {
            return BackupLogic.CarpetaBackups();
        }

        public static string NombreSugerido()
        {
            return BackupLogic.NombreSugerido();
        }

        public static void RealizarBackup(string rutaArchivo)
        {
            BackupLogic.RealizarBackup(rutaArchivo);
        }

        public static void RealizarRestore(string rutaArchivo)
        {
            BackupLogic.RealizarRestore(rutaArchivo);
        }
    }
}
