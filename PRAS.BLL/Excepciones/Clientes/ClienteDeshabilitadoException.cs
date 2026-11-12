namespace PRAS.BLL.Excepciones
{
    /// <summary>El cliente está deshabilitado y no se puede usar en una operación nueva.</summary>
    public class ClienteDeshabilitadoException : ClienteBusinessException
    {
        public string Nombre { get; }

        public ClienteDeshabilitadoException(string nombre)
            : base($"El cliente {nombre} está deshabilitado.", "CLIENTE_DESHABILITADO")
        {
            Nombre = nombre;
        }
    }
}
