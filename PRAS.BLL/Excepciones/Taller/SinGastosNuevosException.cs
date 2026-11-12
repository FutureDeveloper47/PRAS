namespace PRAS.BLL.Excepciones
{
    /// <summary>Se tocó Guardar sin gastos nuevos cargados.</summary>
    public class SinGastosNuevosException : TallerBusinessException
    {
        public SinGastosNuevosException()
            : base("No hay gastos nuevos para guardar.", "SIN_GASTOS_NUEVOS")
        {
        }
    }
}
