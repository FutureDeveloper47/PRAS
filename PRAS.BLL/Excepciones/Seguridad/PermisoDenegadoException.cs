namespace PRAS.BLL.Excepciones
{
    /// <summary>El usuario no tiene la patente (permiso) que exige la operación.</summary>
    public class PermisoDenegadoException : ReglaNegocioException
    {
        public Funcion Funcion { get; }

        public PermisoDenegadoException(Funcion funcion)
            : base("No tiene permisos para realizar esta operación.", "PERMISO_DENEGADO")
        {
            Funcion = funcion;
        }
    }
}
