using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla EvidenciaVehiculo (singleton): fotos, videos o documentos de un diagnóstico.
    /// En la base se guarda solo la ruta del archivo.
    /// </summary>
    internal sealed class EvidenciaVehiculoRepository : IEvidenciaVehiculoRepository
    {
        #region Singleton
        private static readonly EvidenciaVehiculoRepository _instance = new EvidenciaVehiculoRepository();

        public static EvidenciaVehiculoRepository Current
        {
            get { return _instance; }
        }

        private EvidenciaVehiculoRepository()
        {
        }
        #endregion

        #region Statements
        private const string Columnas =
            "IdEvidencia, IdDiagnostico, IdVehiculo, Descripcion, TipoEvidencia, UbicacionArchivo, FechaCarga";

        private string SelectOneStatement
        {
            get => $"SELECT {Columnas} FROM EvidenciaVehiculo WHERE IdEvidencia = @IdEvidencia";
        }

        private string SelectByDiagnosticoStatement
        {
            get => $"SELECT {Columnas} FROM EvidenciaVehiculo WHERE IdDiagnostico = @IdDiagnostico ORDER BY FechaCarga";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO EvidenciaVehiculo (IdDiagnostico, IdVehiculo, Descripcion, TipoEvidencia, UbicacionArchivo, FechaCarga)
                     VALUES (@IdDiagnostico, @IdVehiculo, @Descripcion, @Tipo, @Ubicacion, @Fecha);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }
        #endregion

        public EvidenciaVehiculo GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdEvidencia", id));
            return EvidenciaVehiculoAdaptador.Current.FillPrimero(tabla);
        }

        public List<EvidenciaVehiculo> ListarPorDiagnostico(int idDiagnostico)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectByDiagnosticoStatement, SqlHelper.Param("@IdDiagnostico", idDiagnostico));
            return EvidenciaVehiculoAdaptador.Current.FillLista(tabla);
        }

        public void Add(EvidenciaVehiculo entity)
        {
            object id = SqlHelper.ExecuteScalar(InsertStatement,
                SqlHelper.Param("@IdDiagnostico", entity.IdDiagnostico),
                SqlHelper.Param("@IdVehiculo", entity.IdVehiculo),
                SqlHelper.Param("@Descripcion", entity.Descripcion),
                SqlHelper.Param("@Tipo", entity.TipoEvidencia),
                SqlHelper.Param("@Ubicacion", entity.UbicacionArchivo),
                SqlHelper.Param("@Fecha", entity.FechaCarga));

            entity.IdEvidencia = (int)id;
        }
    }
}
