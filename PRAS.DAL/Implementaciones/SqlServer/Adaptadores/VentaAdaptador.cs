using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de Venta (con JOIN a Consignacion, Agencia, Vehiculo y, si viene, Cliente) en un objeto Venta.
    /// </summary>
    internal sealed class VentaAdaptador : IAdaptador<Venta>
    {
        #region Singleton
        private static readonly VentaAdaptador _instance = new VentaAdaptador();

        public static VentaAdaptador Current
        {
            get { return _instance; }
        }

        private VentaAdaptador()
        {
        }
        #endregion

        public Venta Fill(DataRow fila)
        {
            var venta = new Venta
            {
                IdVenta = (int)fila["IdVenta"],
                IdConsignacion = (int)fila["IdConsignacion"],
                IdCliente = (int)fila["IdCliente"],
                FechaVenta = (DateTime)fila["FechaVenta"],
                PrecioVenta = (decimal)fila["PrecioVenta"],
                MontoComision = (decimal)fila["MontoComision"],
                FormaPago = (string)fila["FormaPago"],
                // De la consignación traemos solo lo que se muestra.
                Consignacion = new Consignacion
                {
                    IdConsignacion = (int)fila["IdConsignacion"],
                    IdVehiculo = (int)fila["IdVehiculo"],
                    IdAgencia = (int)fila["IdAgencia"],
                    Vehiculo = VehiculoAdaptador.Current.Fill(fila),
                    Agencia = AgenciaAdaptador.Current.FillResumen(fila)
                }
            };

            if (fila.Table.Columns.Contains("PrecioMinimoVenta"))
            {
                venta.Consignacion.PrecioMinimoVenta = (decimal)fila["PrecioMinimoVenta"];
                venta.Consignacion.PorcentajeComision = (decimal)fila["PorcentajeComision"];
            }

            // En el historial del cliente no traemos al cliente (ya sabemos cuál es).
            if (fila.Table.Columns.Contains("ClienteNombre"))
                venta.Cliente = ClienteAdaptador.Current.FillResumen(fila);

            return venta;
        }
    }
}
