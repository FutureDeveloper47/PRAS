namespace PRAS.BLL.Excepciones
{
    /// <summary>Se quiere deshabilitar un cliente que ya está deshabilitado.</summary>
    public class ClienteYaDeshabilitadoException : ClienteBusinessException
    {
        public ClienteYaDeshabilitadoException()
            : base("El cliente ya está deshabilitado.", "CLIENTE_YA_DESHABILITADO")
        {
        }
    }
}
