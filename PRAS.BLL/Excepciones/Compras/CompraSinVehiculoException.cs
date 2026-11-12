namespace PRAS.BLL.Excepciones
{
    /// <summary>Se quiso registrar una compra sin los datos del auto.</summary>
    public class CompraSinVehiculoException : CompraBusinessException
    {
        public CompraSinVehiculoException()
            : base("Faltan los datos del vehículo.", "COMPRA_SIN_VEHICULO")
        {
        }
    }
}
