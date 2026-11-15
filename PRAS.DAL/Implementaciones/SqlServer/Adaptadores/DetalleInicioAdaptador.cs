using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila del detalle de una tarjeta del Inicio (TALLER, INGRESOS o AGENCIA) en un objeto DetalleInicio.
    /// </summary>
    internal sealed class DetalleInicioAdaptador : IAdaptador<DetalleInicio>
    {
        #region Singleton
        private static readonly DetalleInicioAdaptador _instance = new DetalleInicioAdaptador();

        public static DetalleInicioAdaptador Current
        {
            get { return _instance; }
        }

        private DetalleInicioAdaptador()
        {
        }
        #endregion

        public DetalleInicio Fill(DataRow fila)
        {
            return new DetalleInicio
            {
                IdVehiculo = (int)fila["IdVehiculo"],
                Fecha = fila["Fecha"] == DBNull.Value ? null : (DateTime?)fila["Fecha"],
                Dominio = (string)fila["Dominio"],
                Vehiculo = $"{fila["Marca"]} {fila["Modelo"]} ({fila["Anio"]})",
                Estado = TextoEstado(Convert.ToInt32(fila["EstadoVehiculo"])),
                Persona = fila["Persona"] == DBNull.Value ? "" : (string)fila["Persona"],
                Monto = fila["Monto"] == DBNull.Value ? null : (decimal?)fila["Monto"],
                Extra = fila["Extra"] == DBNull.Value ? "" : (string)fila["Extra"],
                EsVenta = (bool)fila["EsVenta"]
            };
        }

        private static string TextoEstado(int estado)
        {
            switch (estado)
            {
                case 1: return "Comprado (chocado)";
                case 2: return "En diagnóstico";
                case 3: return "En reparación";
                case 4: return "Reparado";
                case 5: return "En agencia";
                case 6: return "Publicado";
                case 7: return "Vendido";
                default: return "-";
            }
        }
    }
}
