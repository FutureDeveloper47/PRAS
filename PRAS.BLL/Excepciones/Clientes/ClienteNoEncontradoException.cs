namespace PRAS.BLL.Excepciones
{
    /// <summary>El cliente ya no existe en la base.</summary>
    public class ClienteNoEncontradoException : ClienteBusinessException
    {
        public int IdCliente { get; }

        public ClienteNoEncontradoException(int id)
            : base("El cliente ya no existe.", "CLIENTE_NO_ENCONTRADO")
        {
            IdCliente = id;
        }
    }
}
