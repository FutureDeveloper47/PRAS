using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de publicaciones (tabla Publicacion). El Add pasa el auto de En agencia a Publicado.</summary>
    public interface IPublicacionRepository : ICrudRepository<Publicacion>
    {
        List<Publicacion> Listar(bool soloActivas);

        Publicacion ObtenerActivaPorVehiculo(int idVehiculo);

        /// <summary>
        /// Cambia plataforma, precio y descripción de una publicación ACTIVA.
        /// El auto sigue Publicado (no cambia de estado).
        /// </summary>
        void Modificar(Publicacion entity);

        /// <summary>Da de baja el aviso: el auto vuelve a En agencia.</summary>
        void Finalizar(int idPublicacion, DateTime fecha);
    }
}
