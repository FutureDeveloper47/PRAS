using PRAS.Services.Logic.Infraestructure;

namespace PRAS.Services.Facade
{
    /// <summary>
    /// FACHADA de criptografía. Resuelve los casos de uso de arquitectura
    /// CU.Arq.003 Encriptar Datos y CU.Arq.004 Desencriptar Datos.
    /// </summary>
    public static class CryptographyService
    {
        /// <summary>Hash MD5 (de una sola vía), para los dígitos verificadores.</summary>
        public static string HashMd5(string textPlain)
        {
            return CryptographyLogic.HashMd5(textPlain);
        }

        /// <summary>CU.Arq.003 - Encripta con AES-256 (simétrico). Vacío o ya encriptado: lo devuelve igual.</summary>
        public static string Encriptar(string textoPlano)
        {
            return CryptographyLogic.EncriptarAes(textoPlano);
        }

        /// <summary>CU.Arq.004 - Desencripta un texto encriptado con Encriptar. Si no está encriptado, lo devuelve igual.</summary>
        public static string Desencriptar(string textoEncriptado)
        {
            return CryptographyLogic.DesencriptarAes(textoEncriptado);
        }

        /// <summary>true si el texto ya está encriptado.</summary>
        public static bool EstaEncriptado(string texto)
        {
            return CryptographyLogic.EstaEncriptado(texto);
        }
    }
}
