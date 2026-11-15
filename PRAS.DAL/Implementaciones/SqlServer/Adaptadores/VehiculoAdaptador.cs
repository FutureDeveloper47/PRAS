using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la tabla Vehiculo en un objeto Vehiculo.
    /// </summary>
    internal sealed class VehiculoAdaptador : IAdaptador<Vehiculo>
    {
        #region Singleton
        private static readonly VehiculoAdaptador _instance = new VehiculoAdaptador();

        public static VehiculoAdaptador Current
        {
            get { return _instance; }
        }

        private VehiculoAdaptador()
        {
        }
        #endregion

        public Vehiculo Fill(DataRow fila)
        {
            return new Vehiculo
            {
                IdVehiculo = (int)fila["IdVehiculo"],
                Dominio = (string)fila["Dominio"],
                Marca = (string)fila["Marca"],
                Modelo = (string)fila["Modelo"],
                Anio = (int)fila["Anio"],
                Color = fila["Color"] as string,
                NumeroChasis = fila["NumeroChasis"] as string,
                NumeroMotor = fila["NumeroMotor"] as string,
                EstadoVehiculo = (EstadoVehiculo)Convert.ToInt32(fila["EstadoVehiculo"]),   // en la base es TINYINT

                // Ficha para la venta (puede estar vacía: NULL)
                Kilometraje = Entero(fila, "Kilometraje"),
                Combustible = Texto(fila, "Combustible"),
                TieneGNC = SiNo(fila, "TieneGNC"),
                Transmision = Texto(fila, "Transmision"),
                Puertas = Entero(fila, "Puertas"),
                CantidadDuenos = Entero(fila, "CantidadDuenos"),
                VTVVigente = SiNo(fila, "VTVVigente"),
                ServiceOficial = SiNo(fila, "ServiceOficial"),
                AireAcondicionado = SiNo(fila, "AireAcondicionado"),
                DireccionAsistida = SiNo(fila, "DireccionAsistida"),
                EstadoChapaPintura = Texto(fila, "EstadoChapaPintura")
            };
        }

        // Las columnas de la ficha pueden no venir en la consulta (o estar en NULL): se leen con cuidado.
        private static bool Tiene(DataRow fila, string columna) =>
            fila.Table.Columns.Contains(columna) && fila[columna] != DBNull.Value;

        private static int? Entero(DataRow fila, string columna) =>
            Tiene(fila, columna) ? Convert.ToInt32(fila[columna]) : null;   // TINYINT o INT

        private static bool? SiNo(DataRow fila, string columna) =>
            Tiene(fila, columna) ? (bool)fila[columna] : null;

        private static string Texto(DataRow fila, string columna) =>
            Tiene(fila, columna) ? (string)fila[columna] : null;
    }
}
