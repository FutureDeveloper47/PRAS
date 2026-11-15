using Microsoft.Data.SqlClient;
using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;
using PRAS.Services.Exceptions.DataAccess;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Publicacion (singleton): el aviso de venta de un auto que está en la
    /// agencia (MercadoLibre, Marketplace, Instagram, etc.).
    /// </summary>
    internal sealed class PublicacionRepository : IPublicacionRepository
    {
        #region Singleton
        private static readonly PublicacionRepository _instance = new PublicacionRepository();

        public static PublicacionRepository Current
        {
            get { return _instance; }
        }

        private PublicacionRepository()
        {
        }
        #endregion

        #region Statements
        private const string SelectBase =
            @"SELECT P.IdPublicacion, P.IdVehiculo, P.FechaPublicacion, P.FechaFinalizacion, P.PrecioPublicado,
                     P.Plataforma, P.Descripcion, P.EstadoPublicacion, P.Observaciones, " +
                     VehiculoRepository.ColumnasConAlias + @"
              FROM Publicacion P
              INNER JOIN Vehiculo V ON V.IdVehiculo = P.IdVehiculo";

        private string SelectOneStatement
        {
            get => $"{SelectBase} WHERE P.IdPublicacion = @Id";
        }

        private string SelectActivaPorVehiculoStatement
        {
            get => $"{SelectBase} WHERE P.IdVehiculo = @IdVehiculo AND P.EstadoPublicacion = @Activa";
        }

        private string SelectFiltroStatement
        {
            get => $@"{SelectBase}
                      WHERE (@SoloActivas = 0 OR P.EstadoPublicacion = @Activa)
                      ORDER BY P.FechaPublicacion DESC, P.IdPublicacion DESC";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO Publicacion (IdVehiculo, FechaPublicacion, PrecioPublicado, Plataforma,
                                              Descripcion, EstadoPublicacion, Observaciones)
                     VALUES (@IdVehiculo, @Fecha, @Precio, @Plataforma, @Descripcion, @Estado, @Observaciones);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string UpdateStatement
        {
            // Solo se modifica si sigue Activa: si otro usuario la finalizó o se vendió el auto, no toca nada.
            get => @"UPDATE Publicacion SET PrecioPublicado = @Precio, Plataforma = @Plataforma, Descripcion = @Descripcion,
                                            Observaciones = @Observaciones
                     WHERE IdPublicacion = @Id AND EstadoPublicacion = @Activa";
        }

        private string SelectVehiculoActivaStatement
        {
            get => "SELECT IdVehiculo FROM Publicacion WHERE IdPublicacion = @Id AND EstadoPublicacion = @Activa";
        }

        private string FinalizarActivasStatement
        {
            get => @"UPDATE Publicacion SET EstadoPublicacion = @Finalizada, FechaFinalizacion = @Fecha
                     WHERE IdVehiculo = @IdVehiculo AND EstadoPublicacion = @Activa";
        }
        #endregion

        public Publicacion GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@Id", id));
            return PublicacionAdaptador.Current.FillPrimero(tabla);
        }

        public List<Publicacion> Listar(bool soloActivas)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectFiltroStatement,
                SqlHelper.Param("@SoloActivas", soloActivas),
                SqlHelper.Param("@Activa", Estados.PublicacionActiva));
            return PublicacionAdaptador.Current.FillLista(tabla);
        }

        public Publicacion ObtenerActivaPorVehiculo(int idVehiculo)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectActivaPorVehiculoStatement,
                SqlHelper.Param("@IdVehiculo", idVehiculo),
                SqlHelper.Param("@Activa", Estados.PublicacionActiva));
            return PublicacionAdaptador.Current.FillPrimero(tabla);
        }

        /// <summary>Publica el auto: inserta la publicación (Activa) y el auto pasa de En agencia a Publicado.</summary>
        public void Add(Publicacion entity)
        {
            int id = 0;

            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                id = (int)SqlHelper.ExecuteScalar(cn, tx, InsertStatement,
                    SqlHelper.Param("@IdVehiculo", entity.IdVehiculo),
                    SqlHelper.Param("@Fecha", entity.FechaPublicacion),
                    SqlHelper.Param("@Precio", entity.PrecioPublicado),
                    SqlHelper.Param("@Plataforma", entity.Plataforma),
                    SqlHelper.Param("@Descripcion", entity.Descripcion),
                    SqlHelper.Param("@Estado", Estados.PublicacionActiva),
                    SqlHelper.Param("@Observaciones", entity.Observaciones));

                // La ficha del auto (km, combustible...) se graba en la misma transacción.
                if (entity.Vehiculo != null)
                    VehiculoRepository.Current.ActualizarFicha(entity.Vehiculo, cn, tx);

                VehiculoRepository.Current.CambiarEstado(entity.IdVehiculo,
                    new[] { EstadoVehiculo.EnAgencia }, EstadoVehiculo.Publicado, cn, tx);
            });

            entity.IdPublicacion = id;
            entity.EstadoPublicacion = Estados.PublicacionActiva;
        }

        /// <summary>
        /// Modifica plataforma, precio, descripción y observaciones de una publicación activa,
        /// y la ficha del auto, todo en una transacción.
        /// Si no se actualizó ninguna fila es porque ya no está activa: otro usuario la cambió.
        /// </summary>
        public void Modificar(Publicacion entity)
        {
            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                int filas = SqlHelper.ExecuteNonQuery(cn, tx, UpdateStatement,
                    SqlHelper.Param("@Precio", entity.PrecioPublicado),
                    SqlHelper.Param("@Plataforma", entity.Plataforma),
                    SqlHelper.Param("@Descripcion", entity.Descripcion),
                    SqlHelper.Param("@Observaciones", entity.Observaciones),
                    SqlHelper.Param("@Id", entity.IdPublicacion),
                    SqlHelper.Param("@Activa", Estados.PublicacionActiva));

                if (filas == 0)
                    throw new ConcurrencyException("La publicación ya no está activa (otro usuario la modificó).");

                if (entity.Vehiculo != null)
                    VehiculoRepository.Current.ActualizarFicha(entity.Vehiculo, cn, tx);
            });
        }

        /// <summary>Da de baja el aviso (sin vender): la publicación se finaliza y el auto vuelve a En agencia.</summary>
        public void Finalizar(int idPublicacion, DateTime fecha)
        {
            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                object idVehiculo = SqlHelper.ExecuteScalar(cn, tx, SelectVehiculoActivaStatement,
                    SqlHelper.Param("@Id", idPublicacion),
                    SqlHelper.Param("@Activa", Estados.PublicacionActiva));

                if (idVehiculo == null)
                    throw new ConcurrencyException("La publicación ya no está activa (otro usuario la modificó).");

                FinalizarActivas((int)idVehiculo, fecha, cn, tx);

                VehiculoRepository.Current.CambiarEstado((int)idVehiculo,
                    new[] { EstadoVehiculo.Publicado }, EstadoVehiculo.EnAgencia, cn, tx);
            });
        }

        // ---------------------------- Dentro de una transacción ----------------------------

        /// <summary>Finaliza las publicaciones activas del auto (la usan el retiro de la consignación y la venta).</summary>
        internal void FinalizarActivas(int idVehiculo, DateTime fecha, SqlConnection cn, SqlTransaction tx)
        {
            SqlHelper.ExecuteNonQuery(cn, tx, FinalizarActivasStatement,
                SqlHelper.Param("@Finalizada", Estados.PublicacionFinalizada),
                SqlHelper.Param("@Fecha", fecha),
                SqlHelper.Param("@IdVehiculo", idVehiculo),
                SqlHelper.Param("@Activa", Estados.PublicacionActiva));
        }
    }
}
