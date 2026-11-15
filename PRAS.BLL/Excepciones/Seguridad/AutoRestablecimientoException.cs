namespace PRAS.BLL.Excepciones
{
    /// <summary>
    /// El administrador no puede restablecer SU propia contraseña desde "Administrar usuarios":
    /// la suya la cambia en Mi perfil, donde tiene que escribir la contraseña actual
    /// (así nadie que encuentre la PC abierta se la puede cambiar).
    /// </summary>
    public class AutoRestablecimientoException : UsuarioBusinessException
    {
        public AutoRestablecimientoException()
            : base("Su propia contraseña se cambia desde Mi perfil, escribiendo la contraseña actual.", "AUTO_RESTABLECIMIENTO")
        {
        }
    }
}
