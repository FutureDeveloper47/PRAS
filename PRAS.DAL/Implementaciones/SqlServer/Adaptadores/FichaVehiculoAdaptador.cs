using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte la fila de la consulta "ficha del vehículo" (auto + compra + taller + agencia + venta)
    /// en un objeto FichaVehiculo. Las tareas de la orden se cargan aparte (son varias filas).
    /// </summary>
    internal sealed class FichaVehiculoAdaptador : IAdaptador<FichaVehiculo>
    {
        #region Singleton
        private static readonly FichaVehiculoAdaptador _instance = new FichaVehiculoAdaptador();

        public static FichaVehiculoAdaptador Current
        {
            get { return _instance; }
        }

        private FichaVehiculoAdaptador()
        {
        }
        #endregion

        public FichaVehiculo Fill(DataRow fila)
        {
            return new FichaVehiculo
            {
                // Los datos del auto los arma el adaptador de Vehiculo (no repetimos código).
                Vehiculo = VehiculoAdaptador.Current.Fill(fila),

                FechaCompra = Fecha(fila, "FechaCompra"),
                PrecioCompra = Plata(fila, "PrecioCompra"),
                FormaPagoCompra = Texto(fila, "FormaPagoCompra"),
                Vendedor = Texto(fila, "Vendedor"),
                ObservacionesCompra = Texto(fila, "ObservacionesCompra"),

                FechaDiagnostico = Fecha(fila, "FechaDiagnostico"),
                DescripcionDanios = Texto(fila, "DescripcionDanios"),
                EstadoDiagnostico = Texto(fila, "EstadoDiagnostico"),

                MontoPresupuesto = Plata(fila, "MontoPresupuesto"),
                FechaOrden = Fecha(fila, "FechaOrden"),
                EstadoOrden = Texto(fila, "EstadoOrden"),
                Prioridad = Texto(fila, "Prioridad"),

                Agencia = Texto(fila, "Agencia"),
                FechaEntregaAgencia = Fecha(fila, "FechaEntregaAgencia"),
                EstadoConsignacion = Texto(fila, "EstadoConsignacion"),
                FechaPublicacion = Fecha(fila, "FechaPublicacion"),
                Plataforma = Texto(fila, "Plataforma"),
                PrecioPublicado = Plata(fila, "PrecioPublicado"),
                EstadoPublicacion = Texto(fila, "EstadoPublicacion"),

                FechaVenta = Fecha(fila, "FechaVenta"),
                PrecioVenta = Plata(fila, "PrecioVenta"),
                Comprador = Texto(fila, "Comprador"),
                FormaPagoVenta = Texto(fila, "FormaPagoVenta")
            };
        }

        /// <summary>Una tarea de la orden de trabajo (otra consulta, una fila por tarea).</summary>
        public TareaFicha FillTarea(DataRow fila)
        {
            return new TareaFicha
            {
                Descripcion = (string)fila["DescripcionTarea"],
                Mecanico = Texto(fila, "Mecanico"),
                Estado = (string)fila["EstadoTarea"],
                CostoEstimado = (decimal)fila["CostoEstimado"]
            };
        }

        // Como casi todo puede faltar (un auto recién comprado no tiene venta), leemos con cuidado los NULL.
        private static DateTime? Fecha(DataRow fila, string columna) =>
            fila[columna] == DBNull.Value ? null : (DateTime?)fila[columna];

        private static decimal? Plata(DataRow fila, string columna) =>
            fila[columna] == DBNull.Value ? null : (decimal?)fila[columna];

        private static string Texto(DataRow fila, string columna) =>
            fila[columna] == DBNull.Value ? null : (string)fila[columna];
    }
}
