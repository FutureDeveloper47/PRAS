namespace PRAS.BLL.Excepciones
{
    /// <summary>Se quiere deshabilitar un mecánico que ya está deshabilitado.</summary>
    public class MecanicoYaDeshabilitadoException : MecanicoBusinessException
    {
        public MecanicoYaDeshabilitadoException()
            : base("El mecánico ya está deshabilitado.", "MECANICO_YA_DESHABILITADO")
        {
        }
    }
}
