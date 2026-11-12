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
    /// Casos de uso de Clientes: .001 Alta, .002 Modificar, .003 Deshabilitar,
    /// .004 Consultar Historial, .005 Listar.
    /// Valida las reglas, tira excepciones de negocio
    /// propias (ClienteDuplicadoException, ClienteNoEncontradoException...) y usa los repositorios
    /// que le da la Factory de la DAL (solo conoce las interfaces).
    /// </summary>
    public class ClienteService
    {
        private readonly IClienteRepository _repositorio = FactoryDataAccess.ClienteRepository;
        private readonly ICompraRepository _compraRepositorio = FactoryDataAccess.CompraRepository;
        private readonly IVentaRepository _ventaRepositorio = FactoryDataAccess.VentaRepository;

        /// <summary>
        /// CU.Arq.003 - Encriptar datos. Encripta con AES el teléfono y el email de los clientes
        /// que se cargaron antes de que existiera el encriptado (quedaron en texto plano).
        /// Se vuelve a grabar cada cliente (el repositorio encripta al grabar) y se recalcula
        /// su dígito verificador, porque cambió lo que hay guardado en la fila.
        /// Se llama al iniciar sesión; si no hay nada pendiente, no hace nada.
        /// Devuelve cuántos clientes se encriptaron.
        /// </summary>
        public int EncriptarDatosPendientes()
        {
            List<int> ids = _repositorio.ListarIdsSinEncriptar();
            foreach (int id in ids)
            {
                Cliente cliente = _repositorio.GetById(id);   // lo lee en texto plano
                _repositorio.Update(cliente);                  // lo graba encriptado
                DigitoVerificadorService.RecalcularRegistro("Cliente", id);
            }

            if (ids.Count > 0)
                LoggerService.Info($"Se encriptaron con AES el teléfono y el email de {ids.Count} cliente(s).");
            return ids.Count;
        }

        // Listar (CU Clientes.005)
        public List<Cliente> Listar(string filtro, bool incluirDeshabilitados)
        {
            return _repositorio.Listar((filtro ?? "").Trim(), incluirDeshabilitados);
        }

        /// <summary>
        /// Busca un cliente por DNI (lo usan Registrar Compra y Registrar Venta).
        /// Devuelve null si no existe.
        /// </summary>
        public Cliente ObtenerPorDNI(string dni)
        {
            return _repositorio.ObtenerPorDNI(Validar.DNI(dni));
        }

        // Alta (CU Clientes.001)
        public int Alta(Cliente cliente)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarClientes);
                ValidarDatos(cliente);

                cliente.Activo = true;
                _repositorio.Add(cliente);

                DigitoVerificadorService.RecalcularRegistro("Cliente", cliente.IdCliente);
                LoggerService.Info($"Alta del cliente {cliente.NombreCompleto} (DNI {cliente.DNI}).");
                return cliente.IdCliente;
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

        // Modificar (CU Clientes.002)
        public void Modificar(Cliente cliente)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarClientes);

                if (_repositorio.GetById(cliente.IdCliente) == null)
                    throw new ClienteNoEncontradoException(cliente.IdCliente);

                ValidarDatos(cliente);
                _repositorio.Update(cliente);

                DigitoVerificadorService.RecalcularRegistro("Cliente", cliente.IdCliente);
                LoggerService.Info($"Modificación del cliente {cliente.NombreCompleto} (DNI {cliente.DNI}).");
            }
            catch (ReglaNegocioException ex)
            {
                ExceptionManager.HandleException(ex);
                throw;
            }
            catch (DataAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(ex);
                throw;
            }
        }

        // Deshabilitar, no se borra (CU Clientes.003)
        public void Deshabilitar(int idCliente)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarClientes);

                Cliente cliente = _repositorio.GetById(idCliente)
                    ?? throw new ClienteNoEncontradoException(idCliente);
                if (!cliente.Activo)
                    throw new ClienteYaDeshabilitadoException();

                _repositorio.Delete(idCliente);   // baja lógica

                DigitoVerificadorService.RecalcularRegistro("Cliente", idCliente);
                LoggerService.Info($"Deshabilitó al cliente {cliente.NombreCompleto} (DNI {cliente.DNI}).");
            }
            catch (ReglaNegocioException ex)
            {
                ExceptionManager.HandleException(ex);
                throw;
            }
            catch (DataAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(ex);
                throw;
            }
        }

        /// <summary>Vuelve a habilitar un cliente deshabilitado (deshace la baja lógica).</summary>
        public void Habilitar(int idCliente)
        {
            Sesion.ExigirPermiso(Funcion.GestionarClientes);
            _repositorio.CambiarEstado(idCliente, true);

            DigitoVerificadorService.RecalcularRegistro("Cliente", idCliente);
            LoggerService.Info($"Habilitó al cliente con Id {idCliente}.");
        }

        // Historial: lo que nos vendió y lo que nos compró (CU Clientes.004)
        public List<Compra> ListarCompras(int idCliente)
        {
            return _compraRepositorio.ListarPorCliente(idCliente);
        }

        public List<Venta> ListarVentas(int idCliente)
        {
            return _ventaRepositorio.ListarPorCliente(idCliente);
        }

        /// <summary>
        /// Valida y "limpia" los datos (saca espacios, da formato al CUIT).
        /// Los largos máximos son los mismos que las columnas de la tabla Cliente.
        /// </summary>
        private void ValidarDatos(Cliente cliente)
        {
            cliente.Nombre = Validar.Requerido(cliente.Nombre, "Nombre", 50);
            cliente.Apellido = Validar.Requerido(cliente.Apellido, "Apellido", 50);
            cliente.DNI = Validar.DNI(cliente.DNI);
            cliente.CUIT = Validar.CUIT(cliente.CUIT, obligatorio: false);
            cliente.Domicilio = Validar.Opcional(cliente.Domicilio, "Domicilio", 100);
            cliente.Localidad = Validar.Opcional(cliente.Localidad, "Localidad", 50);
            cliente.Telefono = Validar.Opcional(cliente.Telefono, "Teléfono", 20);
            cliente.Email = Validar.Email(cliente.Email, 100);

            // El DNI no se puede repetir, ni con un cliente deshabilitado.
            Cliente existente = _repositorio.ObtenerPorDNI(cliente.DNI);
            if (existente != null && existente.IdCliente != cliente.IdCliente)
                throw new ClienteDuplicadoException(cliente.DNI, existente.NombreCompleto, existente.Activo);
        }
    }
}
