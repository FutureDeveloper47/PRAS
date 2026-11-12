using PRAS.Services.Facade;
using PRAS.BLL.Excepciones;

namespace PRAS.BLL
{
    /// <summary>
    /// Casos de uso Seguridad - Realizar Backup y Restaurar Backup (solo Administrador).
    /// El backup incluye LAS DOS bases (PRAS y PRAS_Seguridad) en un solo archivo .bak.
    /// El trabajo pesado lo hace BackupService (proyecto PRAS.Services);
    /// acá se controla el permiso, se valida y se deja todo anotado en la bitácora.
    /// </summary>
    public class RespaldoService
    {
        public string CarpetaBackups()
        {
            return BackupService.CarpetaBackups();
        }

        public string NombreSugerido()
        {
            return BackupService.NombreSugerido();
        }

        public void RealizarBackup(string rutaArchivo)
        {
            Sesion.ExigirPermiso(Funcion.RealizarBackup);

            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new SeleccionObligatoriaException("dónde guardar el backup");

            try
            {
                BackupService.RealizarBackup(rutaArchivo);
                LoggerService.Info($"Realizó un backup de las bases PRAS y PRAS_Seguridad en \"{rutaArchivo}\".");
            }
            catch (Exception ex)
            {
                LoggerService.Error("Falló el backup de la base.", ex);
                throw;
            }
        }

        public void RealizarRestore(string rutaArchivo)
        {
            Sesion.ExigirPermiso(Funcion.RealizarBackup);

            if (string.IsNullOrWhiteSpace(rutaArchivo) || !File.Exists(rutaArchivo))
                throw new ArchivoInvalidoException("Seleccione un archivo de backup (.bak) existente.");

            // Ojo: la bitácora está en PRAS_Seguridad, así que el restore también la vuelve para atrás.
            // Por eso lo que importa es el registro que se hace después del restore.
            LoggerService.Warn($"Inició la restauración de las bases desde \"{rutaArchivo}\".");

            try
            {
                BackupService.RealizarRestore(rutaArchivo);
                LoggerService.Warn("Las bases PRAS y PRAS_Seguridad fueron restauradas correctamente. Se cierra la sesión.");
            }
            catch (Exception ex)
            {
                LoggerService.Error("Falló la restauración de la base.", ex);
                throw;
            }
        }
    }
}
