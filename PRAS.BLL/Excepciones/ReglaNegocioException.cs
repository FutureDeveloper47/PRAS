using PRAS.Services.Exceptions.Base;

namespace PRAS.BLL.Excepciones
{
    /// <summary>
    /// Base de TODAS las excepciones de negocio de PRAS. Hereda de BusinessRuleException
    /// (proyecto Services).
    /// Es abstracta: siempre se tira una excepción concreta (ClienteDuplicadoException,
    /// PrecioMenorAlMinimoException, etc.) para que el error diga exactamente qué pasó.
    /// </summary>
    public abstract class ReglaNegocioException : BusinessRuleException
    {
        protected ReglaNegocioException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
