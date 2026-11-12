namespace PRAS.BLL.Excepciones
{
    /// <summary>Base de las reglas de negocio de agencias.</summary>
    public abstract class AgenciaBusinessException : ReglaNegocioException
    {
        protected AgenciaBusinessException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
