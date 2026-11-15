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

        // Ficha del auto para venderlo (se carga al publicarlo; null = todavía no se cargó).
        // Son datos DEL AUTO, por eso van en Vehiculo y no en Publicacion.

        public int? Kilometraje { get; set; }

        /// <summary>Nafta, Diésel, GNC, Híbrido o Eléctrico (se guarda en español).</summary>
        public string Combustible { get; set; }

        /// <summary>true si tiene equipo de GNC (puede ser nafta + GNC).</summary>
        public bool? TieneGNC { get; set; }

        /// <summary>Manual o Automática (se guarda en español).</summary>
        public string Transmision { get; set; }

        public int? Puertas { get; set; }
        public int? CantidadDuenos { get; set; }
        public bool? VTVVigente { get; set; }
        public bool? ServiceOficial { get; set; }
        public bool? AireAcondicionado { get; set; }
        public bool? DireccionAsistida { get; set; }

        /// <summary>Excelente, Muy bueno, Bueno o Regular (se guarda en español).</summary>
        public string EstadoChapaPintura { get; set; }
        public string Descripcion => $"{Dominio} - {Marca} {Modelo} ({Anio})";
    }
}
