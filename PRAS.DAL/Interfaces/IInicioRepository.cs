using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Consultas de solo lectura para la pantalla de Inicio.</summary>
    public interface IInicioRepository
    {
        int ContarVehiculos(EstadoVehiculo estado);

        int ContarCompras(DateTime desde, DateTime hastaExclusive);

        int ContarVentas(DateTime desde, DateTime hastaExclusive);

        List<ActividadReciente> ListarActividadReciente(int cantidad);

        List<DetalleInicio> ListarVehiculosEnTaller();

        List<DetalleInicio> ListarComprasDetalle(DateTime desde, DateTime hastaExclusive);

        List<DetalleInicio> ListarAgenciaDetalle(DateTime desde, DateTime hastaExclusive);
    }
}
