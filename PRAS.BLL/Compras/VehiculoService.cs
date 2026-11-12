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
    /// Buscador de autos: listar, ver detalle, modificar y eliminar vehículos.
    /// El alta no está acá: un auto entra al sistema solo cuando se lo compra (CompraService).
    /// </summary>
    public class VehiculoService
    {
        private const int AnioMinimo = 1950;   // igual que el CHECK de la base

        private readonly IVehiculoRepository _repositorio = FactoryDataAccess.VehiculoRepository;
        private readonly ICompraRepository _compraRepositorio = FactoryDataAccess.CompraRepository;

        /// <summary>Texto del estado para mostrar en pantalla.</summary>
        public static string TextoEstado(EstadoVehiculo estado)
        {
            switch (estado)
            {
                case EstadoVehiculo.Comprado: return "Comprado (chocado)";
                case EstadoVehiculo.EnDiagnostico: return "En diagnóstico";
                case EstadoVehiculo.EnReparacion: return "En reparación";
                case EstadoVehiculo.Reparado: return "Reparado";
                case EstadoVehiculo.EnAgencia: return "En agencia";
                case EstadoVehiculo.Publicado: return "Publicado";
                case EstadoVehiculo.Vendido: return "Vendido";
                default: return estado.ToString();
            }
        }

        /// <summary>estado = null trae todos. patente filtra por "contiene".</summary>
        public List<Vehiculo> Listar(EstadoVehiculo? estado, string patente)
        {
            Sesion.ExigirPermiso(Funcion.GestionarVehiculos);

            // La patente se guarda sin espacios ni guiones, así que buscamos igual.
            string texto = (patente ?? "").Trim().ToUpper().Replace(" ", "").Replace("-", "");
            return _repositorio.Listar(estado, texto);
        }

        public Vehiculo ObtenerPorId(int idVehiculo)
        {
            Sesion.ExigirPermiso(Funcion.GestionarVehiculos);
            return _repositorio.GetById(idVehiculo);
        }

        /// <summary>La compra del auto (para "Ver Detalle").</summary>
        public Compra ObtenerCompra(int idVehiculo)
        {
            Sesion.ExigirPermiso(Funcion.GestionarVehiculos);
            return _compraRepositorio.ObtenerPorVehiculo(idVehiculo);
        }

        public void Modificar(Vehiculo vehiculo)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarVehiculos);

                if (_repositorio.GetById(vehiculo.IdVehiculo) == null)
                    throw new VehiculoNoEncontradoException(vehiculo.IdVehiculo);

                ValidarDatos(vehiculo);
                _repositorio.Update(vehiculo);
                LoggerService.Info($"Modificación del vehículo {vehiculo.Dominio}.");
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
        /// Borra el auto y su compra. Solo se puede si el auto sigue Comprado
        /// (se cargó por error y todavía no entró al taller).
        /// </summary>
        public void Eliminar(int idVehiculo)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarVehiculos);

                Vehiculo vehiculo = _repositorio.GetById(idVehiculo)
                    ?? throw new VehiculoNoEncontradoException(idVehiculo);

                if (vehiculo.EstadoVehiculo != EstadoVehiculo.Comprado)
                    throw new EstadoVehiculoInvalidoException(vehiculo,
                        "eliminarlo (solo se pueden eliminar autos Comprados que todavía no entraron al taller)");

                _repositorio.Delete(idVehiculo);
                LoggerService.Warn($"Eliminó el vehículo {vehiculo.Dominio} y su compra.");
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

        /// <summary>Valida los datos del auto. La usan Modificar y Registrar Compra.</summary>
        internal void ValidarDatos(Vehiculo vehiculo)
        {
            vehiculo.Dominio = Validar.Dominio(vehiculo.Dominio);
            vehiculo.Marca = Validar.Requerido(vehiculo.Marca, "Marca", 50);
            vehiculo.Modelo = Validar.Requerido(vehiculo.Modelo, "Modelo", 50);
            vehiculo.Color = Validar.Opcional(vehiculo.Color, "Color", 30);
            vehiculo.NumeroChasis = Validar.Opcional(vehiculo.NumeroChasis, "Nº de chasis", 30);
            vehiculo.NumeroMotor = Validar.Opcional(vehiculo.NumeroMotor, "Nº de motor", 30);

            // Dejamos hasta el año que viene porque a veces patentan el modelo del año siguiente.
            int anioMaximo = DateTime.Today.Year + 1;
            if (vehiculo.Anio < AnioMinimo || vehiculo.Anio > anioMaximo)
                throw new AnioInvalidoException(AnioMinimo, anioMaximo);

            // La patente no se puede repetir, salvo que sea el mismo auto que estamos modificando.
            Vehiculo existente = _repositorio.ObtenerPorDominio(vehiculo.Dominio);
            if (existente != null && existente.IdVehiculo != vehiculo.IdVehiculo)
                throw new DominioDuplicadoException(vehiculo.Dominio, $"{existente.Marca} {existente.Modelo}");
        }
    }
}
