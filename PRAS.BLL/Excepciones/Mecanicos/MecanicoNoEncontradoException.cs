namespace PRAS.BLL.Excepciones
{
    /// <summary>El mecanico ya no existe en la base.</summary>
    public class MecanicoNoEncontradoException : MecanicoBusinessException
    {
        public int IdMecanico { get; }

        public MecanicoNoEncontradoException(int id)
            : base("El mecánico ya no existe.", "MECANICO_NO_ENCONTRADO")
        {
            IdMecanico = id;
        }
    }
}
