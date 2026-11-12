using PRAS.Services.DataAccess.Implementations;
using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.Logic
{
    /// <summary>
    /// Permisos de cada usuario.
    /// Un usuario tiene una lista de Component: familias completas y/o patentes sueltas.
    /// </summary>
    internal static class UsuarioPermisoLogic
    {
        /// <summary>Familias y patentes asignadas DIRECTAMENTE al usuario (cada familia con su árbol).</summary>
        public static List<Component> GetPrivilegios(int idUsuario)
        {
            var privilegios = new List<Component>();
            privilegios.AddRange(new UsuarioFamiliaRepository().GetByObject(idUsuario));
            privilegios.AddRange(new UsuarioPatenteRepository().GetByObject(idUsuario));
            return privilegios;
        }

        public static List<Patente> GetPatentes(int idUsuario)
        {
            return RecorrerPatentes(GetPrivilegios(idUsuario));
        }

        /// <summary>Recorre el árbol del Composite juntando las patentes.</summary>
        public static List<Patente> RecorrerPatentes(List<Component> componentes)
        {
            var patentes = new List<Patente>();
            Recorrer(patentes, componentes);
            return patentes;
        }

        private static void Recorrer(List<Patente> patentes, List<Component> componentes)
        {
            foreach (Component componente in componentes)
            {
                if (componente is Patente patente)
                {
                    if (!patentes.Exists(p => p.Id == patente.Id))
                        patentes.Add(patente);
                }
                else if (componente is Familia familia)
                {
                    Recorrer(patentes, familia.GetHijos());
                }
            }
        }

        public static void AgregarFamilia(Familia familia, int idUsuario)
        {
            new UsuarioFamiliaRepository().Add(familia, idUsuario);
        }

        public static void AgregarPatente(Patente patente, int idUsuario)
        {
            new UsuarioPatenteRepository().Add(patente, idUsuario);
        }

        public static void Quitar(Component componente, int idUsuario)
        {
            if (componente is Patente patente)
                new UsuarioPatenteRepository().Remove(patente, idUsuario);
            else if (componente is Familia familia)
                new UsuarioFamiliaRepository().Remove(familia, idUsuario);
        }
    }
}
