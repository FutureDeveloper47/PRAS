using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de Diagnostico (con JOIN a Vehiculo) en un objeto Diagnostico.
    /// </summary>
    internal sealed class DiagnosticoAdaptador : IAdaptador<Diagnostico>
    {
        #region Singleton
        private static readonly DiagnosticoAdaptador _instance = new DiagnosticoAdaptador();

        public static DiagnosticoAdaptador Current
        {
            get { return _instance; }
        }

        private DiagnosticoAdaptador()
        {
        }
        #endregion

        public Diagnostico Fill(DataRow fila)
        {
            Vehiculo vehiculo = VehiculoAdaptador.Current.Fill(fila);
            return new Diagnostico
            {
                IdDiagnostico = (int)fila["IdDiagnostico"],
                IdRecepcion = (int)fila["IdRecepcion"],
                IdVehiculo = vehiculo.IdVehiculo,
                FechaDiagnostico = (DateTime)fila["FechaDiagnostico"],
                DescripcionDanios = (string)fila["DescripcionDanios"],
                EstadoDiagnostico = (string)fila["EstadoDiagnostico"],
                RequiereReparacion = (bool)fila["RequiereReparacion"],
                Observaciones = fila["Observaciones"] as string,
                Vehiculo = vehiculo
            };
        }
    }
}
