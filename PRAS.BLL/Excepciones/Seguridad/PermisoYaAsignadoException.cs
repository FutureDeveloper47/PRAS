namespace PRAS.BLL.Excepciones
{
    /// <summary>La familia o el usuario ya tiene ese permiso.</summary>
    public class PermisoYaAsignadoException : PermisoBusinessException
    {
        public PermisoYaAsignadoException(string mensaje)
            : base(mensaje, "PERMISO_YA_ASIGNADO")
        {
        }
    }
}
