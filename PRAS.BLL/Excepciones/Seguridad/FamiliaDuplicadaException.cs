namespace PRAS.BLL.Excepciones
{
    /// <summary>Ya existe una familia con ese nombre.</summary>
    public class FamiliaDuplicadaException : PermisoBusinessException
    {
        public string Nombre { get; }

        public FamiliaDuplicadaException(string nombre)
            : base($"Ya existe una familia llamada \"{nombre}\".", "FAMILIA_DUPLICADA")
        {
            Nombre = nombre;
        }
    }
}
