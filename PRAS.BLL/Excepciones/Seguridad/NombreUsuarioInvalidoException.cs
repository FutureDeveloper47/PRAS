namespace PRAS.BLL.Excepciones
{
    /// <summary>El nombre de usuario tiene espacios.</summary>
    public class NombreUsuarioInvalidoException : UsuarioBusinessException
    {
        public string NombreUsuario { get; }

        public NombreUsuarioInvalidoException(string nombreUsuario)
            : base("El nombre de usuario no puede contener espacios.", "NOMBRE_USUARIO_INVALIDO")
        {
            NombreUsuario = nombreUsuario;
        }
    }
}
