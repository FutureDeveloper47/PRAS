namespace PRAS.BLL.Excepciones
{
    /// <summary>El CUIT no tiene 11 números.</summary>
    public class CUITInvalidoException : DatoInvalidoException
    {
        public string CUIT { get; }

        public CUITInvalidoException(string cuit)
            : base("El CUIT debe tener 11 números (ej: 20-12345678-9).", "CUIT_INVALIDO")
        {
            CUIT = cuit;
        }
    }
}
