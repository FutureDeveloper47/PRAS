using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Consultas de solo lectura para la pantalla de Inicio (singleton). No guarda nada:
    /// cuenta y lista lo que ya está en la base.
    /// </summary>
    internal sealed class InicioRepository : IInicioRepository
    {
        #region Singleton
        private static readonly InicioRepository _instance = new InicioRepository();

        public static InicioRepository Current
        {
            get { return _instance; }
        }

        private InicioRepository()
        {
        }
        #endregion

        // Los mismos números que el enum EstadoVehiculo.
        private const int EstadoEnAgencia = 5;
        private const int EstadoPublicado = 6;

        /// <summary>Cuántos vehículos hay en un estado determinado (ej: EnReparacion).</summary>
        public int ContarVehiculos(EstadoVehiculo estado)
        {
            object resultado = SqlHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM Vehiculo WHERE EstadoVehiculo = @Estado",
                SqlHelper.Param("@Estado", (int)estado));

            return (int)resultado;
        }

        /// <summary>
        /// Cuántas compras hay entre dos fechas.
        /// "desde" se incluye y "hastaExclusive" NO (ej: del 1/9 al 1/10 = todo septiembre).
        /// </summary>
        public int ContarCompras(DateTime desde, DateTime hastaExclusive)
        {
            object resultado = SqlHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM Compra WHERE FechaCompra >= @Desde AND FechaCompra < @Hasta",
                SqlHelper.Param("@Desde", desde),
                SqlHelper.Param("@Hasta", hastaExclusive));

            return (int)resultado;
        }

        /// <summary>Cuántas ventas hay entre dos fechas (mismo criterio que ContarCompras).</summary>
        public int ContarVentas(DateTime desde, DateTime hastaExclusive)
        {
            object resultado = SqlHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM Venta WHERE FechaVenta >= @Desde AND FechaVenta < @Hasta",
                SqlHelper.Param("@Desde", desde),
                SqlHelper.Param("@Hasta", hastaExclusive));

            return (int)resultado;
        }

        /// <summary>
        /// Últimos movimientos: junta las compras y las ventas en una sola lista
        /// (UNION ALL = "pegar una lista debajo de la otra") y se queda con las más nuevas.
        /// </summary>
        public List<ActividadReciente> ListarActividadReciente(int cantidad)
        {
            var tabla = SqlHelper.ExecuteDataTable(
                @"SELECT TOP (@Cantidad) IdVehiculo, Fecha, Tipo, Marca, Modelo, Dominio
                  FROM (
                        SELECT V.IdVehiculo, C.FechaCompra AS Fecha, 'Ingreso' AS Tipo, V.Marca, V.Modelo, V.Dominio
                        FROM Compra C
                        INNER JOIN Vehiculo V ON V.IdVehiculo = C.IdVehiculo

                        UNION ALL

                        SELECT V.IdVehiculo, VT.FechaVenta AS Fecha, 'Venta' AS Tipo, V.Marca, V.Modelo, V.Dominio
                        FROM Venta VT
                        INNER JOIN Consignacion CO ON CO.IdConsignacion = VT.IdConsignacion
                        INNER JOIN Vehiculo V      ON V.IdVehiculo      = CO.IdVehiculo
                  ) AS Movimientos
                  ORDER BY Fecha DESC",
                SqlHelper.Param("@Cantidad", cantidad));

            return ActividadRecienteAdaptador.Current.FillLista(tabla);
        }

        // Detalle de las tarjetas

        /// <summary>
        /// TALLER: autos que están ahora en diagnóstico o en reparación, con la fecha
        /// en que entraron, el mecánico asignado y la prioridad de la orden de trabajo.
        /// OUTER APPLY = "traeme el último registro relacionado, si hay; si no, dejalo vacío".
        /// </summary>
        public List<DetalleInicio> ListarVehiculosEnTaller()
        {
            var tabla = SqlHelper.ExecuteDataTable(
                @"SELECT V.IdVehiculo, R.FechaIngreso AS Fecha, V.Dominio, V.Marca, V.Modelo, V.Anio, V.EstadoVehiculo,
                         ISNULL(M.Apellido + ', ' + M.Nombre, '') AS Persona,
                         CAST(NULL AS DECIMAL(18,2)) AS Monto,
                         ISNULL(OT.Prioridad, '') AS Extra,
                         CAST(0 AS BIT) AS EsVenta
                  FROM Vehiculo V
                  OUTER APPLY (SELECT TOP 1 FechaIngreso FROM RecepcionVehiculo
                               WHERE IdVehiculo = V.IdVehiculo ORDER BY FechaIngreso DESC) R
                  OUTER APPLY (SELECT TOP 1 IdOrdenTrabajo, Prioridad FROM OrdenTrabajo
                               WHERE IdVehiculo = V.IdVehiculo ORDER BY FechaCreacion DESC) OT
                  OUTER APPLY (SELECT TOP 1 MC.Nombre, MC.Apellido
                               FROM TareaOrdenTrabajo T
                               INNER JOIN Mecanico MC ON MC.IdMecanico = T.IdMecanico
                               WHERE T.IdOrdenTrabajo = OT.IdOrdenTrabajo ORDER BY T.IdTarea) M
                  WHERE V.EstadoVehiculo IN (@EnDiagnostico, @EnReparacion)
                  ORDER BY R.FechaIngreso DESC",
                SqlHelper.Param("@EnDiagnostico", (int)EstadoVehiculo.EnDiagnostico),
                SqlHelper.Param("@EnReparacion", (int)EstadoVehiculo.EnReparacion));

            return DetalleInicioAdaptador.Current.FillLista(tabla);
        }

        /// <summary>INGRESOS: compras de autos chocados entre dos fechas, con vendedor y precio.</summary>
        public List<DetalleInicio> ListarComprasDetalle(DateTime desde, DateTime hastaExclusive)
        {
            var tabla = SqlHelper.ExecuteDataTable(
                @"SELECT V.IdVehiculo, C.FechaCompra AS Fecha, V.Dominio, V.Marca, V.Modelo, V.Anio, V.EstadoVehiculo,
                         CL.Apellido + ', ' + CL.Nombre AS Persona,
                         C.PrecioCompra AS Monto,
                         C.FormaPago AS Extra,
                         CAST(0 AS BIT) AS EsVenta
                  FROM Compra C
                  INNER JOIN Vehiculo V ON V.IdVehiculo = C.IdVehiculo
                  INNER JOIN Cliente CL ON CL.IdCliente = C.IdCliente
                  WHERE C.FechaCompra >= @Desde AND C.FechaCompra < @Hasta
                  ORDER BY C.FechaCompra DESC",
                SqlHelper.Param("@Desde", desde),
                SqlHelper.Param("@Hasta", hastaExclusive));

            return DetalleInicioAdaptador.Current.FillLista(tabla);
        }

        /// <summary>
        /// AGENCIA: primero las ventas entre dos fechas (con comprador y precio de venta)
        /// y debajo los autos que la agencia tiene ahora para vender (en agencia o publicados).
        /// </summary>
        public List<DetalleInicio> ListarAgenciaDetalle(DateTime desde, DateTime hastaExclusive)
        {
            var tabla = SqlHelper.ExecuteDataTable(
                @"SELECT IdVehiculo, Fecha, Dominio, Marca, Modelo, Anio, EstadoVehiculo, Persona, Monto, Extra, EsVenta
                  FROM (
                        SELECT V.IdVehiculo, VT.FechaVenta AS Fecha, V.Dominio, V.Marca, V.Modelo, V.Anio, V.EstadoVehiculo,
                               CL.Apellido + ', ' + CL.Nombre AS Persona,
                               VT.PrecioVenta AS Monto,
                               A.RazonSocial AS Extra,
                               CAST(1 AS BIT) AS EsVenta
                        FROM Venta VT
                        INNER JOIN Consignacion CO ON CO.IdConsignacion = VT.IdConsignacion
                        INNER JOIN Vehiculo V      ON V.IdVehiculo      = CO.IdVehiculo
                        INNER JOIN Cliente CL      ON CL.IdCliente      = VT.IdCliente
                        INNER JOIN Agencia A       ON A.IdAgencia       = CO.IdAgencia
                        WHERE VT.FechaVenta >= @Desde AND VT.FechaVenta < @Hasta

                        UNION ALL

                        SELECT V.IdVehiculo, CO.FechaEntrega AS Fecha, V.Dominio, V.Marca, V.Modelo, V.Anio, V.EstadoVehiculo,
                               N'' AS Persona,
                               ISNULL(P.PrecioPublicado, CO.PrecioMinimoVenta) AS Monto,
                               ISNULL(A.RazonSocial, N'') AS Extra,
                               CAST(0 AS BIT) AS EsVenta
                        FROM Vehiculo V
                        OUTER APPLY (SELECT TOP 1 IdAgencia, FechaEntrega, PrecioMinimoVenta FROM Consignacion
                                     WHERE IdVehiculo = V.IdVehiculo ORDER BY FechaEntrega DESC) CO
                        LEFT JOIN Agencia A ON A.IdAgencia = CO.IdAgencia
                        OUTER APPLY (SELECT TOP 1 PrecioPublicado FROM Publicacion
                                     WHERE IdVehiculo = V.IdVehiculo AND FechaFinalizacion IS NULL
                                     ORDER BY FechaPublicacion DESC) P
                        WHERE V.EstadoVehiculo IN (@EnAgencia, @Publicado)
                  ) AS Agencia
                  ORDER BY EsVenta DESC, Fecha DESC",
                SqlHelper.Param("@Desde", desde),
                SqlHelper.Param("@Hasta", hastaExclusive),
                SqlHelper.Param("@EnAgencia", EstadoEnAgencia),
                SqlHelper.Param("@Publicado", EstadoPublicado));

            return DetalleInicioAdaptador.Current.FillLista(tabla);
        }

        // ---------------------------- Inicio del mecánico ----------------------------
        // "De un mecánico" = tiene alguna tarea a su nombre en la orden de trabajo (TareaOrdenTrabajo.IdMecanico).

        private const string TieneTareaDelMecanico =
            "EXISTS (SELECT 1 FROM TareaOrdenTrabajo T WHERE T.IdOrdenTrabajo = OT.IdOrdenTrabajo AND T.IdMecanico = @IdMecanico)";

        /// <summary>Los cuatro números de las tarjetas del mecánico, en una sola consulta.</summary>
        public ResumenMecanico ObtenerResumenMecanico(int idMecanico, DateTime desdeMes, DateTime hastaExclusive)
        {
            var tabla = SqlHelper.ExecuteDataTable(
                $@"SELECT
                     (SELECT COUNT(*) FROM Vehiculo WHERE EstadoVehiculo = @EnDiagnostico) AS AutosEnDiagnostico,
                     (SELECT COUNT(*) FROM OrdenTrabajo OT
                      WHERE OT.EstadoOrden = @Abierta AND {TieneTareaDelMecanico}) AS OrdenesAbiertas,
                     (SELECT COUNT(*) FROM PedidoRepuesto P
                      INNER JOIN OrdenTrabajo OT ON OT.IdOrdenTrabajo = P.IdOrdenTrabajo
                      WHERE P.EstadoPedido = @Pendiente AND {TieneTareaDelMecanico}) AS PedidosPendientes,
                     (SELECT COUNT(*) FROM TareaOrdenTrabajo T
                      WHERE T.IdMecanico = @IdMecanico AND T.EstadoTarea = @Finalizada
                        AND T.FechaFin >= @Desde AND T.FechaFin < @Hasta) AS TareasRealizadasMes",
                SqlHelper.Param("@EnDiagnostico", (int)EstadoVehiculo.EnDiagnostico),
                SqlHelper.Param("@Abierta", Estados.OrdenAbierta),
                SqlHelper.Param("@Pendiente", Estados.PedidoPendiente),
                SqlHelper.Param("@Finalizada", Estados.TareaFinalizada),
                SqlHelper.Param("@IdMecanico", idMecanico),
                SqlHelper.Param("@Desde", desdeMes),
                SqlHelper.Param("@Hasta", hastaExclusive));

            System.Data.DataRow fila = tabla.Rows[0];
            return new ResumenMecanico
            {
                AutosEnDiagnostico = (int)fila["AutosEnDiagnostico"],
                OrdenesAbiertas = (int)fila["OrdenesAbiertas"],
                PedidosPendientes = (int)fila["PedidosPendientes"],
                TareasRealizadasMes = (int)fila["TareasRealizadasMes"]
            };
        }

        /// <summary>Autos esperando diagnóstico, con la fecha en que entraron y lo que se anotó al recibirlos.</summary>
        public List<DetalleInicio> ListarAutosEnDiagnostico()
        {
            var tabla = SqlHelper.ExecuteDataTable(
                @"SELECT V.IdVehiculo, R.FechaIngreso AS Fecha, V.Dominio, V.Marca, V.Modelo, V.Anio, V.EstadoVehiculo,
                         N'' AS Persona, CAST(NULL AS DECIMAL(18,2)) AS Monto,
                         ISNULL(LEFT(D.DescripcionDanios, 200), N'') AS Extra, CAST(0 AS BIT) AS EsVenta
                  FROM Vehiculo V
                  OUTER APPLY (SELECT TOP 1 FechaIngreso FROM RecepcionVehiculo
                               WHERE IdVehiculo = V.IdVehiculo ORDER BY FechaIngreso DESC) R
                  OUTER APPLY (SELECT TOP 1 DescripcionDanios FROM Diagnostico
                               WHERE IdVehiculo = V.IdVehiculo ORDER BY FechaDiagnostico DESC) D
                  WHERE V.EstadoVehiculo = @EnDiagnostico
                  ORDER BY R.FechaIngreso DESC",
                SqlHelper.Param("@EnDiagnostico", (int)EstadoVehiculo.EnDiagnostico));
            return DetalleInicioAdaptador.Current.FillLista(tabla);
        }

        /// <summary>Órdenes abiertas del mecánico, con su tarea y la prioridad.</summary>
        public List<DetalleInicio> ListarOrdenesDeMecanico(int idMecanico)
        {
            var tabla = SqlHelper.ExecuteDataTable(
                $@"SELECT V.IdVehiculo, OT.FechaCreacion AS Fecha, V.Dominio, V.Marca, V.Modelo, V.Anio, V.EstadoVehiculo,
                          ISNULL((SELECT TOP 1 T.DescripcionTarea FROM TareaOrdenTrabajo T
                                  WHERE T.IdOrdenTrabajo = OT.IdOrdenTrabajo AND T.IdMecanico = @IdMecanico
                                  ORDER BY T.IdTarea), N'') AS Persona,
                          CAST(NULL AS DECIMAL(18,2)) AS Monto, OT.Prioridad AS Extra, CAST(0 AS BIT) AS EsVenta
                   FROM OrdenTrabajo OT
                   INNER JOIN Vehiculo V ON V.IdVehiculo = OT.IdVehiculo
                   WHERE OT.EstadoOrden = @Abierta AND {TieneTareaDelMecanico}
                   ORDER BY OT.FechaCreacion DESC",
                SqlHelper.Param("@Abierta", Estados.OrdenAbierta),
                SqlHelper.Param("@IdMecanico", idMecanico));
            return DetalleInicioAdaptador.Current.FillLista(tabla);
        }

        /// <summary>Pedidos de repuestos pendientes de las órdenes del mecánico, con proveedor y monto.</summary>
        public List<DetalleInicio> ListarPedidosPendientesDeMecanico(int idMecanico)
        {
            var tabla = SqlHelper.ExecuteDataTable(
                $@"SELECT V.IdVehiculo, P.FechaPedido AS Fecha, V.Dominio, V.Marca, V.Modelo, V.Anio, V.EstadoVehiculo,
                          PR.RazonSocial AS Persona, P.MontoTotal AS Monto, P.EstadoPedido AS Extra, CAST(0 AS BIT) AS EsVenta
                   FROM PedidoRepuesto P
                   INNER JOIN OrdenTrabajo OT ON OT.IdOrdenTrabajo = P.IdOrdenTrabajo
                   INNER JOIN Vehiculo V      ON V.IdVehiculo      = OT.IdVehiculo
                   INNER JOIN Proveedor PR    ON PR.IdProveedor    = P.IdProveedor
                   WHERE P.EstadoPedido = @Pendiente AND {TieneTareaDelMecanico}
                   ORDER BY P.FechaPedido DESC",
                SqlHelper.Param("@Pendiente", Estados.PedidoPendiente),
                SqlHelper.Param("@IdMecanico", idMecanico));
            return DetalleInicioAdaptador.Current.FillLista(tabla);
        }

        /// <summary>Tareas que el mecánico terminó entre dos fechas (el Inicio usa el mes actual).</summary>
        public List<DetalleInicio> ListarTareasRealizadasDeMecanico(int idMecanico, DateTime desde, DateTime hastaExclusive)
        {
            var tabla = SqlHelper.ExecuteDataTable(
                @"SELECT V.IdVehiculo, T.FechaFin AS Fecha, V.Dominio, V.Marca, V.Modelo, V.Anio, V.EstadoVehiculo,
                         T.DescripcionTarea AS Persona, T.CostoEstimado AS Monto, T.EstadoTarea AS Extra, CAST(0 AS BIT) AS EsVenta
                  FROM TareaOrdenTrabajo T
                  INNER JOIN OrdenTrabajo OT ON OT.IdOrdenTrabajo = T.IdOrdenTrabajo
                  INNER JOIN Vehiculo V      ON V.IdVehiculo      = OT.IdVehiculo
                  WHERE T.IdMecanico = @IdMecanico AND T.EstadoTarea = @Finalizada
                    AND T.FechaFin >= @Desde AND T.FechaFin < @Hasta
                  ORDER BY T.FechaFin DESC",
                SqlHelper.Param("@IdMecanico", idMecanico),
                SqlHelper.Param("@Finalizada", Estados.TareaFinalizada),
                SqlHelper.Param("@Desde", desde),
                SqlHelper.Param("@Hasta", hastaExclusive));
            return DetalleInicioAdaptador.Current.FillLista(tabla);
        }
    }
}
