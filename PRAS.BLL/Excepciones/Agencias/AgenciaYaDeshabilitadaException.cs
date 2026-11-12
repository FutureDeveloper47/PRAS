namespace PRAS.BLL.Excepciones
{
    /// <summary>Se quiere deshabilitar una agencia que ya está deshabilitada.</summary>
    public class AgenciaYaDeshabilitadaException : AgenciaBusinessException
    {
        public AgenciaYaDeshabilitadaException()
            : base("La agencia ya está deshabilitada.", "AGENCIA_YA_DESHABILITADA")
        {
        }
    }
}
