namespace PRAS.BLL.Excepciones
{
    /// <summary>No se puede deshabilitar un proveedor que todavía nos debe repuestos.</summary>
    public class ProveedorConPedidosPendientesException : ProveedorBusinessException
    {
        public string RazonSocial { get; }

        public ProveedorConPedidosPendientesException(string razonSocial)
            : base("El proveedor tiene pedidos de repuestos pendientes de entrega. No se puede deshabilitar.", "PROVEEDOR_CON_PEDIDOS_PENDIENTES")
        {
            RazonSocial = razonSocial;
        }
    }
}
