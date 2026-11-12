namespace PRAS.Entidades
{
    /// <summary>
    /// Proveedor de repuestos (se le compra por pedido, sin stock).
    /// </summary>
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string RazonSocial { get; set; }
        public string CUIT { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
    }
}
