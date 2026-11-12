namespace PRAS.BLL.Excepciones
{
    /// <summary>La patente no tiene el formato viejo (ABC123) ni el Mercosur (AB123CD).</summary>
    public class DominioInvalidoException : VehiculoBusinessException
    {
        public string Dominio { get; }

        public DominioInvalidoException(string dominio)
            : base("El dominio no es válido. Formatos aceptados: ABC123 (viejo) o AB123CD (Mercosur).", "DOMINIO_INVALIDO")
        {
            Dominio = dominio;
        }
    }
}
