namespace PRAS.BLL.Excepciones
{
    /// <summary>El vehículo ya no existe en la base.</summary>
    public class VehiculoNoEncontradoException : VehiculoBusinessException
    {
        public int IdVehiculo { get; }

        public VehiculoNoEncontradoException(int idVehiculo)
            : base("El vehículo ya no existe.", "VEHICULO_NO_ENCONTRADO")
        {
            IdVehiculo = idVehiculo;
        }
    }
}
