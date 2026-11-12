using Microsoft.Data.SqlClient;
using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Presupuesto (singleton). Cada gasto de la reparación es un renglón
    /// (DetallePresupuestoRepository) y el total se recalcula en la base.
    /// </summary>
    internal sealed class PresupuestoRepository : IPresupuestoRepository
    {
        #region Singleton
        private static readonly PresupuestoRepository _instance = new PresupuestoRepository();

        public static PresupuestoRepository Current
        {
            get { return _instance; }
        }

        private PresupuestoRepository()
        {
        }
        #endregion

        /// <summary>Estado con el que se crea el presupuesto de un auto propio (se aprueba al cargarlo).</summary>
        internal const string EstadoAprobado = "Aprobado";

        /// <summary>Días hasta que vence el presupuesto.</summary>
        internal const int DiasVencimiento = 30;

        #region Statements
        private const string SelectBase =
            @"SELECT P.IdPresupuesto, P.FechaEmision, P.FechaVencimiento, P.EstadoPresupuesto,
                     P.MontoTotal, P.Observaciones, " + VehiculoRepository.ColumnasConAlias + @"
              FROM Presupuesto P
              INNER JOIN Vehiculo V ON V.IdVehiculo = P.IdVehiculo";

        private string SelectOneStatement
        {
            get => $"{SelectBase} WHERE P.IdPresupuesto = @IdPresupuesto";
        }

        private string SelectEnReparacionStatement
        {
            get => $@"{SelectBase}
                      WHERE V.EstadoVehiculo = @EnReparacion
                        AND P.IdPresupuesto = (SELECT MAX(P2.IdPresupuesto)
                                               FROM Presupuesto P2
                                               WHERE P2.IdVehiculo = V.IdVehiculo)
                      ORDER BY V.Dominio";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO Presupuesto (IdVehiculo, FechaEmision, FechaVencimiento, EstadoPresupuesto, MontoTotal)
                     VALUES (@IdVehiculo, @FechaEmision, @FechaVencimiento, @Estado, 0);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string RecalcularTotalStatement
        {
            get => @"UPDATE Presupuesto
                     SET MontoTotal = (SELECT ISNULL(SUM(Subtotal), 0)
                                       FROM DetallePresupuesto
                                       WHERE IdPresupuesto = @IdPresupuesto)
                     WHERE IdPresupuesto = @IdPresupuesto";
        }
        #endregion

        public Presupuesto GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdPresupuesto", id));
            Presupuesto presupuesto = PresupuestoAdaptador.Current.FillPrimero(tabla);
            if (presupuesto != null)
                presupuesto.Detalles = DetallePresupuestoRepository.Current.ListarPorPresupuesto(id);
            return presupuesto;
        }

        public List<Presupuesto> ListarDeAutosEnReparacion()
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectEnReparacionStatement,
                SqlHelper.Param("@EnReparacion", (int)EstadoVehiculo.EnReparacion));
            return PresupuestoAdaptador.Current.FillLista(tabla);
        }

        public void AgregarDetalles(int idPresupuesto, List<DetallePresupuesto> detalles)
        {
            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                foreach (DetallePresupuesto detalle in detalles)
                {
                    detalle.IdPresupuesto = idPresupuesto;
                    detalle.IdDetallePresupuesto = DetallePresupuestoRepository.Current.Insertar(detalle, cn, tx);
                }

                RecalcularTotal(idPresupuesto, cn, tx);
            });
        }

        // ---------------------------- Dentro de una transacción ----------------------------

        /// <summary>Crea un presupuesto vacío (total 0) para el auto y devuelve su Id.</summary>
        internal int InsertarVacio(int idVehiculo, DateTime fecha, SqlConnection cn, SqlTransaction tx)
        {
            return (int)SqlHelper.ExecuteScalar(cn, tx, InsertStatement,
                SqlHelper.Param("@IdVehiculo", idVehiculo),
                SqlHelper.Param("@FechaEmision", fecha),
                SqlHelper.Param("@FechaVencimiento", fecha.AddDays(DiasVencimiento)),
                SqlHelper.Param("@Estado", EstadoAprobado));
        }

        /// <summary>El total del presupuesto es la suma de sus renglones.</summary>
        internal void RecalcularTotal(int idPresupuesto, SqlConnection cn, SqlTransaction tx)
        {
            SqlHelper.ExecuteNonQuery(cn, tx, RecalcularTotalStatement,
                SqlHelper.Param("@IdPresupuesto", idPresupuesto));
        }
    }
}
