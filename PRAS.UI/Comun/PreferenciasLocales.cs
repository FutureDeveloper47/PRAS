using System.Text.Json;

namespace PRAS.UI
{
    /// <summary>
    /// Preferencias de cada usuario EN ESTA PC (por ahora, el tema claro u oscuro).
    /// Se guardan en un archivo de texto en la carpeta del usuario de Windows
    /// (%AppData%\PRAS\preferencias.json) y NO en la base: es una preferencia visual de la PC,
    /// no un dato del negocio (así lo pidió la consigna).
    /// Si el archivo no se puede leer o escribir, el sistema sigue igual con el tema claro.
    /// </summary>
    public static class PreferenciasLocales
    {
        public const string TemaClaro = "Claro";
        public const string TemaOscuro = "Oscuro";

        /// <summary>Contenido del archivo: el último tema usado en la PC y el tema de cada usuario.</summary>
        private sealed class Datos
        {
            public string UltimoTema { get; set; } = TemaClaro;

            /// <summary>Último usuario que entró en esta PC (solo el nombre, nunca la contraseña).</summary>
            public string UltimoUsuario { get; set; }
            public Dictionary<string, string> TemaPorUsuario { get; set; } = new Dictionary<string, string>();
        }

        private static string Archivo =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PRAS", "preferencias.json");

        /// <summary>
        /// Tema del usuario. Si nunca eligió, el último que se usó en la PC
        /// (usuario = null: antes de iniciar sesión, para la presentación y el ingreso).
        /// </summary>
        public static bool TemaOscuroDe(string usuario)
        {
            Datos datos = Leer();
            string tema = usuario != null && datos.TemaPorUsuario.TryGetValue(usuario.ToLowerInvariant(), out string elegido)
                ? elegido
                : datos.UltimoTema;
            return tema == TemaOscuro;
        }

        /// <summary>Guarda el tema elegido por el usuario (y lo deja como último usado en la PC).</summary>
        public static void GuardarTema(string usuario, bool oscuro)
        {
            Datos datos = Leer();
            string tema = oscuro ? TemaOscuro : TemaClaro;
            datos.UltimoTema = tema;
            if (!string.IsNullOrEmpty(usuario))
                datos.TemaPorUsuario[usuario.ToLowerInvariant()] = tema;

            Grabar(datos);
        }

        private static void Grabar(Datos datos)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Archivo));
                File.WriteAllText(Archivo, JsonSerializer.Serialize(datos, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception)
            {
                // Sin permiso para escribir: la próxima vez arranca con los valores por defecto. No es un error del negocio.
            }
        }

        /// <summary>Nombre del último usuario que entró en esta PC (para dejarlo escrito en el ingreso).</summary>
        public static string UltimoUsuario()
        {
            return Leer().UltimoUsuario;
        }

        /// <summary>Guarda el nombre del usuario que acaba de entrar. La contraseña NUNCA se guarda.</summary>
        public static void GuardarUltimoUsuario(string usuario)
        {
            Datos datos = Leer();
            datos.UltimoUsuario = usuario;
            Grabar(datos);
        }

        private static Datos Leer()
        {
            try
            {
                if (File.Exists(Archivo))
                    return JsonSerializer.Deserialize<Datos>(File.ReadAllText(Archivo)) ?? new Datos();
            }
            catch (Exception)
            {
                // Archivo dañado o sin permiso: se usan los valores por defecto.
            }
            return new Datos();
        }
    }
}
