namespace PRAS.UI
{
    /// <summary>
    /// Controla lo que se puede escribir en cada cuadro de texto según su NOMBRE,
    /// así no hay que programarlo en cada pantalla (se aplica solo desde FormBase):
    /// - DNI: solo números (y puntos).
    /// - CUIT: solo números y guiones.
    /// - Teléfono: números, espacios, guiones, paréntesis y "+".
    /// - Nombre y apellido: no se aceptan números.
    /// - Dominio (patente): letras y números, siempre en mayúsculas.
    /// Esto es una ayuda para el usuario: la validación "de verdad" la sigue haciendo
    /// la BLL (clase Validar), porque los datos también podrían llegar pegados con Ctrl+V.
    /// </summary>
    public static class Entradas
    {
        /// <summary>Recorre todos los controles de la pantalla (también los que están dentro de paneles).</summary>
        public static void Configurar(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox texto && !texto.ReadOnly)
                    ConfigurarTexto(texto);

                if (control.HasChildren && !(control is DataGridView))
                    Configurar(control.Controls);
            }
        }

        private static void ConfigurarTexto(TextBox texto)
        {
            string nombre = texto.Name.ToLower();

            if (nombre.StartsWith("txtdni"))
            {
                Permitir(texto, c => char.IsDigit(c) || c == '.');
                LimitarLargo(texto, 10);
            }
            else if (nombre.StartsWith("txtcuit"))
            {
                Permitir(texto, c => char.IsDigit(c) || c == '-');
                LimitarLargo(texto, 13);
            }
            else if (nombre.StartsWith("txttelefono") || nombre.StartsWith("txtcelular"))
            {
                Permitir(texto, c => char.IsDigit(c) || c == '-' || c == ' ' || c == '(' || c == ')' || c == '+');
                LimitarLargo(texto, 20);
            }
            else if (nombre == "txtnombre" || nombre == "txtapellido")
            {
                Permitir(texto, c => !char.IsDigit(c));
            }
            else if (nombre == "txtdominio" || nombre == "txtpatente")
            {
                Permitir(texto, char.IsLetterOrDigit);
                texto.CharacterCasing = CharacterCasing.Upper;
                LimitarLargo(texto, 7);
            }
        }

        /// <summary>
        /// Si la tecla no cumple la condición, se descarta (e.Handled = true).
        /// Las teclas de control (borrar, Ctrl+C, Ctrl+V...) siempre se aceptan.
        /// </summary>
        private static void Permitir(TextBox texto, Func<char, bool> condicion)
        {
            texto.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !condicion(e.KeyChar))
                    e.Handled = true;   // la tecla no se escribe
            };
        }

        /// <summary>Pone un largo máximo solo si el diseñador no puso uno más chico.</summary>
        private static void LimitarLargo(TextBox texto, int largo)
        {
            if (texto.MaxLength == 0 || texto.MaxLength > largo)
                texto.MaxLength = largo;
        }
    }
}
