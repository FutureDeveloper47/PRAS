namespace PRAS.BLL.Excepciones
{
    /// <summary>Ya hay otro auto registrado con esa patente.</summary>
    public class DominioDuplicadoException : VehiculoBusinessException
    {
        public string Dominio { get; }

        public DominioDuplicadoException(string dominio, string existente)
            : base($"El vehículo con dominio {dominio} ya está registrado ({existente}).", "DOMINIO_DUPLICADO")
        {
            Dominio = dominio;
        }
    }
}
