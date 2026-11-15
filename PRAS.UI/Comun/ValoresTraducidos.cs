using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Muestra en el idioma elegido los VALORES que se guardan en español en la base o vienen de un enum:
    /// estados ("En reparación"), prioridades ("Alta"), formas de pago ("Transferencia"), plataformas,
    /// roles, especialidades, tipos de gasto, etc.
    ///
    /// El dato NO se cambia: en la base y en los objetos sigue en español (así las reglas de negocio
    /// y las consultas funcionan igual en cualquier idioma). Solo cambia lo que se ve:
    ///  - en las grillas, con el evento CellFormatting (se dispara justo antes de dibujar cada celda);
    ///  - en los combos y listas, con el evento Format (se dispara al dibujar cada opción).
    /// Es el mismo criterio que ya se usaba en la bitácora y en la especialidad de los mecánicos,
    /// pero puesto en un solo lugar: FormBase lo conecta en todas las pantallas al abrirlas.
    ///
    /// Solo se traduce un valor si el texto COMPLETO es una clave del archivo de idioma
    /// (TraducirValor): un nombre, una patente o una descripción escrita por el usuario quedan igual.
    /// </summary>
    public static class ValoresTraducidos
    {
        /// <summary>Recorre la pantalla y conecta los eventos en cada grilla, combo y lista.</summary>
        public static void Conectar(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                switch (control)
                {
                    case DataGridView grilla:
                        Conectar(grilla);
                        break;
                    case ComboBox combo:
                        Conectar(combo);
                        break;
                    case ListBox lista:
                        Conectar(lista);
                        break;
                }

                if (control.HasChildren && !(control is DataGridView))
                    Conectar(control.Controls);
            }
        }

        /// <summary>Para grillas que se crean por código después de abrir la pantalla.</summary>
        public static void Conectar(DataGridView grilla)
        {
            grilla.CellFormatting -= Grilla_CellFormatting;   // por si se llama dos veces
            grilla.CellFormatting += Grilla_CellFormatting;
        }

        public static void Conectar(ListControl lista)
        {
            // Las listas dibujadas "a mano" (OwnerDraw) ya traducen su propio texto.
            if (lista is ListBox listBox && listBox.DrawMode != DrawMode.Normal)
                return;
            // (Los combos dibujados por el Tema sí se conectan: dibujan con GetItemText, que usa Format.)

            lista.FormattingEnabled = true;   // sin esto, el evento Format no se dispara
            lista.Format -= Lista_Format;
            lista.Format += Lista_Format;
        }

        /// <summary>
        /// Combo donde el usuario puede elegir de la lista O escribir otro valor (ej: la plataforma).
        /// Se muestran traducidas las opciones de la lista; la pantalla tiene que leer el valor elegido
        /// con SelectedItem (en español) y no con Text (que ya está traducido).
        /// </summary>
        public static void ConectarEditable(ComboBox combo)
        {
            combo.FormattingEnabled = true;
            combo.Format -= ListaEditable_Format;
            combo.Format += ListaEditable_Format;
        }

        private static void ListaEditable_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.Value is string valor)
                e.Value = valor.TraducirValor();
        }

        private static void Grilla_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is string valor)
            {
                string traducido = valor.TraducirValor();
                if (!ReferenceEquals(traducido, valor))
                {
                    e.Value = traducido;
                    e.FormattingApplied = true;
                }
            }
        }

        private static void Lista_Format(object sender, ListControlConvertEventArgs e)
        {
            // Los combos que el usuario puede escribir (DropDown) muestran lo que escribió, no lo tocamos.
            if (sender is ComboBox combo && combo.DropDownStyle != ComboBoxStyle.DropDownList)
                return;

            if (e.Value is string valor)
                e.Value = valor.TraducirValor();
        }
    }
}
