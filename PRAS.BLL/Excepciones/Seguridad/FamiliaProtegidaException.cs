namespace PRAS.BLL.Excepciones
{
    /// <summary>La familia Administrador no se puede borrar ni modificar.</summary>
    public class FamiliaProtegidaException : PermisoBusinessException
    {
        public FamiliaProtegidaException(string mensaje)
            : base(mensaje, "FAMILIA_PROTEGIDA")
        {
        }
    }
}
