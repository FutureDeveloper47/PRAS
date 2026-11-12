using PRAS.Entidades;

namespace PRAS.BLL.Excepciones
{
    /// <summary>El auto no está en el estado que la operación necesita (por ejemplo, publicar un auto que no está en la agencia).</summary>
    public class EstadoVehiculoInvalidoException : VehiculoBusinessException
    {
        public string Dominio { get; }
        public EstadoVehiculo EstadoActual { get; }

        public EstadoVehiculoInvalidoException(Vehiculo vehiculo, string operacion)
            : base($"El vehículo {vehiculo.Dominio} está \"{VehiculoService.TextoEstado(vehiculo.EstadoVehiculo)}\": no se puede {operacion}.", "VEHICULO_ESTADO_INVALIDO")
        {
            Dominio = vehiculo.Dominio;
            EstadoActual = vehiculo.EstadoVehiculo;
        }
    }
}
