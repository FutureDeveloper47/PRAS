namespace PRAS.BLL.Excepciones
{
    /// <summary>El rol elegido no es Dueño, Administrador ni Mecánico.</summary>
    public class RolInvalidoException : UsuarioBusinessException
    {
        public RolInvalidoException()
            : base("Seleccione un rol válido.", "ROL_INVALIDO")
        {
        }
    }
}
