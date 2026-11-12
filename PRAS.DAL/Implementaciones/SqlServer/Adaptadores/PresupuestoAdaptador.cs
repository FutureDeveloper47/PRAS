using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de Presupuesto (con JOIN a Vehiculo) en un objeto Presupuesto.
    /// </summary>
    internal sealed class PresupuestoAdaptador : IAdaptador<Presupuesto>
    {
        #region Singleton
        private static readonly PresupuestoAdaptador _instance = new PresupuestoAdaptador();

        public static PresupuestoAdaptador Current
        {
            get { return _instance; }
        }

        private PresupuestoAdaptador()
        {
        }
        #endregion

        public Presupuesto Fill(DataRow fila)
        {
            Vehiculo vehiculo = VehiculoAdaptador.Current.Fill(fila);
            return new Presupuesto
            {
                IdPresupuesto = (int)fila["IdPresupuesto"],
                IdVehiculo = vehiculo.IdVehiculo,
                FechaEmision = (DateTime)fila["FechaEmision"],
                FechaVencimiento = (DateTime)fila["FechaVencimiento"],
                EstadoPresupuesto = (string)fila["EstadoPresupuesto"],
                MontoTotal = (decimal)fila["MontoTotal"],
                Observaciones = fila["Observaciones"] as string,
                Vehiculo = vehiculo
            };
        }
    }
}
