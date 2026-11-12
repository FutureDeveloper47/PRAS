namespace PRAS.BLL.Excepciones
{
    /// <summary>Base de las reglas de negocio de usuarios e inicio de sesión.</summary>
    public abstract class UsuarioBusinessException : ReglaNegocioException
    {
        protected UsuarioBusinessException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
