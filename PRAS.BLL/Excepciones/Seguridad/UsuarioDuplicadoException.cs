namespace PRAS.BLL.Excepciones
{
    /// <summary>Ya existe un usuario con ese nombre.</summary>
    public class UsuarioDuplicadoException : UsuarioBusinessException
    {
        public string NombreUsuario { get; }

        public UsuarioDuplicadoException(string nombreUsuario)
            : base($"Ya existe un usuario llamado \"{nombreUsuario}\".", "USUARIO_DUPLICADO")
        {
            NombreUsuario = nombreUsuario;
        }
    }
}
