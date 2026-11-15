using PRAS.DAL.Interfaces;

namespace PRAS.DAL.Factory
{
    /// <summary>
    /// FACTORY de la capa de datos.
    /// La BLL pide los repositorios SIEMPRE por acá y solo conoce sus INTERFACES:
    /// las implementaciones (carpeta Implementaciones\SqlServer) son internal y nadie de afuera las ve.
    /// Si mañana cambiamos de motor de base (por ejemplo a memoria o a MySQL), se cambia solo este archivo.
    /// </summary>
    public static class FactoryDataAccess
    {
        public static IVehiculoRepository VehiculoRepository { get; private set; }
        public static ICompraRepository CompraRepository { get; private set; }
        public static IClienteRepository ClienteRepository { get; private set; }
        public static IProveedorRepository ProveedorRepository { get; private set; }
        public static IAgenciaRepository AgenciaRepository { get; private set; }
        public static IMecanicoRepository MecanicoRepository { get; private set; }
        public static IUsuarioRepository UsuarioRepository { get; private set; }
        public static ITallerRepository TallerRepository { get; private set; }
        public static IRecepcionVehiculoRepository RecepcionVehiculoRepository { get; private set; }
        public static IDiagnosticoRepository DiagnosticoRepository { get; private set; }
        public static IEvidenciaVehiculoRepository EvidenciaVehiculoRepository { get; private set; }
        public static IPresupuestoRepository PresupuestoRepository { get; private set; }
        public static IDetallePresupuestoRepository DetallePresupuestoRepository { get; private set; }
        public static IOrdenTrabajoRepository OrdenTrabajoRepository { get; private set; }
        public static ITareaOrdenTrabajoRepository TareaOrdenTrabajoRepository { get; private set; }
        public static IPedidoRepuestoRepository PedidoRepuestoRepository { get; private set; }
        public static IDetallePedidoRepuestoRepository DetallePedidoRepuestoRepository { get; private set; }
        public static IConsignacionRepository ConsignacionRepository { get; private set; }
        public static IPublicacionRepository PublicacionRepository { get; private set; }
        public static IVentaRepository VentaRepository { get; private set; }
        public static IInicioRepository InicioRepository { get; private set; }
        public static IReporteRepository ReporteRepository { get; private set; }
        public static ISistemaRepository SistemaRepository { get; private set; }
        public static IDatosPruebaRepository DatosPruebaRepository { get; private set; }

        static FactoryDataAccess()
        {
            // Versión actual: todos los repositorios son los de SQL Server (singleton, con .Current).
            VehiculoRepository = Implementaciones.SqlServer.VehiculoRepository.Current;
            CompraRepository = Implementaciones.SqlServer.CompraRepository.Current;
            ClienteRepository = Implementaciones.SqlServer.ClienteRepository.Current;
            ProveedorRepository = Implementaciones.SqlServer.ProveedorRepository.Current;
            AgenciaRepository = Implementaciones.SqlServer.AgenciaRepository.Current;
            MecanicoRepository = Implementaciones.SqlServer.MecanicoRepository.Current;
            UsuarioRepository = Implementaciones.SqlServer.UsuarioRepository.Current;
            TallerRepository = Implementaciones.SqlServer.TallerRepository.Current;
            RecepcionVehiculoRepository = Implementaciones.SqlServer.RecepcionVehiculoRepository.Current;
            DiagnosticoRepository = Implementaciones.SqlServer.DiagnosticoRepository.Current;
            EvidenciaVehiculoRepository = Implementaciones.SqlServer.EvidenciaVehiculoRepository.Current;
            PresupuestoRepository = Implementaciones.SqlServer.PresupuestoRepository.Current;
            DetallePresupuestoRepository = Implementaciones.SqlServer.DetallePresupuestoRepository.Current;
            OrdenTrabajoRepository = Implementaciones.SqlServer.OrdenTrabajoRepository.Current;
            TareaOrdenTrabajoRepository = Implementaciones.SqlServer.TareaOrdenTrabajoRepository.Current;
            PedidoRepuestoRepository = Implementaciones.SqlServer.PedidoRepuestoRepository.Current;
            DetallePedidoRepuestoRepository = Implementaciones.SqlServer.DetallePedidoRepuestoRepository.Current;
            ConsignacionRepository = Implementaciones.SqlServer.ConsignacionRepository.Current;
            PublicacionRepository = Implementaciones.SqlServer.PublicacionRepository.Current;
            VentaRepository = Implementaciones.SqlServer.VentaRepository.Current;
            InicioRepository = Implementaciones.SqlServer.InicioRepository.Current;
            ReporteRepository = Implementaciones.SqlServer.ReporteRepository.Current;
            SistemaRepository = Implementaciones.SqlServer.SistemaRepository.Current;
            DatosPruebaRepository = Implementaciones.SqlServer.DatosPruebaRepository.Current;
        }
    }
}
