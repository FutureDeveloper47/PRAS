namespace PRAS.BLL.Excepciones
{
    /// <summary>Base de las reglas de negocio de los pedidos de repuestos.</summary>
    public abstract class PedidoRepuestoBusinessException : ReglaNegocioException
    {
        protected PedidoRepuestoBusinessException(string mensaje, string codigo)
            : base(mensaje, codigo)
        {
        }
    }
}
