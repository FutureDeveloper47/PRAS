namespace PRAS.BLL.Excepciones
{
    /// <summary>Base de las reglas de negocio de la gestión de permisos (Composite).</summary>
    public abstract class PermisoBusinessException : ReglaNegocioException
    {
        protected PermisoBusinessException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
