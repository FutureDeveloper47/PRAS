using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la tabla Mecanico en un objeto Mecanico.
    /// </summary>
    internal sealed class MecanicoAdaptador : IAdaptador<Mecanico>
    {
        #region Singleton
        private static readonly MecanicoAdaptador _instance = new MecanicoAdaptador();

        public static MecanicoAdaptador Current
        {
            get { return _instance; }
        }

        private MecanicoAdaptador()
        {
        }
        #endregion

        public Mecanico Fill(DataRow fila)
        {
            return new Mecanico
            {
                IdMecanico = (int)fila["IdMecanico"],
                Nombre = (string)fila["Nombre"],
                Apellido = (string)fila["Apellido"],
                Especialidad = fila["Especialidad"] as string,
                Telefono = fila["Telefono"] as string,
                Email = fila["Email"] as string,
                Activo = (bool)fila["Activo"]
            };
        }
    }
}
