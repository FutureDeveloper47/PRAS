using PRAS.BLL.Excepciones;
using PRAS.DAL.Factory;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;
using PRAS.Services.DomainModel.Composite;
using PRAS.Services.Facade;

namespace PRAS.BLL
{
    /// <summary>
    /// Caso de uso Seguridad - Gestionar permisos (patrón COMPOSITE).
    ///   - Familia = grupo de permisos. Puede contener patentes y otras familias.
    ///   - Patente = un permiso concreto (una función del sistema).
    ///   - A cada usuario se le asignan familias y/o patentes sueltas.
    /// Las reglas de negocio están acá; el acceso a la base lo hace el proyecto Services
    /// (FamiliaService, PatenteService, UsuarioPermisoService).
    /// </summary>
    public class PermisoService
    {
        /// <summary>
        /// Familia protegida: no se puede borrar ni modificar, así el sistema nunca se queda
        /// sin alguien que pueda administrar usuarios y permisos.
        /// </summary>
        public const string FamiliaAdministrador = "Administrador";

        private const int LargoNombreFamilia = 100;

        private readonly IUsuarioRepository _usuarioRepositorio = FactoryDataAccess.UsuarioRepository;

        // Consultas

        /// <summary>Todas las familias, cada una con su árbol completo de hijos.</summary>
        public List<Familia> ListarFamilias()
        {
            Sesion.ExigirPermiso(Funcion.GestionarPermisos);
            return FamiliaService.GetAll();
        }

        /// <summary>Todas las patentes (una por cada función del sistema).</summary>
        public List<Patente> ListarPatentes()
        {
            Sesion.ExigirPermiso(Funcion.GestionarPermisos);
            return PatenteService.GetAll();
        }

        /// <summary>Usuarios activos, para el combo de la solapa "Usuarios".</summary>
        public List<Usuario> ListarUsuarios()
        {
            Sesion.ExigirPermiso(Funcion.GestionarPermisos);
            return _usuarioRepositorio.Listar(false);
        }

        /// <summary>Familias y patentes asignadas directamente al usuario.</summary>
        public List<Component> ObtenerPrivilegios(int idUsuario)
        {
            Sesion.ExigirPermiso(Funcion.GestionarPermisos);
            return UsuarioPermisoService.GetPrivilegios(idUsuario);
        }

        /// <summary>
        /// Todas las patentes que el usuario tiene en definitiva (recorriendo sus familias).
        /// El recorrido recursivo del Composite lo hace el proyecto Services.
        /// </summary>
        public List<Patente> ObtenerPatentesEfectivas(List<Component> privilegios)
        {
            return UsuarioPermisoService.ObtenerPatentes(privilegios).OrderBy(p => p.Descripcion).ToList();
        }

        // Familias

        public Familia CrearFamilia(string nombre)
        {
            Sesion.ExigirPermiso(Funcion.GestionarPermisos);

            nombre = Validar.Requerido(nombre, "Nombre de la familia", LargoNombreFamilia);
            if (FamiliaService.GetByNombre(nombre) != null)
                throw new FamiliaDuplicadaException(nombre);

            var familia = new Familia { Nombre = nombre };
            FamiliaService.Add(familia);

            LoggerService.Info($"Permisos: creó la familia \"{nombre}\".");
            return familia;
        }

        public void EliminarFamilia(Familia familia)
        {
            Sesion.ExigirPermiso(Funcion.GestionarPermisos);
            ValidarNoProtegida(familia);

            FamiliaService.Delete(familia);
            LoggerService.Warn($"Permisos: eliminó la familia \"{familia.Nombre}\".");
        }

        public void AgregarPatenteAFamilia(Patente patente, Familia familia)
        {
            Sesion.ExigirPermiso(Funcion.GestionarPermisos);
            if (patente == null || familia == null)
                throw new SeleccionObligatoriaException("la familia y la patente");
            ValidarNoProtegida(familia);

            // Contiene() recorre el árbol completo (Composite): si la patente ya está en la familia
            // o en alguna familia de adentro, no se agrega de nuevo.
            if (familia.Contiene(patente.Id))
                throw new PermisoYaAsignadoException($"La familia \"{familia.Nombre}\" ya tiene la patente \"{patente.Descripcion}\".");

            FamiliaService.AgregarPatente(patente, familia);
            LoggerService.Info($"Permisos: agregó la patente \"{patente.Descripcion}\" a la familia \"{familia.Nombre}\".");
        }

        /// <summary>
        /// Agrega una familia dentro de otra. Regla clave del Composite:
        /// no se puede armar un ciclo (A dentro de B y B dentro de A), porque al recorrer
        /// el árbol de permisos el programa no terminaría nunca.
        /// </summary>
        public void AgregarFamiliaAFamilia(Familia hija, Familia padre)
        {
            Sesion.ExigirPermiso(Funcion.GestionarPermisos);
            if (hija == null || padre == null)
                throw new SeleccionObligatoriaException("las dos familias");
            ValidarNoProtegida(padre);

            if (hija.Id == padre.Id || hija.Contiene(padre.Id))
                throw new FamiliaCiclicaException(hija.Nombre, padre.Nombre);

            // Igual que con las patentes: si ya está adentro (directa o indirectamente) no se repite.
            if (padre.Contiene(hija.Id))
                throw new PermisoYaAsignadoException($"La familia \"{padre.Nombre}\" ya contiene a \"{hija.Nombre}\".");

            FamiliaService.AgregarFamilia(hija, padre);
            LoggerService.Info($"Permisos: agregó la familia \"{hija.Nombre}\" dentro de \"{padre.Nombre}\".");
        }

