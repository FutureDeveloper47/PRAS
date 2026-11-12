using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la tabla DetallePedidoRepuesto en un objeto DetallePedidoRepuesto.
    /// </summary>
    internal sealed class DetallePedidoRepuestoAdaptador : IAdaptador<DetallePedidoRepuesto>
    {
        #region Singleton
        private static readonly DetallePedidoRepuestoAdaptador _instance = new DetallePedidoRepuestoAdaptador();

        public static DetallePedidoRepuestoAdaptador Current
        {
            get { return _instance; }
        }

        private DetallePedidoRepuestoAdaptador()
        {
        }
        #endregion

        public DetallePedidoRepuesto Fill(DataRow fila)
        {
            return new DetallePedidoRepuesto
            {
                IdDetallePedido = (int)fila["IdDetallePedido"],
                IdPedido = (int)fila["IdPedido"],
                DescripcionRepuesto = (string)fila["DescripcionRepuesto"],
                Cantidad = (int)fila["Cantidad"],
                PrecioUnitario = (decimal)fila["PrecioUnitario"],
                Subtotal = (decimal)fila["Subtotal"]
            };
        }
    }
}
