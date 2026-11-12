namespace PRAS.BLL.Excepciones
{
    /// <summary>Base de las reglas de negocio de vehículos.</summary>
    public abstract class VehiculoBusinessException : ReglaNegocioException
    {
        protected VehiculoBusinessException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
