namespace PRAS.BLL.Excepciones
{
    /// <summary>El proveedor está deshabilitado y no se puede usar en una operación nueva.</summary>
    public class ProveedorDeshabilitadoException : ProveedorBusinessException
    {
        public string Nombre { get; }

        public ProveedorDeshabilitadoException(string nombre)
            : base($"El proveedor {nombre} está deshabilitado.", "PROVEEDOR_DESHABILITADO")
        {
            Nombre = nombre;
        }
    }
}
