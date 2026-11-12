namespace PRAS.Entidades
{
    /// <summary>
    /// Agencia externa que recibe autos en consignación para venderlos.
    /// </summary>
    public class Agencia
    {
        public int IdAgencia { get; set; }
        public string RazonSocial { get; set; }
        public string CUIT { get; set; }
        public string Domicilio { get; set; }
        public string Localidad { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
    }
}
