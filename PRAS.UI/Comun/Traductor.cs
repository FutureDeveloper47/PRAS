using System.Globalization;
using System.Runtime.CompilerServices;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Traduce una pantalla entera usando el servicio de idiomas del proyecto PRAS.Services.
    ///
    /// Se podría traducir control por control:  btnAgregar.Text = "Agregar Cliente".Traducir();
    /// Acá hacemos eso mismo, pero recorriendo TODOS los controles de la pantalla
    /// (botones, etiquetas, menús, columnas de las grillas...), así no hay que escribir
    /// una línea por cada control en cada formulario.
    ///
    /// Se llama solo desde FormBase, así que todas las pantallas se traducen al abrirse.
    /// </summary>
    public static class Traductor
    {
        /// <summary>
        /// Texto ORIGINAL (en español) de cada control. Lo guardamos para poder volver a
        /// traducir cuando el usuario cambia de idioma (si no, buscaríamos "Save" en vez de "Guardar").
        /// ConditionalWeakTable = diccionario que no impide que el control se libere de memoria.
        /// </summary>
        private static readonly ConditionalWeakTable<object, string> originales = new ConditionalWeakTable<object, string>();

        public static void Traducir(Form form)
        {
            form.Text = TraducirComponente(form, form.Text);
            TraducirControles(form.Controls);
        }

        /// <summary>
        /// Traduce un texto suelto. Si termina en ":" o en ": *" (ej: "Usuario:", "Nombre: *")
        /// traduce la palabra y le vuelve a poner el final, porque en el archivo de idioma
        /// el ":" separa la clave de la traducción.
        /// </summary>
        public static string TraducirTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            string limpio = texto.Trim();
            string sufijo = "";

            // Sacamos el final que no se traduce y se lo volvemos a poner después:
            // "Nombre: *" (campo obligatorio), "Nombre:" o "Nombre *".
            foreach (string final in new[] { ": *", ":*", ":", " *" })
            {
                if (limpio.EndsWith(final))
                {
                    sufijo = final;
                    limpio = limpio.Substring(0, limpio.Length - final.Length).TrimEnd();
                    break;
                }
            }

            // No traducimos números o símbolos sueltos.
            if (limpio == "" || !limpio.Any(char.IsLetter))
                return texto;

            // Textos con ":" en el medio (ej: "PRAS: Programa de Registro Automotor Superior").
            // La clave no puede tener ":", así que se traduce cada parte por separado con
            // TraducirMensaje, que además no anota nada en el archivo si no encuentra la frase.
            if (limpio.Contains(':'))
                return limpio.TraducirMensaje() + sufijo;

            string traduccion = limpio.Traducir();

            // Si el original estaba en mayúsculas (por ejemplo "TALLER"), la traducción también.
            if (limpio == limpio.ToUpper())
                traduccion = traduccion.ToUpper();

            return traduccion + sufijo;
        }

        private static void TraducirControles(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                switch (control)
                {
                    case MenuStrip menu:
                        TraducirItems(menu.Items);
                        break;

                    case StatusStrip:
                        break;   // la barra de estado la llenamos por código (usuario logueado)

                    case DataGridView grilla:
                        foreach (DataGridViewColumn columna in grilla.Columns)
                            columna.HeaderText = TraducirComponente(columna, columna.HeaderText);
                        break;

                    // Donde el usuario escribe o elige datos, el texto es un dato y no se traduce.
                    case TextBoxBase:
                    case ComboBox:
                    case ListBox:
                    case UpDownBase:
                    case DateTimePicker:
                        break;

                    default:
                        control.Text = TraducirComponente(control, control.Text);
                        break;
                }

                if (control.HasChildren && !(control is DataGridView))
                    TraducirControles(control.Controls);
            }
        }

        private static void TraducirItems(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                // Las opciones de idioma se muestran en su propio idioma, no se traducen.
                if (item.Tag is CultureInfo)
                    continue;

                item.Text = TraducirComponente(item, item.Text);

                if (item is ToolStripMenuItem menuItem)
                    TraducirItems(menuItem.DropDownItems);
            }
        }

        /// <summary>Traduce siempre a partir del texto original en español del componente.</summary>
        private static string TraducirComponente(object componente, string textoActual)
        {
            string original = originales.GetValue(componente, _ => textoActual);
            return TraducirTexto(original);
        }
    }
}
