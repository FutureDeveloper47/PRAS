using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.DataAccess.Interfaces
{
    internal interface IFamiliaRepository : IGenericRepository<Familia>
    {
        Familia GetByNombre(string nombre);
    }
}
