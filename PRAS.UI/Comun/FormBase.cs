using System.ComponentModel;

namespace PRAS.UI
{
    /// <summary>
    /// "Molde" de todas las pantallas del sistema. Cada formulario hereda de acá
    /// (": FormBase" en lugar de ": Form"), así todos reciben el mismo tema visual
    /// y la traducción al idioma elegido, sin repetir código en cada uno. Esto es HERENCIA: FormBase es un Form con algo extra.
    /// </summary>
    public class FormBase : Form
    {
        /// <summary>
        /// OnLoad se ejecuta justo antes de mostrar la pantalla.
        /// Primero aplicamos el tema y después (base.OnLoad) corre el Load de cada formulario.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            // En el diseñador de Visual Studio no lo aplicamos, porque si no
            // podría copiar estos colores al .Designer.cs al guardar.
            bool enDiseniador = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            if (!enDiseniador)
            {
                // 1) Traducimos todos los textos de la pantalla al idioma actual.
                // 2) Aplicamos el estilo (va después porque usa el título ya traducido).
                Traductor.Traducir(this);
                Tema.Aplicar(this);

                // 3) Ponemos el ícono de PRAS en la barra de título de la pantalla.
                Icon = IconoPrograma();
            }

            base.OnLoad(e);
        }

        private static Icon _icono;

        /// <summary>
        /// Devuelve el ícono de PRAS. Lo saca del propio .exe (el ícono se le pone en PRAS.UI.csproj
        /// con ApplicationIcon) y lo guarda para no leerlo de nuevo en cada pantalla.
        /// </summary>
        private static Icon IconoPrograma()
        {
            if (_icono == null)
                _icono = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            return _icono;
        }
    }
}
