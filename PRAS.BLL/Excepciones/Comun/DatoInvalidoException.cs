namespace PRAS.BLL.Excepciones
{
    /// <summary>Base de los errores de carga de datos (campo vacío, formato incorrecto, etc.).</summary>
    public abstract class DatoInvalidoException : ReglaNegocioException
    {
        protected DatoInvalidoException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
