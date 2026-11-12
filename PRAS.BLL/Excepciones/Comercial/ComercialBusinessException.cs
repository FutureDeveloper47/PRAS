namespace PRAS.BLL.Excepciones
{
    /// <summary>Base de las reglas de negocio comerciales (consignación, publicación y venta).</summary>
    public abstract class ComercialBusinessException : ReglaNegocioException
    {
        protected ComercialBusinessException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
