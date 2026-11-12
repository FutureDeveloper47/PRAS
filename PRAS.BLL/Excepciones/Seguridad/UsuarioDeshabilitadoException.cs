namespace PRAS.BLL.Excepciones
{
    /// <summary>El usuario está deshabilitado y no puede entrar.</summary>
    public class UsuarioDeshabilitadoException : UsuarioBusinessException
    {
        public string NombreUsuario { get; }

        public UsuarioDeshabilitadoException(string nombreUsuario)
            : base("El usuario está deshabilitado. Consulte con el Administrador.", "USUARIO_DESHABILITADO")
        {
            NombreUsuario = nombreUsuario;
        }
    }
}
