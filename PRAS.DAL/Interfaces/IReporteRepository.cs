using System.Data;
using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Consultas de los reportes. Devuelven DataTable porque cada reporte tiene columnas distintas.</summary>
    public interface IReporteRepository
    {
        DataTable Vehiculos(int? estado);

        DataTable StockPorEstado();

        DataTable Reparaciones(DateTime desde, DateTime hasta);

        DataTable Presupuestos(DateTime desde, DateTime hasta);

        DataTable Ventas(DateTime desde, DateTime hasta);
    }
}
