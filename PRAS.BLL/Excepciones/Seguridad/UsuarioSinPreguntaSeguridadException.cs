namespace PRAS.BLL.Excepciones
{
    /// <summary>
    /// Recuperar contraseña: el usuario no existe, está deshabilitado o nunca cargó su pregunta de seguridad.
    /// Damos el mismo mensaje en los tres casos para no dar pistas a quien intenta adivinar usuarios.
    /// </summary>
    public class UsuarioSinPreguntaSeguridadException : UsuarioBusinessException
    {
        public UsuarioSinPreguntaSeguridadException()
            : base("No se puede recuperar la contraseña de ese usuario. Comuníquese con el administrador.", "USUARIO_SIN_PREGUNTA_SEGURIDAD")
        {
        }
    }
}
