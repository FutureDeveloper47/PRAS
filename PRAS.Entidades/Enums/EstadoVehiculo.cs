namespace PRAS.Entidades
{
    /// <summary>
    /// Ciclo de vida del auto: Comprado -> EnDiagnostico -> EnReparacion -> Reparado
    /// -> EnAgencia -> Publicado -> Vendido. Los números coinciden con la base de datos.
    /// </summary>
    public enum EstadoVehiculo
    {
        Comprado = 1,
        EnDiagnostico = 2,
        EnReparacion = 3,
        Reparado = 4,
        EnAgencia = 5,
        Publicado = 6,
        Vendido = 7
    }
}
