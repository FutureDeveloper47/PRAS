namespace PRAS.BLL.Excepciones
{
    /// <summary>Ya existe una agencia con ese CUIT.</summary>
    public class AgenciaDuplicadaException : AgenciaBusinessException
    {
        public string CUIT { get; }

        public AgenciaDuplicadaException(string cuit, string existente, bool existenteActivo)
            : base($"Ya existe una agencia con el CUIT {cuit}: {existente}" + (existenteActivo ? "." : " (está deshabilitada: puede volver a habilitarla desde la lista)."), "AGENCIA_DUPLICADA")
        {
            CUIT = cuit;
        }
    }
}
