namespace PRAS.BLL.Excepciones
{
    /// <summary>La publicación ya no existe.</summary>
    public class PublicacionNoEncontradaException : ComercialBusinessException
    {
        public int IdPublicacion { get; }

        public PublicacionNoEncontradaException(int idPublicacion)
            : base("La publicación ya no existe.", "PUBLICACION_NO_ENCONTRADA")
        {
            IdPublicacion = idPublicacion;
        }
    }
}
