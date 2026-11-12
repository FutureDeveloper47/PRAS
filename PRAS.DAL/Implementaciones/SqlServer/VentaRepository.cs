using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Venta (singleton). La venta "cuelga" de la consignación,
    /// que es la que tiene el auto y la agencia.
    /// </summary>
    internal sealed class VentaRepository : IVentaRepository
    {
        #region Singleton
        private static readonly VentaRepository _instance = new VentaRepository();

        public static VentaRepository Current
        {
            get { return _instance; }
        }

        private VentaRepository()
        {
        }
        #endregion

        #region Statements
        /// <summary>Venta + consignación + agencia + auto + datos del cliente que se muestran.</summary>
        private const string SelectConCliente =
            @"SELECT VT.IdVenta, VT.IdConsignacion, VT.IdCliente, VT.FechaVenta,
                     VT.PrecioVenta, VT.MontoComision, VT.FormaPago,
                     CO.IdAgencia, CO.PrecioMinimoVenta, CO.PorcentajeComision,
                     A.RazonSocial AS AgenciaRazonSocial,
                     C.Nombre AS ClienteNombre, C.Apellido AS ClienteApellido, C.DNI AS ClienteDNI, " +
                     VehiculoRepository.ColumnasConAlias + @"
              FROM Venta VT
              INNER JOIN Consignacion CO ON CO.IdConsignacion = VT.IdConsignacion
              INNER JOIN Vehiculo V      ON V.IdVehiculo      = CO.IdVehiculo
              INNER JOIN Agencia A       ON A.IdAgencia       = CO.IdAgencia
              INNER JOIN Cliente C       ON C.IdCliente       = VT.IdCliente";

        private string SelectOneStatement
        {
            get => $"{SelectConCliente} WHERE VT.IdVenta = @IdVenta";
        }

        private string SelectFiltroStatement
        {
            get => $@"{SelectConCliente}
                      WHERE VT.FechaVenta >= @Desde AND VT.FechaVenta < @HastaMasUnDia
                        AND (@Filtro = '' OR V.Dominio LIKE '%' + @Filtro + '%' OR V.Marca LIKE '%' + @Filtro + '%'
                             OR V.Modelo LIKE '%' + @Filtro + '%' OR C.Apellido LIKE '%' + @Filtro + '%'
                             OR C.DNI LIKE '%' + @Filtro + '%' OR A.RazonSocial LIKE '%' + @Filtro + '%')
                      ORDER BY VT.FechaVenta DESC, VT.IdVenta DESC";
        }

        private string SelectByClienteStatement
        {
            get => $@"SELECT VT.IdVenta, VT.IdConsignacion, VT.IdCliente, VT.FechaVenta,
                             VT.PrecioVenta, VT.MontoComision, VT.FormaPago,
                             CO.IdAgencia, A.RazonSocial AS AgenciaRazonSocial,
                             {VehiculoRepository.ColumnasConAlias}
                      FROM Venta VT
                      INNER JOIN Consignacion CO ON CO.IdConsignacion = VT.IdConsignacion
                      INNER JOIN Vehiculo V      ON V.IdVehiculo      = CO.IdVehiculo
                      INNER JOIN Agencia A       ON A.IdAgencia       = CO.IdAgencia
                      WHERE VT.IdCliente = @IdCliente
                      ORDER BY VT.FechaVenta DESC";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO Venta (IdConsignacion, IdCliente, FechaVenta, PrecioVenta, MontoComision, FormaPago)
                     VALUES (@IdConsignacion, @IdCliente, @Fecha, @Precio, @Comision, @FormaPago);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }
        #endregion

        public Venta GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdVenta", id));
            return VentaAdaptador.Current.FillPrimero(tabla);
        }

        public List<Venta> Listar(DateTime desde, DateTime hasta, string filtro)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectFiltroStatement,
                SqlHelper.Param("@Desde", desde.Date),
                SqlHelper.Param("@HastaMasUnDia", hasta.Date.AddDays(1)),
                SqlHelper.Param("@Filtro", filtro ?? ""));
            return VentaAdaptador.Current.FillLista(tabla);
        }

        public List<Venta> ListarPorCliente(int idCliente)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectByClienteStatement, SqlHelper.Param("@IdCliente", idCliente));
            return VentaAdaptador.Current.FillLista(tabla);
        }

        public int Registrar(Venta venta, int idVehiculo)
        {
            int idVenta = 0;

            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                // 1) La consignación pasa a Vendida (si otro usuario ya la cerró, se cancela todo).
                ConsignacionRepository.Current.Cerrar(venta.IdConsignacion, Estados.ConsignacionVendida, venta.FechaVenta, cn, tx);

                // 2) La venta.
                idVenta = (int)SqlHelper.ExecuteScalar(cn, tx, InsertStatement,
                    SqlHelper.Param("@IdConsignacion", venta.IdConsignacion),
                    SqlHelper.Param("@IdCliente", venta.IdCliente),
                    SqlHelper.Param("@Fecha", venta.FechaVenta),
                    SqlHelper.Param("@Precio", venta.PrecioVenta),
                    SqlHelper.Param("@Comision", venta.MontoComision),
                    SqlHelper.Param("@FormaPago", venta.FormaPago));

                // 3) Se baja el aviso y 4) el auto queda Vendido.
                PublicacionRepository.Current.FinalizarActivas(idVehiculo, venta.FechaVenta, cn, tx);

                VehiculoRepository.Current.CambiarEstado(idVehiculo,
                    new[] { EstadoVehiculo.EnAgencia, EstadoVehiculo.Publicado }, EstadoVehiculo.Vendido, cn, tx);
            });

            venta.IdVenta = idVenta;
            return idVenta;
        }
    }
}
