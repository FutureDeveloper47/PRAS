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

                // 4) Cuadros de texto que solo aceptan números (DNI, CUIT, teléfono...).
                Entradas.Configurar(Controls);

                // 4b) Valores guardados en español (estados, prioridades, formas de pago...) que se
                //     muestran en grillas y combos: se traducen al mostrarlos, sin cambiar el dato.
                ValoresTraducidos.Conectar(Controls);

                // 5) Las pantallas con listados se agrandan para aprovechar la pantalla.
                AgrandarSiEsListado();
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// F1 en cualquier pantalla abre la ayuda en la página de esa pantalla.
        /// ProcessCmdKey recibe la tecla antes que cualquier control de la pantalla.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                AyudaUI.Mostrar(this, Name);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Las pantallas de consulta (las que se pueden agrandar y tienen una grilla o lista
        /// que crece hacia abajo) se abren más altas y un poco más anchas, sin pasarse del
        /// tamaño del monitor. Como sus controles están "anclados" (Anchor), se acomodan solos.
        /// Las pantallas de carga (tamaño fijo) quedan como se diseñaron.
        /// </summary>
        private void AgrandarSiEsListado()
        {
            if (FormBorderStyle != FormBorderStyle.Sizable || MainMenuStrip != null || WindowState != FormWindowState.Normal)
                return;

            if (!TieneListaQueCrece(Controls))
                return;

            Rectangle pantalla = Screen.FromControl(this).WorkingArea;
            int alto = Math.Max(Height, (int)(pantalla.Height * 0.88));
            int ancho = Math.Max(Width, Math.Min((int)(Width * 1.2), (int)(pantalla.Width * 0.9)));

            Size = new Size(Math.Min(ancho, pantalla.Width), Math.Min(alto, pantalla.Height));
            Location = new Point(pantalla.Left + (pantalla.Width - Width) / 2,
                                 pantalla.Top + (pantalla.Height - Height) / 2);
        }

        /// <summary>true si hay una grilla, lista o árbol anclado abajo (o sea, que crece con la ventana).</summary>
        private static bool TieneListaQueCrece(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                bool esLista = control is DataGridView || control is ListBox || control is TreeView;
                bool estiraHaciaAbajo = control.Dock == DockStyle.Fill
                    || (control.Anchor & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom);

                if (esLista && estiraHaciaAbajo)
                    return true;

                if (control.HasChildren && !(control is DataGridView) && TieneListaQueCrece(control.Controls))
                    return true;
            }
            return false;
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
