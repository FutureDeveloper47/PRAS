using Microsoft.Data.SqlClient;
using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;
using PRAS.Services.Exceptions.DataAccess;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Diagnostico (singleton). El diagnóstico se crea cuando el auto entra
    /// al taller; acá se consulta, se completa y se finaliza. Las evidencias tienen su propio repositorio.
    /// </summary>
    internal sealed class DiagnosticoRepository : IDiagnosticoRepository
    {
        #region Singleton
        private static readonly DiagnosticoRepository _instance = new DiagnosticoRepository();

        public static DiagnosticoRepository Current
        {
            get { return _instance; }
        }

        private DiagnosticoRepository()
        {
        }
        #endregion

        private const string RecepcionDiagnosticada = "Diagnosticada";
        private const string DescripcionTarea = "Reparación según diagnóstico";

        #region Statements
        private const string SelectBase =
            @"SELECT D.IdDiagnostico, D.IdRecepcion, D.FechaDiagnostico, D.DescripcionDanios,
                     D.EstadoDiagnostico, D.RequiereReparacion, D.Observaciones, " +
                     VehiculoRepository.ColumnasConAlias + @"
              FROM Diagnostico D
              INNER JOIN Vehiculo V ON V.IdVehiculo = D.IdVehiculo";

        private string SelectOneStatement
        {
            get => $"{SelectBase} WHERE D.IdDiagnostico = @Id";
        }

        private string SelectUltimosStatement
        {
            get => $@"{SelectBase}
                      WHERE D.IdDiagnostico = (SELECT MAX(D2.IdDiagnostico) FROM Diagnostico D2
                                               WHERE D2.IdVehiculo = D.IdVehiculo)
                        AND (@IncluirTodos = 1 OR V.EstadoVehiculo IN (@EnDiagnostico, @EnReparacion))
                      ORDER BY V.Dominio";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO Diagnostico (IdRecepcion, IdVehiculo, FechaDiagnostico, DescripcionDanios,
                                              EstadoDiagnostico, RequiereReparacion, Observaciones)
                     VALUES (@IdRecepcion, @IdVehiculo, @Fecha, @Danios, @Estado, @Requiere, @Observaciones);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string UpdateStatement
        {
            get => @"UPDATE Diagnostico
                     SET DescripcionDanios = @Danios, Observaciones = @Observaciones,
                         RequiereReparacion = @Requiere, EstadoDiagnostico = @Estado
                     WHERE IdDiagnostico = @Id";
        }

        private string FinalizarStatement
        {
            get => @"UPDATE Diagnostico
                     SET DescripcionDanios = @Danios, Observaciones = @Observaciones,
                         RequiereReparacion = @Requiere, EstadoDiagnostico = @Finalizado
                     WHERE IdDiagnostico = @Id AND EstadoDiagnostico <> @Finalizado";
        }
        #endregion

        public Diagnostico GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@Id", id));
            Diagnostico diagnostico = DiagnosticoAdaptador.Current.FillPrimero(tabla);
            if (diagnostico != null)
                diagnostico.Evidencias = EvidenciaVehiculoRepository.Current.ListarPorDiagnostico(id);
            return diagnostico;
        }

        public List<Diagnostico> ListarUltimos(bool incluirTodos)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectUltimosStatement,
                SqlHelper.Param("@IncluirTodos", incluirTodos),
                SqlHelper.Param("@EnDiagnostico", (int)EstadoVehiculo.EnDiagnostico),
                SqlHelper.Param("@EnReparacion", (int)EstadoVehiculo.EnReparacion));
            return DiagnosticoAdaptador.Current.FillLista(tabla);
        }

        public void Update(Diagnostico diagnostico)
        {
            SqlHelper.ExecuteNonQuery(UpdateStatement,
                SqlHelper.Param("@Danios", diagnostico.DescripcionDanios),
                SqlHelper.Param("@Observaciones", diagnostico.Observaciones),
                SqlHelper.Param("@Requiere", diagnostico.RequiereReparacion),
                SqlHelper.Param("@Estado", diagnostico.EstadoDiagnostico),
                SqlHelper.Param("@Id", diagnostico.IdDiagnostico));
        }

        public void Finalizar(Diagnostico diagnostico, int? idMecanico, string prioridad)
        {
            DateTime ahora = DateTime.Now;

            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                int filas = SqlHelper.ExecuteNonQuery(cn, tx, FinalizarStatement,
                    SqlHelper.Param("@Danios", diagnostico.DescripcionDanios),
                    SqlHelper.Param("@Observaciones", diagnostico.Observaciones),
                    SqlHelper.Param("@Requiere", diagnostico.RequiereReparacion),
                    SqlHelper.Param("@Finalizado", Estados.DiagnosticoFinalizado),
                    SqlHelper.Param("@Id", diagnostico.IdDiagnostico));
                if (filas != 1)
                    throw new ConcurrencyException("El diagnóstico ya estaba finalizado (otro usuario lo modificó).");

                RecepcionVehiculoRepository.Current.CambiarEstado(diagnostico.IdRecepcion, RecepcionDiagnosticada, cn, tx);

                // Si no hay nada que reparar, el auto queda listo (Reparado).
                if (!diagnostico.RequiereReparacion)
                {
                    VehiculoRepository.Current.CambiarEstado(diagnostico.IdVehiculo,
                        new[] { EstadoVehiculo.EnDiagnostico }, EstadoVehiculo.Reparado, cn, tx);
                    return;
                }

                // Hay que repararlo: presupuesto vacío + orden de trabajo + tarea para el mecánico.
                int idTaller = TallerRepository.Current.ObtenerIdPrincipal(cn, tx);
                int idPresupuesto = PresupuestoRepository.Current.InsertarVacio(diagnostico.IdVehiculo, ahora, cn, tx);

                int idOrden = OrdenTrabajoRepository.Current.Insertar(new OrdenTrabajo
                {
                    IdRecepcion = diagnostico.IdRecepcion,
                    IdPresupuesto = idPresupuesto,
                    IdVehiculo = diagnostico.IdVehiculo,
                    IdTaller = idTaller,
                    FechaCreacion = ahora,
                    Prioridad = prioridad
                }, cn, tx);

                TareaOrdenTrabajoRepository.Current.Insertar(new TareaOrdenTrabajo
                {
                    IdOrdenTrabajo = idOrden,
                    IdMecanico = idMecanico.Value,   // la BLL ya exigió el mecánico cuando hay que reparar
                    DescripcionTarea = DescripcionTarea,
                    CostoEstimado = 0,
                    EstadoTarea = Estados.TareaPendiente
                }, cn, tx);

                VehiculoRepository.Current.CambiarEstado(diagnostico.IdVehiculo,
                    new[] { EstadoVehiculo.EnDiagnostico }, EstadoVehiculo.EnReparacion, cn, tx);
            });
        }

        // ---------------------------- Dentro de una transacción ----------------------------

        internal int Insertar(Diagnostico diagnostico, SqlConnection cn, SqlTransaction tx)
        {
            return (int)SqlHelper.ExecuteScalar(cn, tx, InsertStatement,
                SqlHelper.Param("@IdRecepcion", diagnostico.IdRecepcion),
                SqlHelper.Param("@IdVehiculo", diagnostico.IdVehiculo),
                SqlHelper.Param("@Fecha", diagnostico.FechaDiagnostico),
                SqlHelper.Param("@Danios", diagnostico.DescripcionDanios),
                SqlHelper.Param("@Estado", diagnostico.EstadoDiagnostico),
                SqlHelper.Param("@Requiere", diagnostico.RequiereReparacion),
                SqlHelper.Param("@Observaciones", diagnostico.Observaciones));
        }
    }
}
