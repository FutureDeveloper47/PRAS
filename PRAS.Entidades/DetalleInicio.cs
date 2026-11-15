namespace PRAS.Entidades
{
    /// <summary>
    /// Un renglón del detalle que se abre al tocar una tarjeta del Inicio
    /// (TALLER, INGRESOS o AGENCIA). No es una tabla de la base: es un "resumen"
    /// armado solo para mostrar en pantalla, con el auto y los datos que importan.
    /// </summary>
    public class DetalleInicio
    {
        /// <summary>Auto del renglón (con doble clic se abre su ficha).</summary>
        public int IdVehiculo { get; set; }

        /// <summary>Fecha del movimiento: ingreso al taller, compra, venta o entrega a la agencia.</summary>
        public DateTime? Fecha { get; set; }

        public string Dominio { get; set; } = "";

        /// <summary>Marca, modelo y año juntos. Ej: "Ford Ranger (2019)".</summary>
        public string Vehiculo { get; set; } = "";

        /// <summary>Estado del auto en texto. Ej: "En reparación".</summary>
        public string Estado { get; set; } = "";

        /// <summary>Persona relacionada: mecánico (taller), vendedor (ingresos) o comprador (agencia).</summary>
        public string Persona { get; set; } = "";

        /// <summary>Precio de compra, de venta o publicado, según la tarjeta. Vacío si no aplica.</summary>
        public decimal? Monto { get; set; }

        /// <summary>Dato extra según la tarjeta: prioridad, forma de pago o agencia.</summary>
        public string Extra { get; set; } = "";

        /// <summary>true si el renglón es una venta (sirve para pintarlo distinto y para el total).</summary>
        public bool EsVenta { get; set; }
    }
}
