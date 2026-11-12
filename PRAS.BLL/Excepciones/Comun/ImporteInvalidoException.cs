namespace PRAS.BLL.Excepciones
{
    /// <summary>Un precio, cantidad o importe tiene que ser mayor a 0.</summary>
    public class ImporteInvalidoException : DatoInvalidoException
    {
        public string Campo { get; }

        public ImporteInvalidoException(string campo)
            : base($"{campo} debe ser mayor a 0.", "IMPORTE_INVALIDO")
        {
            Campo = campo;
        }
    }
}
