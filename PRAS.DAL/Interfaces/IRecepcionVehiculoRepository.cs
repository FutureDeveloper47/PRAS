using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de recepciones (tabla RecepcionVehiculo): cada vez que un auto entra al taller.</summary>
    public interface IRecepcionVehiculoRepository : IGetByIdRepository<RecepcionVehiculo>
    {
        List<RecepcionVehiculo> ListarPorVehiculo(int idVehiculo);
    }
}
