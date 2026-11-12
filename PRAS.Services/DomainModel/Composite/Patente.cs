namespace PRAS.Services.DomainModel.Composite
{
    /// <summary>
    /// COMPOSITE - la "hoja": un permiso concreto.
    /// DataKey identifica el permiso en el código. En PRAS es el nombre de la función
    /// (enum Funcion de la BLL), ej: "GestionarVentas". El menú y la BLL preguntan por esa clave.
    /// </summary>
    public class Patente : Component
    {
        public string DataKey { get; set; }

        public TipoAcceso TipoAcceso { get; set; }

        public string Nombre { get; set; }

        public override string Descripcion => string.IsNullOrEmpty(Nombre) ? DataKey : Nombre;

        public Patente()
        {
        }

        public override void Add(Component component)
        {
            throw new InvalidOperationException("No se pueden agregar elementos en un hijo tipo hoja");
        }

        public override void Remove(Component component)
        {
            throw new InvalidOperationException("No se pueden eliminar elementos en un hijo tipo hoja");
        }
    }

    /// <summary>A qué tipo de recurso da acceso la patente.</summary>
    public enum TipoAcceso
    {
        Pantalla,
        CasoUso,
        Servicio,
        SP,
        Tabla
    }
}
