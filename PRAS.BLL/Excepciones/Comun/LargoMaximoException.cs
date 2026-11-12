namespace PRAS.BLL.Excepciones
{
    /// <summary>El texto supera el largo máximo de la columna en la base.</summary>
    public class LargoMaximoException : DatoInvalidoException
    {
        public string Campo { get; }
        public int LargoMaximo { get; }

        public LargoMaximoException(string campo, int largoMaximo)
            : base($"El campo {campo} admite como máximo {largoMaximo} caracteres.", "LARGO_MAXIMO")
        {
            Campo = campo;
            LargoMaximo = largoMaximo;
        }
    }
}
