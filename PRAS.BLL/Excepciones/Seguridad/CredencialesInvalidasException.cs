namespace PRAS.BLL.Excepciones
{
    /// <summary>Usuario o contraseña incorrectos. Mismo mensaje en los dos casos para no dar pistas a quien intenta adivinar usuarios.</summary>
    public class CredencialesInvalidasException : UsuarioBusinessException
    {
        public CredencialesInvalidasException()
            : base("Usuario o contraseña incorrectos.", "CREDENCIALES_INVALIDAS")
        {
        }
    }
}
