namespace PRAS.BLL.Excepciones
{
    /// <summary>La contraseña es muy corta.</summary>
    public class ContrasenaInvalidaException : UsuarioBusinessException
    {
        public int LargoMinimo { get; }

        public ContrasenaInvalidaException(int largoMinimo)
            : base($"La contraseña debe tener al menos {largoMinimo} caracteres.", "CONTRASENA_INVALIDA")
        {
            LargoMinimo = largoMinimo;
        }
    }
}
