using System.Security.Cryptography;
using System.Text;
using PRAS.BLL.Excepciones;
using PRAS.DAL.Factory;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;
using PRAS.Services.DomainModel.Composite;
using PRAS.Services.Exceptions.DataAccess;
using PRAS.Services.Facade;
using PRAS.Services.Facade.ExtensionsMethods;

namespace PRAS.BLL
{
    /// <summary>
    /// Casos de uso de Seguridad: .001 Iniciar Sesión, .002 Alta Usuario, .003 Modificar Usuario,
    /// .004 Deshabilitar Usuario. Los permisos de cada usuario (Composite) los maneja PermisoService.
    /// </summary>
    public class UsuarioService
    {
        private const int LargoMinimoContrasena = 6;

        private readonly IUsuarioRepository _repositorio = FactoryDataAccess.UsuarioRepository;
        private readonly IMecanicoRepository _mecanicoRepositorio = FactoryDataAccess.MecanicoRepository;
        private readonly PermisoService _permisoService = new PermisoService();

        // Iniciar sesión (CU Seguridad.001)

        public Usuario IniciarSesion(string nombreUsuario, string contrasena)
        {
            try
            {
                nombreUsuario = (nombreUsuario ?? "").Trim();
                if (nombreUsuario == "")
                    throw new DatoObligatorioException("Usuario");
                if (string.IsNullOrEmpty(contrasena))
                    throw new DatoObligatorioException("Contraseña");

                Usuario usuario = _repositorio.ObtenerPorNombreUsuario(nombreUsuario);

                // Damos el mismo mensaje si no existe o si la contraseña está mal,
                // así no le damos pistas a alguien que quiere adivinar usuarios.
                if (usuario == null || !ContrasenaCoincide(contrasena, usuario.ContrasenaHash))
                    throw new CredencialesInvalidasException();

                if (!usuario.Activo)
                    throw new UsuarioDeshabilitadoException(nombreUsuario);

                // Cargamos sus familias y patentes desde PRAS_Seguridad (fachada de Services).
                List<Patente> patentes = UsuarioPermisoService.GetPatentes(usuario.IdUsuario);
                if (patentes.Count == 0)
                    throw new UsuarioSinPermisosException(nombreUsuario);

                Sesion.Iniciar(usuario, patentes);
                LoggerService.Info("Inició sesión.");
                return usuario;
            }
            catch (UsuarioBusinessException ex)
            {
                // Intento fallido: queda en la bitácora (sin la contraseña) y sigue hasta la pantalla.
                ExceptionManager.HandleException(ex, new object[] { nombreUsuario });
                throw;
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

        public void CerrarSesion()
        {
            LoggerService.Info("Cerró sesión.");
            Sesion.Cerrar();
        }

        /// <summary>Guarda el idioma elegido como preferido del usuario logueado.</summary>
        public void CambiarIdioma(string cultura)
        {
            if (!Sesion.HayUsuarioLogueado)
                return;

            Usuario usuario = Sesion.UsuarioActual;
            _repositorio.CambiarIdioma(usuario.IdUsuario, cultura);
            usuario.Idioma = cultura;

            DigitoVerificadorService.RecalcularRegistro("Usuario", usuario.IdUsuario);
            LoggerService.Info($"Cambió el idioma a {cultura}.");
        }

        // Listar (lo usa la pantalla de usuarios)

        public List<Usuario> Listar(bool incluirDeshabilitados)
        {
            Sesion.ExigirPermiso(Funcion.GestionarUsuarios);
            return _repositorio.Listar(incluirDeshabilitados);
        }

        // Alta de usuario (CU Seguridad.002)

        public int Alta(Usuario usuario, string contrasena, string confirmacion)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarUsuarios);

                usuario.NombreUsuario = ValidarNombreUsuario(usuario.NombreUsuario, 0);
                ValidarRol(usuario.Rol);
                ValidarMecanicoVinculado(usuario, null);
                ValidarContrasena(contrasena, confirmacion);

                usuario.ContrasenaHash = CalcularHash(contrasena);
                usuario.Activo = true;
                _repositorio.Add(usuario);

                DigitoVerificadorService.RecalcularRegistro("Usuario", usuario.IdUsuario);
                _permisoService.AsignarFamiliaDelRol(usuario.IdUsuario, usuario.Rol);
                LoggerService.Info($"Alta del usuario \"{usuario.NombreUsuario}\" (rol {Sesion.TextoRol(usuario.Rol)}).");
                return usuario.IdUsuario;
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

        // Modificar usuario (CU Seguridad.003)

        public void Modificar(Usuario usuario, string contrasena, string confirmacion)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarUsuarios);

                Usuario actual = _repositorio.GetById(usuario.IdUsuario)
                    ?? throw new UsuarioNoEncontradoException(usuario.IdUsuario);

                usuario.NombreUsuario = ValidarNombreUsuario(usuario.NombreUsuario, usuario.IdUsuario);
                ValidarRol(usuario.Rol);
                ValidarMecanicoVinculado(usuario, actual.IdMecanico);

                // Si le sacamos el rol de administrador, tiene que quedar otro administrador activo.
                if (actual.Rol == Rol.Administrador && usuario.Rol != Rol.Administrador && actual.Activo)
                    ValidarQueQuedeOtroAdministrador(usuario.IdUsuario);

