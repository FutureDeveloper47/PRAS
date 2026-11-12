namespace PRAS.Services.DataAccess.Interfaces
{
    /// <summary>
    /// Repositorio de una tabla de RELACIÓN:
    /// T = el hijo, Y = el padre. Ej: FamiliaPatente -> T = Patente, Y = Familia.
    /// Se agregó Remove(hijo, padre) para poder quitar un permiso de una familia o de un usuario.
    /// </summary>
    internal interface IJoinRepository<T, Y>
    {
        List<T> GetByObject(Y obj);

        void Delete(T obj);

        void Add(T obj, Y parent);

        void AddRange(List<T> children, Y parent);

        void Remove(T obj, Y parent);
    }
}
