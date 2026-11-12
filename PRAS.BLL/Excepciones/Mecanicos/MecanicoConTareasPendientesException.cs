namespace PRAS.BLL.Excepciones
{
    /// <summary>No se puede dar de baja a un mecánico con tareas sin terminar.</summary>
    public class MecanicoConTareasPendientesException : MecanicoBusinessException
    {
        public string Nombre { get; }

        public MecanicoConTareasPendientesException(string nombre)
            : base($"El mecánico {nombre} tiene tareas asignadas sin terminar. No se puede deshabilitar.", "MECANICO_CON_TAREAS_PENDIENTES")
        {
            Nombre = nombre;
        }
    }
}
