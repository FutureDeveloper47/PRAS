using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de la consulta de "Actividad reciente" (pantalla de Inicio) en un objeto ActividadReciente.
    /// </summary>
    internal sealed class ActividadRecienteAdaptador : IAdaptador<ActividadReciente>
    {
        #region Singleton
        private static readonly ActividadRecienteAdaptador _instance = new ActividadRecienteAdaptador();

        public static ActividadRecienteAdaptador Current
        {
            get { return _instance; }
        }

        private ActividadRecienteAdaptador()
        {
        }
        #endregion

        public ActividadReciente Fill(DataRow fila)
        {
            return new ActividadReciente
            {
                Fecha = (DateTime)fila["Fecha"],
                Tipo = (string)fila["Tipo"] == "Ingreso" ? TipoActividad.Ingreso : TipoActividad.Venta,
                Marca = (string)fila["Marca"],
                Modelo = (string)fila["Modelo"],
                Dominio = (string)fila["Dominio"]
            };
        }
    }
}
