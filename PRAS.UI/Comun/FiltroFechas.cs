namespace PRAS.UI
{
    /// <summary>
    /// Valores por defecto de los filtros "Desde / Hasta" de todas las pantallas de consulta.
    /// Están en un solo lugar para que todas arranquen igual: así, al abrirlas,
    /// se ven todos los registros cargados desde 2024 (incluidos los de prueba) y no una pantalla vacía.
    /// </summary>
    public static class FiltroFechas
    {
        /// <summary>Primer día que se muestra al abrir una consulta: 01/01/2024.</summary>
        public static DateTime DesdePorDefecto => new DateTime(2024, 1, 1);

        /// <summary>Último día que se muestra al abrir una consulta: hoy.</summary>
        public static DateTime HastaPorDefecto => DateTime.Today;
    }
}
