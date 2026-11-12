using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.DataAccess.Implementations.Adapters
{
    /// <summary>
    /// Convierte una fila de la tabla Familia en un objeto Familia
    /// y la "hidrata" con sus hijos: primero sus subfamilias (que a su vez traen las suyas,
    /// de forma recursiva) y después sus patentes.
    /// </summary>
    internal class FamiliaAdapter : IAdapter<Familia>
    {
        #region Singleton
        private readonly static FamiliaAdapter _instance = new FamiliaAdapter();

        public static FamiliaAdapter Current
        {
            get
            {
                return _instance;
            }
        }

        private FamiliaAdapter()
        {
        }
        #endregion

        /// <summary>values = IdFamilia, Nombre</summary>
        public Familia Get(object[] values)
        {
            Familia familia = new Familia();
            // Cargamos los datos simples
            familia.Id = Guid.Parse(values[0].ToString());
            familia.Nombre = values[1].ToString();

            // Cargamos los hijos
            familia.AddRange(new FamiliaFamiliaRepository().GetByObject(familia));
            familia.AddRange(new FamiliaPatenteRepository().GetByObject(familia));

            return familia;
        }
    }
}
