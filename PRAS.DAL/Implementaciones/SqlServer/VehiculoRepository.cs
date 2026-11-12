using Microsoft.Data.SqlClient;
using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;
using PRAS.Services.Exceptions.DataAccess;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Vehiculo en SQL Server (singleton).
    /// El alta "normal" de un auto se hace dentro de la compra (CompraRepository.Add).
    /// </summary>
    internal sealed class VehiculoRepository : IVehiculoRepository
    {
        #region Singleton
        private static readonly VehiculoRepository _instance = new VehiculoRepository();

        public static VehiculoRepository Current
        {
            get { return _instance; }
        }

        private VehiculoRepository()
        {
        }
        #endregion

        #region Statements
        /// <summary>Columnas del vehículo con el alias "V". Las usan también los repositorios que hacen JOIN a Vehiculo.</summary>
        internal const string ColumnasConAlias =
            "V.IdVehiculo, V.Dominio, V.Marca, V.Modelo, V.Anio, V.Color, V.NumeroChasis, V.NumeroMotor, V.EstadoVehiculo";

        private string SelectOneStatement
        {
            get => $"SELECT {ColumnasConAlias} FROM Vehiculo V WHERE V.IdVehiculo = @IdVehiculo";
        }

        private string SelectByDominioStatement
        {
            get => $"SELECT {ColumnasConAlias} FROM Vehiculo V WHERE V.Dominio = @Dominio";
        }

        private string SelectFiltroStatement
        {
            get => $@"SELECT {ColumnasConAlias}
                      FROM Vehiculo V
                      WHERE (@Estado IS NULL OR V.EstadoVehiculo = @Estado)
                        AND (@Patente = '' OR V.Dominio LIKE '%' + @Patente + '%')
                      ORDER BY V.Dominio";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO Vehiculo (Dominio, Marca, Modelo, Anio, Color, NumeroChasis, NumeroMotor, EstadoVehiculo)
                     VALUES (@Dominio, @Marca, @Modelo, @Anio, @Color, @NumeroChasis, @NumeroMotor, @EstadoVehiculo);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string UpdateStatement
        {
            get => @"UPDATE Vehiculo
                     SET Dominio = @Dominio, Marca = @Marca, Modelo = @Modelo, Anio = @Anio,
                         Color = @Color, NumeroChasis = @NumeroChasis, NumeroMotor = @NumeroMotor
                     WHERE IdVehiculo = @IdVehiculo";
        }

        private string DeleteCompraStatement
        {
            get => @"DELETE C FROM Compra C
                     INNER JOIN Vehiculo V ON V.IdVehiculo = C.IdVehiculo
                     WHERE C.IdVehiculo = @IdVehiculo AND V.EstadoVehiculo = @Comprado";
        }

        private string DeleteStatement
        {
            get => "DELETE FROM Vehiculo WHERE IdVehiculo = @IdVehiculo AND EstadoVehiculo = @Comprado";
        }
        #endregion

        public Vehiculo GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectOneStatement, SqlHelper.Param("@IdVehiculo", id));
            return VehiculoAdaptador.Current.FillPrimero(tabla);
        }

        public Vehiculo ObtenerPorDominio(string dominio)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectByDominioStatement, SqlHelper.Param("@Dominio", dominio));
            return VehiculoAdaptador.Current.FillPrimero(tabla);
        }

        public List<Vehiculo> Listar(EstadoVehiculo? estado, string patente)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectFiltroStatement,
                SqlHelper.Param("@Estado", estado.HasValue ? (int)estado.Value : null),
                SqlHelper.Param("@Patente", patente ?? ""));
            return VehiculoAdaptador.Current.FillLista(tabla);
        }

        public List<Vehiculo> GetAll()
        {
            return Listar(null, "");
        }

        public void Add(Vehiculo entity)
        {
            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                entity.IdVehiculo = Insertar(entity, cn, tx);
            });
        }

        public void Update(Vehiculo entity)
        {
            SqlHelper.ExecuteNonQuery(UpdateStatement,
                SqlHelper.Param("@Dominio", entity.Dominio),
                SqlHelper.Param("@Marca", entity.Marca),
                SqlHelper.Param("@Modelo", entity.Modelo),
                SqlHelper.Param("@Anio", entity.Anio),
                SqlHelper.Param("@Color", entity.Color),
                SqlHelper.Param("@NumeroChasis", entity.NumeroChasis),
                SqlHelper.Param("@NumeroMotor", entity.NumeroMotor),
                SqlHelper.Param("@IdVehiculo", entity.IdVehiculo));
        }

        /// <summary>Borra el auto (y su compra). Solo se puede si sigue en estado Comprado.</summary>
        public void Delete(int id)
        {
            if (!EliminarConCompra(id))
                throw new ConcurrencyException("El vehículo ya no está en estado Comprado y no se puede eliminar.");
        }

        public bool EliminarConCompra(int idVehiculo)
        {
            bool eliminado = false;

            SqlHelper.ExecuteInTransaction((cn, tx) =>
            {
                SqlHelper.ExecuteNonQuery(cn, tx, DeleteCompraStatement,
                    SqlHelper.Param("@IdVehiculo", idVehiculo),
                    SqlHelper.Param("@Comprado", (int)EstadoVehiculo.Comprado));

                int filas = SqlHelper.ExecuteNonQuery(cn, tx, DeleteStatement,
                    SqlHelper.Param("@IdVehiculo", idVehiculo),
                    SqlHelper.Param("@Comprado", (int)EstadoVehiculo.Comprado));

                eliminado = filas == 1;
            });

            return eliminado;
        }

        // ---------------------------- Dentro de una transacción (los usan otros repositorios) ----------------------------

        /// <summary>Inserta el auto dentro de una transacción abierta y devuelve el Id generado.</summary>
        internal int Insertar(Vehiculo vehiculo, SqlConnection cn, SqlTransaction tx)
        {
            object id = SqlHelper.ExecuteScalar(cn, tx, InsertStatement,
                SqlHelper.Param("@Dominio", vehiculo.Dominio),
                SqlHelper.Param("@Marca", vehiculo.Marca),
                SqlHelper.Param("@Modelo", vehiculo.Modelo),
                SqlHelper.Param("@Anio", vehiculo.Anio),
                SqlHelper.Param("@Color", vehiculo.Color),
                SqlHelper.Param("@NumeroChasis", vehiculo.NumeroChasis),
                SqlHelper.Param("@NumeroMotor", vehiculo.NumeroMotor),
                SqlHelper.Param("@EstadoVehiculo", (int)vehiculo.EstadoVehiculo));

            return (int)id;
        }

        /// <summary>
        /// Cambia el estado del auto solo si está en alguno de los estados esperados.
        /// Si no (por ejemplo, otro usuario lo cambió recién), se cancela toda la transacción.
        /// </summary>
        internal void CambiarEstado(int idVehiculo, EstadoVehiculo[] estadosAnteriores, EstadoVehiculo nuevo,
            SqlConnection cn, SqlTransaction tx)
        {
            string lista = string.Join(",", estadosAnteriores.Select(e => ((int)e).ToString()));

            int filas = SqlHelper.ExecuteNonQuery(cn, tx,
                $@"UPDATE Vehiculo SET EstadoVehiculo = @Nuevo
                   WHERE IdVehiculo = @IdVehiculo AND EstadoVehiculo IN ({lista})",
                SqlHelper.Param("@Nuevo", (int)nuevo),
                SqlHelper.Param("@IdVehiculo", idVehiculo));

            if (filas != 1)
                throw new ConcurrencyException("El vehículo cambió de estado mientras se hacía la operación. Vuelva a intentarlo.");
        }
    }
}
