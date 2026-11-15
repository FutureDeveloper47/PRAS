using PRAS.DAL.Implementaciones.SqlServer.Adaptadores;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;
using PRAS.Entidades;
using PRAS.Services.Facade;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Repositorio de la tabla Usuario (singleton). Ojo: la tabla está en la base PRAS_Seguridad,
    /// por eso usa los métodos "...Seguridad" del SqlHelper. Solo SQL: las reglas están en la BLL.
    /// </summary>
    internal sealed class UsuarioRepository : IUsuarioRepository
    {
        #region Singleton
        private static readonly UsuarioRepository _instance = new UsuarioRepository();

        public static UsuarioRepository Current
        {
            get { return _instance; }
        }

        private UsuarioRepository()
        {
        }
        #endregion

        #region Statements
        // LEFT JOIN: la pregunta de seguridad y los datos personales están en otras tablas
        // y pueden no existir (quedan NULL).
        private const string SelectBase =
            @"SELECT U.IdUsuario, U.NombreUsuario, U.ContrasenaHash, U.Rol, U.IdMecanico, U.Activo, U.Idioma,
                     R.PreguntaSeguridad, R.RespuestaHash,
                     P.Nombre AS PersonaNombre, P.Apellido AS PersonaApellido, P.DNI AS PersonaDNI,
                     P.Telefono AS PersonaTelefono, P.Email AS PersonaEmail
              FROM Usuario U
              LEFT JOIN UsuarioRecuperacion R ON R.IdUsuario = U.IdUsuario
              LEFT JOIN UsuarioPersona P      ON P.IdUsuario = U.IdUsuario";

        private string SelectOneStatement
        {
            get => $"{SelectBase} WHERE U.IdUsuario = @IdUsuario";
        }

        private string SelectByNombreStatement
        {
            get => $"{SelectBase} WHERE U.NombreUsuario = @NombreUsuario";
        }

        private string SelectByMecanicoStatement
        {
            get => $"{SelectBase} WHERE U.IdMecanico = @IdMecanico";
        }

        /// <summary>
        /// Lista de usuarios con su último ingreso: se saca de la bitácora (el registro "Inició sesión."
        /// que deja cada login), así no hace falta una columna nueva en Usuario.
        /// </summary>
        private string SelectAllStatement
        {
            get => $@"SELECT X.*, ING.UltimoIngreso
                      FROM ({SelectBase}) X
                      OUTER APPLY (SELECT MAX(B.Fecha) AS UltimoIngreso FROM Bitacora B
                                   WHERE B.Usuario = X.NombreUsuario AND B.Mensaje = N'Inició sesión.') ING
                      WHERE (@IncluirInactivos = 1 OR X.Activo = 1)
                      ORDER BY X.NombreUsuario";
        }

        /// <summary>Si el usuario ya tenía datos personales los reemplaza; si no, los agrega.</summary>
        private string GuardarPersonaStatement
        {
            get => @"IF EXISTS (SELECT 1 FROM UsuarioPersona WHERE IdUsuario = @IdUsuario)
                         UPDATE UsuarioPersona
                         SET Nombre = @Nombre, Apellido = @Apellido, DNI = @DNI, Telefono = @Telefono, Email = @Email
                         WHERE IdUsuario = @IdUsuario
                     ELSE
                         INSERT INTO UsuarioPersona (IdUsuario, Nombre, Apellido, DNI, Telefono, Email)
                         VALUES (@IdUsuario, @Nombre, @Apellido, @DNI, @Telefono, @Email)";
        }

        private string InsertStatement
        {
            get => @"INSERT INTO Usuario (NombreUsuario, ContrasenaHash, Rol, IdMecanico, Activo)
                     VALUES (@NombreUsuario, @ContrasenaHash, @Rol, @IdMecanico, @Activo);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
        }

        private string UpdateStatement
        {
            get => @"UPDATE Usuario
                     SET NombreUsuario = @NombreUsuario, ContrasenaHash = @ContrasenaHash,
                         Rol = @Rol, IdMecanico = @IdMecanico
                     WHERE IdUsuario = @IdUsuario";
        }

        private string CambiarEstadoStatement
        {
            get => "UPDATE Usuario SET Activo = @Activo WHERE IdUsuario = @IdUsuario";
        }

        private string CambiarIdiomaStatement
        {
            get => "UPDATE Usuario SET Idioma = @Idioma WHERE IdUsuario = @IdUsuario";
        }

        private string CambiarContrasenaStatement
        {
            get => "UPDATE Usuario SET ContrasenaHash = @ContrasenaHash WHERE IdUsuario = @IdUsuario";
        }

        /// <summary>Si el usuario ya tenía pregunta la reemplaza; si no, la agrega.</summary>
        private string GuardarPreguntaStatement
        {
            get => @"IF EXISTS (SELECT 1 FROM UsuarioRecuperacion WHERE IdUsuario = @IdUsuario)
                         UPDATE UsuarioRecuperacion
                         SET PreguntaSeguridad = @Pregunta, RespuestaHash = @RespuestaHash
                         WHERE IdUsuario = @IdUsuario
                     ELSE
                         INSERT INTO UsuarioRecuperacion (IdUsuario, PreguntaSeguridad, RespuestaHash)
                         VALUES (@IdUsuario, @Pregunta, @RespuestaHash)";
        }

        private string ContarAdministradoresStatement
        {
            get => @"SELECT COUNT(*) FROM Usuario
                     WHERE Rol = @Rol AND Activo = 1 AND IdUsuario <> @IdUsuarioExcluido";
        }
        #endregion

        public Usuario GetById(int id)
        {
            var tabla = SqlHelper.ExecuteDataTableSeguridad(SelectOneStatement, SqlHelper.Param("@IdUsuario", id));
            return CompletarMecanico(UsuarioAdaptador.Current.FillPrimero(tabla));
        }

        public Usuario ObtenerPorNombreUsuario(string nombreUsuario)
        {
            var tabla = SqlHelper.ExecuteDataTableSeguridad(SelectByNombreStatement, SqlHelper.Param("@NombreUsuario", nombreUsuario));
            return CompletarMecanico(UsuarioAdaptador.Current.FillPrimero(tabla));
        }

        public Usuario ObtenerPorIdMecanico(int idMecanico)
        {
            var tabla = SqlHelper.ExecuteDataTableSeguridad(SelectByMecanicoStatement, SqlHelper.Param("@IdMecanico", idMecanico));
            return CompletarMecanico(UsuarioAdaptador.Current.FillPrimero(tabla));
        }

        public List<Usuario> Listar(bool incluirInactivos)
        {
            var tabla = SqlHelper.ExecuteDataTableSeguridad(SelectAllStatement, SqlHelper.Param("@IncluirInactivos", incluirInactivos));
            List<Usuario> usuarios = UsuarioAdaptador.Current.FillLista(tabla);
            foreach (Usuario usuario in usuarios)
                CompletarMecanico(usuario);
            return usuarios;
        }

        public List<Usuario> GetAll()
        {
            return Listar(true);
        }

        public void Add(Usuario entity)
        {
            object id = SqlHelper.ExecuteScalarSeguridad(InsertStatement,
                SqlHelper.Param("@NombreUsuario", entity.NombreUsuario),
                SqlHelper.Param("@ContrasenaHash", entity.ContrasenaHash),
                SqlHelper.Param("@Rol", (int)entity.Rol),
                SqlHelper.Param("@IdMecanico", entity.IdMecanico),   // si es null se guarda NULL
                SqlHelper.Param("@Activo", entity.Activo));

            entity.IdUsuario = (int)id;
        }

        public void Update(Usuario entity)
        {
            SqlHelper.ExecuteNonQuerySeguridad(UpdateStatement,
                SqlHelper.Param("@NombreUsuario", entity.NombreUsuario),
                SqlHelper.Param("@ContrasenaHash", entity.ContrasenaHash),
                SqlHelper.Param("@Rol", (int)entity.Rol),
                SqlHelper.Param("@IdMecanico", entity.IdMecanico),
                SqlHelper.Param("@IdUsuario", entity.IdUsuario));
        }

        /// <summary>Baja lógica: el usuario queda deshabilitado.</summary>
        public void Delete(int id)
        {
            CambiarEstado(id, false);
        }

        public void CambiarEstado(int idUsuario, bool activo)
        {
            SqlHelper.ExecuteNonQuerySeguridad(CambiarEstadoStatement,
                SqlHelper.Param("@Activo", activo),
                SqlHelper.Param("@IdUsuario", idUsuario));
        }

        public void CambiarIdioma(int idUsuario, string idioma)
        {
            SqlHelper.ExecuteNonQuerySeguridad(CambiarIdiomaStatement,
                SqlHelper.Param("@Idioma", idioma),
                SqlHelper.Param("@IdUsuario", idUsuario));
        }

        /// <summary>Cambia solo la contraseña (Mi perfil y Recuperar contraseña).</summary>
        public void CambiarContrasena(int idUsuario, string contrasenaHash)
        {
            SqlHelper.ExecuteNonQuerySeguridad(CambiarContrasenaStatement,
                SqlHelper.Param("@ContrasenaHash", contrasenaHash),
                SqlHelper.Param("@IdUsuario", idUsuario));
        }

        /// <summary>Guarda la pregunta de seguridad y el hash de la respuesta (tabla UsuarioRecuperacion).</summary>
        public void GuardarPreguntaSeguridad(int idUsuario, string pregunta, string respuestaHash)
        {
            SqlHelper.ExecuteNonQuerySeguridad(GuardarPreguntaStatement,
                SqlHelper.Param("@IdUsuario", idUsuario),
                SqlHelper.Param("@Pregunta", pregunta),
                SqlHelper.Param("@RespuestaHash", respuestaHash));
        }

        /// <summary>Datos personales del usuario (tabla UsuarioPersona). Teléfono y email se guardan con AES.</summary>
        public void GuardarPersona(Usuario usuario)
        {
            SqlHelper.ExecuteNonQuerySeguridad(GuardarPersonaStatement,
                SqlHelper.Param("@IdUsuario", usuario.IdUsuario),
                SqlHelper.Param("@Nombre", usuario.Nombre),
                SqlHelper.Param("@Apellido", usuario.Apellido),
                SqlHelper.Param("@DNI", usuario.DNI),
                SqlHelper.Param("@Telefono", CryptographyService.Encriptar(usuario.Telefono)),   // AES
                SqlHelper.Param("@Email", CryptographyService.Encriptar(usuario.Email)));        // AES
        }

        public int ContarAdministradoresActivos(int idUsuarioExcluido)
        {
            object cantidad = SqlHelper.ExecuteScalarSeguridad(ContarAdministradoresStatement,
                SqlHelper.Param("@Rol", (int)Rol.Administrador),
                SqlHelper.Param("@IdUsuarioExcluido", idUsuarioExcluido));
            return (int)cantidad;
        }

        /// <summary>El mecánico está en la otra base (PRAS): lo buscamos aparte para mostrar su nombre.</summary>
        private static Usuario CompletarMecanico(Usuario usuario)
        {
            if (usuario?.IdMecanico != null)
                usuario.Mecanico = MecanicoRepository.Current.GetById(usuario.IdMecanico.Value);
            return usuario;
        }
    }
}
