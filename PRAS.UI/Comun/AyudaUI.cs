namespace PRAS.UI
{
    /// <summary>
    /// Abre la ayuda del sistema: un archivo de Ayuda HTML de Windows (PRAS.chm) armado con
    /// HTML Help Workshop (htmlhelp.exe). El .chm está en la carpeta Ayuda, al lado del .exe.
    /// - Menú Sistema > Ayuda: abre el índice.
    /// - Tecla F1 en cualquier pantalla: abre directamente el tema de esa pantalla
    ///   (cada pantalla tiene su página, con el mismo nombre que el formulario: FrmClientes.htm).
    /// </summary>
    public static class AyudaUI
    {
        private const string ArchivoAyuda = "PRAS.chm";

        /// <summary>Pantallas que tienen su propia página en la ayuda.</summary>
        private static readonly HashSet<string> PantallasConTema = new HashSet<string>
        {
            "FrmLogin", "FrmPrincipal", "FrmRecuperarContrasena", "FrmMiPerfil", "FrmPedirAyuda",
            "FrmUsuarios", "FrmPermisos", "FrmBitacora", "FrmBackup", "FrmIntegridad",
            "FrmClientes", "FrmMecanicos", "FrmProveedores", "FrmAgencias",
            "FrmRegistrarCompra", "FrmCompras", "FrmBuscadorAutos",
            "FrmAsignacionPresupuesto", "FrmDiagnosticos", "FrmCargaCostos", "FrmOrdenesTrabajo", "FrmTareasRealizadas",
            "FrmPedidos", "FrmConsignaciones", "FrmPublicaciones", "FrmVentas", "FrmReporte",
            // 15/11: pantallas nuevas
            "FrmFichaVehiculo", "FrmSolicitarCuenta", "FrmUsuariosDePrueba", "FrmRestablecerContrasena"
        };

        /// <summary>Ruta completa del .chm (carpeta Ayuda al lado del .exe).</summary>
        private static string RutaAyuda => Path.Combine(AppContext.BaseDirectory, "Ayuda", ArchivoAyuda);

        /// <summary>
        /// Muestra la ayuda. Si se indica una pantalla que tiene tema propio, abre esa página;
        /// si no, abre el índice general.
        /// </summary>
        public static void Mostrar(Control duenio, string pantalla = null)
        {
            if (!File.Exists(RutaAyuda))
            {
                Mensajes.Advertencia("No se encontró el archivo de ayuda (Ayuda\\PRAS.chm). Reinstale el sistema o consulte al administrador.");
                return;
            }

            try
            {
                if (pantalla != null && PantallasConTema.Contains(pantalla))
                    Help.ShowHelp(duenio, RutaAyuda, HelpNavigator.Topic, pantalla + ".htm");
                else
                    Help.ShowHelp(duenio, RutaAyuda, HelpNavigator.TableOfContents);
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
