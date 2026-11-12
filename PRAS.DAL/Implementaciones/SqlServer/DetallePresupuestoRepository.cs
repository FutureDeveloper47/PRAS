using Microsoft.Data.SqlClient;
using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla DetallePresupuesto (singleton): cada renglón (gasto) de un presupuesto.
    /// Las altas siempre van dentro de una transacción, porque después hay que recalcular el total.
    /// </summary>
    internal sealed class DetallePresupuestoRepository : IDetallePresupuestoRepository
    {
        #region Singleton
        private static readonly DetallePresupuestoRepository _instance = new DetallePresupuestoRepository();

        public static DetallePresupuestoRepository Current
        {
            get { return _instance; }
        }

        private DetallePresupuestoRepository()
        {
        }
        #endregion

        #region Statements
        private const string Columnas =
            "IdDetallePresupuesto, IdPresupuesto, DescripcionItem, Cantidad, PrecioUnitario, Subtotal, Aprobado";

        private string SelectOneStatement
        {
            get => $"SELECT {Columnas} FROM DetallePresupuesto WHERE IdDetallePresupuesto = @Id";
        }

        private string SelectByPresupuestoStatement
        {
            get => $@"SELECT {Columnas} FROM DetallePresupuesto
                      WHERE IdPresupuesto = @IdPresupuesto
                      ORDER BY IdDetallePresupuesto";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO DetallePresupuesto (IdPresupuesto, DescripcionItem, Cantidad, PrecioUnitario, Subtotal, Aprobado)
                     VALUES (@IdPresupuesto, @Descripcion, @Cantidad, @Precio, @Subtotal, @Aprobado);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        /// <summary>Copia los repuestos de un pedido entregado al presupuesto de su orden de trabajo.</summary>
        private string InsertDesdePedidoStatement
        {
            get => @"INSERT INTO DetallePresupuesto (IdPresupuesto, DescripcionItem, Cantidad, PrecioUnitario, Subtotal, Aprobado)
                     SELECT O.IdPresupuesto, LEFT(N'Repuesto: ' + D.DescripcionRepuesto, 200),
                            D.Cantidad, D.PrecioUnitario, D.Subtotal, 1
                     FROM DetallePedidoRepuesto D
                     INNER JOIN PedidoRepuesto PR ON PR.IdPedido = D.IdPedido
                     INNER JOIN OrdenTrabajo O    ON O.IdOrdenTrabajo = PR.IdOrdenTrabajo
                     WHERE D.IdPedido = @IdPedido";
        }
        #endregion

        public DetallePresupuesto GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@Id", id));
            return DetallePresupuestoAdaptador.Current.FillPrimero(tabla);
        }

        public List<DetallePresupuesto> ListarPorPresupuesto(int idPresupuesto)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectByPresupuestoStatement, SqlHelper.Param("@IdPresupuesto", idPresupuesto));
            return DetallePresupuestoAdaptador.Current.FillLista(tabla);
        }

        // ---------------------------- Dentro de una transacción ----------------------------

        internal int Insertar(DetallePresupuesto detalle, SqlConnection cn, SqlTransaction tx)
        {
            return (int)SqlHelper.ExecuteScalar(cn, tx, InsertStatement,
                SqlHelper.Param("@IdPresupuesto", detalle.IdPresupuesto),
                SqlHelper.Param("@Descripcion", detalle.DescripcionItem),
                SqlHelper.Param("@Cantidad", detalle.Cantidad),
                SqlHelper.Param("@Precio", detalle.PrecioUnitario),
                SqlHelper.Param("@Subtotal", detalle.Subtotal),
                SqlHelper.Param("@Aprobado", detalle.Aprobado));
        }

        internal void InsertarRepuestosDePedido(int idPedido, SqlConnection cn, SqlTransaction tx)
        {
            SqlHelper.ExecuteNonQuery(cn, tx, InsertDesdePedidoStatement, SqlHelper.Param("@IdPedido", idPedido));
        }
    }
}
