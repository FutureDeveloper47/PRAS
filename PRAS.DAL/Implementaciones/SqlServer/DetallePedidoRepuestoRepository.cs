using Microsoft.Data.SqlClient;
using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla DetallePedidoRepuesto (singleton): cada repuesto de un pedido.
    /// Las altas van dentro de la transacción del pedido (PedidoRepuestoRepository.Add).
    /// </summary>
    internal sealed class DetallePedidoRepuestoRepository : IDetallePedidoRepuestoRepository
    {
        #region Singleton
        private static readonly DetallePedidoRepuestoRepository _instance = new DetallePedidoRepuestoRepository();

        public static DetallePedidoRepuestoRepository Current
        {
            get { return _instance; }
        }

        private DetallePedidoRepuestoRepository()
        {
        }
        #endregion

        #region Statements
        private const string Columnas = "IdDetallePedido, IdPedido, DescripcionRepuesto, Cantidad, PrecioUnitario, Subtotal";

        private string SelectOneStatement
        {
            get => $"SELECT {Columnas} FROM DetallePedidoRepuesto WHERE IdDetallePedido = @Id";
        }

        private string SelectByPedidoStatement
        {
            get => $"SELECT {Columnas} FROM DetallePedidoRepuesto WHERE IdPedido = @IdPedido ORDER BY IdDetallePedido";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO DetallePedidoRepuesto (IdPedido, DescripcionRepuesto, Cantidad, PrecioUnitario, Subtotal)
                     VALUES (@IdPedido, @Descripcion, @Cantidad, @Precio, @Subtotal);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }
        #endregion

        public DetallePedidoRepuesto GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@Id", id));
            return DetallePedidoRepuestoAdaptador.Current.FillPrimero(tabla);
        }

        public List<DetallePedidoRepuesto> ListarPorPedido(int idPedido)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectByPedidoStatement, SqlHelper.Param("@IdPedido", idPedido));
            return DetallePedidoRepuestoAdaptador.Current.FillLista(tabla);
        }

        // ---------------------------- Dentro de una transacción ----------------------------

        internal int Insertar(DetallePedidoRepuesto detalle, SqlConnection cn, SqlTransaction tx)
        {
            return (int)SqlHelper.ExecuteScalar(cn, tx, InsertStatement,
                SqlHelper.Param("@IdPedido", detalle.IdPedido),
                SqlHelper.Param("@Descripcion", detalle.DescripcionRepuesto),
                SqlHelper.Param("@Cantidad", detalle.Cantidad),
                SqlHelper.Param("@Precio", detalle.PrecioUnitario),
                SqlHelper.Param("@Subtotal", detalle.Subtotal));
        }
    }
}
