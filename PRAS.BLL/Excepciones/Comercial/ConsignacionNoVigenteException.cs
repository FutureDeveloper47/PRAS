namespace PRAS.BLL.Excepciones
{
    /// <summary>La consignación ya no está vigente (el auto ya se retiró o se vendió).</summary>
    public class ConsignacionNoVigenteException : ComercialBusinessException
    {
        public ConsignacionNoVigenteException(string mensaje)
            : base(mensaje, "CONSIGNACION_NO_VIGENTE")
        {
        }
    }
}
