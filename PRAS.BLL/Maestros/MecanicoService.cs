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
    /// Casos de uso de Mecánicos: .001 Alta, .002 Modificar, .003 Deshabilitar, .004 Listar.
    /// Misma forma que ClienteService.
    /// </summary>
    public class MecanicoService
    {
        private readonly IMecanicoRepository _repositorio = FactoryDataAccess.MecanicoRepository;

        // Listar (CU Mecanicos.004)
        public List<Mecanico> Listar(string filtro, bool incluirDeshabilitados)
        {
            return _repositorio.Listar((filtro ?? "").Trim(), incluirDeshabilitados);
        }

        // Alta (CU Mecanicos.001)
        public int Alta(Mecanico mecanico)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarMecanicos);
                ValidarDatos(mecanico);

                mecanico.Activo = true;
                _repositorio.Add(mecanico);
                LoggerService.Info($"Alta del mecánico {mecanico.NombreCompleto}.");
                return mecanico.IdMecanico;
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

        // Modificar (CU Mecanicos.002)
        public void Modificar(Mecanico mecanico)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarMecanicos);

                if (_repositorio.GetById(mecanico.IdMecanico) == null)
                    throw new MecanicoNoEncontradoException(mecanico.IdMecanico);

                ValidarDatos(mecanico);
                _repositorio.Update(mecanico);
                LoggerService.Info($"Modificación del mecánico {mecanico.NombreCompleto}.");
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

        // Deshabilitar, no se borra (CU Mecanicos.003)
        public void Deshabilitar(int idMecanico)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarMecanicos);

                Mecanico mecanico = _repositorio.GetById(idMecanico)
                    ?? throw new MecanicoNoEncontradoException(idMecanico);
                if (!mecanico.Activo)
                    throw new MecanicoYaDeshabilitadoException();

                // No lo podemos dar de baja si tiene tareas sin terminar.
                if (_repositorio.TieneTareasPendientes(idMecanico))
                    throw new MecanicoConTareasPendientesException(mecanico.NombreCompleto);

                _repositorio.Delete(idMecanico);   // baja lógica
                LoggerService.Info($"Deshabilitó al mecánico {mecanico.NombreCompleto}.");
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

        /// <summary>Vuelve a habilitar un mecánico deshabilitado (deshace la baja lógica).</summary>
        public void Habilitar(int idMecanico)
        {
            Sesion.ExigirPermiso(Funcion.GestionarMecanicos);
            _repositorio.CambiarEstado(idMecanico, true);
            LoggerService.Info($"Habilitó al mecánico con Id {idMecanico}.");
        }

        /// <summary>Valida y "limpia" los datos. Largos máximos = columnas de la tabla Mecanico.</summary>
        private static void ValidarDatos(Mecanico mecanico)
        {
            mecanico.Nombre = Validar.Requerido(mecanico.Nombre, "Nombre", 50);
            mecanico.Apellido = Validar.Requerido(mecanico.Apellido, "Apellido", 50);
            mecanico.Especialidad = Validar.Opcional(mecanico.Especialidad, "Especialidad", 50);
            mecanico.Telefono = Validar.Opcional(mecanico.Telefono, "Teléfono", 20);
            mecanico.Email = Validar.Email(mecanico.Email, 100);
        }
    }
}
