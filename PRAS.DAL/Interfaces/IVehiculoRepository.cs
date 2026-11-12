using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de vehículos (tabla Vehiculo).</summary>
    public interface IVehiculoRepository : IGenericRepository<Vehiculo>
    {
        Vehiculo ObtenerPorDominio(string dominio);

        /// <summary>estado = null trae todos. patente filtra "contiene".</summary>
        List<Vehiculo> Listar(EstadoVehiculo? estado, string patente);

        /// <summary>
        /// Borra el auto y su compra, solo si sigue en estado Comprado (se cargó por error).
        /// Devuelve false si el auto ya avanzó y no se puede borrar.
        /// </summary>
        bool EliminarConCompra(int idVehiculo);
    }
}
