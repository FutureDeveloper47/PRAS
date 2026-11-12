namespace PRAS.Services.DomainModel.Composite
{
    /// <summary>
    /// PATRÓN COMPOSITE: clase base de los permisos.
    /// Un permiso puede ser:
    ///   - una Patente (hoja): un permiso concreto, ej. "Registrar venta".
    ///   - una Familia (compuesto): un grupo de patentes y/o de otras familias, ej. "Comercial".
    /// Como las dos heredan de Component, se tratan igual: un usuario tiene una lista de
    /// Component y no le importa si cada uno es una patente suelta o una familia entera.
    /// </summary>
    public abstract class Component
    {
        public Guid Id { get; set; }

        /// <summary>Nombre que se muestra en pantalla.</summary>
        public abstract string Descripcion { get; }

        public Component()
        {
        }

        public abstract void Add(Component component);

        public abstract void Remove(Component component);

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
