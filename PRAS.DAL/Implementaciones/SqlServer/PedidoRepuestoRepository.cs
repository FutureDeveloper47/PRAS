using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla PedidoRepuesto (singleton).
    /// Regla del negocio: no tenemos stock; los repuestos se piden al proveedor para una orden de trabajo.
    /// Los renglones del pedido están en DetallePedidoRepuestoRepository.
    /// </summary>
    internal sealed class PedidoRepuestoRepository : IPedidoRepuestoRepository
    {
        #region Singleton
        private static readonly PedidoRepuestoRepository _instance = new PedidoRepuestoRepository();

        public static PedidoRepuestoRepository Current
        {
            get { return _instance; }
        }

        private PedidoRepuestoRepository()
        {
        }
        #endregion

        #region Statements
        /// <summary>Pedido + razón social del proveedor + patente del auto de la orden.</summary>
        private const string SelectBase =
            @"SELECT PR.IdPedido, PR.IdProveedor, PR.IdOrdenTrabajo, PR.FechaPedido, PR.FechaEntrega,
                     PR.EstadoPedido, PR.MontoTotal,
                     P.RazonSocial AS ProveedorRazonSocial, V.Dominio
              FROM PedidoRepuesto PR
              INNER JOIN Proveedor P    ON P.IdProveedor = PR.IdProveedor
              INNER JOIN OrdenTrabajo O ON O.IdOrdenTrabajo = PR.IdOrdenTrabajo
              INNER JOIN Vehiculo V     ON V.IdVehiculo = O.IdVehiculo";

        private string SelectOneStatement
        {
            get => $"{SelectBase} WHERE PR.IdPedido = @IdPedido";
        }

        private string SelectFiltroStatement
        {
            get => $@"{SelectBase}
                      WHERE (@Estado IS NULL OR PR.EstadoPedido = @Estado)
                      ORDER BY PR.FechaPedido DESC, PR.IdPedido DESC";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO PedidoRepuesto (IdProveedor, IdOrdenTrabajo, FechaPedido, EstadoPedido, MontoTotal)
                     VALUES (@IdProveedor, @IdOrden, @Fecha, @Estado, 0);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string RecalcularTotalStatement
        {
            get => @"UPDATE PedidoRepuesto
                     SET MontoTotal = (SELECT ISNULL(SUM(Subtotal), 0) FROM DetallePedidoRepuesto WHERE IdPedido = @IdPedido)
                     WHERE IdPedido = @IdPedido";
        }

        private string MarcarEntregadoStatement
        {
            get => @"UPDATE PedidoRepuesto SET EstadoPedido = @Entregado, FechaEntrega = @Fecha
                     WHERE IdPedido = @IdPedido AND EstadoPedido = @Pendiente";
        }

        private string IdPresupuestoDelPedidoStatement
        {
            get => @"SELECT O.IdPresupuesto
                     FROM PedidoRepuesto PR
                     INNER JOIN OrdenTrabajo O ON O.IdOrdenTrabajo = PR.IdOrdenTrabajo
                     WHERE PR.IdPedido = @IdPedido";
        }
        #endregion

        public PedidoRepuesto GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdPedido", id));
            PedidoRepuesto pedido = PedidoRepuestoAdaptador.Current.FillPrimero(tabla);
            if (pedido != null)
                pedido.Detalles = DetallePedidoRepuestoRepository.Current.ListarPorPedido(id);
            return pedido;
        }

        public List<PedidoRepuesto> Listar(string estado)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectFiltroStatement, SqlHelper.Param("@Estado", estado));
            return PedidoRepuestoAdaptador.Current.FillLista(tabla);
        }

        /// <summary>
        /// Guarda el pedido (Pendiente) y sus renglones en una transacción. El total lo calcula SQL Server.
        /// Le carga al pedido el Id generado.
        /// </summary>
        public void Add(PedidoRepuesto entity)
        {
            int idPedido = 0;

            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                idPedido = (int)SqlHelper.ExecuteScalar(cn, tx, InsertStatement,
                    SqlHelper.Param("@IdProveedor", entity.IdProveedor),
                    SqlHelper.Param("@IdOrden", entity.IdOrdenTrabajo),
                    SqlHelper.Param("@Fecha", entity.FechaPedido),
                    SqlHelper.Param("@Estado", Estados.PedidoPendiente));

                foreach (DetallePedidoRepuesto detalle in entity.Detalles)
                {
                    detalle.IdPedido = idPedido;
                    detalle.IdDetallePedido = DetallePedidoRepuestoRepository.Current.Insertar(detalle, cn, tx);
                }

                SqlHelper.ExecuteNonQuery(cn, tx, RecalcularTotalStatement, SqlHelper.Param("@IdPedido", idPedido));
            });

            entity.IdPedido = idPedido;
            entity.EstadoPedido = Estados.PedidoPendiente;
        }

        /// <summary>
        /// El proveedor entregó el pedido. En la misma transacción:
        /// 1) el pedido pasa a Entregado con la fecha de entrega;
        /// 2) cada repuesto se suma como gasto al presupuesto de la orden de trabajo;
        /// 3) se recalcula el total del presupuesto.
        /// Devuelve false si el pedido ya no estaba pendiente.
        /// </summary>
        public bool RegistrarEntrega(int idPedido, DateTime fechaEntrega)
        {
            bool registrado = false;

            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                int filas = SqlHelper.ExecuteNonQuery(cn, tx, MarcarEntregadoStatement,
                    SqlHelper.Param("@Entregado", Estados.PedidoEntregado),
                    SqlHelper.Param("@Fecha", fechaEntrega),
                    SqlHelper.Param("@IdPedido", idPedido),
                    SqlHelper.Param("@Pendiente", Estados.PedidoPendiente));

                if (filas != 1)
                    return;   // ya estaba entregado, no tocamos nada

                DetallePresupuestoRepository.Current.InsertarRepuestosDePedido(idPedido, cn, tx);

                int idPresupuesto = (int)SqlHelper.ExecuteScalar(cn, tx, IdPresupuestoDelPedidoStatement,
                    SqlHelper.Param("@IdPedido", idPedido));
                PresupuestoRepository.Current.RecalcularTotal(idPresupuesto, cn, tx);

                registrado = true;
            });

            return registrado;
        }
    }
}
