namespace PRAS.BLL.Excepciones
{
    /// <summary>Base de las reglas de negocio de mecánicos.</summary>
    public abstract class MecanicoBusinessException : ReglaNegocioException
    {
        protected MecanicoBusinessException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
