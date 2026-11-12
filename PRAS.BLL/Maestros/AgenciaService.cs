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
    /// Casos de uso de Agencias: .001 Alta, .002 Modificar, .003 Deshabilitar, .004 Listar.
    /// Misma forma que ClienteService.
    /// </summary>
    public class AgenciaService
    {
        private readonly IAgenciaRepository _repositorio = FactoryDataAccess.AgenciaRepository;

        // Listar (CU Agencias.004)
        public List<Agencia> Listar(string filtro, bool incluirDeshabilitadas)
        {
            return _repositorio.Listar((filtro ?? "").Trim(), incluirDeshabilitadas);
        }

        // Alta (CU Agencias.001)
        public int Alta(Agencia agencia)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarAgencias);
                ValidarDatos(agencia);

                agencia.Activo = true;
                _repositorio.Add(agencia);
                LoggerService.Info($"Alta de la agencia {agencia.RazonSocial}.");
                return agencia.IdAgencia;
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

        // Modificar (CU Agencias.002)
        public void Modificar(Agencia agencia)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarAgencias);

                if (_repositorio.GetById(agencia.IdAgencia) == null)
                    throw new AgenciaNoEncontradaException(agencia.IdAgencia);

                ValidarDatos(agencia);
                _repositorio.Update(agencia);
                LoggerService.Info($"Modificación de la agencia {agencia.RazonSocial}.");
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

        // Deshabilitar, no se borra (CU Agencias.003)
        public void Deshabilitar(int idAgencia)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarAgencias);

                Agencia agencia = _repositorio.GetById(idAgencia)
                    ?? throw new AgenciaNoEncontradaException(idAgencia);
                if (!agencia.Activo)
                    throw new AgenciaYaDeshabilitadaException();

                // Si todavía tiene autos nuestros, primero hay que retirarlos.
                if (_repositorio.TieneConsignacionesVigentes(idAgencia))
                    throw new AgenciaConConsignacionesException(agencia.RazonSocial);

                _repositorio.Delete(idAgencia);   // baja lógica
                LoggerService.Info($"Deshabilitó a la agencia {agencia.RazonSocial}.");
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

        /// <summary>Vuelve a habilitar una agencia deshabilitada (deshace la baja lógica).</summary>
        public void Habilitar(int idAgencia)
        {
            Sesion.ExigirPermiso(Funcion.GestionarAgencias);
            _repositorio.CambiarEstado(idAgencia, true);
            LoggerService.Info($"Habilitó a la agencia con Id {idAgencia}.");
        }

        /// <summary>Valida y "limpia" los datos. Largos máximos = columnas de la tabla Agencia.</summary>
        private void ValidarDatos(Agencia agencia)
        {
            agencia.RazonSocial = Validar.Requerido(agencia.RazonSocial, "Razón social", 100);
            agencia.CUIT = Validar.CUIT(agencia.CUIT, obligatorio: true);
            agencia.Domicilio = Validar.Opcional(agencia.Domicilio, "Domicilio", 100);
            agencia.Localidad = Validar.Opcional(agencia.Localidad, "Localidad", 50);
            agencia.Telefono = Validar.Opcional(agencia.Telefono, "Teléfono", 20);
            agencia.Email = Validar.Email(agencia.Email, 100);

            // El CUIT no se puede repetir, ni con una agencia deshabilitada.
            Agencia existente = _repositorio.ObtenerPorCUIT(agencia.CUIT);
            if (existente != null && existente.IdAgencia != agencia.IdAgencia)
                throw new AgenciaDuplicadaException(agencia.CUIT, existente.RazonSocial, existente.Activo);
        }
    }
}
