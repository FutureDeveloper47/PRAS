namespace PRAS.Entidades
{
    /// <summary>
    /// Diagnóstico de los daños detectados en una recepción.
    /// </summary>
    public class Diagnostico
    {
        public int IdDiagnostico { get; set; }
        public int IdRecepcion { get; set; }
        public int IdVehiculo { get; set; }
        public DateTime FechaDiagnostico { get; set; }
        public string DescripcionDanios { get; set; }
        public string EstadoDiagnostico { get; set; }
        public bool RequiereReparacion { get; set; }
        public string Observaciones { get; set; }
        public List<EvidenciaVehiculo> Evidencias { get; set; } = new List<EvidenciaVehiculo>();

        /// <summary>Auto diagnosticado (para mostrar en pantalla).</summary>
        public Vehiculo Vehiculo { get; set; }
    }
}
