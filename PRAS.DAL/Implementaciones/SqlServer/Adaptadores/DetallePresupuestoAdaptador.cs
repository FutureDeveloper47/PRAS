using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la tabla DetallePresupuesto en un objeto DetallePresupuesto.
    /// </summary>
    internal sealed class DetallePresupuestoAdaptador : IAdaptador<DetallePresupuesto>
    {
        #region Singleton
        private static readonly DetallePresupuestoAdaptador _instance = new DetallePresupuestoAdaptador();

        public static DetallePresupuestoAdaptador Current
        {
            get { return _instance; }
        }

        private DetallePresupuestoAdaptador()
        {
        }
        #endregion

        public DetallePresupuesto Fill(DataRow fila)
        {
            return new DetallePresupuesto
            {
                IdDetallePresupuesto = (int)fila["IdDetallePresupuesto"],
                IdPresupuesto = (int)fila["IdPresupuesto"],
                DescripcionItem = (string)fila["DescripcionItem"],
                Cantidad = (int)fila["Cantidad"],
                PrecioUnitario = (decimal)fila["PrecioUnitario"],
                Subtotal = (decimal)fila["Subtotal"],
                Aprobado = (bool)fila["Aprobado"]
            };
        }
    }
}
