namespace PRAS.BLL.Excepciones
{
    /// <summary>El precio de venta (o el publicado) es menor al mínimo pactado con la agencia.</summary>
    public class PrecioMenorAlMinimoException : ComercialBusinessException
    {
        public decimal Precio { get; }
        public decimal PrecioMinimo { get; }

        public PrecioMenorAlMinimoException(decimal precio, decimal precioMinimo)
            : base($"El precio ({precio:C2}) no puede ser menor al precio mínimo de la consignación ({precioMinimo:C2}).", "PRECIO_MENOR_AL_MINIMO")
        {
            Precio = precio;
            PrecioMinimo = precioMinimo;
        }
    }
}
