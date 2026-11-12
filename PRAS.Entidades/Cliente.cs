namespace PRAS.Entidades
{
    /// <summary>
    /// Persona externa: vende autos chocados a la empresa (Compra) o compra autos reparados (Venta).
    /// </summary>
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string CUIT { get; set; }
        public string Domicilio { get; set; }
        public string Localidad { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
        public string NombreCompleto => $"{Apellido}, {Nombre}";
    }
}
