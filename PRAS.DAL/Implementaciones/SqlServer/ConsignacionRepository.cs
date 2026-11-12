using Microsoft.Data.SqlClient;
using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;
using PRAS.Services.Exceptions.DataAccess;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Consignacion (singleton): el auto reparado se le deja a una agencia
    /// para que lo venda. Todo lo que cambia el estado del auto va en una transacción.
    /// </summary>
    internal sealed class ConsignacionRepository : IConsignacionRepository
    {
        #region Singleton
        private static readonly ConsignacionRepository _instance = new ConsignacionRepository();

        public static ConsignacionRepository Current
        {
            get { return _instance; }
        }

        private ConsignacionRepository()
        {
        }
        #endregion

        #region Statements
        private const string SelectBase =
            @"SELECT CO.IdConsignacion, CO.IdVehiculo, CO.IdAgencia, CO.FechaEntrega, CO.FechaRetiro,
                     CO.PrecioMinimoVenta, CO.PorcentajeComision, CO.EstadoConsignacion,
                     A.RazonSocial AS AgenciaRazonSocial, " +
                     VehiculoRepository.ColumnasConAlias + @"
              FROM Consignacion CO
              INNER JOIN Agencia A  ON A.IdAgencia = CO.IdAgencia
              INNER JOIN Vehiculo V ON V.IdVehiculo = CO.IdVehiculo";

        private string SelectOneStatement
        {
            get => $"{SelectBase} WHERE CO.IdConsignacion = @Id";
        }

        private string SelectVigentePorVehiculoStatement
        {
            get => $"{SelectBase} WHERE CO.IdVehiculo = @IdVehiculo AND CO.EstadoConsignacion = @Vigente";
        }

        private string SelectFiltroStatement
        {
            get => $@"{SelectBase}
                      WHERE (@SoloVigentes = 0 OR CO.EstadoConsignacion = @Vigente)
                      ORDER BY CO.FechaEntrega DESC, CO.IdConsignacion DESC";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO Consignacion (IdVehiculo, IdAgencia, FechaEntrega, PrecioMinimoVenta,
                                               PorcentajeComision, EstadoConsignacion)
                     VALUES (@IdVehiculo, @IdAgencia, @Fecha, @PrecioMinimo, @Comision, @Estado);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string SelectVehiculoVigenteStatement
        {
            get => "SELECT IdVehiculo FROM Consignacion WHERE IdConsignacion = @Id AND EstadoConsignacion = @Vigente";
        }

        private string CerrarStatement
        {
            get => @"UPDATE Consignacion SET EstadoConsignacion = @NuevoEstado, FechaRetiro = @Fecha
                     WHERE IdConsignacion = @Id AND EstadoConsignacion = @Vigente";
        }
        #endregion

        public Consignacion GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@Id", id));
            return ConsignacionAdaptador.Current.FillPrimero(tabla);
        }

        public List<Consignacion> Listar(bool soloVigentes)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectFiltroStatement,
                SqlHelper.Param("@SoloVigentes", soloVigentes),
                SqlHelper.Param("@Vigente", Estados.ConsignacionVigente));
            return ConsignacionAdaptador.Current.FillLista(tabla);
        }

        public Consignacion ObtenerVigentePorVehiculo(int idVehiculo)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectVigentePorVehiculoStatement,
                SqlHelper.Param("@IdVehiculo", idVehiculo),
                SqlHelper.Param("@Vigente", Estados.ConsignacionVigente));
            return ConsignacionAdaptador.Current.FillPrimero(tabla);
        }

        /// <summary>
        /// Entrega el auto a la agencia: inserta la consignación (Vigente) y el auto pasa de
        /// Reparado a En agencia. Le carga a la consignación el Id generado.
        /// </summary>
        public void Add(Consignacion entity)
        {
            int id = 0;

            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                id = (int)SqlHelper.ExecuteScalar(cn, tx, InsertStatement,
                    SqlHelper.Param("@IdVehiculo", entity.IdVehiculo),
                    SqlHelper.Param("@IdAgencia", entity.IdAgencia),
                    SqlHelper.Param("@Fecha", entity.FechaEntrega),
                    SqlHelper.Param("@PrecioMinimo", entity.PrecioMinimoVenta),
                    SqlHelper.Param("@Comision", entity.PorcentajeComision),
                    SqlHelper.Param("@Estado", Estados.ConsignacionVigente));

                VehiculoRepository.Current.CambiarEstado(entity.IdVehiculo,
                    new[] { EstadoVehiculo.Reparado }, EstadoVehiculo.EnAgencia, cn, tx);
            });

            entity.IdConsignacion = id;
            entity.EstadoConsignacion = Estados.ConsignacionVigente;
        }

        /// <summary>
        /// Retiramos el auto de la agencia (no se vendió). En la misma transacción: la consignación
        /// pasa a Retirada, si tenía una publicación activa se finaliza y el auto vuelve a Reparado.
        /// </summary>
        public void RegistrarRetiro(int idConsignacion, DateTime fechaRetiro)
        {
            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                object idVehiculo = SqlHelper.ExecuteScalar(cn, tx, SelectVehiculoVigenteStatement,
                    SqlHelper.Param("@Id", idConsignacion),
                    SqlHelper.Param("@Vigente", Estados.ConsignacionVigente));

                if (idVehiculo == null)
                    throw new ConcurrencyException("La consignación ya no está vigente (otro usuario la modificó).");

                Cerrar(idConsignacion, Estados.ConsignacionRetirada, fechaRetiro, cn, tx);

                PublicacionRepository.Current.FinalizarActivas((int)idVehiculo, fechaRetiro, cn, tx);

                VehiculoRepository.Current.CambiarEstado((int)idVehiculo,
                    new[] { EstadoVehiculo.EnAgencia, EstadoVehiculo.Publicado }, EstadoVehiculo.Reparado, cn, tx);
            });
        }

        // ---------------------------- Dentro de una transacción ----------------------------

        /// <summary>
        /// Cierra una consignación vigente (Retirada o Vendida) con su fecha.
        /// Si ya no estaba vigente, cancela la transacción.
        /// </summary>
        internal void Cerrar(int idConsignacion, string nuevoEstado, DateTime fecha, SqlConnection cn, SqlTransaction tx)
        {
            int filas = SqlHelper.ExecuteNonQuery(cn, tx, CerrarStatement,
                SqlHelper.Param("@NuevoEstado", nuevoEstado),
                SqlHelper.Param("@Fecha", fecha),
                SqlHelper.Param("@Id", idConsignacion),
                SqlHelper.Param("@Vigente", Estados.ConsignacionVigente));

            if (filas != 1)
                throw new ConcurrencyException("La consignación del vehículo ya no está vigente (otro usuario la modificó).");
        }
    }
}
