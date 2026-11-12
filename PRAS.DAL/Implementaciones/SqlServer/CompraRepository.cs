using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Compra (singleton). La compra es la única forma en que un auto
    /// entra al sistema: el Add inserta el Vehiculo y la Compra en la misma transacción.
    /// </summary>
    internal sealed class CompraRepository : ICompraRepository
    {
        #region Singleton
        private static readonly CompraRepository _instance = new CompraRepository();

        public static CompraRepository Current
        {
            get { return _instance; }
        }

        private CompraRepository()
        {
        }
        #endregion

        #region Statements
        /// <summary>Compra + vehículo + datos del cliente que se muestran.</summary>
        private const string SelectConCliente =
            @"SELECT C.IdCompra, C.IdCliente, C.FechaCompra, C.PrecioCompra, C.FormaPago, C.Observaciones,
                     CL.Nombre AS ClienteNombre, CL.Apellido AS ClienteApellido, CL.DNI AS ClienteDNI, " +
                     VehiculoRepository.ColumnasConAlias + @"
              FROM Compra C
              INNER JOIN Vehiculo V ON V.IdVehiculo = C.IdVehiculo
              INNER JOIN Cliente CL ON CL.IdCliente = C.IdCliente";

        private string SelectOneStatement
        {
            get => $"{SelectConCliente} WHERE C.IdCompra = @IdCompra";
        }

        private string SelectByVehiculoStatement
        {
            get => $"{SelectConCliente} WHERE C.IdVehiculo = @IdVehiculo";
        }

        private string SelectFiltroStatement
        {
            get => $@"{SelectConCliente}
                      WHERE C.FechaCompra >= @Desde AND C.FechaCompra < @HastaExclusive
                        AND (@Filtro = ''
                             OR V.Dominio   LIKE '%' + @Filtro + '%'
                             OR V.Marca     LIKE '%' + @Filtro + '%'
                             OR V.Modelo    LIKE '%' + @Filtro + '%'
                             OR CL.Apellido LIKE '%' + @Filtro + '%'
                             OR CL.DNI      LIKE '%' + @Filtro + '%')
                      ORDER BY C.FechaCompra DESC, C.IdCompra DESC";
        }

        private string SelectByClienteStatement
        {
            get => $@"SELECT C.IdCompra, C.IdCliente, C.FechaCompra, C.PrecioCompra, C.FormaPago, C.Observaciones,
                             {VehiculoRepository.ColumnasConAlias}
                      FROM Compra C
                      INNER JOIN Vehiculo V ON V.IdVehiculo = C.IdVehiculo
                      WHERE C.IdCliente = @IdCliente
                      ORDER BY C.FechaCompra DESC";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO Compra (IdVehiculo, IdCliente, FechaCompra, PrecioCompra, FormaPago, Observaciones)
                     VALUES (@IdVehiculo, @IdCliente, @FechaCompra, @PrecioCompra, @FormaPago, @Observaciones);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }
        #endregion

        public Compra GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdCompra", id));
            return CompraAdaptador.Current.FillPrimero(tabla);
        }

        /// <summary>
        /// Registra la compra: 1) alta del vehículo (entra como Comprado), 2) alta de la compra.
        /// Le carga a la compra (y a su vehículo) los Id generados.
        /// </summary>
        public void Add(Compra entity)
        {
            int idCompra = 0;

            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                entity.IdVehiculo = VehiculoRepository.Current.Insertar(entity.Vehiculo, cn, tx);
                entity.Vehiculo.IdVehiculo = entity.IdVehiculo;

                idCompra = (int)SqlHelper.ExecuteScalar(cn, tx, InsertStatement,
                    SqlHelper.Param("@IdVehiculo", entity.IdVehiculo),
                    SqlHelper.Param("@IdCliente", entity.IdCliente),
                    SqlHelper.Param("@FechaCompra", entity.FechaCompra),
                    SqlHelper.Param("@PrecioCompra", entity.PrecioCompra),
                    SqlHelper.Param("@FormaPago", entity.FormaPago),
                    SqlHelper.Param("@Observaciones", entity.Observaciones));
            });

            entity.IdCompra = idCompra;
        }

        public List<Compra> Listar(DateTime desde, DateTime hasta, string filtro)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectFiltroStatement,
                SqlHelper.Param("@Desde", desde.Date),
                SqlHelper.Param("@HastaExclusive", hasta.Date.AddDays(1)),   // con "menor al día siguiente" entra todo el último día
                SqlHelper.Param("@Filtro", filtro ?? ""));
            return CompraAdaptador.Current.FillLista(tabla);
        }

        public Compra ObtenerPorVehiculo(int idVehiculo)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectByVehiculoStatement, SqlHelper.Param("@IdVehiculo", idVehiculo));
            return CompraAdaptador.Current.FillPrimero(tabla);
        }

        public List<Compra> ListarPorCliente(int idCliente)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectByClienteStatement, SqlHelper.Param("@IdCliente", idCliente));
            return CompraAdaptador.Current.FillLista(tabla);
        }
    }
}
