namespace PRAS.Services.Logic.CustomExceptions
{
    /// <summary>
    /// Se lanza cuando una palabra no está en el archivo del idioma actual.
    /// </summary>
    internal class PalabraNoEncontradaException : Exception
    {
        // Acá se puede agregar cualquier dato que le sirva
        // a quien atrape este error.
        public string Palabra { get; private set; }

        public PalabraNoEncontradaException(string palabra) : base($"La palabra {palabra} no ha sido encontrada")
        {
            this.Palabra = palabra;
        }
    }
}
