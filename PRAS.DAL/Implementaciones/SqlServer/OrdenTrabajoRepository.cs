using System.Data;
using Microsoft.Data.SqlClient;
using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla OrdenTrabajo (singleton). Las tareas de cada orden están
    /// en su propio repositorio (TareaOrdenTrabajoRepository).
    /// </summary>
    internal sealed class OrdenTrabajoRepository : IOrdenTrabajoRepository
    {
        #region Singleton
        private static readonly OrdenTrabajoRepository _instance = new OrdenTrabajoRepository();

        public static OrdenTrabajoRepository Current
        {
            get { return _instance; }
        }

        private OrdenTrabajoRepository()
        {
        }
        #endregion

        #region Statements
        /// <summary>Orden + auto + total del presupuesto + mecánicos que tienen tareas en ella.</summary>
        private const string SelectBase =
            @"SELECT O.IdOrdenTrabajo, O.IdRecepcion, O.IdPresupuesto, O.IdTaller, O.FechaCreacion,
                     O.EstadoOrden, O.Prioridad, O.Observaciones,
                     ISNULL(P.MontoTotal, 0) AS MontoPresupuesto,
                     ISNULL((SELECT STRING_AGG(M.Apellido + ', ' + M.Nombre, ' / ')
                             FROM (SELECT DISTINCT T.IdMecanico FROM TareaOrdenTrabajo T
                                   WHERE T.IdOrdenTrabajo = O.IdOrdenTrabajo) X
                             INNER JOIN Mecanico M ON M.IdMecanico = X.IdMecanico), '') AS Mecanicos, " +
                     VehiculoRepository.ColumnasConAlias + @"
              FROM OrdenTrabajo O
              INNER JOIN Vehiculo V ON V.IdVehiculo = O.IdVehiculo
              LEFT JOIN Presupuesto P ON P.IdPresupuesto = O.IdPresupuesto";

        private string SelectOneStatement
        {
            get => $"{SelectBase} WHERE O.IdOrdenTrabajo = @IdOrdenTrabajo";
        }

        private string SelectFiltroStatement
        {
            get => $@"{SelectBase}
                      WHERE (@Estado IS NULL OR O.EstadoOrden = @Estado)
                        AND (@IdMecanico IS NULL OR EXISTS (SELECT 1 FROM TareaOrdenTrabajo T
                                                            WHERE T.IdOrdenTrabajo = O.IdOrdenTrabajo
                                                              AND T.IdMecanico = @IdMecanico))
                      ORDER BY O.FechaCreacion DESC";
        }

        private string CambiarPrioridadStatement
        {
            get => "UPDATE OrdenTrabajo SET Prioridad = @Prioridad WHERE IdOrdenTrabajo = @IdOrdenTrabajo";
        }

        private string ContarPendientesStatement
        {
            get => @"SELECT (SELECT COUNT(*) FROM TareaOrdenTrabajo WHERE IdOrdenTrabajo = @Id AND EstadoTarea <> @Finalizada) AS Tareas,
                            (SELECT COUNT(*) FROM PedidoRepuesto   WHERE IdOrdenTrabajo = @Id AND EstadoPedido = @Pendiente) AS Pedidos";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO OrdenTrabajo (IdRecepcion, IdPresupuesto, IdVehiculo, IdTaller, FechaCreacion, EstadoOrden, Prioridad)
                     VALUES (@IdRecepcion, @IdPresupuesto, @IdVehiculo, @IdTaller, @FechaCreacion, @Estado, @Prioridad);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string FinalizarAbiertasStatement
        {
            get => @"UPDATE OrdenTrabajo SET EstadoOrden = @Finalizada
                     WHERE IdVehiculo = @IdVehiculo AND EstadoOrden = @Abierta";
        }
        #endregion

        public OrdenTrabajo GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdOrdenTrabajo", id));
            return OrdenTrabajoAdaptador.Current.FillPrimero(tabla);
        }

        public List<OrdenTrabajo> Listar(string estado, int? idMecanico)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectFiltroStatement,
                SqlHelper.Param("@Estado", estado),
                SqlHelper.Param("@IdMecanico", idMecanico));
            return OrdenTrabajoAdaptador.Current.FillLista(tabla);
        }

        public void CambiarPrioridad(int idOrdenTrabajo, string prioridad)
        {
            SqlHelper.ExecuteNonQuery(CambiarPrioridadStatement,
                SqlHelper.Param("@Prioridad", prioridad),
                SqlHelper.Param("@IdOrdenTrabajo", idOrdenTrabajo));
        }

        /// <summary>Cuántas tareas sin finalizar y cuántos pedidos sin entregar tiene la orden.</summary>
        public void ContarPendientes(int idOrdenTrabajo, out int tareasPendientes, out int pedidosPendientes)
        {
            var tabla = SqlHelper.ExecuteDataTable(ContarPendientesStatement,
                SqlHelper.Param("@Id", idOrdenTrabajo),
                SqlHelper.Param("@Finalizada", Estados.TareaFinalizada),
                SqlHelper.Param("@Pendiente", Estados.PedidoPendiente));

            DataRow fila = tabla.Rows[0];
            tareasPendientes = (int)fila["Tareas"];
            pedidosPendientes = (int)fila["Pedidos"];
        }

        // ---------------------------- Dentro de una transacción ----------------------------

        /// <summary>Crea la orden (Abierta) y devuelve su Id.</summary>
        internal int Insertar(OrdenTrabajo orden, SqlConnection cn, SqlTransaction tx)
        {
            return (int)SqlHelper.ExecuteScalar(cn, tx, InsertStatement,
                SqlHelper.Param("@IdRecepcion", orden.IdRecepcion),
                SqlHelper.Param("@IdPresupuesto", orden.IdPresupuesto),
                SqlHelper.Param("@IdVehiculo", orden.IdVehiculo),
                SqlHelper.Param("@IdTaller", orden.IdTaller),
                SqlHelper.Param("@FechaCreacion", orden.FechaCreacion),
                SqlHelper.Param("@Estado", Estados.OrdenAbierta),
                SqlHelper.Param("@Prioridad", orden.Prioridad));
        }

        /// <summary>Cierra las órdenes abiertas del auto.</summary>
        internal void FinalizarAbiertasDeVehiculo(int idVehiculo, SqlConnection cn, SqlTransaction tx)
        {
            SqlHelper.ExecuteNonQuery(cn, tx, FinalizarAbiertasStatement,
                SqlHelper.Param("@Finalizada", Estados.OrdenFinalizada),
                SqlHelper.Param("@IdVehiculo", idVehiculo),
                SqlHelper.Param("@Abierta", Estados.OrdenAbierta));
        }
    }
}