        /// <summary>Saca una patente o una subfamilia de una familia (no la borra del sistema).</summary>
        public void QuitarDeFamilia(Component componente, Familia familia)
        {
            Sesion.ExigirPermiso(Funcion.GestionarPermisos);
            if (componente == null || familia == null)
                throw new SeleccionObligatoriaException("el elemento que quiere quitar");
            ValidarNoProtegida(familia);

            FamiliaService.Quitar(componente, familia);
            LoggerService.Info($"Permisos: quitó \"{componente.Descripcion}\" de la familia \"{familia.Nombre}\".");
        }

        // Usuarios

        public void AsignarAUsuario(Component componente, Usuario usuario)
        {
            Sesion.ExigirPermiso(Funcion.GestionarPermisos);
            if (componente == null || usuario == null)
                throw new SeleccionObligatoriaException("el usuario y el permiso a asignar");
            ValidarNoEsElMismo(usuario);

            List<Component> actuales = UsuarioPermisoService.GetPrivilegios(usuario.IdUsuario);
            if (actuales.Exists(c => c.Id == componente.Id))
                throw new PermisoYaAsignadoException($"El usuario \"{usuario.NombreUsuario}\" ya tiene asignado \"{componente.Descripcion}\".");

            // Composite: si el permiso ya le llega por una de sus familias, tampoco se asigna
            // (ej: la patente "Registrar venta" ya está dentro de su familia "Comercial").
            Familia contenedora = actuales.OfType<Familia>().FirstOrDefault(f => f.Contiene(componente.Id));
            if (contenedora != null)
                throw new PermisoYaAsignadoException(
                    $"El usuario \"{usuario.NombreUsuario}\" ya tiene \"{componente.Descripcion}\" dentro de la familia \"{contenedora.Nombre}\".");

            if (componente is Familia familia)
                UsuarioPermisoService.AgregarFamilia(familia, usuario.IdUsuario);
            else if (componente is Patente patente)
                UsuarioPermisoService.AgregarPatente(patente, usuario.IdUsuario);

            LoggerService.Info($"Permisos: asignó \"{componente.Descripcion}\" al usuario \"{usuario.NombreUsuario}\".");
        }

        public void QuitarAUsuario(Component componente, Usuario usuario)
        {
            Sesion.ExigirPermiso(Funcion.GestionarPermisos);
            if (componente == null || usuario == null)
                throw new SeleccionObligatoriaException("el permiso que quiere quitar");
            ValidarNoEsElMismo(usuario);

            UsuarioPermisoService.Quitar(componente, usuario.IdUsuario);
            LoggerService.Info($"Permisos: quitó \"{componente.Descripcion}\" al usuario \"{usuario.NombreUsuario}\".");
        }

        // La familia de cada rol (la usa UsuarioService en el alta y cuando cambia el rol)

        /// <summary>
        /// Al crear un usuario se le asigna la familia de su rol (Dueño, Administrador o Mecánico).
        /// Después se puede ajustar en Seguridad > Permisos.
        /// </summary>
        internal void AsignarFamiliaDelRol(int idUsuario, Rol rol)
        {
            Familia familia = FamiliaService.GetByNombre(NombreFamiliaDelRol(rol));
            if (familia != null)
                UsuarioPermisoService.AgregarFamilia(familia, idUsuario);
        }

        internal void CambiarFamiliaDelRol(int idUsuario, Rol rolAnterior, Rol rolNuevo)
        {
            Familia anterior = FamiliaService.GetByNombre(NombreFamiliaDelRol(rolAnterior));
            if (anterior != null)
                UsuarioPermisoService.Quitar(anterior, idUsuario);

            List<Component> actuales = UsuarioPermisoService.GetPrivilegios(idUsuario);
            Familia nueva = FamiliaService.GetByNombre(NombreFamiliaDelRol(rolNuevo));
            if (nueva != null && !actuales.Exists(c => c.Id == nueva.Id))
                UsuarioPermisoService.AgregarFamilia(nueva, idUsuario);
        }

        private static string NombreFamiliaDelRol(Rol rol)
        {
            return Sesion.TextoRol(rol);   // "Dueño", "Administrador", "Mecánico"
        }

        // Validaciones

        private static void ValidarNoProtegida(Familia familia)
        {
            if (familia != null && familia.Nombre == FamiliaAdministrador)
                throw new FamiliaProtegidaException(
                    $"La familia \"{FamiliaAdministrador}\" está protegida: no se puede modificar ni eliminar.");
        }

        /// <summary>Nadie puede cambiar sus propios permisos (evita quedarse sin acceso por error).</summary>
        private static void ValidarNoEsElMismo(Usuario usuario)
        {
            if (Sesion.UsuarioActual != null && usuario.IdUsuario == Sesion.UsuarioActual.IdUsuario)
                throw new AutoModificacionPermisosException();
        }
    }
}
