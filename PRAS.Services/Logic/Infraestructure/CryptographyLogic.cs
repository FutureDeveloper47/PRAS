using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace PRAS.Services.Logic.Infraestructure
{
    /// <summary>
    /// Criptografía del sistema:
    /// - HashMd5: para los dígitos verificadores. Es de una sola vía: el mismo texto SIEMPRE da
    ///   el mismo hash, y si cambia una sola letra el hash es otro. No se puede volver atrás.
    /// - EncriptarAes / DesencriptarAes: encriptado SIMÉTRICO (AES-256). Con la misma clave se
    ///   encripta y se desencripta, así el dato se puede volver a leer. Se usa para los datos
    ///   personales del cliente (teléfono y email).
    /// </summary>
    internal static class CryptographyLogic
    {
        /// <summary>Marca que se le pone al texto encriptado, para saber si un dato ya lo está.</summary>
        private const string Prefijo = "ENC:";

        public static string HashMd5(string textPlain)
        {
            StringBuilder sb = new StringBuilder();

            using (MD5 md5 = MD5.Create())
            {
                byte[] retVal = md5.ComputeHash(Encoding.Unicode.GetBytes(textPlain));
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Encripta con AES-256 (modo CBC). Cada vez se genera un IV (vector de inicialización)
        /// al azar, así el mismo teléfono encriptado dos veces da textos distintos.
        /// Resultado: "ENC:" + Base64(IV + datos encriptados).
        /// Si el texto viene vacío o ya está encriptado, se devuelve tal cual.
        /// </summary>
        public static string EncriptarAes(string textoPlano)
        {
            if (string.IsNullOrEmpty(textoPlano) || EstaEncriptado(textoPlano))
                return textoPlano;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Clave();
                aes.GenerateIV();

                byte[] cifrado = aes.EncryptCbc(Encoding.UTF8.GetBytes(textoPlano), aes.IV, PaddingMode.PKCS7);

                // Guardamos el IV adelante: hace falta para desencriptar (no es secreto)
                byte[] todo = new byte[aes.IV.Length + cifrado.Length];
                Buffer.BlockCopy(aes.IV, 0, todo, 0, aes.IV.Length);
                Buffer.BlockCopy(cifrado, 0, todo, aes.IV.Length, cifrado.Length);

                return Prefijo + Convert.ToBase64String(todo);
            }
        }

        /// <summary>
        /// Desencripta un texto hecho con EncriptarAes. Si el texto no tiene la marca "ENC:"
        /// es un dato viejo que todavía no se encriptó: se devuelve tal cual.
        /// </summary>
        public static string DesencriptarAes(string textoEncriptado)
        {
            if (string.IsNullOrEmpty(textoEncriptado) || !EstaEncriptado(textoEncriptado))
                return textoEncriptado;

            byte[] todo = Convert.FromBase64String(textoEncriptado.Substring(Prefijo.Length));

            using (Aes aes = Aes.Create())
            {
                aes.Key = Clave();

                // Separamos el IV (primeros 16 bytes) de los datos encriptados
                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] cifrado = new byte[todo.Length - iv.Length];
                Buffer.BlockCopy(todo, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(todo, iv.Length, cifrado, 0, cifrado.Length);

                return Encoding.UTF8.GetString(aes.DecryptCbc(cifrado, iv, PaddingMode.PKCS7));
            }
        }

        /// <summary>true si el texto ya fue encriptado por el sistema.</summary>
        public static bool EstaEncriptado(string texto)
        {
            return texto != null && texto.StartsWith(Prefijo, StringComparison.Ordinal);
        }

        /// <summary>Lee la clave AES del App.config ("ClaveAES", 32 bytes en Base64).</summary>
        private static byte[] Clave()
        {
            string base64 = ConfigurationManager.AppSettings["ClaveAES"];
            if (string.IsNullOrWhiteSpace(base64))
                throw new InvalidOperationException("Falta la clave \"ClaveAES\" en el archivo de configuración.");

            byte[] clave = Convert.FromBase64String(base64);
            if (clave.Length != 32)
                throw new InvalidOperationException("La clave \"ClaveAES\" tiene que tener 32 bytes (AES-256).");

            return clave;
        }
    }
}
