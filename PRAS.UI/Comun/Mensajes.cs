using PRAS.Services.Facade.Extensions;
using PRAS.Services.Facade.ExtensionsMethods;

namespace PRAS.UI
{         
    /// <summary>
    /// Cuadros de mensaje comunes a todos los formularios, para que todos
    /// muestren los avisos y errores de la misma forma.
    /// </summary>
    public static class Mensajes
    {
        private const string Titulo = "PRAS";

        public static void Info(string texto)
        {
            MessageBox.Show(texto, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Advertencia(string texto)
        {
            MessageBox.Show(texto, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>Pregunta Sí/No. Devuelve true si el usuario eligió "Sí".</summary>
        public static bool Confirmar(string texto)
        {
            return MessageBox.Show(texto, Titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                   == DialogResult.Yes;
        }

        /// <summary>
        /// Muestra una excepción aplicando la política de excepciones del proyecto Services
        /// (ex.Handle() -> ExceptionManager):
        /// 1) ex.Handle() la registra en la bitácora con el nivel que corresponde.
        /// 2) Si es una regla de negocio, se muestra como aviso (no es un error del programa).
        ///    Si es de acceso a datos u otra cosa, se muestra como error.
        /// </summary>
        public static void Error(Exception ex)
        {
            ex.Handle();

            // Los mensajes de nuestros errores se muestran tal cual (tienen datos como una patente o un monto).
            // Solo traducimos el mensaje genérico de error inesperado.
            string mensaje = ExceptionManager.MensajeParaUsuario(ex);
            if (mensaje != ex.Message)
                mensaje = mensaje.Traducir();

            if (ExceptionManager.EsReglaDeNegocio(ex))
                Advertencia(mensaje);
            else
                MessageBox.Show(mensaje, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
