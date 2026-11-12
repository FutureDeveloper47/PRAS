using PRAS.BLL.Excepciones;
using PRAS.DAL.Factory;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;
using PRAS.Services.Exceptions.DataAccess;
using PRAS.Services.Facade;
using PRAS.Services.Facade.ExtensionsMethods;

namespace PRAS.BLL
{
    /// <summary>
    /// Casos de uso Repuestos: Registrar pedido, Registrar entrega de pedido y Consultar pedidos.
    /// Regla del negocio: no hay stock. Los repuestos se piden al proveedor para una orden de trabajo
    /// y, cuando llegan, su costo se suma solo al presupuesto de esa reparación.
    /// </summary>
    public class PedidoRepuestoService
    {
        private const int LargoDescripcion = 200;

        private readonly IPedidoRepuestoRepository _repositorio = FactoryDataAccess.PedidoRepuestoRepository;
        private readonly IDetallePedidoRepuestoRepository _detalleRepositorio = FactoryDataAccess.DetallePedidoRepuestoRepository;
        private readonly IOrdenTrabajoRepository _ordenRepositorio = FactoryDataAccess.OrdenTrabajoRepository;
        private readonly IProveedorRepository _proveedorRepositorio = FactoryDataAccess.ProveedorRepository;

        /// <summary>estado = null trae todos.</summary>
        public List<PedidoRepuesto> Listar(string estado)
        {
            ExigirAlgunPermiso();
            return _repositorio.Listar(estado);
        }

        public List<DetallePedidoRepuesto> ListarDetalles(int idPedido)
        {
            ExigirAlgunPermiso();
            return _detalleRepositorio.ListarPorPedido(idPedido);
        }

        public bool PuedeRegistrarPedido() => Sesion.TienePermiso(Funcion.RegistrarPedidoRepuestos);
        public bool PuedeRegistrarEntrega() => Sesion.TienePermiso(Funcion.RegistrarEntregaPedido);

        /// <summary>Órdenes abiertas para elegir el auto. El mecánico ve solo las suyas.</summary>
        public List<OrdenTrabajo> ListarOrdenesAbiertas()
        {
            Sesion.ExigirPermiso(Funcion.RegistrarPedidoRepuestos);
            int? idMecanico = Sesion.TienePermiso(Funcion.GestionarOrdenesTrabajo) ? null : Sesion.UsuarioActual.IdMecanico;
            return _ordenRepositorio.Listar(Estados.OrdenAbierta, idMecanico);
        }

        public List<Proveedor> ListarProveedores()
        {
            Sesion.ExigirPermiso(Funcion.RegistrarPedidoRepuestos);
            return _proveedorRepositorio.Listar("", false);
        }

        /// <summary>Arma y valida un renglón del pedido (todavía no se guarda).</summary>
        public DetallePedidoRepuesto CrearDetalle(string descripcion, int cantidad, decimal precioUnitario)
        {
            descripcion = Validar.Requerido(descripcion, "Repuesto", LargoDescripcion);
            Validar.MayorACero(cantidad, "La cantidad");
            Validar.MayorACero(precioUnitario, "El precio unitario");

            return new DetallePedidoRepuesto
            {
                DescripcionRepuesto = descripcion,
                Cantidad = cantidad,
                PrecioUnitario = precioUnitario,
                Subtotal = cantidad * precioUnitario
            };
        }

        /// <summary>Guarda el pedido (Pendiente) con sus repuestos. Devuelve el Id del pedido.</summary>
        public int Registrar(PedidoRepuesto pedido)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.RegistrarPedidoRepuestos);

                Validar.Seleccionado(pedido.IdOrdenTrabajo, "la orden de trabajo (el auto) para la que se piden los repuestos");
                OrdenTrabajo orden = _ordenRepositorio.GetById(pedido.IdOrdenTrabajo)
                    ?? throw new OrdenTrabajoNoEncontradaException(pedido.IdOrdenTrabajo);
                if (orden.EstadoOrden != Estados.OrdenAbierta)
                    throw new OrdenTrabajoCerradaException("La orden de trabajo ya está cerrada: no se le pueden pedir repuestos.");

                Validar.Seleccionado(pedido.IdProveedor, "el proveedor");
                Proveedor proveedor = _proveedorRepositorio.GetById(pedido.IdProveedor)
                    ?? throw new ProveedorNoEncontradoException(pedido.IdProveedor);
                if (!proveedor.Activo)
                    throw new ProveedorDeshabilitadoException(proveedor.RazonSocial);

                if (pedido.Detalles == null || pedido.Detalles.Count == 0)
                    throw new PedidoSinRepuestosException();

                pedido.FechaPedido = DateTime.Now;
                _repositorio.Add(pedido);

                decimal total = pedido.Detalles.Sum(d => d.Subtotal);
                LoggerService.Info($"Registró el pedido de repuestos {pedido.IdPedido} a {proveedor.RazonSocial} para la OT {pedido.IdOrdenTrabajo} ({total:C2}).");
                return pedido.IdPedido;
            }
            catch (ReglaNegocioException ex)
            {
                // Aviso de negocio: queda en la bitácora y sigue hasta la pantalla.
                ExceptionManager.HandleException(ex);
                throw;
            }
            catch (DataAccessException)
            {
                // Ya lo registró la DAL.
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(ex);
                throw;
            }
        }

        /// <summary>
        /// El proveedor entregó el pedido: pasa a Entregado y sus repuestos se suman al presupuesto
        /// de la reparación (así el costo del auto queda completo sin cargarlo dos veces).
        /// </summary>
        public void RegistrarEntrega(int idPedido)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.RegistrarEntregaPedido);

                PedidoRepuesto pedido = _repositorio.GetById(idPedido)
                    ?? throw new PedidoNoEncontradoException(idPedido);
                if (pedido.EstadoPedido != Estados.PedidoPendiente)
                    throw new PedidoYaProcesadoException($"El pedido {idPedido} ya está {pedido.EstadoPedido.ToLower()}.");

                if (!_repositorio.RegistrarEntrega(idPedido, DateTime.Now))
                    throw new PedidoYaProcesadoException("El pedido cambió de estado. Actualice la lista.");

                LoggerService.Info($"Registró la entrega del pedido {idPedido}: {pedido.MontoTotal:C2} sumados al presupuesto de la OT {pedido.IdOrdenTrabajo}.");
            }
            catch (ReglaNegocioException ex)
            {
                // Aviso de negocio: queda en la bitácora y sigue hasta la pantalla.
                ExceptionManager.HandleException(ex);
                throw;
            }
            catch (DataAccessException)
            {
                // Ya lo registró la DAL.
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(ex);
                throw;
            }
        }

        /// <summary>Para consultar alcanza con cualquiera de los tres permisos de Repuestos.</summary>
        private static void ExigirAlgunPermiso()
        {
            if (!Sesion.TienePermiso(Funcion.RegistrarPedidoRepuestos) && !Sesion.TienePermiso(Funcion.RegistrarEntregaPedido))
                Sesion.ExigirPermiso(Funcion.ConsultarPedidosRepuestos);
        }
    }
}
