using System.Configuration;
using System.Globalization;
using System.Text;
using PRAS.DAL.Factory;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;
using PRAS.Services.Facade;
using PRAS.Services.Facade.ExtensionsMethods;

namespace PRAS.BLL
{
    /// <summary>
    /// Datos de prueba para mostrar el sistema (defensa de la tesis):
    /// - Usuarios de prueba: admin, dueno, mecanico (Carlos Ferreyra) y uno por cada mecánico que todavía
    ///   no tenga usuario (ej: jquiroga / mecanico123). Todos habilitados, con pregunta de seguridad
    ///   (respuesta: pras) y contraseña guardada con hash SHA-256.
    /// - Operaciones del negocio repartidas entre 2024 y 2026 (compras, taller, agencia y ventas).
    ///
    /// Se hace desde la BLL (y no con SQL suelto) para que pase por los mismos pasos que un alta normal:
    /// hash de la contraseña, dígito verificador (DVH/DVV), familia de permisos del rol y encriptado AES
    /// de los teléfonos de los clientes. Así la verificación de integridad sigue dando bien.
    ///
    /// Se activa con la clave CargarDatosDePrueba de App.config (en una instalación real se pone en false).
    /// Nunca cambia la contraseña de un usuario que ya existe.
    /// </summary>
    public class DatosPruebaService
    {
        /// <summary>Contraseña de prueba de todos los mecánicos.</summary>
        public const string ContrasenaMecanicos = "mecanico123";

        /// <summary>Respuesta de prueba de la pregunta de seguridad de todos los usuarios de prueba.</summary>
        private const string PreguntaPrueba = "¿Cuál es el nombre del sistema?";
        private const string RespuestaPrueba = "pras";

        private readonly IUsuarioRepository _usuarioRepositorio = FactoryDataAccess.UsuarioRepository;
        private readonly IMecanicoRepository _mecanicoRepositorio = FactoryDataAccess.MecanicoRepository;
        private readonly IDatosPruebaRepository _datosRepositorio = FactoryDataAccess.DatosPruebaRepository;
        private readonly PermisoService _permisoService = new PermisoService();

        /// <summary>true si App.config dice CargarDatosDePrueba = true.</summary>
        public bool CargaHabilitada => LeerBooleano("CargarDatosDePrueba");

        /// <summary>true si App.config dice MostrarUsuariosDePrueba = true (link en la pantalla de ingreso).</summary>
        public bool MostrarUsuariosDePrueba => LeerBooleano("MostrarUsuariosDePrueba");

        /// <summary>
        /// Usuarios fijos con su contraseña de prueba. Los mecánicos se agregan solos (uno por mecánico).
        /// </summary>
        private static readonly (string Usuario, string Contrasena, Rol Rol, string Nombre, string Apellido)[] UsuariosFijos =
        {
            ("admin", "admin123", Rol.Administrador, "Matías", "Montoto"),
            ("dueno", "dueno123", Rol.Dueno, "Gustavo", "Ibarra"),
        };

        /// <summary>
        /// Crea lo que falte. Se llama al arrancar el programa (después de actualizar la estructura de la base).
        /// No hay usuario logueado todavía: es un proceso del sistema, igual que ActualizarEstructura.
        /// </summary>
        public void AsegurarDatosDePrueba()
        {
            if (!CargaHabilitada)
                return;

            try
            {
                CargarOperaciones();
                CrearUsuariosDePrueba();
            }
            catch (Exception ex)
            {
                // Si falla, el sistema sigue funcionando igual (solo faltan datos de prueba). Queda en la bitácora.
                ExceptionManager.HandleException(ex);
            }
        }

        private void CargarOperaciones()
        {
            List<int> clientesNuevos = _datosRepositorio.CargarOperacionesDePrueba();
            if (clientesNuevos.Count == 0)
                return;   // ya estaban cargados

            // Los teléfonos se insertaron en texto plano: se encriptan con AES y se recalcula el DVH de cada uno.
            new ClienteService().EncriptarDatosPendientes();
            foreach (int idCliente in clientesNuevos)
                DigitoVerificadorService.RecalcularRegistro("Cliente", idCliente);

            LoggerService.Info($"Se cargaron los datos de prueba del negocio ({clientesNuevos.Count} cliente(s) nuevos).");
        }

