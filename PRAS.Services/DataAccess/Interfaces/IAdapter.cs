namespace PRAS.Services.DataAccess.Interfaces
{
    /// <summary>
    /// ADAPTER: convierte una fila leída de la base (object[])
    /// en un objeto del modelo (Familia, Patente).
    /// </summary>
    internal interface IAdapter<T> where T : class
    {
        T Get(object[] values);
    }
}
