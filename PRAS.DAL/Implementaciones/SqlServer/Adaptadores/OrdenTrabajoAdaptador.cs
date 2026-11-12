using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de OrdenTrabajo (con JOIN a Vehiculo, monto del presupuesto y mecánicos) en un objeto OrdenTrabajo.
    /// </summary>
    internal sealed class OrdenTrabajoAdaptador : IAdaptador<OrdenTrabajo>
    {
        #region Singleton
        private static readonly OrdenTrabajoAdaptador _instance = new OrdenTrabajoAdaptador();

        public static OrdenTrabajoAdaptador Current
        {
            get { return _instance; }
        }

        private OrdenTrabajoAdaptador()
        {
        }
        #endregion

        public OrdenTrabajo Fill(DataRow fila)
        {
            return new OrdenTrabajo
            {
                IdOrdenTrabajo = (int)fila["IdOrdenTrabajo"],
                IdRecepcion = (int)fila["IdRecepcion"],
                IdPresupuesto = (int)fila["IdPresupuesto"],
                IdVehiculo = (int)fila["IdVehiculo"],
                IdTaller = (int)fila["IdTaller"],
                FechaCreacion = (DateTime)fila["FechaCreacion"],
                EstadoOrden = (string)fila["EstadoOrden"],
                Prioridad = (string)fila["Prioridad"],
                Observaciones = fila["Observaciones"] as string,
                MontoPresupuesto = (decimal)fila["MontoPresupuesto"],
                Mecanicos = (string)fila["Mecanicos"],
                Vehiculo = VehiculoAdaptador.Current.Fill(fila)
            };
        }
    }
}
