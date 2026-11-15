namespace PRAS.Entidades
{
    /// <summary>
    /// Números que muestra la pantalla de Inicio (Pantalla A de EA).
    /// No es una tabla: es un "paquete" de datos que la BLL arma consultando varias tablas.
    /// </summary>
    public class ResumenInicio
    {
        /// <summary>Recuadro TALLER: autos en diagnóstico o en reparación.</summary>
        public int VehiculosEnTaller { get; set; }

        /// <summary>Recuadro INGRESOS: autos chocados comprados en los últimos 12 meses (incluye el actual).</summary>
        public int ChocadosIngresadosUltimoAnio { get; set; }

        /// <summary>Recuadro AGENCIA: ventas realizadas en los últimos 12 meses (incluye el actual).</summary>
        public int VentasUltimoAnio { get; set; }

        /// <summary>Lista ACTIVIDAD RECIENTE: últimos ingresos y ventas, del más nuevo al más viejo.</summary>
        public List<ActividadReciente> Actividades { get; set; } = new List<ActividadReciente>();
    }
}
