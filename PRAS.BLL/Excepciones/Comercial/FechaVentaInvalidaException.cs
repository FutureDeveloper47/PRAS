namespace PRAS.BLL.Excepciones
{
    /// <summary>La fecha de venta es anterior a la entrega del auto a la agencia.</summary>
    public class FechaVentaInvalidaException : ComercialBusinessException
    {
        public FechaVentaInvalidaException()
            : base("La fecha de venta no puede ser anterior a la entrega del auto a la agencia.", "FECHA_VENTA_INVALIDA")
        {
        }
    }
}
