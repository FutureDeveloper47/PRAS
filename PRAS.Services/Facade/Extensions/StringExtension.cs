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

        /// <summary>
        /// Para valores que vienen de la base o de un enum y se guardan en español:
        ///     e.Value = estado.TraducirValor();   // "En reparación" -> "Under repair"
        /// Solo traduce si el texto entero es una clave del archivo; los nombres y patentes quedan igual.
        /// </summary>
        public static string TraducirValor(this string valor)
        {
            return Logic.IdiomaLogic.TraducirValor(valor);
        }
    }
}
