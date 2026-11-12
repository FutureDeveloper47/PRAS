using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Proveedor en SQL Server (singleton).
    /// Le compramos repuestos por pedido (no tenemos stock).
    /// </summary>
    internal sealed class ProveedorRepository : IProveedorRepository
    {
        #region Singleton
        private static readonly ProveedorRepository _instance = new ProveedorRepository();

        public static ProveedorRepository Current
        {
            get { return _instance; }
        }

        private ProveedorRepository()
        {
        }
        #endregion

        #region Statements
        private const string Columnas = "IdProveedor, RazonSocial, CUIT, Domicilio, Telefono, Email, Activo";

        private string SelectOneStatement
        {
            get => $"SELECT {Columnas} FROM Proveedor WHERE IdProveedor = @IdProveedor";
        }

        private string SelectByCUITStatement
        {
            get => $"SELECT {Columnas} FROM Proveedor WHERE CUIT = @CUIT";
        }

        private string SelectFiltroStatement
        {
            get => $@"SELECT {Columnas} FROM Proveedor
                      WHERE (@IncluirInactivos = 1 OR Activo = 1)
                        AND (@Filtro = ''
                             OR RazonSocial LIKE '%' + @Filtro + '%'
                             OR CUIT LIKE '%' + @Filtro + '%')
                      ORDER BY RazonSocial";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO Proveedor (RazonSocial, CUIT, Domicilio, Telefono, Email, Activo)
                     VALUES (@RazonSocial, @CUIT, @Domicilio, @Telefono, @Email, @Activo);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string UpdateStatement
        {
            get => @"UPDATE Proveedor
                     SET RazonSocial = @RazonSocial, CUIT = @CUIT, Domicilio = @Domicilio, Telefono = @Telefono, Email = @Email
                     WHERE IdProveedor = @IdProveedor";
        }

        private string CambiarEstadoStatement
        {
            get => "UPDATE Proveedor SET Activo = @Activo WHERE IdProveedor = @IdProveedor";
        }

        private string PedidosPendientesStatement
        {
            get => @"SELECT COUNT(*) FROM PedidoRepuesto
                     WHERE IdProveedor = @IdProveedor AND EstadoPedido = @Pendiente";
        }
        #endregion

        public Proveedor GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdProveedor", id));
            return ProveedorAdaptador.Current.FillPrimero(tabla);
        }

        public Proveedor ObtenerPorCUIT(string cuit)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectByCUITStatement, SqlHelper.Param("@CUIT", cuit));
            return ProveedorAdaptador.Current.FillPrimero(tabla);
        }

        public List<Proveedor> Listar(string filtro, bool incluirInactivos)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectFiltroStatement,
                SqlHelper.Param("@IncluirInactivos", incluirInactivos),
                SqlHelper.Param("@Filtro", filtro ?? ""));
            return ProveedorAdaptador.Current.FillLista(tabla);
        }

        /// <summary>Todos, incluidos los deshabilitados.</summary>
        public List<Proveedor> GetAll()
        {
            return Listar("", true);
        }

        public void Add(Proveedor entity)
        {
            object id = SqlHelper.ExecuteScalar(InsertStatement,
                SqlHelper.Param("@RazonSocial", entity.RazonSocial),
                SqlHelper.Param("@CUIT", entity.CUIT),
                SqlHelper.Param("@Domicilio", entity.Domicilio),
                SqlHelper.Param("@Telefono", entity.Telefono),
                SqlHelper.Param("@Email", entity.Email),
                SqlHelper.Param("@Activo", entity.Activo));

            entity.IdProveedor = (int)id;
        }

        public void Update(Proveedor entity)
        {
            SqlHelper.ExecuteNonQuery(UpdateStatement,
                SqlHelper.Param("@RazonSocial", entity.RazonSocial),
                SqlHelper.Param("@CUIT", entity.CUIT),
                SqlHelper.Param("@Domicilio", entity.Domicilio),
                SqlHelper.Param("@Telefono", entity.Telefono),
                SqlHelper.Param("@Email", entity.Email),
                SqlHelper.Param("@IdProveedor", entity.IdProveedor));
        }

        /// <summary>Baja lógica: el registro queda en la base pero deshabilitado (tiene historia).</summary>
        public void Delete(int id)
        {
            CambiarEstado(id, false);
        }

        public void CambiarEstado(int proveedorId, bool activo)
        {
            SqlHelper.ExecuteNonQuery(CambiarEstadoStatement,
                SqlHelper.Param("@Activo", activo),
                SqlHelper.Param("@IdProveedor", proveedorId));
        }

        public bool TienePedidosPendientes(int idProveedor)
        {
            object cantidad = SqlHelper.ExecuteScalar(PedidosPendientesStatement,
                SqlHelper.Param("@IdProveedor", idProveedor),
                SqlHelper.Param("@Pendiente", Estados.PedidoPendiente));
            return (int)cantidad > 0;
        }
    }
}
