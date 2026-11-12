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
                @"SELECT TOP (@Cantidad) Fecha, Tipo, Marca, Modelo, Dominio
                  FROM (
                        SELECT C.FechaCompra AS Fecha, 'Ingreso' AS Tipo, V.Marca, V.Modelo, V.Dominio
                        FROM Compra C
                        INNER JOIN Vehiculo V ON V.IdVehiculo = C.IdVehiculo

                        UNION ALL

                        SELECT VT.FechaVenta AS Fecha, 'Venta' AS Tipo, V.Marca, V.Modelo, V.Dominio
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
                @"SELECT R.FechaIngreso AS Fecha, V.Dominio, V.Marca, V.Modelo, V.Anio, V.EstadoVehiculo,
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
                @"SELECT C.FechaCompra AS Fecha, V.Dominio, V.Marca, V.Modelo, V.Anio, V.EstadoVehiculo,
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
                @"SELECT Fecha, Dominio, Marca, Modelo, Anio, EstadoVehiculo, Persona, Monto, Extra, EsVenta
                  FROM (
                        SELECT VT.FechaVenta AS Fecha, V.Dominio, V.Marca, V.Modelo, V.Anio, V.EstadoVehiculo,
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

                        SELECT CO.FechaEntrega AS Fecha, V.Dominio, V.Marca, V.Modelo, V.Anio, V.EstadoVehiculo,
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
    }
}
