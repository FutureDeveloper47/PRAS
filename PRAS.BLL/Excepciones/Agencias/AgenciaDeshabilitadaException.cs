namespace PRAS.BLL.Excepciones
{
    /// <summary>La agencia está deshabilitada y no se puede usar en una operación nueva.</summary>
    public class AgenciaDeshabilitadaException : AgenciaBusinessException
    {
        public string Nombre { get; }

        public AgenciaDeshabilitadaException(string nombre)
            : base($"La agencia {nombre} está deshabilitada.", "AGENCIA_DESHABILITADA")
        {
            Nombre = nombre;
        }
    }
}
