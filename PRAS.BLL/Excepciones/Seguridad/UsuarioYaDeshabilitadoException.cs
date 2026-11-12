namespace PRAS.BLL.Excepciones
{
    /// <summary>Se quiere deshabilitar un usuario que ya está deshabilitado.</summary>
    public class UsuarioYaDeshabilitadoException : UsuarioBusinessException
    {
        public string NombreUsuario { get; }

        public UsuarioYaDeshabilitadoException(string nombreUsuario)
            : base("El usuario ya está deshabilitado.", "USUARIO_YA_DESHABILITADO")
        {
            NombreUsuario = nombreUsuario;
        }
    }
}
