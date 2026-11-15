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

        /// <summary>
        /// Ficha del vehículo: el auto y, al lado, lo último de cada etapa.
        /// OUTER APPLY = "traeme el último registro relacionado, si hay; si no, dejalo vacío"
        /// (un auto recién comprado no tiene diagnóstico ni venta, y igual tiene que aparecer).
        /// </summary>
        private string SelectFichaStatement
        {
            get => $@"SELECT {ColumnasConAlias},
                             C.FechaCompra, C.PrecioCompra, C.FormaPago AS FormaPagoCompra,
                             VEN.Apellido + ', ' + VEN.Nombre AS Vendedor, C.Observaciones AS ObservacionesCompra,
                             D.FechaDiagnostico, D.DescripcionDanios, D.EstadoDiagnostico,
                             P.MontoTotal AS MontoPresupuesto,
                             OT.FechaCreacion AS FechaOrden, OT.EstadoOrden, OT.Prioridad,
                             A.RazonSocial AS Agencia, CO.FechaEntrega AS FechaEntregaAgencia, CO.EstadoConsignacion,
                             PU.FechaPublicacion, PU.Plataforma, PU.PrecioPublicado, PU.EstadoPublicacion,
                             VT.FechaVenta, VT.PrecioVenta, COMP.Apellido + ', ' + COMP.Nombre AS Comprador,
                             VT.FormaPago AS FormaPagoVenta
                      FROM Vehiculo V
                      LEFT JOIN Compra C    ON C.IdVehiculo = V.IdVehiculo
                      LEFT JOIN Cliente VEN ON VEN.IdCliente = C.IdCliente
                      OUTER APPLY (SELECT TOP 1 FechaDiagnostico, DescripcionDanios, EstadoDiagnostico FROM Diagnostico
                                   WHERE IdVehiculo = V.IdVehiculo ORDER BY FechaDiagnostico DESC, IdDiagnostico DESC) D
                      OUTER APPLY (SELECT TOP 1 MontoTotal FROM Presupuesto
                                   WHERE IdVehiculo = V.IdVehiculo ORDER BY FechaEmision DESC, IdPresupuesto DESC) P
                      OUTER APPLY (SELECT TOP 1 FechaCreacion, EstadoOrden, Prioridad FROM OrdenTrabajo
                                   WHERE IdVehiculo = V.IdVehiculo ORDER BY FechaCreacion DESC, IdOrdenTrabajo DESC) OT
                      OUTER APPLY (SELECT TOP 1 IdConsignacion, IdAgencia, FechaEntrega, EstadoConsignacion FROM Consignacion
                                   WHERE IdVehiculo = V.IdVehiculo ORDER BY FechaEntrega DESC, IdConsignacion DESC) CO
                      LEFT JOIN Agencia A ON A.IdAgencia = CO.IdAgencia
                      OUTER APPLY (SELECT TOP 1 FechaPublicacion, Plataforma, PrecioPublicado, EstadoPublicacion FROM Publicacion
                                   WHERE IdVehiculo = V.IdVehiculo ORDER BY FechaPublicacion DESC, IdPublicacion DESC) PU
                      LEFT JOIN Venta VT     ON VT.IdConsignacion = CO.IdConsignacion
                      LEFT JOIN Cliente COMP ON COMP.IdCliente = VT.IdCliente
                      WHERE V.IdVehiculo = @IdVehiculo";
        }

        /// <summary>Tareas de la última orden de trabajo del auto, con el mecánico de cada una.</summary>
        private string SelectTareasFichaStatement
        {
            get => @"SELECT T.DescripcionTarea, T.EstadoTarea, T.CostoEstimado,
                            M.Apellido + ', ' + M.Nombre AS Mecanico
                     FROM TareaOrdenTrabajo T
                     INNER JOIN Mecanico M ON M.IdMecanico = T.IdMecanico
                     WHERE T.IdOrdenTrabajo = (SELECT TOP 1 IdOrdenTrabajo FROM OrdenTrabajo
                                               WHERE IdVehiculo = @IdVehiculo
                                               ORDER BY FechaCreacion DESC, IdOrdenTrabajo DESC)
                     ORDER BY T.IdTarea";
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

        public FichaVehiculo ObtenerFicha(int idVehiculo)
        {
            var tabla = SqlHelper.ExecuteDataTable(SelectFichaStatement, SqlHelper.Param("@IdVehiculo", idVehiculo));
            FichaVehiculo ficha = FichaVehiculoAdaptador.Current.FillPrimero(tabla);
            if (ficha == null)
                return null;

            var tareas = SqlHelper.ExecuteDataTable(SelectTareasFichaStatement, SqlHelper.Param("@IdVehiculo", idVehiculo));
            foreach (System.Data.DataRow fila in tareas.Rows)
                ficha.Tareas.Add(FichaVehiculoAdaptador.Current.FillTarea(fila));

            return ficha;
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
