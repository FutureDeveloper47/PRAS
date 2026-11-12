namespace PRAS.BLL.Excepciones
{
    /// <summary>Base de las reglas de negocio de clientes.</summary>
    public abstract class ClienteBusinessException : ReglaNegocioException
    {
        protected ClienteBusinessException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
