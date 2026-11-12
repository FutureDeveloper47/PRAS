namespace PRAS.Services.DomainModel.Composite
{
    /// <summary>
    /// COMPOSITE - el "compuesto": agrupa patentes y otras familias.
    /// Ej: la familia "Dueño" contiene las familias "Taller", "Comercial", "Reportes", etc.
    /// y la familia "Administrador" contiene a "Dueño" + "Seguridad".
    /// </summary>
    public class Familia : Component
    {
        private readonly List<Component> hijos = new List<Component>();

        public string Nombre { get; set; }

        public override string Descripcion => Nombre;

        public Familia()
        {
        }

        public Familia(Component component)
        {
            hijos.Add(component);
        }

        public override void Add(Component component)
        {
            hijos.Add(component);
        }

        public void AddRange(IEnumerable<Component> components)
        {
            hijos.AddRange(components);
        }

        public override void Remove(Component component)
        {
            hijos.RemoveAll(h => h.Id == component.Id);
        }

        public List<Component> GetHijos()
        {
            return hijos;
        }

        /// <summary>
        /// true si esta familia contiene (directa o indirectamente) al componente indicado.
        /// Sirve para no armar ciclos (una familia adentro de sí misma).
        /// </summary>
        public bool Contiene(Guid id)
        {
            foreach (Component hijo in hijos)
            {
                if (hijo.Id == id)
                    return true;
                if (hijo is Familia familia && familia.Contiene(id))
                    return true;
            }
            return false;
        }
    }
}
