namespace PRAS.Entidades
{
    /// <summary>
    /// Qué tipo de movimiento se muestra en "Actividad reciente" de la pantalla de Inicio.
    /// </summary>
    public enum TipoActividad
    {
        Ingreso,   // compramos un auto chocado
        Venta   // la agencia vendió un auto
    }

    /// <summary>
    /// Un renglón de la lista "Actividad reciente" de la pantalla de Inicio.
    /// Ejemplo: "Ingresó Ford Focus".
    /// </summary>
    public class ActividadReciente
    {
        /// <summary>Auto del movimiento (sirve para abrir su ficha al hacer clic).</summary>
        public int IdVehiculo { get; set; }
        public DateTime Fecha { get; set; }
        public TipoActividad Tipo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Dominio { get; set; }

        /// <summary>Texto listo para mostrar, igual que en el diseño de EA.</summary>
        public string Descripcion =>
            Tipo == TipoActividad.Ingreso
                ? $"Ingresó {Marca} {Modelo}"
                : $"Venta {Marca} {Modelo}";
    }
}
