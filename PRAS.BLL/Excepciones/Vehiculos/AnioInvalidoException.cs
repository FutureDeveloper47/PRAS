namespace PRAS.BLL.Excepciones
{
    /// <summary>El año del auto está fuera del rango permitido.</summary>
    public class AnioInvalidoException : VehiculoBusinessException
    {
        public AnioInvalidoException(int anioMinimo, int anioMaximo)
            : base($"El año debe estar entre {anioMinimo} y {anioMaximo}.", "ANIO_INVALIDO")
        {
        }
    }
}
