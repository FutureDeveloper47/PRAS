using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;
using PRAS.Services.Facade;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la tabla Cliente en un objeto Cliente.
    /// El teléfono y el email están encriptados con AES en la base: acá se desencriptan.
    /// </summary>
    internal sealed class ClienteAdaptador : IAdaptador<Cliente>
    {
        #region Singleton
        private static readonly ClienteAdaptador _instance = new ClienteAdaptador();

        public static ClienteAdaptador Current
        {
            get { return _instance; }
        }

        private ClienteAdaptador()
        {
        }
        #endregion

        public Cliente Fill(DataRow fila)
        {
            return new Cliente
            {
                IdCliente = (int)fila["IdCliente"],
                Nombre = (string)fila["Nombre"],
                Apellido = (string)fila["Apellido"],
                DNI = (string)fila["DNI"],
                CUIT = fila["CUIT"] as string,   // "as string" deja null si en la base hay NULL
                Domicilio = fila["Domicilio"] as string,
                Localidad = fila["Localidad"] as string,
                Telefono = CryptographyService.Desencriptar(fila["Telefono"] as string),
                Email = CryptographyService.Desencriptar(fila["Email"] as string),
                Activo = (bool)fila["Activo"]
            };
        }

        /// <summary>
        /// Cliente "resumido" que viene en un JOIN (Compra, Venta): solo los datos que se muestran.
        /// Las columnas vienen con alias: ClienteNombre, ClienteApellido, ClienteDNI.
        /// </summary>
        public Cliente FillResumen(DataRow fila)
        {
            return new Cliente
            {
                IdCliente = (int)fila["IdCliente"],
                Nombre = (string)fila["ClienteNombre"],
                Apellido = (string)fila["ClienteApellido"],
                DNI = (string)fila["ClienteDNI"]
            };
        }
    }
}
