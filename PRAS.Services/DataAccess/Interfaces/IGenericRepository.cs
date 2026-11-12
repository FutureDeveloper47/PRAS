namespace PRAS.Services.DataAccess.Interfaces
{
    /// <summary>Operaciones básicas de un repositorio.</summary>
    internal interface IGenericRepository<T>
    {
        void Add(T entity);

        void Update(T entity);

        void Delete(Guid id);

        T GetById(Guid id);

        List<T> GetAll();
    }
}
