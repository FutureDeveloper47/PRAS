using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.DataAccess.Implementations.Adapters
{
    /// <summary>Convierte una fila de la tabla Patente en un objeto Patente.</summary>
    internal class PatenteAdapter : IAdapter<Patente>
    {
        #region Singleton
        private readonly static PatenteAdapter _instance = new PatenteAdapter();

        public static PatenteAdapter Current
        {
            get
            {
                return _instance;
            }
        }

        private PatenteAdapter()
        {
        }
        #endregion

        /// <summary>values = IdPatente, DataKey, TipoAcceso, Nombre</summary>
        public Patente Get(object[] values)
        {
            Patente patente = new Patente();
            patente.Id = Guid.Parse(values[0].ToString());
            patente.DataKey = values[1].ToString();
            patente.TipoAcceso = (TipoAcceso)Enum.Parse(typeof(TipoAcceso), values[2].ToString());
            patente.Nombre = values[3] == DBNull.Value ? null : values[3].ToString();
            return patente;
        }
    }
}
