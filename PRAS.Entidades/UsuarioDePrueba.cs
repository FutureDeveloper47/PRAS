namespace PRAS.Entidades
{
    /// <summary>
    /// Un renglón de la tabla "Usuarios de prueba" de la pantalla de ingreso.
    /// No es una tabla de la base: la arma la BLL con los usuarios de prueba conocidos.
    /// </summary>
    public class UsuarioDePrueba
    {
        public string NombreUsuario { get; set; }

        /// <summary>
        /// Contraseña de prueba (es fija y conocida, NO se lee de la base: ahí solo está el hash).
        /// null si alguien la cambió y ya no es la de prueba.
        /// </summary>
        public string Contrasena { get; set; }

        public Rol Rol { get; set; }

        /// <summary>"Carlos Ferreyra". null si no tiene datos personales.</summary>
        public string NombrePersona { get; set; }

        public bool Activo { get; set; }
    }
}