        private void CrearUsuariosDePrueba()
        {
            var creados = new List<string>();

            foreach (var fijo in UsuariosFijos)
            {
                if (_usuarioRepositorio.ObtenerPorNombreUsuario(fijo.Usuario) != null)
                    continue;   // ya existe: no se toca (ni su contraseña)

                var usuario = new Usuario { NombreUsuario = fijo.Usuario, Rol = fijo.Rol, Nombre = fijo.Nombre, Apellido = fijo.Apellido };
                Crear(usuario, fijo.Contrasena);
                _usuarioRepositorio.GuardarPersona(usuario);
                creados.Add(fijo.Usuario);
            }

            // Un usuario por cada mecánico activo que todavía no tenga uno.
            // Carlos Ferreyra es "mecanico" (el usuario de prueba de siempre); el resto, inicial + apellido.
            foreach (Mecanico mecanico in _mecanicoRepositorio.Listar("", false))
            {
                if (_usuarioRepositorio.ObtenerPorIdMecanico(mecanico.IdMecanico) != null)
                    continue;   // ya tiene usuario (por ejemplo "juan"): se respeta

                bool esFerreyra = mecanico.Nombre == "Carlos" && mecanico.Apellido == "Ferreyra";
                string nombreUsuario = NombreDisponible(esFerreyra ? "mecanico" : NombreUsuarioDe(mecanico));

                Crear(new Usuario { NombreUsuario = nombreUsuario, Rol = Rol.Mecanico, IdMecanico = mecanico.IdMecanico },
                      ContrasenaMecanicos);
                creados.Add(nombreUsuario);
            }

            if (creados.Count > 0)
                LoggerService.Info($"Se crearon los usuarios de prueba: {string.Join(", ", creados)}.");
        }

        /// <summary>Mismos pasos que el alta de un usuario: hash, DVH, familia del rol y pregunta de seguridad.</summary>
        private void Crear(Usuario usuario, string contrasena)
        {
            usuario.ContrasenaHash = UsuarioService.CalcularHash(contrasena);
            usuario.Activo = true;
            _usuarioRepositorio.Add(usuario);

            DigitoVerificadorService.RecalcularRegistro("Usuario", usuario.IdUsuario);
            _permisoService.AsignarFamiliaDelRol(usuario.IdUsuario, usuario.Rol);
            _usuarioRepositorio.GuardarPreguntaSeguridad(usuario.IdUsuario, PreguntaPrueba,
                UsuarioService.CalcularHash(RespuestaPrueba));
        }

        /// <summary>"Jorge Quiroga" -> "jquiroga" (sin tildes ni espacios, en minúsculas).</summary>
        private static string NombreUsuarioDe(Mecanico mecanico)
        {
            string texto = (mecanico.Nombre.Trim().Substring(0, 1) + mecanico.Apellido).ToLowerInvariant();
            var resultado = new StringBuilder();
            foreach (char letra in texto.Normalize(NormalizationForm.FormD))
            {
                bool esTilde = CharUnicodeInfo.GetUnicodeCategory(letra) == UnicodeCategory.NonSpacingMark;
                if (!esTilde && char.IsLetterOrDigit(letra))
                    resultado.Append(letra);
            }
            return resultado.ToString();
        }

        /// <summary>Si el nombre ya está usado, le agrega un número (jperez2, jperez3...).</summary>
        private string NombreDisponible(string nombre)
        {
            string candidato = nombre;
            for (int i = 2; _usuarioRepositorio.ObtenerPorNombreUsuario(candidato) != null; i++)
                candidato = nombre + i;
            return candidato;
        }

        // Tabla "Usuarios de prueba" de la pantalla de ingreso

        /// <summary>
        /// Lista para la pantalla de ingreso: usuario, contraseña de prueba, rol y nombre.
        /// Las contraseñas NO se leen de la base (ahí solo está el hash SHA-256, que no se puede volver atrás):
        /// se muestran las contraseñas de prueba conocidas y se COMPRUEBA contra el hash que sigan siendo válidas.
        /// Si alguien la cambió, se muestra "(cambiada)" en lugar de la contraseña.
        /// </summary>
        public List<UsuarioDePrueba> ListarUsuariosDePrueba()
        {
            if (!MostrarUsuariosDePrueba)
                return new List<UsuarioDePrueba>();

            var lista = new List<UsuarioDePrueba>();
            foreach (Usuario usuario in _usuarioRepositorio.Listar(true))
            {
                string candidata = ContrasenaConocida(usuario);
                if (candidata == null)
                    continue;   // no es un usuario de prueba

                bool sigueSiendoLaDePrueba = string.Equals(UsuarioService.CalcularHash(candidata), usuario.ContrasenaHash,
                                                           StringComparison.OrdinalIgnoreCase);
                lista.Add(new UsuarioDePrueba
                {
                    NombreUsuario = usuario.NombreUsuario,
                    Contrasena = sigueSiendoLaDePrueba ? candidata : null,
                    Rol = usuario.Rol,
                    NombrePersona = usuario.NombrePersona,
                    Activo = usuario.Activo
                });
            }

            // Primero admin y dueño, después los mecánicos en orden alfabético.
            return lista.OrderBy(u => u.Rol == Rol.Mecanico ? 1 : 0).ThenBy(u => u.NombreUsuario).ToList();
        }

        private static string ContrasenaConocida(Usuario usuario)
        {
            foreach (var fijo in UsuariosFijos)
                if (string.Equals(fijo.Usuario, usuario.NombreUsuario, StringComparison.OrdinalIgnoreCase))
                    return fijo.Contrasena;

            return usuario.Rol == Rol.Mecanico && usuario.IdMecanico.HasValue ? ContrasenaMecanicos : null;
        }

        private static bool LeerBooleano(string clave)
        {
            return bool.TryParse(ConfigurationManager.AppSettings[clave], out bool valor) && valor;
        }
    }
}
