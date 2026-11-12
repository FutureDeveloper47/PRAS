using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Chequeos generales de la base de datos (que haya conexión con las dos bases).</summary>
    public interface ISistemaRepository
    {
        bool ProbarConexion(out string error);

        bool ProbarConexionSeguridad(out string error);

        /// <summary>true si ya existen las dos bases (PRAS y PRAS_Seguridad).</summary>
        bool ExistenLasBases();

        /// <summary>Crea las dos bases ejecutando el script de instalación completo.</summary>
        void InstalarBases(string rutaScript);
    }
}
