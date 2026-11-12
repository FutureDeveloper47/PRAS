namespace PRAS.DAL.Interfaces
{
    /// <summary>Alta + búsqueda por Id.</summary>
    public interface ICrudRepository<T> : IAddRepository<T>, IGetByIdRepository<T>
    {
    }
}
