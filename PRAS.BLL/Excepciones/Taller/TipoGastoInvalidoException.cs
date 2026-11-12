namespace PRAS.BLL.Excepciones
{
    /// <summary>El gasto no es Repuesto ni Mano de obra.</summary>
    public class TipoGastoInvalidoException : TallerBusinessException
    {
        public TipoGastoInvalidoException()
            : base("Elija el tipo de gasto: Repuesto o Mano de obra.", "TIPO_GASTO_INVALIDO")
        {
        }
    }
}
