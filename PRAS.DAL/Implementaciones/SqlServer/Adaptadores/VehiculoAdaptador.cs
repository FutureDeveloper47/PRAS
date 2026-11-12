using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la tabla Vehiculo en un objeto Vehiculo.
    /// </summary>
    internal sealed class VehiculoAdaptador : IAdaptador<Vehiculo>
    {
        #region Singleton
        private static readonly VehiculoAdaptador _instance = new VehiculoAdaptador();

        public static VehiculoAdaptador Current
        {
            get { return _instance; }
        }

        private VehiculoAdaptador()
        {
        }
        #endregion

        public Vehiculo Fill(DataRow fila)
        {
            return new Vehiculo
            {
                IdVehiculo = (int)fila["IdVehiculo"],
                Dominio = (string)fila["Dominio"],
                Marca = (string)fila["Marca"],
                Modelo = (string)fila["Modelo"],
                Anio = (int)fila["Anio"],
                Color = fila["Color"] as string,
                NumeroChasis = fila["NumeroChasis"] as string,
                NumeroMotor = fila["NumeroMotor"] as string,
                EstadoVehiculo = (EstadoVehiculo)Convert.ToInt32(fila["EstadoVehiculo"])   // en la base es TINYINT
            };
        }
    }
}
