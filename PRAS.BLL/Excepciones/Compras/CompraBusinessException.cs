namespace PRAS.BLL.Excepciones
{
    /// <summary>Base de las reglas de negocio de las compras de autos.</summary>
    public abstract class CompraBusinessException : ReglaNegocioException
    {
        protected CompraBusinessException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
