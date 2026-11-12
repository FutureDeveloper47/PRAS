using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Consultas de los reportes (singleton). Devuelven un DataTable (filas y columnas listas para
    /// la grilla y para exportar a CSV), porque cada reporte tiene columnas distintas y no es una entidad.
    /// Por eso no usan adaptador.
    /// </summary>
    internal sealed class ReporteRepository : IReporteRepository
    {
        #region Singleton
        private static readonly ReporteRepository _instance = new ReporteRepository();

        public static ReporteRepository Current
        {
            get { return _instance; }
        }

        private ReporteRepository()
        {
        }
        #endregion

        /// <summary>Texto del estado del auto calculado en SQL (igual que VehiculoService.TextoEstado).</summary>
        private const string TextoEstado =
            @"CASE V.EstadoVehiculo WHEN 1 THEN N'Comprado (chocado)' WHEN 2 THEN N'En diagnóstico'
                                    WHEN 3 THEN N'En reparación' WHEN 4 THEN N'Reparado' WHEN 5 THEN N'En agencia'
                                    WHEN 6 THEN N'Publicado' ELSE N'Vendido' END";

        /// <summary>Costo de reparación de un auto = suma de los totales de sus presupuestos.</summary>
        private const string CostoReparacion =
            "ISNULL((SELECT SUM(P.MontoTotal) FROM Presupuesto P WHERE P.IdVehiculo = V.IdVehiculo), 0)";

        /// <summary>
        /// Reporte de VEHÍCULOS: cada auto con su estado, lo que se pagó, lo que costó repararlo
        /// y la inversión total. estado = null trae todos.
        /// </summary>
        public DataTable Vehiculos(int? estado)
        {
            return SqlHelper.ExecuteDataTable(
                $@"SELECT V.Dominio, V.Marca + N' ' + V.Modelo + N' (' + CAST(V.Anio AS NVARCHAR(4)) + N')' AS Vehiculo,
                          {TextoEstado} AS Estado,
                          C.FechaCompra, C.PrecioCompra,
                          {CostoReparacion} AS CostoReparacion,
                          C.PrecioCompra + {CostoReparacion} AS InversionTotal
                   FROM Vehiculo V
                   INNER JOIN Compra C ON C.IdVehiculo = V.IdVehiculo
                   WHERE (@Estado IS NULL OR V.EstadoVehiculo = @Estado)
                   ORDER BY V.EstadoVehiculo, V.Dominio",
                SqlHelper.Param("@Estado", estado));
        }

        /// <summary>Resumen: cantidad de autos e inversión por estado (el "stock" de la empresa).</summary>
        public DataTable StockPorEstado()
        {
            return SqlHelper.ExecuteDataTable(
                $@"SELECT {TextoEstado} AS Estado, COUNT(*) AS Cantidad,
                          SUM(C.PrecioCompra + R.Costo) AS InversionTotal
                   FROM Vehiculo V
                   INNER JOIN Compra C ON C.IdVehiculo = V.IdVehiculo
                   -- SQL Server no deja poner un SELECT adentro de un SUM, así que el costo
                   -- de reparación de cada auto se calcula antes con OUTER APPLY.
                   OUTER APPLY (SELECT ISNULL(SUM(P.MontoTotal), 0) AS Costo
                                FROM Presupuesto P WHERE P.IdVehiculo = V.IdVehiculo) R
                   GROUP BY V.EstadoVehiculo
                   ORDER BY V.EstadoVehiculo");
        }

        /// <summary>
        /// Reporte de REPARACIONES: órdenes de trabajo creadas entre dos fechas, con su costo,
        /// los mecánicos y los días que el auto estuvo (o lleva) en el taller.
        /// </summary>
        public DataTable Reparaciones(DateTime desde, DateTime hasta)
        {
            return SqlHelper.ExecuteDataTable(
                $@"SELECT O.IdOrdenTrabajo AS OT, O.FechaCreacion, V.Dominio,
                          V.Marca + N' ' + V.Modelo AS Vehiculo, O.Prioridad, O.EstadoOrden AS Estado,
                          ISNULL((SELECT STRING_AGG(M.Apellido, ', ')
                                  FROM (SELECT DISTINCT T.IdMecanico FROM TareaOrdenTrabajo T
                                        WHERE T.IdOrdenTrabajo = O.IdOrdenTrabajo) X
                                  INNER JOIN Mecanico M ON M.IdMecanico = X.IdMecanico), N'') AS Mecanicos,
                          (SELECT COUNT(*) FROM TareaOrdenTrabajo T WHERE T.IdOrdenTrabajo = O.IdOrdenTrabajo) AS Tareas,
                          ISNULL(P.MontoTotal, 0) AS Costo,
                          DATEDIFF(DAY, O.FechaCreacion,
                                   CASE WHEN O.EstadoOrden = 'Finalizada'
                                        THEN ISNULL((SELECT MAX(T.FechaFin) FROM TareaOrdenTrabajo T
                                                     WHERE T.IdOrdenTrabajo = O.IdOrdenTrabajo), GETDATE())
                                        ELSE GETDATE() END) AS DiasEnTaller
                   FROM OrdenTrabajo O
                   INNER JOIN Vehiculo V ON V.IdVehiculo = O.IdVehiculo
                   LEFT JOIN Presupuesto P ON P.IdPresupuesto = O.IdPresupuesto
                   WHERE O.FechaCreacion >= @Desde AND O.FechaCreacion < @HastaMasUnDia
                   ORDER BY O.FechaCreacion DESC",
                SqlHelper.Param("@Desde", desde.Date),
                SqlHelper.Param("@HastaMasUnDia", hasta.Date.AddDays(1)));
        }

        /// <summary>Reporte de PRESUPUESTOS emitidos entre dos fechas, con la cantidad de ítems y el total.</summary>
        public DataTable Presupuestos(DateTime desde, DateTime hasta)
        {
            return SqlHelper.ExecuteDataTable(
                @"SELECT P.IdPresupuesto AS Presupuesto, P.FechaEmision, V.Dominio,
                         V.Marca + N' ' + V.Modelo AS Vehiculo, P.EstadoPresupuesto AS Estado,
                         (SELECT COUNT(*) FROM DetallePresupuesto D WHERE D.IdPresupuesto = P.IdPresupuesto) AS Items,
                         ISNULL((SELECT SUM(D.Subtotal) FROM DetallePresupuesto D
                                 WHERE D.IdPresupuesto = P.IdPresupuesto AND D.DescripcionItem LIKE N'Repuesto%'), 0) AS Repuestos,
                         ISNULL((SELECT SUM(D.Subtotal) FROM DetallePresupuesto D
                                 WHERE D.IdPresupuesto = P.IdPresupuesto AND D.DescripcionItem NOT LIKE N'Repuesto%'), 0) AS ManoDeObraYOtros,
                         P.MontoTotal
                  FROM Presupuesto P
                  INNER JOIN Vehiculo V ON V.IdVehiculo = P.IdVehiculo
                  WHERE P.FechaEmision >= @Desde AND P.FechaEmision < @HastaMasUnDia
                  ORDER BY P.FechaEmision DESC",
                SqlHelper.Param("@Desde", desde.Date),
                SqlHelper.Param("@HastaMasUnDia", hasta.Date.AddDays(1)));
        }

        /// <summary>
        /// Reporte de VENTAS entre dos fechas, con la GANANCIA de cada auto:
        /// precio de venta - comisión de la agencia - precio de compra - costo de reparación.
        /// </summary>
        public DataTable Ventas(DateTime desde, DateTime hasta)
        {
            return SqlHelper.ExecuteDataTable(
                $@"SELECT VT.FechaVenta, V.Dominio, V.Marca + N' ' + V.Modelo AS Vehiculo,
                          A.RazonSocial AS Agencia, CL.Apellido + N', ' + CL.Nombre AS Comprador,
                          VT.PrecioVenta, VT.MontoComision AS Comision,
                          CP.PrecioCompra, {CostoReparacion} AS CostoReparacion,
                          VT.PrecioVenta - VT.MontoComision - CP.PrecioCompra - {CostoReparacion} AS Ganancia
                   FROM Venta VT
                   INNER JOIN Consignacion CO ON CO.IdConsignacion = VT.IdConsignacion
                   INNER JOIN Vehiculo V      ON V.IdVehiculo = CO.IdVehiculo
                   INNER JOIN Agencia A       ON A.IdAgencia = CO.IdAgencia
                   INNER JOIN Cliente CL      ON CL.IdCliente = VT.IdCliente
                   INNER JOIN Compra CP       ON CP.IdVehiculo = V.IdVehiculo
                   WHERE VT.FechaVenta >= @Desde AND VT.FechaVenta < @HastaMasUnDia
                   ORDER BY VT.FechaVenta DESC",
                SqlHelper.Param("@Desde", desde.Date),
                SqlHelper.Param("@HastaMasUnDia", hasta.Date.AddDays(1)));
        }
    }
}
