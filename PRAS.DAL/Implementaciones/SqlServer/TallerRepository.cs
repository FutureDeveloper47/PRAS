using Microsoft.Data.SqlClient;
using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;
using PRAS.Services.Exceptions.DataAccess;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Taller (singleton) y de las dos operaciones que mueven al auto dentro
    /// del taller. Cada operación es UNA transacción que usa los repositorios de cada tabla
    /// (Recepción, Diagnóstico, Presupuesto, Orden de trabajo, Tarea y Vehículo).
    /// </summary>
    internal sealed class TallerRepository : ITallerRepository
    {
        #region Singleton
        private static readonly TallerRepository _instance = new TallerRepository();

        public static TallerRepository Current
        {
            get { return _instance; }
        }

        private TallerRepository()
        {
        }
        #endregion

        private const string RecepcionDiagnosticada = "Diagnosticada";
        private const string MotivoIngreso = "Reparación de vehículo chocado comprado";
        private const string DescripcionTarea = "Reparación según diagnóstico inicial";

        #region Statements
        private const string Columnas = "IdTaller, RazonSocial, CUIT, Domicilio, Localidad, Telefono, Email";

        private string SelectOneStatement
        {
            get => $"SELECT {Columnas} FROM Taller WHERE IdTaller = @IdTaller";
        }

        private string SelectPrincipalStatement
        {
            get => $"SELECT TOP 1 {Columnas} FROM Taller ORDER BY IdTaller";
        }

        private string SelectIdPrincipalStatement
        {
            get => "SELECT TOP 1 IdTaller FROM Taller ORDER BY IdTaller";
        }
        #endregion

        public Taller GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdTaller", id));
            return TallerAdaptador.Current.FillPrimero(tabla);
        }

        public Taller ObtenerPrincipal()
        {
            return TallerAdaptador.Current.FillPrimero(SqlHelper.ExecuteDataTable(SelectPrincipalStatement));
        }

        public int RegistrarIngreso(int idVehiculo, int idMecanico, string diagnostico, string prioridad)
        {
            int idOrden = 0;
            DateTime ahora = DateTime.Now;

            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                int idTaller = ObtenerIdPrincipal(cn, tx);

                // 1) Recepción
                int idRecepcion = RecepcionVehiculoRepository.Current.Insertar(new RecepcionVehiculo
                {
                    IdVehiculo = idVehiculo,
                    FechaIngreso = ahora,
                    MotivoIngreso = MotivoIngreso,
                    EstadoRecepcion = RecepcionDiagnosticada
                }, cn, tx);

                // 2) Diagnóstico (ya finalizado: el ingreso simplificado lo carga en el mismo paso)
                DiagnosticoRepository.Current.Insertar(new Diagnostico
                {
                    IdRecepcion = idRecepcion,
                    IdVehiculo = idVehiculo,
                    FechaDiagnostico = ahora,
                    DescripcionDanios = diagnostico,
                    EstadoDiagnostico = Estados.DiagnosticoFinalizado,
                    RequiereReparacion = true
                }, cn, tx);

                // 3) Presupuesto vacío
                int idPresupuesto = PresupuestoRepository.Current.InsertarVacio(idVehiculo, ahora, cn, tx);

                // 4) Orden de trabajo
                idOrden = OrdenTrabajoRepository.Current.Insertar(new OrdenTrabajo
                {
                    IdRecepcion = idRecepcion,
                    IdPresupuesto = idPresupuesto,
                    IdVehiculo = idVehiculo,
                    IdTaller = idTaller,
                    FechaCreacion = ahora,
                    Prioridad = prioridad
                }, cn, tx);

                // 5) Tarea para el mecánico
                TareaOrdenTrabajoRepository.Current.Insertar(new TareaOrdenTrabajo
                {
                    IdOrdenTrabajo = idOrden,
                    IdMecanico = idMecanico,
                    DescripcionTarea = DescripcionTarea,
                    CostoEstimado = 0,
                    EstadoTarea = Estados.TareaPendiente,
                    FechaInicio = ahora
                }, cn, tx);

                // 6) El auto pasa a En reparación. Si ya no estaba Comprado
                // (porque otro usuario lo ingresó recién), se cancela todo.
                VehiculoRepository.Current.CambiarEstado(idVehiculo,
                    new[] { EstadoVehiculo.Comprado }, EstadoVehiculo.EnReparacion, cn, tx);
            });

            return idOrden;
        }

        public bool FinalizarReparacion(int idVehiculo)
        {
            DateTime ahora = DateTime.Now;

            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                TareaOrdenTrabajoRepository.Current.FinalizarPendientesDeVehiculo(idVehiculo, ahora, cn, tx);
                OrdenTrabajoRepository.Current.FinalizarAbiertasDeVehiculo(idVehiculo, cn, tx);
                VehiculoRepository.Current.CambiarEstado(idVehiculo,
                    new[] { EstadoVehiculo.EnReparacion }, EstadoVehiculo.Reparado, cn, tx);
            });

            return true;
        }

        // ---------------------------- Dentro de una transacción ----------------------------

        /// <summary>El taller somos nosotros: usamos el primero cargado (lo crea el script de la base).</summary>
        internal int ObtenerIdPrincipal(SqlConnection cn, SqlTransaction tx)
        {
            object idTaller = SqlHelper.ExecuteScalar(cn, tx, SelectIdPrincipalStatement);
            if (idTaller == null)
                throw new RecordNotFoundException("No hay ningún taller cargado en la tabla Taller.");
            return (int)idTaller;
        }
    }
}
