using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de Publicacion (con JOIN a Vehiculo) en un objeto Publicacion.
    /// </summary>
    internal sealed class PublicacionAdaptador : IAdaptador<Publicacion>
    {
        #region Singleton
        private static readonly PublicacionAdaptador _instance = new PublicacionAdaptador();

        public static PublicacionAdaptador Current
        {
            get { return _instance; }
        }

        private PublicacionAdaptador()
        {
        }
        #endregion

        public Publicacion Fill(DataRow fila)
        {
            return new Publicacion
            {
                IdPublicacion = (int)fila["IdPublicacion"],
                IdVehiculo = (int)fila["IdVehiculo"],
                FechaPublicacion = (DateTime)fila["FechaPublicacion"],
                FechaFinalizacion = fila["FechaFinalizacion"] as DateTime?,
                PrecioPublicado = (decimal)fila["PrecioPublicado"],
                Plataforma = (string)fila["Plataforma"],
                Descripcion = fila["Descripcion"] as string,
                EstadoPublicacion = (string)fila["EstadoPublicacion"],
                Vehiculo = VehiculoAdaptador.Current.Fill(fila)
            };
        }
    }
}
