namespace PRAS.Entidades
{
    /// <summary>
    /// Pedido de cuenta que hace una persona desde la pantalla de ingreso (CU.Seguridad.016 - Solicitar cuenta).
    /// No es una tabla: con estos datos la BLL crea el usuario (DESHABILITADO), sus datos personales
    /// y, si pide ser Mecánico, también el registro en la tabla Mecanico.
    /// La contraseña y la respuesta de seguridad NO van acá: se pasan aparte y se guardan solo como hash.
    /// </summary>
    public class SolicitudCuenta
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string NombreUsuario { get; set; }

        /// <summary>Rol que pide. El administrador lo puede cambiar antes de habilitar la cuenta.</summary>
        public Rol RolSolicitado { get; set; }

        /// <summary>Pregunta de seguridad elegida (en español).</summary>
        public string PreguntaSeguridad { get; set; }

        // Solo si pide ser Mecánico
        public string Especialidad { get; set; }
        public string QueRepara { get; set; }
        public string MatriculaExperiencia { get; set; }
    }
}
