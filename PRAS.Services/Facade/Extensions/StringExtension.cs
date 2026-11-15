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

        /// <summary>
        /// Igual que Traducir pero para mensajes que traen datos:
        ///     Mensajes.Info($"Se guardó el cliente {nombre}.".TraducirMensaje());
        /// Usa las claves con huecos {0} del archivo de idioma.
        /// </summary>
        public static string TraducirMensaje(this string mensaje)
        {
            return Logic.IdiomaLogic.TraducirMensaje(mensaje);
        }
    }
}
