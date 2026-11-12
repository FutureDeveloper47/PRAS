namespace PRAS.BLL.Excepciones
{
    /// <summary>Problema con un archivo elegido por el usuario (no existe, no se pudo copiar o guardar).</summary>
    public class ArchivoInvalidoException : DatoInvalidoException
    {
        public ArchivoInvalidoException(string mensaje)
            : base(mensaje, "ARCHIVO_INVALIDO")
        {
        }
    }
}
