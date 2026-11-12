namespace PRAS.BLL.Excepciones
{
    /// <summary>El porcentaje de comisión de la agencia está fuera de rango.</summary>
    public class ComisionInvalidaException : ComercialBusinessException
    {
        public decimal Porcentaje { get; }

        public ComisionInvalidaException(decimal porcentaje)
            : base("El porcentaje de comisión debe ser mayor a 0 y como máximo 100.", "COMISION_INVALIDA")
        {
            Porcentaje = porcentaje;
        }
    }
}
