using PRAS.Services.DataAccess.Implementations;
using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.Logic
{
    internal static class FamiliaLogic
    {
        private static readonly IFamiliaRepository _repo = new FamiliaRepository();

        public static void Add(Familia familia)
        {
            _repo.Add(familia);
        }

        public static void Delete(Familia familia)
        {
            _repo.Delete(familia.Id);
        }

        public static void AgregarPatente(Patente patente, Familia familia)
        {
            new FamiliaPatenteRepository().Add(patente, familia);
        }

        public static void AgregarFamilia(Familia hijo, Familia padre)
        {
            new FamiliaFamiliaRepository().Add(hijo, padre);
        }

        /// <summary>Quita una patente o una subfamilia de la familia (no borra el elemento).</summary>
        public static void Quitar(Component componente, Familia familia)
        {
            if (componente is Patente patente)
                new FamiliaPatenteRepository().Remove(patente, familia);
            else if (componente is Familia hijo)
                new FamiliaFamiliaRepository().Remove(hijo, familia);
        }

        public static Familia GetById(Guid id)
        {
            return _repo.GetById(id);
        }

        public static Familia GetByNombre(string nombre)
        {
            return _repo.GetByNombre(nombre);
        }

        public static List<Familia> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
