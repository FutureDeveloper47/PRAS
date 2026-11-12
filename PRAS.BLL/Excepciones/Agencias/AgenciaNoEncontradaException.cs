namespace PRAS.BLL.Excepciones
{
    /// <summary>La agencia ya no existe en la base.</summary>
    public class AgenciaNoEncontradaException : AgenciaBusinessException
    {
        public int IdAgencia { get; }

        public AgenciaNoEncontradaException(int id)
            : base("La agencia ya no existe.", "AGENCIA_NO_ENCONTRADA")
        {
            IdAgencia = id;
        }
    }
}
