using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la tabla Taller en un objeto Taller.
    /// </summary>
    internal sealed class TallerAdaptador : IAdaptador<Taller>
    {
        #region Singleton
        private static readonly TallerAdaptador _instance = new TallerAdaptador();

        public static TallerAdaptador Current
        {
            get { return _instance; }
        }

        private TallerAdaptador()
        {
        }
        #endregion

        public Taller Fill(DataRow fila)
        {
            return new Taller
            {
                IdTaller = (int)fila["IdTaller"],
                RazonSocial = (string)fila["RazonSocial"],
                CUIT = (string)fila["CUIT"],
                Domicilio = fila["Domicilio"] as string,
                Localidad = fila["Localidad"] as string,
                Telefono = fila["Telefono"] as string,
                Email = fila["Email"] as string
            };
        }
    }
}
