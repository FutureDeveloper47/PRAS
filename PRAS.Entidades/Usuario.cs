namespace PRAS.Entidades
{
    /// <summary>
    /// Usuario del sistema.
    /// - Rol: tipo de usuario (Dueño, Administrador o Mecánico). Si es Mecánico, IdMecanico
    ///   indica qué mecánico del taller es (para mostrarle sus tareas).
    /// Sus PERMISOS (familias y patentes, patrón Composite) no van acá: los carga la BLL
    /// (Sesion) desde el proyecto Services, así Entidades no depende de ninguna otra capa.
    /// </summary>
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ContrasenaHash { get; set; }
        public Rol Rol { get; set; }
        public int? IdMecanico { get; set; }
        public bool Activo { get; set; }

        /// <summary>Idioma elegido por el usuario (ej: "es-AR", "en-US"). null = idioma por defecto.</summary>
        public string Idioma { get; set; }
        public Mecanico Mecanico { get; set; }
    }
}
