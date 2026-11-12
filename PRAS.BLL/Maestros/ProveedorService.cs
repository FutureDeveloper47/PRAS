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
    /// Casos de uso de Proveedores: .001 Alta, .002 Modificar, .003 Deshabilitar, .004 Listar.
    /// Misma forma que ClienteService.
    /// </summary>
    public class ProveedorService
    {
        private readonly IProveedorRepository _repositorio = FactoryDataAccess.ProveedorRepository;

        // Listar (CU Proveedores.004)
        public List<Proveedor> Listar(string filtro, bool incluirDeshabilitados)
        {
            return _repositorio.Listar((filtro ?? "").Trim(), incluirDeshabilitados);
        }

        // Alta (CU Proveedores.001)
        public int Alta(Proveedor proveedor)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarProveedores);
                ValidarDatos(proveedor);

                proveedor.Activo = true;
                _repositorio.Add(proveedor);
                LoggerService.Info($"Alta del proveedor {proveedor.RazonSocial}.");
                return proveedor.IdProveedor;
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

        // Modificar (CU Proveedores.002)
        public void Modificar(Proveedor proveedor)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarProveedores);

                if (_repositorio.GetById(proveedor.IdProveedor) == null)
                    throw new ProveedorNoEncontradoException(proveedor.IdProveedor);

                ValidarDatos(proveedor);
                _repositorio.Update(proveedor);
                LoggerService.Info($"Modificación del proveedor {proveedor.RazonSocial}.");
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

        // Deshabilitar, no se borra (CU Proveedores.003)
        public void Deshabilitar(int idProveedor)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarProveedores);

                Proveedor proveedor = _repositorio.GetById(idProveedor)
                    ?? throw new ProveedorNoEncontradoException(idProveedor);
                if (!proveedor.Activo)
                    throw new ProveedorYaDeshabilitadoException();

                // No lo damos de baja si todavía nos debe repuestos.
                if (_repositorio.TienePedidosPendientes(idProveedor))
                    throw new ProveedorConPedidosPendientesException(proveedor.RazonSocial);

                _repositorio.Delete(idProveedor);   // baja lógica
                LoggerService.Info($"Deshabilitó al proveedor {proveedor.RazonSocial}.");
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

        /// <summary>Vuelve a habilitar un proveedor deshabilitado (deshace la baja lógica).</summary>
        public void Habilitar(int idProveedor)
        {
            Sesion.ExigirPermiso(Funcion.GestionarProveedores);
            _repositorio.CambiarEstado(idProveedor, true);
            LoggerService.Info($"Habilitó al proveedor con Id {idProveedor}.");
        }

        /// <summary>Valida y "limpia" los datos. Largos máximos = columnas de la tabla Proveedor.</summary>
        private void ValidarDatos(Proveedor proveedor)
        {
            proveedor.RazonSocial = Validar.Requerido(proveedor.RazonSocial, "Razón social", 100);
            proveedor.CUIT = Validar.CUIT(proveedor.CUIT, obligatorio: true);
            proveedor.Domicilio = Validar.Opcional(proveedor.Domicilio, "Domicilio", 100);
            proveedor.Telefono = Validar.Opcional(proveedor.Telefono, "Teléfono", 20);
            proveedor.Email = Validar.Email(proveedor.Email, 100);

            // El CUIT no se puede repetir, ni con un proveedor deshabilitado.
            Proveedor existente = _repositorio.ObtenerPorCUIT(proveedor.CUIT);
            if (existente != null && existente.IdProveedor != proveedor.IdProveedor)
                throw new ProveedorDuplicadoException(proveedor.CUIT, existente.RazonSocial, existente.Activo);
        }
    }
}
