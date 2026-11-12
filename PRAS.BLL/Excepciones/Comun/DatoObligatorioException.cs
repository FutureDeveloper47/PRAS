namespace PRAS.BLL.Excepciones
{
    /// <summary>Falta completar un campo obligatorio.</summary>
    public class DatoObligatorioException : DatoInvalidoException
    {
        public string Campo { get; }

        public DatoObligatorioException(string campo)
            : base($"El campo {campo} es obligatorio.", "DATO_OBLIGATORIO")
        {
            Campo = campo;
        }
    }
}
