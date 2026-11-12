namespace PRAS.BLL.Excepciones
{
    /// <summary>El mecánico está deshabilitado y no se puede usar en una operación nueva.</summary>
    public class MecanicoDeshabilitadoException : MecanicoBusinessException
    {
        public string Nombre { get; }

        public MecanicoDeshabilitadoException(string nombre)
            : base($"El mecánico {nombre} está deshabilitado.", "MECANICO_DESHABILITADO")
        {
            Nombre = nombre;
        }
    }
}
