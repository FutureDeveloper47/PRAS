namespace PRAS.BLL.Excepciones
{
    /// <summary>Mi perfil: para cambiar la contraseña hay que escribir bien la contraseña actual.</summary>
    public class ContrasenaActualIncorrectaException : UsuarioBusinessException
    {
        public ContrasenaActualIncorrectaException()
            : base("La contraseña actual no es correcta.", "CONTRASENA_ACTUAL_INCORRECTA")
        {
        }
    }
}
