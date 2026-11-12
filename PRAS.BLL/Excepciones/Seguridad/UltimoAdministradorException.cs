namespace PRAS.BLL.Excepciones
{
    /// <summary>Se le quiere sacar el rol (o deshabilitar) al último administrador activo.</summary>
    public class UltimoAdministradorException : UsuarioBusinessException
    {
        public UltimoAdministradorException(string mensaje)
            : base(mensaje, "ULTIMO_ADMINISTRADOR")
        {
        }
    }
}
