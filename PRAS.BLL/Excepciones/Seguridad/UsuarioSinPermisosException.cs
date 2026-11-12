namespace PRAS.BLL.Excepciones
{
    /// <summary>El usuario no tiene ninguna familia ni patente asignada.</summary>
    public class UsuarioSinPermisosException : UsuarioBusinessException
    {
        public string NombreUsuario { get; }

        public UsuarioSinPermisosException(string nombreUsuario)
            : base("El usuario no tiene permisos asignados. Consulte con el Administrador.", "USUARIO_SIN_PERMISOS")
        {
            NombreUsuario = nombreUsuario;
        }
    }
}
