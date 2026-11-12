namespace PRAS.BLL.Excepciones
{
    /// <summary>Ya existe un proveedor con ese CUIT.</summary>
    public class ProveedorDuplicadoException : ProveedorBusinessException
    {
        public string CUIT { get; }

        public ProveedorDuplicadoException(string cuit, string existente, bool existenteActivo)
            : base($"Ya existe un proveedor con el CUIT {cuit}: {existente}" + (existenteActivo ? "." : " (está deshabilitado: puede volver a habilitarlo desde la lista)."), "PROVEEDOR_DUPLICADO")
        {
            CUIT = cuit;
        }
    }
}
