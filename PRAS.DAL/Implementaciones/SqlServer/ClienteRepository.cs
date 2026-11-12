using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;
using PRAS.Services.Facade;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Cliente en SQL Server (singleton).
    /// El cliente nos vende autos chocados (Compra) o nos compra autos reparados (Venta).
    /// El teléfono y el email se guardan ENCRIPTADOS con AES (CryptographyService):
    /// se encriptan al grabar (Add / Update) y el adaptador los desencripta al leer.
    /// </summary>
    internal sealed class ClienteRepository : IClienteRepository
    {
        #region Singleton
        private static readonly ClienteRepository _instance = new ClienteRepository();

        public static ClienteRepository Current
        {
            get { return _instance; }
        }

        private ClienteRepository()
        {
        }
        #endregion

        #region Statements
        private const string Columnas = "IdCliente, Nombre, Apellido, DNI, CUIT, Domicilio, Localidad, Telefono, Email, Activo";

        private string SelectOneStatement
        {
            get => $"SELECT {Columnas} FROM Cliente WHERE IdCliente = @IdCliente";
        }

        private string SelectByDNIStatement
        {
            get => $"SELECT {Columnas} FROM Cliente WHERE DNI = @DNI";
        }

        private string SelectFiltroStatement
        {
            get => $@"SELECT {Columnas} FROM Cliente
                      WHERE (@IncluirInactivos = 1 OR Activo = 1)
                        AND (@Filtro = ''
                             OR Apellido LIKE '%' + @Filtro + '%'
                             OR Nombre LIKE '%' + @Filtro + '%'
                             OR DNI LIKE '%' + @Filtro + '%')
                      ORDER BY Apellido, Nombre";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO Cliente (Nombre, Apellido, DNI, CUIT, Domicilio, Localidad, Telefono, Email, Activo)
                     VALUES (@Nombre, @Apellido, @DNI, @CUIT, @Domicilio, @Localidad, @Telefono, @Email, @Activo);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string UpdateStatement
        {
            get => @"UPDATE Cliente
                     SET Nombre = @Nombre, Apellido = @Apellido, DNI = @DNI, CUIT = @CUIT, Domicilio = @Domicilio, Localidad = @Localidad, Telefono = @Telefono, Email = @Email
                     WHERE IdCliente = @IdCliente";
        }

        private string SinEncriptarStatement
        {
            // Los datos encriptados empiezan con "ENC:"; los que no, son datos viejos en texto plano
            get => @"SELECT IdCliente FROM Cliente
                     WHERE (Telefono IS NOT NULL AND Telefono <> '' AND Telefono NOT LIKE 'ENC:%')
                        OR (Email IS NOT NULL AND Email <> '' AND Email NOT LIKE 'ENC:%')";
        }

        private string AmpliarColumnasStatement
        {
            // El texto encriptado es más largo que el original: si la base es vieja, agrandamos las columnas.
            get => @"IF COL_LENGTH('Cliente', 'Telefono') < 100 ALTER TABLE Cliente ALTER COLUMN Telefono VARCHAR(100) NULL;
                     IF COL_LENGTH('Cliente', 'Email') < 600 ALTER TABLE Cliente ALTER COLUMN Email NVARCHAR(300) NULL;";
        }

        private string CambiarEstadoStatement
        {
            get => "UPDATE Cliente SET Activo = @Activo WHERE IdCliente = @IdCliente";
        }
        #endregion

        public Cliente GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdCliente", id));
            return ClienteAdaptador.Current.FillPrimero(tabla);
        }

        public Cliente ObtenerPorDNI(string dni)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectByDNIStatement, SqlHelper.Param("@DNI", dni));
            return ClienteAdaptador.Current.FillPrimero(tabla);
        }

        public List<Cliente> Listar(string filtro, bool incluirInactivos)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectFiltroStatement,
                SqlHelper.Param("@IncluirInactivos", incluirInactivos),
                SqlHelper.Param("@Filtro", filtro ?? ""));
            return ClienteAdaptador.Current.FillLista(tabla);
        }

        /// <summary>Todos, incluidos los deshabilitados.</summary>
        public List<Cliente> GetAll()
        {
            return Listar("", true);
        }

        public void Add(Cliente entity)
        {
            object id = SqlHelper.ExecuteScalar(InsertStatement,
                SqlHelper.Param("@Nombre", entity.Nombre),
                SqlHelper.Param("@Apellido", entity.Apellido),
                SqlHelper.Param("@DNI", entity.DNI),
                SqlHelper.Param("@CUIT", entity.CUIT),
                SqlHelper.Param("@Domicilio", entity.Domicilio),
                SqlHelper.Param("@Localidad", entity.Localidad),
                SqlHelper.Param("@Telefono", CryptographyService.Encriptar(entity.Telefono)),   // AES
                SqlHelper.Param("@Email", CryptographyService.Encriptar(entity.Email)),         // AES
                SqlHelper.Param("@Activo", entity.Activo));

            entity.IdCliente = (int)id;
        }

        public void Update(Cliente entity)
        {
            SqlHelper.ExecuteNonQuery(UpdateStatement,
                SqlHelper.Param("@Nombre", entity.Nombre),
                SqlHelper.Param("@Apellido", entity.Apellido),
                SqlHelper.Param("@DNI", entity.DNI),
                SqlHelper.Param("@CUIT", entity.CUIT),
                SqlHelper.Param("@Domicilio", entity.Domicilio),
                SqlHelper.Param("@Localidad", entity.Localidad),
                SqlHelper.Param("@Telefono", CryptographyService.Encriptar(entity.Telefono)),   // AES
                SqlHelper.Param("@Email", CryptographyService.Encriptar(entity.Email)),         // AES
                SqlHelper.Param("@IdCliente", entity.IdCliente));
        }

        /// <summary>Baja lógica: el registro queda en la base pero deshabilitado (tiene historia).</summary>
        public void Delete(int id)
        {
            CambiarEstado(id, false);
        }

        /// <summary>Clientes con teléfono o email todavía sin encriptar (antes agranda las columnas si hace falta).</summary>
        public List<int> ListarIdsSinEncriptar()
        {
            SqlHelper.ExecuteNonQuery(AmpliarColumnasStatement);

            var tabla = SqlHelper.ExecuteDataTable(SinEncriptarStatement);
            List<int> ids = new List<int>();
            foreach (System.Data.DataRow fila in tabla.Rows)
                ids.Add((int)fila["IdCliente"]);
            return ids;
        }

        public void CambiarEstado(int clienteId, bool activo)
        {
            SqlHelper.ExecuteNonQuery(CambiarEstadoStatement,
                SqlHelper.Param("@Activo", activo),
                SqlHelper.Param("@IdCliente", clienteId));
        }
    }
}
