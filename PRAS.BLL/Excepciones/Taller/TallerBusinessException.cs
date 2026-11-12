namespace PRAS.BLL.Excepciones
{
    /// <summary>Base de las reglas de negocio del taller (diagnósticos, órdenes, tareas y costos).</summary>
    public abstract class TallerBusinessException : ReglaNegocioException
    {
        protected TallerBusinessException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
