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
    /// Caso de uso Comercial - Gestionar publicaciones. Un auto que está en la agencia se publica
    /// en internet (MercadoLibre, Marketplace, etc.).
    /// CU.Publicacion.001 Publicar (Registrar), .002 Modificar y .003 Finalizar.
    /// Regla: el precio publicado no puede ser menor al precio mínimo pactado con la agencia.
    /// </summary>
    public class PublicacionService
    {
        public static readonly string[] Plataformas =
            { "MercadoLibre", "Facebook Marketplace", "Instagram", "Autocosmos", "DeAutos", "Sitio web de la agencia" };

        private const int LargoPlataforma = 50;
        private const int LargoDescripcion = 1000;

        private readonly IPublicacionRepository _repositorio = FactoryDataAccess.PublicacionRepository;
        private readonly IConsignacionRepository _consignacionRepositorio = FactoryDataAccess.ConsignacionRepository;

        public List<Publicacion> Listar(bool soloActivas)
        {
            Sesion.ExigirPermiso(Funcion.GestionarPublicaciones);
            return _repositorio.Listar(soloActivas);
        }

        /// <summary>Autos que están en una agencia y todavía no se publicaron.</summary>
        public List<Consignacion> ListarConsignacionesParaPublicar()
        {
            Sesion.ExigirPermiso(Funcion.GestionarPublicaciones);
            return _consignacionRepositorio.Listar(true)
                .Where(c => c.Vehiculo.EstadoVehiculo == EstadoVehiculo.EnAgencia)
                .ToList();
        }

        /// <summary>Publica el auto: pasa de En agencia a Publicado. Devuelve el Id de la publicación.</summary>
        public int Registrar(Publicacion publicacion)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarPublicaciones);

                Validar.Seleccionado(publicacion.IdVehiculo, "el vehículo a publicar");

                Consignacion consignacion = _consignacionRepositorio.ObtenerVigentePorVehiculo(publicacion.IdVehiculo)
                    ?? throw new VehiculoSinConsignacionException(publicacion.IdVehiculo);
                if (consignacion.Vehiculo.EstadoVehiculo != EstadoVehiculo.EnAgencia)
                    throw new EstadoVehiculoInvalidoException(consignacion.Vehiculo, "publicarlo");

                Validar.MayorACero(publicacion.PrecioPublicado, "El precio publicado");
                if (publicacion.PrecioPublicado < consignacion.PrecioMinimoVenta)
                    throw new PrecioMenorAlMinimoException(publicacion.PrecioPublicado, consignacion.PrecioMinimoVenta);

                publicacion.Plataforma = Validar.Requerido(publicacion.Plataforma, "Plataforma", LargoPlataforma);
                publicacion.Descripcion = Validar.Opcional(publicacion.Descripcion, "Descripción", LargoDescripcion);
                publicacion.FechaPublicacion = DateTime.Now;

                _repositorio.Add(publicacion);
                LoggerService.Info($"Publicó el vehículo {consignacion.Vehiculo.Dominio} en {publicacion.Plataforma} a {publicacion.PrecioPublicado:C2}.");
                return publicacion.IdPublicacion;
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
        /// Consignación vigente del auto. La pantalla de modificar la usa para mostrar
        /// el precio mínimo pactado con la agencia.
        /// </summary>
        public Consignacion ObtenerConsignacionVigente(int idVehiculo)
        {
            Sesion.ExigirPermiso(Funcion.GestionarPublicaciones);
            return _consignacionRepositorio.ObtenerVigentePorVehiculo(idVehiculo)
                ?? throw new VehiculoSinConsignacionException(idVehiculo);
        }

        /// <summary>
        /// CU.Publicacion.002 - Modificar publicación. Cambia plataforma, precio y descripción
        /// de un aviso ACTIVO. Mismas reglas que al publicar: el precio no puede bajar del mínimo
        /// pactado con la agencia. El auto sigue Publicado.
        /// </summary>
        public void Modificar(Publicacion publicacion)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarPublicaciones);

                // Buscamos la publicación como está en la base (no confiamos en lo que trae la pantalla)
                Publicacion actual = _repositorio.GetById(publicacion.IdPublicacion)
                    ?? throw new PublicacionNoEncontradaException(publicacion.IdPublicacion);
                if (actual.EstadoPublicacion != Estados.PublicacionActiva)
                    throw new PublicacionFinalizadaException(publicacion.IdPublicacion);

                Consignacion consignacion = _consignacionRepositorio.ObtenerVigentePorVehiculo(actual.IdVehiculo)
                    ?? throw new VehiculoSinConsignacionException(actual.IdVehiculo);

                Validar.MayorACero(publicacion.PrecioPublicado, "El precio publicado");
                if (publicacion.PrecioPublicado < consignacion.PrecioMinimoVenta)
                    throw new PrecioMenorAlMinimoException(publicacion.PrecioPublicado, consignacion.PrecioMinimoVenta);

                actual.Plataforma = Validar.Requerido(publicacion.Plataforma, "Plataforma", LargoPlataforma);
                actual.Descripcion = Validar.Opcional(publicacion.Descripcion, "Descripción", LargoDescripcion);
                decimal precioAnterior = actual.PrecioPublicado;
                actual.PrecioPublicado = publicacion.PrecioPublicado;

                _repositorio.Modificar(actual);
                LoggerService.Info($"Modificó la publicación del vehículo {actual.Vehiculo.Dominio} en {actual.Plataforma}: " +
                                   $"precio {precioAnterior:C2} -> {actual.PrecioPublicado:C2}.");
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

        /// <summary>Da de baja el aviso sin vender: el auto vuelve a En agencia.</summary>
        public void Finalizar(int idPublicacion)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarPublicaciones);

                Publicacion publicacion = _repositorio.GetById(idPublicacion)
                    ?? throw new PublicacionNoEncontradaException(idPublicacion);
                if (publicacion.EstadoPublicacion != Estados.PublicacionActiva)
                    throw new PublicacionFinalizadaException(idPublicacion);

                _repositorio.Finalizar(idPublicacion, DateTime.Now);
                LoggerService.Info($"Finalizó la publicación del vehículo {publicacion.Vehiculo.Dominio} en {publicacion.Plataforma}.");
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
    }
}
