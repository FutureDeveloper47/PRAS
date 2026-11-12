namespace PRAS.BLL.Excepciones
{
    /// <summary>Se quiere deshabilitar un proveedor que ya está deshabilitado.</summary>
    public class ProveedorYaDeshabilitadoException : ProveedorBusinessException
    {
        public ProveedorYaDeshabilitadoException()
            : base("El proveedor ya está deshabilitado.", "PROVEEDOR_YA_DESHABILITADO")
        {
        }
    }
}
