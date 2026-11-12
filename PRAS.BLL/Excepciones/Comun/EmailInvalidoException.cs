namespace PRAS.BLL.Excepciones
{
    /// <summary>El email no tiene un formato válido.</summary>
    public class EmailInvalidoException : DatoInvalidoException
    {
        public string Email { get; }

        public EmailInvalidoException(string email)
            : base("El email no tiene un formato válido (ej: nombre@dominio.com).", "EMAIL_INVALIDO")
        {
            Email = email;
        }
    }
}
