using PRAS.Services.DataAccess.Implementations;
using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.Logic
{
    internal static class PatenteLogic
    {
        private static readonly IPatenteRepository _repo = new PatenteRepository();

        public static void Add(Patente patente)
        {
            _repo.Add(patente);
        }

        public static Patente GetById(Guid id)
        {
            return _repo.GetById(id);
        }

        public static Patente GetByDataKey(string dataKey)
        {
            return _repo.GetByDataKey(dataKey);
        }

        public static List<Patente> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
