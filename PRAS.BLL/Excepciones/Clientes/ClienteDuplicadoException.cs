namespace PRAS.BLL.Excepciones
{
    /// <summary>Ya existe un cliente con ese DNI.</summary>
    public class ClienteDuplicadoException : ClienteBusinessException
    {
        public string DNI { get; }

        public ClienteDuplicadoException(string dni, string existente, bool existenteActivo)
            : base($"Ya existe un cliente con el DNI {dni}: {existente}" + (existenteActivo ? "." : " (está deshabilitado: puede volver a habilitarlo desde la lista)."), "CLIENTE_DUPLICADO")
        {
            DNI = dni;
        }
    }
}
