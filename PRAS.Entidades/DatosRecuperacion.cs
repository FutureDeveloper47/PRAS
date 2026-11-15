namespace PRAS.Entidades
{
    /// <summary>
    /// Lo que muestra "Recuperar contraseña" en el paso 1: de quién es el usuario y su pregunta.
    /// Es un paquete chico a propósito: la pantalla de ingreso la puede usar cualquiera,
    /// así que no lleva la contraseña ni la respuesta (ni sus hash).
    /// </summary>
    public class DatosRecuperacion
    {
        public string NombreUsuario { get; set; }

        /// <summary>"Carlos Ferreyra". null si el usuario no tiene datos personales cargados.</summary>
        public string NombrePersona { get; set; }

        public Rol Rol { get; set; }

        /// <summary>Pregunta de seguridad, en español (la pantalla la traduce).</summary>
        public string Pregunta { get; set; }
    }
}
