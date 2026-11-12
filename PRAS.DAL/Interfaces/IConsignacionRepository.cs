using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de consignaciones (tabla Consignacion). El Add pasa el auto de Reparado a En agencia.</summary>
    public interface IConsignacionRepository : ICrudRepository<Consignacion>
    {
        List<Consignacion> Listar(bool soloVigentes);

        Consignacion ObtenerVigentePorVehiculo(int idVehiculo);

        /// <summary>El auto vuelve de la agencia sin venderse (pasa a Reparado).</summary>
        void RegistrarRetiro(int idConsignacion, DateTime fechaRetiro);
    }
}
