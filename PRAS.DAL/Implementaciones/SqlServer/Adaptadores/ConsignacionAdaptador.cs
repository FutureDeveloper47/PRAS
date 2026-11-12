using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de Consignacion (con JOIN a Agencia y Vehiculo) en un objeto Consignacion.
    /// </summary>
    internal sealed class ConsignacionAdaptador : IAdaptador<Consignacion>
    {
        #region Singleton
        private static readonly ConsignacionAdaptador _instance = new ConsignacionAdaptador();

        public static ConsignacionAdaptador Current
        {
            get { return _instance; }
        }

        private ConsignacionAdaptador()
        {
        }
        #endregion

        public Consignacion Fill(DataRow fila)
        {
            return new Consignacion
            {
                IdConsignacion = (int)fila["IdConsignacion"],
                IdVehiculo = (int)fila["IdVehiculo"],
                IdAgencia = (int)fila["IdAgencia"],
                FechaEntrega = (DateTime)fila["FechaEntrega"],
                FechaRetiro = fila["FechaRetiro"] as DateTime?,
                PrecioMinimoVenta = (decimal)fila["PrecioMinimoVenta"],
                PorcentajeComision = (decimal)fila["PorcentajeComision"],
                EstadoConsignacion = (string)fila["EstadoConsignacion"],
                Vehiculo = VehiculoAdaptador.Current.Fill(fila),
                Agencia = AgenciaAdaptador.Current.FillResumen(fila)
            };
        }
    }
}
