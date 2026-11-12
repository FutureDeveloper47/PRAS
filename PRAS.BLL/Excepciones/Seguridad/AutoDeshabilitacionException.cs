namespace PRAS.BLL.Excepciones
{
    /// <summary>Un usuario no se puede deshabilitar a sí mismo.</summary>
    public class AutoDeshabilitacionException : UsuarioBusinessException
    {
        public AutoDeshabilitacionException()
            : base("No puede deshabilitar su propio usuario.", "AUTO_DESHABILITACION")
        {
        }
    }
}
