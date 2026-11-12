using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de PedidoRepuesto (con JOIN a Proveedor y a la patente del auto) en un objeto PedidoRepuesto.
    /// </summary>
    internal sealed class PedidoRepuestoAdaptador : IAdaptador<PedidoRepuesto>
    {
        #region Singleton
        private static readonly PedidoRepuestoAdaptador _instance = new PedidoRepuestoAdaptador();

        public static PedidoRepuestoAdaptador Current
        {
            get { return _instance; }
        }

        private PedidoRepuestoAdaptador()
        {
        }
        #endregion

        public PedidoRepuesto Fill(DataRow fila)
        {
            return new PedidoRepuesto
            {
                IdPedido = (int)fila["IdPedido"],
                IdProveedor = (int)fila["IdProveedor"],
                IdOrdenTrabajo = (int)fila["IdOrdenTrabajo"],
                FechaPedido = (DateTime)fila["FechaPedido"],
                FechaEntrega = fila["FechaEntrega"] as DateTime?,
                EstadoPedido = (string)fila["EstadoPedido"],
                MontoTotal = (decimal)fila["MontoTotal"],
                DominioVehiculo = (string)fila["Dominio"],
                Proveedor = new Proveedor
                {
                    IdProveedor = (int)fila["IdProveedor"],
                    RazonSocial = (string)fila["ProveedorRazonSocial"]
                }
            };
        }
    }
}
