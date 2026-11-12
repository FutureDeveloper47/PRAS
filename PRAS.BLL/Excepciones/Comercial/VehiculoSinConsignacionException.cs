namespace PRAS.BLL.Excepciones
{
    /// <summary>Se quiso publicar un auto que no está en ninguna agencia.</summary>
    public class VehiculoSinConsignacionException : ComercialBusinessException
    {
        public int IdVehiculo { get; }

        public VehiculoSinConsignacionException(int idVehiculo)
            : base("El vehículo no está en ninguna agencia: primero hay que registrar la consignación.", "VEHICULO_SIN_CONSIGNACION")
        {
            IdVehiculo = idVehiculo;
        }
    }
}
