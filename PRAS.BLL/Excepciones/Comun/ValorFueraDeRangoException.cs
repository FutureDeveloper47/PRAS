namespace PRAS.BLL.Excepciones
{
    /// <summary>
    /// Un número cargado está fuera de lo permitido (ej: kilometraje negativo, 7 puertas, 0 dueños).
    /// La usa la ficha del auto de la publicación (Comercial > Publicaciones).
    /// </summary>
    public class ValorFueraDeRangoException : DatoInvalidoException
    {
        public string Campo { get; }

        public ValorFueraDeRangoException(string campo, int minimo, int maximo)
            : base($"El campo {campo} debe estar entre {minimo} y {maximo}.", "VALOR_FUERA_DE_RANGO")
        {
            Campo = campo;
        }
    }
}
