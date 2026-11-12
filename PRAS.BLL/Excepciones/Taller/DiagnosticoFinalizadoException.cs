namespace PRAS.BLL.Excepciones
{
    /// <summary>El diagnóstico ya estaba finalizado.</summary>
    public class DiagnosticoFinalizadoException : TallerBusinessException
    {
        public int IdDiagnostico { get; }

        public DiagnosticoFinalizadoException(int idDiagnostico)
            : base("El diagnóstico ya está finalizado.", "DIAGNOSTICO_FINALIZADO")
        {
            IdDiagnostico = idDiagnostico;
        }
    }
}
