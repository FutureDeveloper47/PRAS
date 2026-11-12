namespace PRAS.BLL.Excepciones
{
    /// <summary>El mecánico elegido ya tiene otro usuario.</summary>
    public class MecanicoConUsuarioException : UsuarioBusinessException
    {
        public MecanicoConUsuarioException(string mensaje)
            : base(mensaje, "MECANICO_CON_USUARIO")
        {
        }
    }
}
