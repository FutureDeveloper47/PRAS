using PRAS.DAL.Factory;
using PRAS.BLL.Excepciones;
using PRAS.Services.Facade;

namespace PRAS.BLL
{
    /// <summary>
    /// Servicios generales del sistema. La UI nunca habla directo con la DAL:
    /// siempre pasa por la BLL.
    /// </summary>
    public class SistemaService
    {
        /// <summary>
        /// Verifica que se pueda conectar a LAS DOS bases:
        /// PRAS (negocio) y PRAS_Seguridad (usuarios, dígitos verificadores y bitácora).
        /// </summary>
        public bool ProbarConexion(out string mensaje)
        {
            if (!FactoryDataAccess.SistemaRepository.ProbarConexion(out string error))
            {
                mensaje = "No se pudo conectar con la base de datos PRAS:\n\n" + error;
                return false;
            }

            if (!FactoryDataAccess.SistemaRepository.ProbarConexionSeguridad(out string errorSeguridad))
            {
                mensaje = "No se pudo conectar con la base de datos de seguridad PRAS_Seguridad:\n\n" + errorSeguridad +
                          "\n\nSi todavía no existe, ejecutá el script BaseDeDatos\\PRAS_InstalacionCompleta.sql en SQL Server Management Studio.";
                return false;
            }

            mensaje = "Conexión exitosa con las bases de datos PRAS y PRAS_Seguridad.";
            return true;
        }

        /// <summary>
        /// Si la base se instaló con una versión anterior del sistema, le agrega las tablas nuevas
        /// (UsuarioRecuperacion, para la pregunta de seguridad). Si ya están, no hace nada.
        /// </summary>
        public void ActualizarEstructura()
        {
            FactoryDataAccess.SistemaRepository.ActualizarEstructura();
        }

        /// <summary>
        /// true si las dos bases ya existen. Si SQL Server LocalDB no está instalado,
        /// tira la excepción de conexión (la pantalla avisa que hay que instalarlo).
        /// </summary>
        public bool BasesInstaladas()
        {
            return FactoryDataAccess.SistemaRepository.ExistenLasBases();
        }

        /// <summary>
        /// Primera vez en una PC: crea las bases PRAS y PRAS_Seguridad con el script que el
        /// instalador copia en la carpeta del programa (BaseDeDatos\PRAS_InstalacionCompleta.sql).
        /// </summary>
        public void InstalarBases()
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, "BaseDeDatos", "PRAS_InstalacionCompleta.sql");
            if (!File.Exists(ruta))
                throw new ArchivoInvalidoException("No se encontró el script de instalación de la base: " + ruta);

            FactoryDataAccess.SistemaRepository.InstalarBases(ruta);
            LoggerService.Info("Primera ejecución: se crearon las bases PRAS y PRAS_Seguridad.");
        }
    }
}
