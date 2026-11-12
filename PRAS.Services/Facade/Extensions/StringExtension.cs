namespace PRAS.Services.Facade.Extensions
{
    /// <summary>
    /// Método de extensión: permite escribir
    ///     btnGuardar.Text = "Guardar".Traducir();
    /// como si Traducir() fuera un método más de string.
    /// </summary>
    public static class StringExtension
    {
        public static string Traducir(this string texto)
        {
            return Logic.IdiomaLogic.Traducir(texto);
        }
    }
}
