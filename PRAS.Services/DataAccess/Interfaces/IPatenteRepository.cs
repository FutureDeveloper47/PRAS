using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.DataAccess.Interfaces
{
    internal interface IPatenteRepository : IGenericRepository<Patente>
    {
        Patente GetByDataKey(string dataKey);
    }
}
