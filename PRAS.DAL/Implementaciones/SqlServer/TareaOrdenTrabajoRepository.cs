using Microsoft.Data.SqlClient;
using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla TareaOrdenTrabajo (singleton): las tareas que hace cada mecánico.
    /// </summary>
    internal sealed class TareaOrdenTrabajoRepository : ITareaOrdenTrabajoRepository
    {
        #region Singleton
        private static readonly TareaOrdenTrabajoRepository _instance = new TareaOrdenTrabajoRepository();

        public static TareaOrdenTrabajoRepository Current
        {
            get { return _instance; }
        }

        private TareaOrdenTrabajoRepository()
        {
        }
        #endregion

        #region Statements
        /// <summary>Tarea + mecánico + prioridad de la orden + auto.</summary>
        private const string SelectBase =
            @"SELECT T.IdTarea, T.IdOrdenTrabajo, T.IdMecanico, T.DescripcionTarea, T.CostoEstimado,
                     T.EstadoTarea, T.FechaInicio, T.FechaFin,
                     M.Nombre AS MecanicoNombre, M.Apellido AS MecanicoApellido,
                     O.Prioridad, " + VehiculoRepository.ColumnasConAlias + @"
              FROM TareaOrdenTrabajo T
              INNER JOIN Mecanico M     ON M.IdMecanico = T.IdMecanico
              INNER JOIN OrdenTrabajo O ON O.IdOrdenTrabajo = T.IdOrdenTrabajo
              INNER JOIN Vehiculo V     ON V.IdVehiculo = O.IdVehiculo";

        private string SelectOneStatement
        {
            get => $"{SelectBase} WHERE T.IdTarea = @IdTarea";
        }

        private string SelectByOrdenStatement
        {
            get => $"{SelectBase} WHERE T.IdOrdenTrabajo = @IdOrden ORDER BY T.IdTarea";
        }

        private string SelectDeMecanicoStatement
        {
            get => $@"{SelectBase}
                      WHERE O.EstadoOrden = @Abierta
                        AND (@IdMecanico IS NULL OR T.IdMecanico = @IdMecanico)
                        AND (@IncluirFinalizadas = 1 OR T.EstadoTarea <> @Finalizada)
                      ORDER BY CASE O.Prioridad WHEN 'Alta' THEN 1 WHEN 'Media' THEN 2 ELSE 3 END, T.IdTarea";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO TareaOrdenTrabajo (IdOrdenTrabajo, IdMecanico, DescripcionTarea, CostoEstimado, EstadoTarea, FechaInicio)
                     VALUES (@IdOrden, @IdMecanico, @Descripcion, @Costo, @Estado, @FechaInicio);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string FinalizarPendientesDeVehiculoStatement
        {
            get => @"UPDATE T SET EstadoTarea = @Finalizada, FechaFin = @Fecha
                     FROM TareaOrdenTrabajo T
                     INNER JOIN OrdenTrabajo O ON O.IdOrdenTrabajo = T.IdOrdenTrabajo
                     WHERE O.IdVehiculo = @IdVehiculo AND O.EstadoOrden = @Abierta
                       AND T.EstadoTarea <> @Finalizada";
        }
        #endregion

        public TareaOrdenTrabajo GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdTarea", id));
            return TareaOrdenTrabajoAdaptador.Current.FillPrimero(tabla);
        }

        public List<TareaOrdenTrabajo> ListarPorOrden(int idOrdenTrabajo)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectByOrdenStatement, SqlHelper.Param("@IdOrden", idOrdenTrabajo));
            return TareaOrdenTrabajoAdaptador.Current.FillLista(tabla);
        }

        public List<TareaOrdenTrabajo> ListarDeMecanico(int? idMecanico, bool incluirFinalizadas)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectDeMecanicoStatement,
                SqlHelper.Param("@Abierta", Estados.OrdenAbierta),
                SqlHelper.Param("@IdMecanico", idMecanico),
                SqlHelper.Param("@IncluirFinalizadas", incluirFinalizadas),
                SqlHelper.Param("@Finalizada", Estados.TareaFinalizada));
            return TareaOrdenTrabajoAdaptador.Current.FillLista(tabla);
        }

        /// <summary>Agrega una tarea Pendiente a una orden y le carga el Id generado.</summary>
        public void Add(TareaOrdenTrabajo entity)
        {
            entity.EstadoTarea = Estados.TareaPendiente;
            entity.IdTarea = (int)SqlHelper.ExecuteScalar(InsertStatement, ParametrosAlta(entity));
        }

        public bool CambiarEstado(int idTarea, string estadoAnterior, string estadoNuevo, DateTime fecha)
        {
            // Al terminar guardamos la fecha de fin (y la de inicio, si nunca se había empezado).
            // Al empezar guardamos la fecha de inicio.
            string fechas = estadoNuevo == Estados.TareaFinalizada
                ? "FechaFin = @Fecha, FechaInicio = ISNULL(FechaInicio, @Fecha)"
                : "FechaInicio = @Fecha";

            int filas = SqlHelper.ExecuteNonQuery(
                $@"UPDATE TareaOrdenTrabajo
                   SET EstadoTarea = @Nuevo, {fechas}
                   WHERE IdTarea = @IdTarea AND EstadoTarea = @Anterior",
                SqlHelper.Param("@Nuevo", estadoNuevo),
                SqlHelper.Param("@Fecha", fecha),
                SqlHelper.Param("@IdTarea", idTarea),
                SqlHelper.Param("@Anterior", estadoAnterior));

            return filas == 1;
        }

        // ---------------------------- Dentro de una transacción ----------------------------

        internal int Insertar(TareaOrdenTrabajo tarea, SqlConnection cn, SqlTransaction tx)
        {
            return (int)SqlHelper.ExecuteScalar(cn, tx, InsertStatement, ParametrosAlta(tarea));
        }

        /// <summary>Termina todas las tareas sin terminar de las órdenes abiertas del auto.</summary>
        internal void FinalizarPendientesDeVehiculo(int idVehiculo, DateTime fecha, SqlConnection cn, SqlTransaction tx)
        {
            SqlHelper.ExecuteNonQuery(cn, tx, FinalizarPendientesDeVehiculoStatement,
                SqlHelper.Param("@Finalizada", Estados.TareaFinalizada),
                SqlHelper.Param("@Fecha", fecha),
                SqlHelper.Param("@IdVehiculo", idVehiculo),
                SqlHelper.Param("@Abierta", Estados.OrdenAbierta));
        }

        private static SqlParameter[] ParametrosAlta(TareaOrdenTrabajo tarea)
        {
            return new[]
            {
                SqlHelper.Param("@IdOrden", tarea.IdOrdenTrabajo),
                SqlHelper.Param("@IdMecanico", tarea.IdMecanico),
                SqlHelper.Param("@Descripcion", tarea.DescripcionTarea),
                SqlHelper.Param("@Costo", tarea.CostoEstimado),
                SqlHelper.Param("@Estado", tarea.EstadoTarea ?? Estados.TareaPendiente),
                SqlHelper.Param("@FechaInicio", tarea.FechaInicio)
            };
        }
    }
}
