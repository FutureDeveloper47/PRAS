using System.Net.Mail;
using PRAS.BLL.Excepciones;

namespace PRAS.BLL
{
    /// <summary>
    /// Validaciones que se repiten en varios servicios (campos obligatorios, DNI, CUIT, email, patente).
    /// Cada método devuelve el valor ya "limpio" (sin espacios de más) o tira la excepción de negocio
    /// que corresponde, con un mensaje claro para el usuario.
    /// </summary>
    public static class Validar
    {
        /// <summary>Campo obligatorio con largo máximo (el mismo que la columna en la base).</summary>
        public static string Requerido(string valor, string campo, int largoMaximo)
        {
            valor = (valor ?? "").Trim();
            if (valor == "")
                throw new DatoObligatorioException(campo);
            if (valor.Length > largoMaximo)
                throw new LargoMaximoException(campo, largoMaximo);
            return valor;
        }

        /// <summary>Campo opcional: si viene vacío devuelve null (así en la base queda NULL).</summary>
        public static string Opcional(string valor, string campo, int largoMaximo)
        {
            valor = (valor ?? "").Trim();
            if (valor == "")
                return null;
            if (valor.Length > largoMaximo)
                throw new LargoMaximoException(campo, largoMaximo);
            return valor;
        }

        /// <summary>DNI de 7 u 8 números. Se aceptan puntos (se sacan).</summary>
        public static string DNI(string valor)
        {
            string dni = (valor ?? "").Trim().Replace(".", "");
            if (dni == "")
                throw new DatoObligatorioException("DNI");
            if (!SoloDigitos(dni) || dni.Length < 7 || dni.Length > 8)
                throw new DNIInvalidoException(dni);
            return dni;
        }

        /// <summary>CUIT de 11 números. Se devuelve con el formato 20-12345678-9.</summary>
        public static string CUIT(string valor, bool obligatorio)
        {
            string cuit = (valor ?? "").Trim().Replace("-", "");
            if (cuit == "")
            {
                if (obligatorio)
                    throw new DatoObligatorioException("CUIT");
                return null;
            }
            if (!SoloDigitos(cuit) || cuit.Length != 11)
                throw new CUITInvalidoException(valor);

            return $"{cuit.Substring(0, 2)}-{cuit.Substring(2, 8)}-{cuit.Substring(10, 1)}";
        }

        /// <summary>Email opcional; si viene, tiene que tener un formato válido.</summary>
        public static string Email(string valor, int largoMaximo)
        {
            string email = Opcional(valor, "Email", largoMaximo);
            if (email != null && !MailAddress.TryCreate(email, out _))
                throw new EmailInvalidoException(email);
            return email;
        }

        /// <summary>Patente: formato viejo (ABC123) o Mercosur (AB123CD). Se guarda sin espacios ni guiones.</summary>
        public static string Dominio(string valor)
        {
            string dominio = (valor ?? "").Trim().ToUpper().Replace(" ", "").Replace("-", "");
            if (dominio == "")
                throw new DatoObligatorioException("Dominio");

            if (!CoincideFormato(dominio, "LLLNNN") && !CoincideFormato(dominio, "LLNNNLL"))
                throw new DominioInvalidoException(dominio);
            return dominio;
        }

        /// <summary>En las consultas por período, "desde" no puede ser posterior a "hasta".</summary>
        public static void RangoFechas(DateTime desde, DateTime hasta)
        {
            if (desde.Date > hasta.Date)
                throw new RangoFechasInvalidoException(desde, hasta);
        }

        /// <summary>Una operación que ya ocurrió no puede tener fecha futura.</summary>
        public static void NoFutura(DateTime fecha, string que)
        {
            if (fecha.Date > DateTime.Today)
                throw new FechaFuturaException(que);
        }

        /// <summary>Precios, cantidades e importes tienen que ser mayores a 0.</summary>
        public static void MayorACero(decimal valor, string campo)
        {
            if (valor <= 0)
                throw new ImporteInvalidoException(campo);
        }

        /// <summary>Un dato elegido de una lista (Id) tiene que venir cargado.</summary>
        public static void Seleccionado(int id, string que)
        {
            if (id <= 0)
                throw new SeleccionObligatoriaException(que);
        }

        private static bool CoincideFormato(string texto, string formato)
        {
            if (texto.Length != formato.Length)
                return false;

            for (int i = 0; i < texto.Length; i++)
            {
                char c = texto[i];
                bool esLetra = c >= 'A' && c <= 'Z';
                if (formato[i] == 'L' && !esLetra) return false;
                if (formato[i] == 'N' && !char.IsDigit(c)) return false;
            }
            return true;
        }

        private static bool SoloDigitos(string texto)
        {
            foreach (char c in texto)
                if (!char.IsDigit(c))
                    return false;
            return true;
        }
    }
}
