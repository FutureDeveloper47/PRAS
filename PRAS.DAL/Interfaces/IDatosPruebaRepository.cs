namespace PRAS.DAL.Interfaces
{
    /// <summary>Carga de los datos de prueba del negocio (autos, compras, taller, agencia y ventas 2024-2026).</summary>
    public interface IDatosPruebaRepository
    {
        /// <summary>
        /// Carga los datos solo si todavía no estaban. Devuelve los Id de los clientes nuevos
        /// (la BLL les encripta el teléfono y les calcula el dígito verificador). Lista vacía = ya estaban.
        /// </summary>
        List<int> CargarOperacionesDePrueba();
    }
}
