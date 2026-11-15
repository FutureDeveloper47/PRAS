using System.Configuration;
using PRAS.BLL.Excepciones;
using PRAS.Services.Facade;

namespace PRAS.BLL
{
    /// <summary>
    /// CU Seguridad - Pedir ayuda.
    /// El usuario elige un tema, escribe qué le pasa y el pedido queda registrado en la bitácora
    /// como advertencia (Warn), así el Administrador lo ve en Seguridad > Bitácora
    /// (buscando "Pedido de ayuda"). No hace falta una tabla nueva: la bitácora ya guarda
    /// fecha, hora y usuario.
    /// También devuelve los datos de contacto del administrador (están en App.config).
    /// </summary>
    public class SoporteService
    {
        private const int LargoMaximoDetalle = 1000;

        /// <summary>Temas posibles del desplegable (en español: la pantalla los traduce).</summary>
        public List<string> TemasDeAyuda()
        {
            return new List<string>
            {
                "No puedo iniciar sesión",
                "Error al guardar datos",
                "No veo una opción del menú (permisos)",
                "Problema con un reporte",
                "Problema con el backup o la restauración",
                "El sistema está lento o no responde",
                "Otro problema"
            };
        }

        /// <summary>Registra el pedido de ayuda en la bitácora.</summary>
        public void RegistrarPedido(string tema, string detalle)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tema))
                    throw new SeleccionObligatoriaException("el tema del problema");

                detalle = Validar.Requerido(detalle, "Detalle", LargoMaximoDetalle);

                LoggerService.Warn($"Pedido de ayuda [{tema}]: {detalle}");
            }
            catch (ReglaNegocioException ex)
            {
                Services.Facade.ExtensionsMethods.ExceptionManager.HandleException(ex);
                throw;
            }
        }

        /// <summary>Nombre del administrador del sistema (App.config, clave SoporteNombre).</summary>
        public string NombreAdministrador()
        {
            return LeerConfiguracion("SoporteNombre", "Matías Montoto");
        }

        /// <summary>Celular del administrador (App.config, clave SoporteTelefono).</summary>
        public string TelefonoAdministrador()
        {
            return LeerConfiguracion("SoporteTelefono", "");
        }

        /// <summary>Email del administrador (App.config, clave SoporteEmail).</summary>
        public string EmailAdministrador()
        {
            return LeerConfiguracion("SoporteEmail", "matiasmontoto5@gmail.com");
        }

        private static string LeerConfiguracion(string clave, string porDefecto)
        {
            string valor = ConfigurationManager.AppSettings[clave];
            return string.IsNullOrWhiteSpace(valor) ? porDefecto : valor.Trim();
        }
    }
}
