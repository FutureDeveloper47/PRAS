namespace PRAS.BLL.Excepciones
{
    /// <summary>Un administrador no puede cambiar sus propios permisos.</summary>
    public class AutoModificacionPermisosException : PermisoBusinessException
    {
        public AutoModificacionPermisosException()
            : base("No puede modificar sus propios permisos. Pídaselo a otro administrador.", "AUTO_MODIFICACION_PERMISOS")
        {
        }
    }
}