                if (string.IsNullOrEmpty(contrasena) && string.IsNullOrEmpty(confirmacion))
                {
                    usuario.ContrasenaHash = actual.ContrasenaHash;   // la contraseña no se toca
                }
                else
                {
                    ValidarContrasena(contrasena, confirmacion);
                    usuario.ContrasenaHash = CalcularHash(contrasena);
                }

                usuario.Activo = actual.Activo;
                usuario.Idioma = actual.Idioma;
                _repositorio.Update(usuario);

                DigitoVerificadorService.RecalcularRegistro("Usuario", usuario.IdUsuario);

                // Si cambió el rol, le cambiamos también la familia de permisos.
                if (actual.Rol != usuario.Rol)
                    _permisoService.CambiarFamiliaDelRol(usuario.IdUsuario, actual.Rol, usuario.Rol);

                LoggerService.Info($"Modificación del usuario \"{usuario.NombreUsuario}\".");

                // Si se modificó a sí mismo, actualizamos la sesión (datos y permisos).
                if (usuario.IdUsuario == Sesion.UsuarioActual.IdUsuario)
                    Sesion.Iniciar(usuario, UsuarioPermisoService.GetPatentes(usuario.IdUsuario));
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

        // Deshabilitar usuario, no se borra (CU Seguridad.004)

        public void Deshabilitar(int idUsuario)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.GestionarUsuarios);

                Usuario usuario = _repositorio.GetById(idUsuario)
                    ?? throw new UsuarioNoEncontradoException(idUsuario);

                if (!usuario.Activo)
                    throw new UsuarioYaDeshabilitadoException(usuario.NombreUsuario);

                if (idUsuario == Sesion.UsuarioActual.IdUsuario)
                    throw new AutoDeshabilitacionException();

                if (usuario.Rol == Rol.Administrador)
                    ValidarQueQuedeOtroAdministrador(idUsuario);

                _repositorio.Delete(idUsuario);   // baja lógica

                DigitoVerificadorService.RecalcularRegistro("Usuario", idUsuario);
                LoggerService.Info($"Deshabilitó al usuario \"{usuario.NombreUsuario}\".");
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

        public void Habilitar(int idUsuario)
        {
            Sesion.ExigirPermiso(Funcion.GestionarUsuarios);
            _repositorio.CambiarEstado(idUsuario, true);

            DigitoVerificadorService.RecalcularRegistro("Usuario", idUsuario);
            LoggerService.Info($"Habilitó al usuario con Id {idUsuario}.");
        }

        // Métodos de ayuda

        /// <summary>La contraseña nunca se guarda: se guarda su hash SHA-256.</summary>
        public static string CalcularHash(string contrasena)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(contrasena)));
        }

        private static bool ContrasenaCoincide(string contrasena, string hashGuardado)
        {
            return string.Equals(CalcularHash(contrasena), hashGuardado, StringComparison.OrdinalIgnoreCase);
        }

        private string ValidarNombreUsuario(string nombreUsuario, int idUsuario)
        {
            string nombre = Validar.Requerido(nombreUsuario, "Usuario", 50);
            if (nombre.Contains(' '))
                throw new NombreUsuarioInvalidoException(nombre);

            Usuario existente = _repositorio.ObtenerPorNombreUsuario(nombre);
            if (existente != null && existente.IdUsuario != idUsuario)
                throw new UsuarioDuplicadoException(nombre);

            return nombre;
        }

        private static void ValidarRol(Rol rol)
        {
            if (!Enum.IsDefined(typeof(Rol), rol))
                throw new RolInvalidoException();
        }

        /// <summary>Si el rol es Mecánico, tiene que estar vinculado a un mecánico (y uno solo por usuario).</summary>
        private void ValidarMecanicoVinculado(Usuario usuario, int? idMecanicoAnterior)
        {
            if (usuario.Rol != Rol.Mecanico)
            {
                usuario.IdMecanico = null;
                return;
            }

            if (usuario.IdMecanico == null)
                throw new SeleccionObligatoriaException("el mecánico al que corresponde este usuario");

            Mecanico mecanico = _mecanicoRepositorio.GetById(usuario.IdMecanico.Value)
                ?? throw new MecanicoNoEncontradoException(usuario.IdMecanico.Value);

            if (!mecanico.Activo && usuario.IdMecanico != idMecanicoAnterior)
                throw new MecanicoDeshabilitadoException(mecanico.NombreCompleto);

            Usuario otro = _repositorio.ObtenerPorIdMecanico(usuario.IdMecanico.Value);
            if (otro != null && otro.IdUsuario != usuario.IdUsuario)
                throw new MecanicoConUsuarioException($"El mecánico {mecanico.NombreCompleto} ya tiene el usuario \"{otro.NombreUsuario}\".");
        }

        private static void ValidarContrasena(string contrasena, string confirmacion)
        {
            if (string.IsNullOrEmpty(contrasena) || contrasena.Length < LargoMinimoContrasena)
                throw new ContrasenaInvalidaException(LargoMinimoContrasena);
            if (contrasena != confirmacion)
                throw new ContrasenasNoCoincidenException();
        }

        private void ValidarQueQuedeOtroAdministrador(int idUsuario)
        {
            if (_repositorio.ContarAdministradoresActivos(idUsuario) == 0)
                throw new UltimoAdministradorException("Debe quedar al menos un Administrador activo en el sistema.");
        }
    }
}
