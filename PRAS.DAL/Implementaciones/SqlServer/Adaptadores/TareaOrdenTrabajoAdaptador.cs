using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;

namespace PRAS.DAL.Implementaciones.SqlServer.Adaptadores
{
    /// <summary>
    /// Convierte una fila de TareaOrdenTrabajo (con JOIN a Mecanico, OrdenTrabajo y Vehiculo) en un objeto TareaOrdenTrabajo.
    /// </summary>
    internal sealed class TareaOrdenTrabajoAdaptador : IAdaptador<TareaOrdenTrabajo>
    {
        #region Singleton
        private static readonly TareaOrdenTrabajoAdaptador _instance = new TareaOrdenTrabajoAdaptador();

        public static TareaOrdenTrabajoAdaptador Current
        {
            get { return _instance; }
        }

        private TareaOrdenTrabajoAdaptador()
        {
        }
        #endregion

        public TareaOrdenTrabajo Fill(DataRow fila)
        {
            return new TareaOrdenTrabajo
            {
                IdTarea = (int)fila["IdTarea"],
                IdOrdenTrabajo = (int)fila["IdOrdenTrabajo"],
                IdMecanico = (int)fila["IdMecanico"],
                DescripcionTarea = (string)fila["DescripcionTarea"],
                CostoEstimado = (decimal)fila["CostoEstimado"],
                EstadoTarea = (string)fila["EstadoTarea"],
                FechaInicio = fila["FechaInicio"] as DateTime?,
                FechaFin = fila["FechaFin"] as DateTime?,
                PrioridadOrden = (string)fila["Prioridad"],
                Mecanico = new Mecanico
                {
                    IdMecanico = (int)fila["IdMecanico"],
                    Nombre = (string)fila["MecanicoNombre"],
                    Apellido = (string)fila["MecanicoApellido"]
                },
                Vehiculo = VehiculoAdaptador.Current.Fill(fila)
            };
        }
    }
}
