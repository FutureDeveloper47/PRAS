namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio que sabe dar de alta.</summary>
    public interface IAddRepository<T>
    {
        /// <summary>Da de alta la entidad y le carga el Id que generó la base.</summary>
        void Add(T entity);
    }
}
