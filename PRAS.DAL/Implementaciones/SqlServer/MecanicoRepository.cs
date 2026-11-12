using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Mecanico en SQL Server (singleton).
    /// Los mecánicos del taller (a cada uno se le asignan tareas).
    /// </summary>
    internal sealed class MecanicoRepository : IMecanicoRepository
    {
        #region Singleton
        private static readonly MecanicoRepository _instance = new MecanicoRepository();

        public static MecanicoRepository Current
        {
            get { return _instance; }
        }

        private MecanicoRepository()
        {
        }
        #endregion

        #region Statements
        private const string Columnas = "IdMecanico, Nombre, Apellido, Especialidad, Telefono, Email, Activo";

        private string SelectOneStatement
        {
            get => $"SELECT {Columnas} FROM Mecanico WHERE IdMecanico = @IdMecanico";
        }

        private string SelectFiltroStatement
        {
            get => $@"SELECT {Columnas} FROM Mecanico
                      WHERE (@IncluirInactivos = 1 OR Activo = 1)
                        AND (@Filtro = ''
                             OR Apellido LIKE '%' + @Filtro + '%'
                             OR Nombre LIKE '%' + @Filtro + '%'
                             OR Especialidad LIKE '%' + @Filtro + '%')
                      ORDER BY Apellido, Nombre";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO Mecanico (Nombre, Apellido, Especialidad, Telefono, Email, Activo)
                     VALUES (@Nombre, @Apellido, @Especialidad, @Telefono, @Email, @Activo);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string UpdateStatement
        {
            get => @"UPDATE Mecanico
                     SET Nombre = @Nombre, Apellido = @Apellido, Especialidad = @Especialidad, Telefono = @Telefono, Email = @Email
                     WHERE IdMecanico = @IdMecanico";
        }

        private string CambiarEstadoStatement
        {
            get => "UPDATE Mecanico SET Activo = @Activo WHERE IdMecanico = @IdMecanico";
        }

        private string TareasPendientesStatement
        {
            get => @"SELECT COUNT(*) FROM TareaOrdenTrabajo
                     WHERE IdMecanico = @IdMecanico AND EstadoTarea <> @Finalizada";
        }
        #endregion

        public Mecanico GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdMecanico", id));
            return MecanicoAdaptador.Current.FillPrimero(tabla);
        }

        public List<Mecanico> Listar(string filtro, bool incluirInactivos)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectFiltroStatement,
                SqlHelper.Param("@IncluirInactivos", incluirInactivos),
                SqlHelper.Param("@Filtro", filtro ?? ""));
            return MecanicoAdaptador.Current.FillLista(tabla);
        }

        /// <summary>Todos, incluidos los deshabilitados.</summary>
        public List<Mecanico> GetAll()
        {
            return Listar("", true);
        }

        public void Add(Mecanico entity)
        {
            object id = SqlHelper.ExecuteScalar(InsertStatement,
                SqlHelper.Param("@Nombre", entity.Nombre),
                SqlHelper.Param("@Apellido", entity.Apellido),
                SqlHelper.Param("@Especialidad", entity.Especialidad),
                SqlHelper.Param("@Telefono", entity.Telefono),
                SqlHelper.Param("@Email", entity.Email),
                SqlHelper.Param("@Activo", entity.Activo));

            entity.IdMecanico = (int)id;
        }

        public void Update(Mecanico entity)
        {
            SqlHelper.ExecuteNonQuery(UpdateStatement,
                SqlHelper.Param("@Nombre", entity.Nombre),
                SqlHelper.Param("@Apellido", entity.Apellido),
                SqlHelper.Param("@Especialidad", entity.Especialidad),
                SqlHelper.Param("@Telefono", entity.Telefono),
                SqlHelper.Param("@Email", entity.Email),
                SqlHelper.Param("@IdMecanico", entity.IdMecanico));
        }

        /// <summary>Baja lógica: el registro queda en la base pero deshabilitado (tiene historia).</summary>
        public void Delete(int id)
        {
            CambiarEstado(id, false);
        }

        public void CambiarEstado(int mecanicoId, bool activo)
        {
            SqlHelper.ExecuteNonQuery(CambiarEstadoStatement,
                SqlHelper.Param("@Activo", activo),
                SqlHelper.Param("@IdMecanico", mecanicoId));
        }

        public bool TieneTareasPendientes(int idMecanico)
        {
            object cantidad = SqlHelper.ExecuteScalar(TareasPendientesStatement,
                SqlHelper.Param("@IdMecanico", idMecanico),
                SqlHelper.Param("@Finalizada", Estados.TareaFinalizada));
            return (int)cantidad > 0;
        }
    }
}
