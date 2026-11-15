namespace PRAS.Entidades
{
    /// <summary>
    /// Mecánico del taller.
    /// </summary>
    public class Mecanico
    {
        public int IdMecanico { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Especialidad { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }

        /// <summary>Qué repara (texto libre). Se carga al pedir una cuenta de mecánico.</summary>
        public string QueRepara { get; set; }

        /// <summary>Matrícula o experiencia (texto libre). Se carga al pedir una cuenta de mecánico.</summary>
        public string MatriculaExperiencia { get; set; }
        public string NombreCompleto => $"{Apellido}, {Nombre}";
    }
}
