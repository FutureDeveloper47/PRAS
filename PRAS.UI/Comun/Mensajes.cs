using PRAS.Services.Facade.Extensions;
using PRAS.Services.Facade.ExtensionsMethods;

namespace PRAS.UI
{         
    /// <summary>
    /// Cuadros de mensaje comunes a todos los formularios, para que todos
    /// muestren los avisos y errores de la misma forma.
    /// Todos los textos pasan por TraducirMensaje(): así cualquier aviso sale en el idioma
    /// elegido, aunque traiga datos (ej: "¿Deshabilitar al mecánico Juan Perez?").
    /// Se muestran con FrmMensaje (y no con MessageBox) para que respeten el tema claro u oscuro.
    /// </summary>
    public static class Mensajes
    {
        public static void Info(string texto)
        {
            FrmMensaje.Mostrar(texto.TraducirMensaje(), FrmMensaje.Tipo.Informacion);
        }

        public static void Advertencia(string texto)
        {
            FrmMensaje.Mostrar(texto.TraducirMensaje(), FrmMensaje.Tipo.Advertencia);
        }

        /// <summary>Pregunta Sí/No. Devuelve true si el usuario eligió "Sí".</summary>
        public static bool Confirmar(string texto)
        {
            return FrmMensaje.Mostrar(texto.TraducirMensaje(), FrmMensaje.Tipo.Pregunta) == DialogResult.Yes;
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

            // Traducimos el mensaje con TraducirMensaje(): si trae datos (una patente, un monto)
            // se usan las claves con huecos {0} del archivo de idioma.
            string mensaje = ExceptionManager.MensajeParaUsuario(ex).TraducirMensaje();

            if (ExceptionManager.EsReglaDeNegocio(ex))
                Advertencia(mensaje);
            else
                FrmMensaje.Mostrar(mensaje, FrmMensaje.Tipo.Error);
        }
    }
}
