namespace PRAS.BLL.Excepciones
{
    /// <summary>No se puede deshabilitar una agencia que todavía tiene autos nuestros.</summary>
    public class AgenciaConConsignacionesException : AgenciaBusinessException
    {
        public string RazonSocial { get; }

        public AgenciaConConsignacionesException(string razonSocial)
            : base("La agencia tiene autos en consignación. Primero hay que retirarlos para poder deshabilitarla.", "AGENCIA_CON_CONSIGNACIONES")
        {
            RazonSocial = razonSocial;
        }
    }
}
