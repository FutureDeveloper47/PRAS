namespace PRAS.Entidades
{
    /// <summary>
    /// Foto o archivo que respalda un diagnóstico.
    /// </summary>
    public class EvidenciaVehiculo
    {
        public int IdEvidencia { get; set; }
        public int IdDiagnostico { get; set; }
        public int IdVehiculo { get; set; }
        public string Descripcion { get; set; }
        public string TipoEvidencia { get; set; }
        public string UbicacionArchivo { get; set; }
        public DateTime FechaCarga { get; set; }
    }
}
