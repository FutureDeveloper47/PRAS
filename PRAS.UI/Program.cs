using PRAS.BLL;
using PRAS.Services.Facade;
using PRAS.Services.Facade.ExtensionsMethods;

namespace PRAS.UI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Si alguna pantalla no atrapó un error, lo agarramos acá:
            // lo registramos en la bitácora y mostramos un mensaje,
            // así el programa no se cierra de golpe.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => Mensajes.Error(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                    ex.Handle();
            };

            // 0) Arrancamos en español. Al iniciar sesión pasa al idioma de cada usuario.
            IdiomaService.CambiarIdioma("es-AR");

            var sistema = new SistemaService();

            // Pantalla de presentación mientras revisamos la base de datos.
            var splash = new FrmSplash();
            splash.Show();
            splash.Avanzar("Buscando las bases de datos...", 15);

            // 1) Primera vez en esta PC: si las bases no existen, se crean con el script de instalación.
            try
            {
                if (!sistema.BasesInstaladas())
                {
                    splash.Hide();   // el mensaje no puede quedar tapado por la presentación
                    var respuesta = MessageBox.Show(
                        "Es la primera vez que se usa PRAS en esta PC: todavía no existen las bases de datos.\n\n" +
                        "¿Crearlas ahora? Tarda unos segundos.",
                        "PRAS - Primera ejecución", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (respuesta != DialogResult.Yes)
                    {
                        splash.Close();
                        return;
                    }

                    splash.Show();
                    splash.Avanzar("Creando las bases de datos PRAS y PRAS_Seguridad...", 30);
                    Cursor.Current = Cursors.WaitCursor;
                    sistema.InstalarBases();
                    Cursor.Current = Cursors.Default;

                    splash.Hide();
                    MessageBox.Show(
                        "Las bases de datos se crearon correctamente.\n\n" +
                        "Usuarios para ingresar:\n  admin / admin123\n  dueno / dueno123\n  mecanico / mecanico123",
                        "PRAS - Primera ejecución", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    splash.Show();
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                splash.Close();
                MessageBox.Show(
                    "No se pudo conectar con SQL Server LocalDB para preparar la base de datos.\n\n" +
                    "Revisá que esté instalado \"SQL Server Express LocalDB\" (ver el Manual de Instalación).\n\n" +
                    "Detalle: " + ex.Message,
                    "PRAS - Error de instalación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2) Antes de pedir el usuario, revisamos que haya conexión con la base.
            splash.Avanzar("Conectando con las bases de datos...", 55);
            if (!sistema.ProbarConexion(out string mensaje))
            {
                splash.Close();
                LoggerService.Fatal(mensaje);
                MessageBox.Show(mensaje, "PRAS - Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2b) Si la base es de una versión anterior, le agregamos las tablas nuevas.
            splash.Avanzar("Actualizando la estructura de la base...", 75);
            try
            {
                sistema.ActualizarEstructura();
            }
            catch (Exception ex)
            {
                // No frena el sistema: solo no va a funcionar la pregunta de seguridad. Queda en la bitácora.
                ex.Handle();
            }

            splash.Avanzar("Listo. Abriendo el sistema...", 100);
            Thread.Sleep(600);   // para que se alcance a ver la presentación completa
            splash.Close();
            splash.Dispose();

            // 3) Login y después el menú. Si elige Cerrar sesión,
            // se repite y vuelve a aparecer el login.
            while (true)
            {
                IdiomaService.CambiarIdioma("es-AR");   // el login siempre en español

                using (var login = new FrmLogin())
                {
                    if (login.ShowDialog() != DialogResult.OK)
                        return;   // canceló o se pasó de intentos, cerramos el programa
                }

                var principal = new FrmPrincipal();
                Application.Run(principal);

                if (!principal.CerroSesion)
                    return;   // cerró la ventana o eligió Salir
            }
        }
    }
}
