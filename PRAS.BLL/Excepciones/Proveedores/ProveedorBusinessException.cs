namespace PRAS.BLL.Excepciones
{
    /// <summary>Base de las reglas de negocio de proveedores.</summary>
    public abstract class ProveedorBusinessException : ReglaNegocioException
    {
        protected ProveedorBusinessException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
