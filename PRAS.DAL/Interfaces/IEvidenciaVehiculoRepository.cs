using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de evidencias (tabla EvidenciaVehiculo): fotos, videos o documentos de un diagnóstico.</summary>
    public interface IEvidenciaVehiculoRepository : ICrudRepository<EvidenciaVehiculo>
    {
        List<EvidenciaVehiculo> ListarPorDiagnostico(int idDiagnostico);
    }
}
