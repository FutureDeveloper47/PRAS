using Microsoft.Data.SqlClient;
using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla RecepcionVehiculo (singleton): cada vez que un auto entra al taller.
    /// El alta se hace dentro del ingreso al taller (TallerRepository.RegistrarIngreso).
    /// </summary>
    internal sealed class RecepcionVehiculoRepository : IRecepcionVehiculoRepository
    {
        #region Singleton
        private static readonly RecepcionVehiculoRepository _instance = new RecepcionVehiculoRepository();

        public static RecepcionVehiculoRepository Current
        {
            get { return _instance; }
        }

        private RecepcionVehiculoRepository()
        {
        }
        #endregion

        #region Statements
        private const string Columnas = "IdRecepcion, IdVehiculo, FechaIngreso, MotivoIngreso, EstadoRecepcion, Observaciones";

        private string SelectOneStatement
        {
            get => $"SELECT {Columnas} FROM RecepcionVehiculo WHERE IdRecepcion = @IdRecepcion";
        }

        private string SelectByVehiculoStatement
        {
            get => $"SELECT {Columnas} FROM RecepcionVehiculo WHERE IdVehiculo = @IdVehiculo ORDER BY FechaIngreso DESC";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO RecepcionVehiculo (IdVehiculo, FechaIngreso, MotivoIngreso, EstadoRecepcion, Observaciones)
                     VALUES (@IdVehiculo, @FechaIngreso, @MotivoIngreso, @EstadoRecepcion, @Observaciones);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string CambiarEstadoStatement
        {
            get => "UPDATE RecepcionVehiculo SET EstadoRecepcion = @Estado WHERE IdRecepcion = @IdRecepcion";
        }
        #endregion

        public RecepcionVehiculo GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdRecepcion", id));
            return RecepcionVehiculoAdaptador.Current.FillPrimero(tabla);
        }

        public List<RecepcionVehiculo> ListarPorVehiculo(int idVehiculo)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectByVehiculoStatement, SqlHelper.Param("@IdVehiculo", idVehiculo));
            return RecepcionVehiculoAdaptador.Current.FillLista(tabla);
        }

        // ---------------------------- Dentro de una transacción ----------------------------

        internal int Insertar(RecepcionVehiculo recepcion, SqlConnection cn, SqlTransaction tx)
        {
            return (int)SqlHelper.ExecuteScalar(cn, tx, InsertStatement,
                SqlHelper.Param("@IdVehiculo", recepcion.IdVehiculo),
                SqlHelper.Param("@FechaIngreso", recepcion.FechaIngreso),
                SqlHelper.Param("@MotivoIngreso", recepcion.MotivoIngreso),
                SqlHelper.Param("@EstadoRecepcion", recepcion.EstadoRecepcion),
                SqlHelper.Param("@Observaciones", recepcion.Observaciones));
        }

        internal void CambiarEstado(int idRecepcion, string estado, SqlConnection cn, SqlTransaction tx)
        {
            SqlHelper.ExecuteNonQuery(cn, tx, CambiarEstadoStatement,
                SqlHelper.Param("@Estado", estado),
                SqlHelper.Param("@IdRecepcion", idRecepcion));
        }
    }
}
