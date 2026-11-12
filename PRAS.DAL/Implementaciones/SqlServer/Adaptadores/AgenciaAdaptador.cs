using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la tabla Agencia en un objeto Agencia.
    /// </summary>
    internal sealed class AgenciaAdaptador : IAdaptador<Agencia>
    {
        #region Singleton
        private static readonly AgenciaAdaptador _instance = new AgenciaAdaptador();

        public static AgenciaAdaptador Current
        {
            get { return _instance; }
        }

        private AgenciaAdaptador()
        {
        }
        #endregion

        public Agencia Fill(DataRow fila)
        {
            return new Agencia
            {
                IdAgencia = (int)fila["IdAgencia"],
                RazonSocial = (string)fila["RazonSocial"],
                CUIT = (string)fila["CUIT"],
                Domicilio = fila["Domicilio"] as string,
                Localidad = fila["Localidad"] as string,
                Telefono = fila["Telefono"] as string,
                Email = fila["Email"] as string,
                Activo = (bool)fila["Activo"]
            };
        }

        /// <summary>Agencia "resumida" que viene en un JOIN: Id y razón social (alias AgenciaRazonSocial).</summary>
        public Agencia FillResumen(DataRow fila)
        {
            return new Agencia
            {
                IdAgencia = (int)fila["IdAgencia"],
                RazonSocial = (string)fila["AgenciaRazonSocial"]
            };
        }
    }
}
