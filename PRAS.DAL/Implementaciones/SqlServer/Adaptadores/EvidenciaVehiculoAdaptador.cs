using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la tabla EvidenciaVehiculo en un objeto EvidenciaVehiculo.
    /// </summary>
    internal sealed class EvidenciaVehiculoAdaptador : IAdaptador<EvidenciaVehiculo>
    {
        #region Singleton
        private static readonly EvidenciaVehiculoAdaptador _instance = new EvidenciaVehiculoAdaptador();

        public static EvidenciaVehiculoAdaptador Current
        {
            get { return _instance; }
        }

        private EvidenciaVehiculoAdaptador()
        {
        }
        #endregion

        public EvidenciaVehiculo Fill(DataRow fila)
        {
            return new EvidenciaVehiculo
            {
                IdEvidencia = (int)fila["IdEvidencia"],
                IdDiagnostico = (int)fila["IdDiagnostico"],
                IdVehiculo = (int)fila["IdVehiculo"],
                Descripcion = fila["Descripcion"] as string,
                TipoEvidencia = (string)fila["TipoEvidencia"],
                UbicacionArchivo = (string)fila["UbicacionArchivo"],
                FechaCarga = (DateTime)fila["FechaCarga"]
            };
        }
    }
}
