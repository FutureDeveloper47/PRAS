namespace PRAS.Entidades
{
    /// <summary>
    /// Renglón de un presupuesto.
    /// </summary>
    public class DetallePresupuesto
    {
        public int IdDetallePresupuesto { get; set; }
        public int IdPresupuesto { get; set; }
        public string DescripcionItem { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public bool Aprobado { get; set; }
    }
}
