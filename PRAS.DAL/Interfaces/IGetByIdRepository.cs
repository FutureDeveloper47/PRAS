namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio que sabe buscar por Id.</summary>
    public interface IGetByIdRepository<T>
    {
        /// <summary>Devuelve la entidad, o null si no existe.</summary>
        T GetById(int id);
    }
}
