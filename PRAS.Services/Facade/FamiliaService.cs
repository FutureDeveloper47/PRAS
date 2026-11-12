using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.Facade
{
    /// <summary>FACHADA de las familias de permisos.</summary>
    public static class FamiliaService
    {
        public static void Add(Familia familia)
        {
            Logic.FamiliaLogic.Add(familia);
        }

        public static void Delete(Familia familia)
        {
            Logic.FamiliaLogic.Delete(familia);
        }

        public static void AgregarPatente(Patente patente, Familia familia)
        {
            Logic.FamiliaLogic.AgregarPatente(patente, familia);
        }

        public static void AgregarFamilia(Familia hijo, Familia padre)
        {
            Logic.FamiliaLogic.AgregarFamilia(hijo, padre);
        }

        public static void Quitar(Component componente, Familia familia)
        {
            Logic.FamiliaLogic.Quitar(componente, familia);
        }

        public static Familia GetById(Guid id)
        {
            return Logic.FamiliaLogic.GetById(id);
        }

        public static Familia GetByNombre(string nombre)
        {
            return Logic.FamiliaLogic.GetByNombre(nombre);
        }

        public static List<Familia> GetAll()
        {
            return Logic.FamiliaLogic.GetAll();
        }
    }
}
