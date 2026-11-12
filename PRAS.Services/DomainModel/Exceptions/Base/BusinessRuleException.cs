namespace PRAS.Services.Exceptions.Base
{
    /// <summary>
    /// Excepción de REGLA DE NEGOCIO. No es un error del programa:
    /// es un aviso esperado para el usuario (ej: "El precio es menor al precio mínimo").
    /// En la BLL de PRAS la usa ReglaNegocioException y las excepciones de cada caso de uso.
    /// </summary>
    public abstract class BusinessRuleException : ApplicationException
    {
        protected BusinessRuleException(string message, string code = "BUSINESS_RULE_ERROR")
            : base(message, code)
        {
        }
    }
}
