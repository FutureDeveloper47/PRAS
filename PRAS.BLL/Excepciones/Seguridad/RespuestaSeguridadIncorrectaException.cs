namespace PRAS.BLL.Excepciones
{
    /// <summary>
    /// Recuperar contraseña: la respuesta a la pregunta de seguridad no coincide con la guardada.
    /// </summary>
    public class RespuestaSeguridadIncorrectaException : UsuarioBusinessException
    {
        public RespuestaSeguridadIncorrectaException()
            : base("La respuesta a la pregunta de seguridad no es correcta.", "RESPUESTA_SEGURIDAD_INCORRECTA")
        {
        }
    }
}
