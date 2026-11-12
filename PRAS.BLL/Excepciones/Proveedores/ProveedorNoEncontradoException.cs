namespace PRAS.BLL.Excepciones
{
    /// <summary>El proveedor ya no existe en la base.</summary>
    public class ProveedorNoEncontradoException : ProveedorBusinessException
    {
        public int IdProveedor { get; }

        public ProveedorNoEncontradoException(int id)
            : base("El proveedor ya no existe.", "PROVEEDOR_NO_ENCONTRADO")
        {
            IdProveedor = id;
        }
    }
}
