namespace PRAS.BLL.Excepciones
{
    /// <summary>El diagnóstico ya no existe.</summary>
    public class DiagnosticoNoEncontradoException : TallerBusinessException
    {
        public int IdDiagnostico { get; }

        public DiagnosticoNoEncontradoException(int idDiagnostico)
            : base("El diagnóstico ya no existe.", "DIAGNOSTICO_NO_ENCONTRADO")
        {
            IdDiagnostico = idDiagnostico;
        }
    }
}
