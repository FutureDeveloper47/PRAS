namespace PRAS.BLL.Excepciones
{
    /// <summary>La publicación ya estaba finalizada.</summary>
    public class PublicacionFinalizadaException : ComercialBusinessException
    {
        public int IdPublicacion { get; }

        public PublicacionFinalizadaException(int idPublicacion)
            : base("La publicación ya está finalizada.", "PUBLICACION_FINALIZADA")
        {
            IdPublicacion = idPublicacion;
        }
    }
}
