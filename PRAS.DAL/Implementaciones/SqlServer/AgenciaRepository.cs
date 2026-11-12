using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Agencia en SQL Server (singleton).
    /// Las agencias reciben nuestros autos reparados (consignación) y los venden.
    /// </summary>
    internal sealed class AgenciaRepository : IAgenciaRepository
    {
        #region Singleton
        private static readonly AgenciaRepository _instance = new AgenciaRepository();

        public static AgenciaRepository Current
        {
            get { return _instance; }
        }

        private AgenciaRepository()
        {
        }
        #endregion

        #region Statements
        private const string Columnas = "IdAgencia, RazonSocial, CUIT, Domicilio, Localidad, Telefono, Email, Activo";

        private string SelectOneStatement
        {
            get => $"SELECT {Columnas} FROM Agencia WHERE IdAgencia = @IdAgencia";
        }

        private string SelectByCUITStatement
        {
            get => $"SELECT {Columnas} FROM Agencia WHERE CUIT = @CUIT";
        }

        private string SelectFiltroStatement
        {
            get => $@"SELECT {Columnas} FROM Agencia
                      WHERE (@IncluirInactivos = 1 OR Activo = 1)
                        AND (@Filtro = ''
                             OR RazonSocial LIKE '%' + @Filtro + '%'
                             OR CUIT LIKE '%' + @Filtro + '%'
                             OR Localidad LIKE '%' + @Filtro + '%')
                      ORDER BY RazonSocial";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO Agencia (RazonSocial, CUIT, Domicilio, Localidad, Telefono, Email, Activo)
                     VALUES (@RazonSocial, @CUIT, @Domicilio, @Localidad, @Telefono, @Email, @Activo);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string UpdateStatement
        {
            get => @"UPDATE Agencia
                     SET RazonSocial = @RazonSocial, CUIT = @CUIT, Domicilio = @Domicilio, Localidad = @Localidad, Telefono = @Telefono, Email = @Email
                     WHERE IdAgencia = @IdAgencia";
        }

        private string CambiarEstadoStatement
        {
            get => "UPDATE Agencia SET Activo = @Activo WHERE IdAgencia = @IdAgencia";
        }

        private string ConsignacionesVigentesStatement
        {
            get => @"SELECT COUNT(*) FROM Consignacion
                     WHERE IdAgencia = @IdAgencia AND EstadoConsignacion = @Vigente";
        }
        #endregion

        public Agencia GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdAgencia", id));
            return AgenciaAdaptador.Current.FillPrimero(tabla);
        }

        public Agencia ObtenerPorCUIT(string cuit)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectByCUITStatement, SqlHelper.Param("@CUIT", cuit));
            return AgenciaAdaptador.Current.FillPrimero(tabla);
        }

        public List<Agencia> Listar(string filtro, bool incluirInactivos)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectFiltroStatement,
                SqlHelper.Param("@IncluirInactivos", incluirInactivos),
                SqlHelper.Param("@Filtro", filtro ?? ""));
            return AgenciaAdaptador.Current.FillLista(tabla);
        }

        /// <summary>Todos, incluidos los deshabilitados.</summary>
        public List<Agencia> GetAll()
        {
            return Listar("", true);
        }

        public void Add(Agencia entity)
        {
            object id = SqlHelper.ExecuteScalar(InsertStatement,
                SqlHelper.Param("@RazonSocial", entity.RazonSocial),
                SqlHelper.Param("@CUIT", entity.CUIT),
                SqlHelper.Param("@Domicilio", entity.Domicilio),
                SqlHelper.Param("@Localidad", entity.Localidad),
                SqlHelper.Param("@Telefono", entity.Telefono),
                SqlHelper.Param("@Email", entity.Email),
                SqlHelper.Param("@Activo", entity.Activo));

            entity.IdAgencia = (int)id;
        }

        public void Update(Agencia entity)
        {
            SqlHelper.ExecuteNonQuery(UpdateStatement,
                SqlHelper.Param("@RazonSocial", entity.RazonSocial),
                SqlHelper.Param("@CUIT", entity.CUIT),
                SqlHelper.Param("@Domicilio", entity.Domicilio),
                SqlHelper.Param("@Localidad", entity.Localidad),
                SqlHelper.Param("@Telefono", entity.Telefono),
                SqlHelper.Param("@Email", entity.Email),
                SqlHelper.Param("@IdAgencia", entity.IdAgencia));
        }

        /// <summary>Baja lógica: el registro queda en la base pero deshabilitado (tiene historia).</summary>
        public void Delete(int id)
        {
            CambiarEstado(id, false);
        }

        public void CambiarEstado(int agenciaId, bool activo)
        {
            SqlHelper.ExecuteNonQuery(CambiarEstadoStatement,
                SqlHelper.Param("@Activo", activo),
                SqlHelper.Param("@IdAgencia", agenciaId));
        }

        public bool TieneConsignacionesVigentes(int idAgencia)
        {
            object cantidad = SqlHelper.ExecuteScalar(ConsignacionesVigentesStatement,
                SqlHelper.Param("@IdAgencia", idAgencia),
                SqlHelper.Param("@Vigente", Estados.ConsignacionVigente));
            return (int)cantidad > 0;
        }
    }
}
