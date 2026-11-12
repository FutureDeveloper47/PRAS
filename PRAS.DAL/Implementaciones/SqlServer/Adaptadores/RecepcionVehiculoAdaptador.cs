using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la tabla RecepcionVehiculo en un objeto RecepcionVehiculo.
    /// </summary>
    internal sealed class RecepcionVehiculoAdaptador : IAdaptador<RecepcionVehiculo>
    {
        #region Singleton
        private static readonly RecepcionVehiculoAdaptador _instance = new RecepcionVehiculoAdaptador();

        public static RecepcionVehiculoAdaptador Current
        {
            get { return _instance; }
        }

        private RecepcionVehiculoAdaptador()
        {
        }
        #endregion

        public RecepcionVehiculo Fill(DataRow fila)
        {
            return new RecepcionVehiculo
            {
                IdRecepcion = (int)fila["IdRecepcion"],
                IdVehiculo = (int)fila["IdVehiculo"],
                FechaIngreso = (DateTime)fila["FechaIngreso"],
                MotivoIngreso = (string)fila["MotivoIngreso"],
                EstadoRecepcion = (string)fila["EstadoRecepcion"],
                Observaciones = fila["Observaciones"] as string
            };
        }
    }
}
