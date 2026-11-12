namespace PRAS.BLL.Excepciones
{
    /// <summary>El DNI no tiene 7 u 8 números.</summary>
    public class DNIInvalidoException : DatoInvalidoException
    {
        public string DNI { get; }

        public DNIInvalidoException(string dni)
            : base("El DNI debe tener 7 u 8 números (sin letras).", "DNI_INVALIDO")
        {
            DNI = dni;
        }
    }
}
