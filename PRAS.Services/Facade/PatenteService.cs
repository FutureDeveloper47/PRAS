using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.Facade
{
    /// <summary>FACHADA de las patentes.</summary>
    public static class PatenteService
    {
        public static void Add(Patente patente)
        {
            Logic.PatenteLogic.Add(patente);
        }

        public static Patente GetById(Guid id)
        {
            return Logic.PatenteLogic.GetById(id);
        }

        public static Patente GetByDataKey(string dataKey)
        {
            return Logic.PatenteLogic.GetByDataKey(dataKey);
        }

        public static List<Patente> GetAll()
        {
            return Logic.PatenteLogic.GetAll();
        }
    }
}
