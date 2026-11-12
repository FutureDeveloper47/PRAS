namespace PRAS.DAL.Interfaces
{
    /// <summary>
    /// Operaciones básicas de un repositorio.
    /// En PRAS los Id son INT (identity) en lugar de Guid.
    /// </summary>
    public interface IGenericRepository<T> : ICrudRepository<T>
    {
        void Update(T entity);

        /// <summary>
        /// En los maestros es una baja lógica (Activo = 0): no borramos datos que tienen historia.
        /// </summary>
        void Delete(int id);

        List<T> GetAll();
    }
}
