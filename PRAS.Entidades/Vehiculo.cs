namespace PRAS.Entidades
{
    /// <summary>
    /// Auto propio de la empresa. Su EstadoVehiculo indica en qué etapa del recorrido está.
    /// </summary>
    public class Vehiculo
    {
        public int IdVehiculo { get; set; }
        public string Dominio { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public string Color { get; set; }
        public string NumeroChasis { get; set; }
        public string NumeroMotor { get; set; }
        public EstadoVehiculo EstadoVehiculo { get; set; }
        public string Descripcion => $"{Dominio} - {Marca} {Modelo} ({Anio})";
    }
}
