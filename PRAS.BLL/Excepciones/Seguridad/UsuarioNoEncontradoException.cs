namespace PRAS.BLL.Excepciones
{
    /// <summary>El usuario ya no existe en la base.</summary>
    public class UsuarioNoEncontradoException : UsuarioBusinessException
    {
        public int IdUsuario { get; }

        public UsuarioNoEncontradoException(int idUsuario)
            : base("El usuario ya no existe.", "USUARIO_NO_ENCONTRADO")
        {
            IdUsuario = idUsuario;
        }
    }
}
