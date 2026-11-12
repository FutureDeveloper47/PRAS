using System.Data;

namespace PRAS.DAL.Interfaces
{
    /// <summary>
    /// ADAPTER: convierte una fila leída de la base
    /// en un objeto de Entidades. Así el repositorio no arma objetos "a mano".
    /// Recibe la fila completa (DataRow) en lugar de object[],
    /// para leer las columnas por nombre y no por posición (más seguro cuando hay JOIN).
    /// </summary>
    internal interface IAdaptador<T>
    {
        T Fill(DataRow fila);
    }
}
