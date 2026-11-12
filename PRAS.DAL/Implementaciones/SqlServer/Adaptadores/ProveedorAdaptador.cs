using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la tabla Proveedor en un objeto Proveedor.
    /// </summary>
    internal sealed class ProveedorAdaptador : IAdaptador<Proveedor>
    {
        #region Singleton
        private static readonly ProveedorAdaptador _instance = new ProveedorAdaptador();

        public static ProveedorAdaptador Current
        {
            get { return _instance; }
        }

        private ProveedorAdaptador()
        {
        }
        #endregion

        public Proveedor Fill(DataRow fila)
        {
            return new Proveedor
            {
                IdProveedor = (int)fila["IdProveedor"],
                RazonSocial = (string)fila["RazonSocial"],
                CUIT = (string)fila["CUIT"],
                Domicilio = fila["Domicilio"] as string,
                Telefono = fila["Telefono"] as string,
                Email = fila["Email"] as string,
                Activo = (bool)fila["Activo"]
            };
        }
    }
}
