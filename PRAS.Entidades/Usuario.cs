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

        /// <summary>
        /// Pregunta de seguridad que eligió el usuario en "Mi perfil" (null = nunca la cargó).
        /// Se usa para recuperar la contraseña desde la pantalla de ingreso.
        /// Se guarda en la tabla UsuarioRecuperacion de PRAS_Seguridad.
        /// </summary>
        public string PreguntaSeguridad { get; set; }

        /// <summary>Hash SHA-256 de la respuesta (igual que la contraseña, la respuesta nunca se guarda en texto plano).</summary>
        public string RespuestaSeguridadHash { get; set; }
        public Mecanico Mecanico { get; set; }

        // Datos personales (tabla UsuarioPersona de PRAS_Seguridad). Van aparte de Usuario
        // para no cambiar las columnas que protege el dígito verificador (DVH).
        // Para un mecánico, el nombre sale de la tabla Mecanico.

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }

        /// <summary>Teléfono: en la base se guarda encriptado con AES (igual que en Cliente).</summary>
        public string Telefono { get; set; }

        /// <summary>Email: en la base se guarda encriptado con AES (igual que en Cliente).</summary>
        public string Email { get; set; }

        /// <summary>Última vez que inició sesión (sale de la bitácora). null = nunca, o no se consultó.</summary>
        public DateTime? UltimoIngreso { get; set; }

        /// <summary>
        /// Nombre de la persona dueña del usuario, para mostrar: "Carlos Ferreyra".
        /// Si es mecánico se usa la tabla Mecanico; si no, sus datos personales. null si no hay datos.
        /// </summary>
        public string NombrePersona
        {
            get
            {
                if (Mecanico != null)
                    return $"{Mecanico.Nombre} {Mecanico.Apellido}";
                string nombre = $"{Nombre} {Apellido}".Trim();
                return nombre == "" ? null : nombre;
            }
        }
    }
}
