namespace PRAS.BLL.Excepciones
{
    /// <summary>La contraseña y su confirmación no son iguales.</summary>
    public class ContrasenasNoCoincidenException : UsuarioBusinessException
    {
        public ContrasenasNoCoincidenException()
            : base("La contraseña y su confirmación no coinciden.", "CONTRASENAS_NO_COINCIDEN")
        {
        }
    }
}
