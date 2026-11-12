using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de Compra (con JOIN a Vehiculo y, si viene, a Cliente) en un objeto Compra.
    /// </summary>
    internal sealed class CompraAdaptador : IAdaptador<Compra>
    {
        #region Singleton
        private static readonly CompraAdaptador _instance = new CompraAdaptador();

        public static CompraAdaptador Current
        {
            get { return _instance; }
        }

        private CompraAdaptador()
        {
        }
        #endregion

        public Compra Fill(DataRow fila)
        {
            var compra = new Compra
            {
                IdCompra = (int)fila["IdCompra"],
                IdVehiculo = (int)fila["IdVehiculo"],
                IdCliente = (int)fila["IdCliente"],
                FechaCompra = (DateTime)fila["FechaCompra"],
                PrecioCompra = (decimal)fila["PrecioCompra"],
                FormaPago = (string)fila["FormaPago"],
                Observaciones = fila["Observaciones"] as string,
                Vehiculo = VehiculoAdaptador.Current.Fill(fila)
            };

            // En el historial del cliente no traemos al cliente (ya sabemos cuál es).
            if (fila.Table.Columns.Contains("ClienteNombre"))
                compra.Cliente = ClienteAdaptador.Current.FillResumen(fila);

            return compra;
        }
    }
}
