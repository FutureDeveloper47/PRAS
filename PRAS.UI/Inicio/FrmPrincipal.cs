using System.Globalization;
using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.DomainModel;
using PRAS.Services.Facade;
using PRAS.Services.Facade.ExtensionsMethods;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Menú principal + Pantalla A de EA (Inicio): recuadros TALLER, INGRESOS y AGENCIA
    /// con números reales de la base, y la lista de ACTIVIDAD RECIENTE.
    /// Los recuadros funcionan como botones: al tocarlos se abre el detalle con cada auto.
    /// Al abrirse muestra solo las opciones para las que el usuario tiene permiso (patentes).
    /// </summary>
    public partial class FrmPrincipal : FormBase
    {
        private readonly UsuarioService _usuarioService = new UsuarioService();
        private readonly InicioService _inicioService = new InicioService();

        /// <summary>Tarjeta que tiene el mouse encima (se dibuja con borde azul). null = ninguna.</summary>
        private Panel _tarjetaResaltada;

        /// <summary>Espacio libre entre el borde de la tarjeta y sus textos (así los textos no tapan el borde).</summary>
        private const int MargenTarjeta = 4;

        /// <summary>Color de fondo de la tarjeta mientras tiene el mouse encima (sale del tema: claro u oscuro).</summary>
        private static Color FondoTarjetaResaltada => Tema.FondoResaltado;

        /// <summary>Renglón de "Actividad reciente" que tiene el mouse encima (se dibuja subrayado). -1 = ninguno.</summary>
        private int _actividadResaltada = -1;

        /// <summary>
        /// Un renglón de la lista "Actividad reciente": el texto que se ve y el auto al que pertenece
        /// (para abrir su ficha con un clic). ToString es lo que dibuja la lista.
        /// </summary>
        private sealed class ItemActividad
        {
            public string Texto { get; init; }
            public int? IdVehiculo { get; init; }
            public override string ToString() => Texto;
        }

        // Inicio del mecánico (se arma por código en el constructor, así el tema y la traducción lo alcanzan)
        private readonly Panel pnlMecanico = new Panel();
        private readonly List<(Panel Tarjeta, Label Titulo, Label Dato, Label Aclaracion, Label Ver, Funcion Permiso)> _tarjetasMecanico =
            new List<(Panel, Label, Label, Label, Label, Funcion)>();
        private readonly List<(Button Boton, Func<bool> Visible)> _botonesMecanico = new List<(Button, Func<bool>)>();

        /// <summary>true si el usuario eligió "Cerrar sesión" (Program.cs vuelve a mostrar el login).</summary>
        public bool CerroSesion { get; private set; }

        public FrmPrincipal()
        {
            InitializeComponent();

            // Cada vez que cambia el tamaño de la ventana, las tarjetas se reparten la pantalla.
            Resize += (sender, e) => AcomodarInicio();

            PrepararActividad();
            CrearInicioMecanico();
            CrearMenuTema();
            CrearEncabezadoInicio();
            CrearPanelSistema();
        }

        // Saludo y "Actualizar" (arriba del inicio, para todos)

        private readonly Label lblSaludo = new Label();
        private readonly Button btnActualizarInicio = new Button();

        private void CrearEncabezadoInicio()
        {
            lblSaludo.Name = "lblSaludo";
            lblSaludo.AutoSize = false;
            lblSaludo.TextAlign = ContentAlignment.MiddleLeft;
            lblSaludo.Font = new Font("Segoe UI", 10.5F);
            pnlInicio.Controls.Add(lblSaludo);

            btnActualizarInicio.Name = "btnActualizarInicio";
            btnActualizarInicio.Text = "Actualizar";
            btnActualizarInicio.Click += (sender, e) => CargarInicio();
            pnlInicio.Controls.Add(btnActualizarInicio);
        }

        /// <summary>"Hola, Matías Montoto · domingo, 15 de noviembre de 2026" (en el idioma elegido).</summary>
        private void MostrarSaludo()
        {
            Usuario usuario = Sesion.UsuarioActual;
            if (usuario == null)
                return;
            string nombre = usuario.NombrePersona ?? usuario.NombreUsuario;
            string fecha = DateTime.Today.ToString("D", CultureInfo.CurrentCulture);
            lblSaludo.Text = $"{"Hola".Traducir()}, {nombre}  ·  {fecha}";
        }

        // Panel SISTEMA (solo para quien administra la seguridad)

        private readonly Panel pnlSistema = new Panel();
        private readonly Label lblSistemaTitulo = new Label();
        private readonly Label lblIntegridadIndicador = new Label();
        private readonly Label lblIntegridadDato = new Label();
        private readonly Label lblBackupDato = new Label();
        private readonly Label lblUsuariosDato = new Label();
        private readonly Label lblEventosTitulo = new Label();
        private readonly ListBox lstEventos = new ListBox();
        private readonly FlowLayoutPanel pnlAccesos = new FlowLayoutPanel();
        private readonly List<(Button Boton, Funcion Permiso)> _accesosSistema = new List<(Button, Funcion)>();
        private readonly IntegridadService _integridadService = new IntegridadService();
        private readonly BitacoraService _bitacoraService = new BitacoraService();

        /// <summary>
        /// Panel del administrador del sistema: integridad (DVH/DVV) con color, último backup, usuarios
        /// (activos, deshabilitados y solicitudes pendientes), últimos eventos de la bitácora y accesos rápidos.
        /// Todo sale de servicios que ya existen; cada dato y cada botón aparece solo con su permiso (Composite).
        /// </summary>
        private void CrearPanelSistema()
        {
            pnlSistema.Name = "pnlSistema";
            pnlSistema.BorderStyle = BorderStyle.FixedSingle;   // Tema lo dibuja como tarjeta
            pnlSistema.Visible = false;

            lblSistemaTitulo.Name = "lblSistemaTitulo";
            lblSistemaTitulo.Text = "SISTEMA";
            lblSistemaTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblSistemaTitulo.AutoSize = true;

            // Indicador de integridad: un círculo de color (verde = bien, rojo = cambios por fuera del sistema).
            lblIntegridadIndicador.Name = "lblIntegridadIndicador";
            lblIntegridadIndicador.Text = "●";
            lblIntegridadIndicador.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblIntegridadIndicador.AutoSize = true;

            foreach (Label dato in new[] { lblIntegridadDato, lblBackupDato, lblUsuariosDato })
            {
                dato.AutoSize = false;
                dato.Font = new Font("Segoe UI", 10.5F);
                dato.Cursor = Cursors.Hand;   // los datos llevan a la pantalla que corresponde
            }
            lblIntegridadDato.Name = "lblIntegridadDato";
            lblBackupDato.Name = "lblBackupDato";
            lblUsuariosDato.Name = "lblUsuariosDato";
            lblIntegridadDato.Click += (sender, e) => { if (Sesion.TienePermiso(Funcion.VerificarIntegridad)) mnuIntegridad_Click(sender, e); };
            lblIntegridadIndicador.Click += (sender, e) => { if (Sesion.TienePermiso(Funcion.VerificarIntegridad)) mnuIntegridad_Click(sender, e); };
            lblBackupDato.Click += (sender, e) => { if (Sesion.TienePermiso(Funcion.RealizarBackup)) mnuBackup_Click(sender, e); };
            lblUsuariosDato.Click += (sender, e) => { if (Sesion.TienePermiso(Funcion.GestionarUsuarios)) mnuUsuarios_Click(sender, e); };

            lblEventosTitulo.Name = "lblEventosTitulo";
            lblEventosTitulo.Text = "ÚLTIMOS EVENTOS DE LA BITÁCORA";
            lblEventosTitulo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblEventosTitulo.AutoSize = true;

            lstEventos.Name = "lstEventos";
            lstEventos.IntegralHeight = false;
            lstEventos.SelectionMode = SelectionMode.None;
            lstEventos.Cursor = Cursors.Hand;
            lstEventos.Click += (sender, e) => { if (Sesion.TienePermiso(Funcion.VerBitacora)) mnuBitacora_Click(sender, e); };

            pnlAccesos.Name = "pnlAccesos";
            pnlAccesos.WrapContents = true;
            AgregarAcceso("btnAccesoBackup", "Backup", Funcion.RealizarBackup, mnuBackup_Click);
            AgregarAcceso("btnAccesoBitacora", "Bitácora", Funcion.VerBitacora, mnuBitacora_Click);
            AgregarAcceso("btnAccesoPermisos", "Permisos", Funcion.GestionarPermisos, mnuPermisos_Click);
            AgregarAcceso("btnAccesoUsuarios", "Administrar usuarios", Funcion.GestionarUsuarios, mnuUsuarios_Click);
            AgregarAcceso("btnAccesoIntegridad", "Integridad", Funcion.VerificarIntegridad, mnuIntegridad_Click);

            pnlSistema.Controls.AddRange(new Control[] { lblSistemaTitulo, lblIntegridadIndicador, lblIntegridadDato, lblBackupDato,
                                                         lblUsuariosDato, lblEventosTitulo, lstEventos, pnlAccesos });
            pnlInicio.Controls.Add(pnlSistema);
        }

        private void AgregarAcceso(string nombre, string texto, Funcion permiso, EventHandler accion)
        {
            var boton = new Button { Name = nombre, Text = texto, AutoSize = true, Padding = new Padding(10, 2, 10, 2), Margin = new Padding(0, 0, 8, 8) };
            boton.Click += accion;
            pnlAccesos.Controls.Add(boton);
            _accesosSistema.Add((boton, permiso));
        }

        /// <summary>El panel SISTEMA se ve si el usuario tiene alguna de las funciones de Seguridad.</summary>
        private bool MostrarPanelSistema()
        {
            return Sesion.HayUsuarioLogueado && _inicioService.PuedeVerResumen()
                && _accesosSistema.Any(a => Sesion.TienePermiso(a.Permiso));
        }

        private void CargarPanelSistema()
        {
            bool mostrar = MostrarPanelSistema();
            pnlSistema.Visible = mostrar;
            if (!mostrar)
                return;

            foreach (var acceso in _accesosSistema)
                acceso.Boton.Visible = Sesion.TienePermiso(acceso.Permiso);

            // Integridad (DVH y DVV)
            try
            {
                List<string> problemas = _integridadService.Verificar();
                bool bien = problemas.Count == 0;
                lblIntegridadIndicador.ForeColor = bien ? Tema.Exito : Tema.Peligro;
                lblIntegridadDato.Text = bien
                    ? "Integridad: la base está íntegra (DVH y DVV correctos).".TraducirMensaje()
                    : $"Integridad: {problemas.Count} problema(s) detectado(s).".TraducirMensaje();
                lblIntegridadDato.ForeColor = bien ? Tema.Texto : Tema.Peligro;
            }
            catch (Exception ex)
            {
                lblIntegridadIndicador.ForeColor = Tema.TextoApagado;
                lblIntegridadDato.Text = "Integridad: no se pudo verificar.".TraducirMensaje();
                ex.Handle();
            }

            // Último backup y últimos eventos: salen de la bitácora (cada backup queda registrado ahí).
            if (Sesion.TienePermiso(Funcion.VerBitacora))
            {
                try
                {
                    List<LogEntry> backups = _bitacoraService.Listar(FiltroFechas.DesdePorDefecto, DateTime.Today, null, "", "Realizó un backup");
                    LogEntry ultimo = backups.OrderByDescending(b => b.TimeStamp).FirstOrDefault();
                    lblBackupDato.Text = ultimo == null
                        ? "Último backup: nunca (desde 2024).".TraducirMensaje()
                        : $"Último backup: {ultimo.TimeStamp:dd/MM/yyyy HH:mm}".TraducirMensaje();
                    lblBackupDato.ForeColor = ultimo == null || ultimo.TimeStamp < DateTime.Today.AddDays(-7) ? Tema.Peligro : Tema.Texto;

                    lstEventos.Items.Clear();
                    foreach (LogEntry evento in _bitacoraService.Listar(DateTime.Today.AddDays(-30), DateTime.Today, null, "", "")
                                                               .OrderByDescending(b => b.TimeStamp).Take(8))
                        lstEventos.Items.Add($"{evento.TimeStamp:dd/MM HH:mm}  {evento.Usuario}  {evento.Message.TraducirMensaje()}");
                    if (lstEventos.Items.Count == 0)
                        lstEventos.Items.Add("Sin eventos en los últimos 30 días.".Traducir());
                }
                catch (Exception ex)
                {
                    ex.Handle();
                }
            }
            lblBackupDato.Visible = lblEventosTitulo.Visible = lstEventos.Visible = Sesion.TienePermiso(Funcion.VerBitacora);

            // Usuarios: total, activos, deshabilitados y solicitudes de acceso pendientes
            // (una cuenta pedida desde el ingreso queda deshabilitada y nunca entró).
            if (Sesion.TienePermiso(Funcion.GestionarUsuarios))
            {
                try
                {
                    List<Usuario> usuarios = _usuarioService.Listar(true);
                    int activos = usuarios.Count(u => u.Activo);
                    int deshabilitados = usuarios.Count - activos;
                    int pendientes = usuarios.Count(u => !u.Activo && u.UltimoIngreso == null);
                    lblUsuariosDato.Text = $"Usuarios: {usuarios.Count} en total, {activos} activos, {deshabilitados} deshabilitados, {pendientes} solicitud(es) pendiente(s).".TraducirMensaje();
                    lblUsuariosDato.ForeColor = pendientes > 0 ? Tema.Peligro : Tema.Texto;
                }
                catch (Exception ex)
                {
                    ex.Handle();
                }
            }
            lblUsuariosDato.Visible = Sesion.TienePermiso(Funcion.GestionarUsuarios);
            AcomodarInicio();
        }

        /// <summary>Ubica los datos del panel SISTEMA según su tamaño.</summary>
        private void AcomodarPanelSistema()
        {
            // Medidas pensadas al 100 %: se multiplican por la escala de Windows (k).
            float k = DeviceDpi / 96f;
            int S(int valor) => (int)(valor * k);
            int ancho = pnlSistema.ClientSize.Width;
            int alto = pnlSistema.ClientSize.Height;
            int m = S(14);
            int renglon = S(26);
            lblSistemaTitulo.Location = new Point(m, S(10));
            int y = lblSistemaTitulo.Bottom + S(4);
            lblIntegridadIndicador.Location = new Point(m - S(3), y - S(4));
            lblIntegridadDato.SetBounds(m + S(22), y, ancho - 2 * m - S(22), renglon);
            y += renglon;
            foreach (Label dato in new[] { lblBackupDato, lblUsuariosDato })
            {
                if (!dato.Visible)
                    continue;
                dato.SetBounds(m, y, ancho - 2 * m, renglon);
                y += renglon;
            }

            // Los accesos rápidos pueden ocupar uno o dos renglones según el ancho.
            int altoAccesos = pnlAccesos.GetPreferredSize(new Size(ancho - 2 * m, 0)).Height;
            pnlAccesos.SetBounds(m, alto - altoAccesos - S(6), ancho - 2 * m, altoAccesos);
            int abajoEventos = pnlAccesos.Top - 8;
            if (lstEventos.Visible)
            {
                lblEventosTitulo.Location = new Point(m, y + 8);
                int arribaLista = lblEventosTitulo.Bottom + 4;
                lstEventos.SetBounds(m, arribaLista, ancho - 2 * m, Math.Max(40, abajoEventos - arribaLista));
            }
        }

        // Tema claro / oscuro

        private readonly ToolStripMenuItem mnuTema = new ToolStripMenuItem("Tema");
        private readonly ToolStripMenuItem mnuTemaClaro = new ToolStripMenuItem("Claro");
        private readonly ToolStripMenuItem mnuTemaOscuro = new ToolStripMenuItem("Oscuro");

        /// <summary>Sistema > Tema > Claro / Oscuro (debajo de Idioma). Se crea por código para no tocar el diseñador.</summary>
        private void CrearMenuTema()
        {
            mnuTema.Name = "mnuTema";
            mnuTemaClaro.Name = "mnuTemaClaro";
            mnuTemaOscuro.Name = "mnuTemaOscuro";
            mnuTemaClaro.Click += (sender, e) => ElegirTema(false);
            mnuTemaOscuro.Click += (sender, e) => ElegirTema(true);
            mnuTema.DropDownItems.AddRange(new ToolStripItem[] { mnuTemaClaro, mnuTemaOscuro });
            mnuSistema.DropDownItems.Insert(mnuSistema.DropDownItems.IndexOf(mnuIdioma) + 1, mnuTema);
            MarcarTema();
        }

        private void MarcarTema()
        {
            mnuTemaClaro.Checked = !Tema.Oscuro;
            mnuTemaOscuro.Checked = Tema.Oscuro;
        }

        /// <summary>Cambia el tema, repinta las ventanas abiertas y lo recuerda para la próxima vez (por usuario).</summary>
        private void ElegirTema(bool oscuro)
        {
            Tema.CambiarModo(oscuro);
            PreferenciasLocales.GuardarTema(Sesion.UsuarioActual?.NombreUsuario, oscuro);
            MarcarTema();
            lstActividad.Invalidate();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            MostrarUsuario();
            CargarIdiomas();
            AplicarPermisos();
            PrepararTarjetas();
            AcomodarInicio();
            CargarInicio();
        }

        // Tamaño de la pantalla de Inicio

        /// <summary>
        /// Reparte la pantalla de Inicio en todo el espacio de la ventana (antes quedaba un bloque
        /// fijo de 960 x 470 en el medio). Columnas: TALLER | INGRESOS | AGENCIA + ACTIVIDAD RECIENTE.
        /// TALLER e INGRESOS ocupan una cuarta parte cada una y la columna derecha, la mitad.
        /// </summary>
        private void AcomodarInicio()
        {
            if (pnlInicio == null || ClientSize.Width == 0)
                return;   // la ventana está minimizada

            const int margen = 24;
            const int separacion = 20;
            const int arribaTarjetas = 92;

            int arriba = menuPrincipal.Bottom + 10;
            int abajo = barraEstado.Top - 10;
            int ancho = Math.Max(900, ClientSize.Width - 2 * margen);
            int alto = Math.Max(450, abajo - arriba);

            pnlInicio.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pnlInicio.SetBounds(Math.Max(0, (ClientSize.Width - ancho) / 2), arriba, ancho, alto);
            lblTitulo.SetBounds(0, 0, ancho, 50);
            lblSaludo.SetBounds(0, 52, ancho - 160, 28);
            btnActualizarInicio.SetBounds(ancho - 140, 50, 140, 32);

            int columna = (ancho - 2 * separacion) / 4;
            int altoTarjetas = alto - arribaTarjetas - 5;
            // Si se ve el panel SISTEMA, TALLER e INGRESOS se achican y el panel ocupa el espacio de abajo.
            int altoIzquierda = MostrarPanelSistema() ? Math.Max(170, altoTarjetas * 36 / 100) : altoTarjetas;
            pnlTaller.SetBounds(0, arribaTarjetas, columna, altoIzquierda);
            pnlIngresos.SetBounds(columna + separacion, arribaTarjetas, columna, altoIzquierda);
            if (MostrarPanelSistema())
            {
                pnlSistema.SetBounds(0, arribaTarjetas + altoIzquierda + separacion, 2 * columna + separacion,
                                     altoTarjetas - altoIzquierda - separacion);
                AcomodarPanelSistema();
            }

            int xDerecha = 2 * (columna + separacion);
            int anchoDerecha = ancho - xDerecha;
            int altoAgencia = Math.Max(160, altoTarjetas * 38 / 100);
            pnlAgencia.SetBounds(xDerecha, arribaTarjetas, anchoDerecha, altoAgencia);

            lblActividadTitulo.Location = new Point(xDerecha, arribaTarjetas + altoAgencia + 15);
            int arribaLista = lblActividadTitulo.Bottom + 5;
            lstActividad.SetBounds(xDerecha, arribaLista, anchoDerecha, arribaTarjetas + altoTarjetas - arribaLista);

            AcomodarInicioMecanico(ancho, alto, arribaTarjetas);

            AcomodarTarjeta(pnlTaller, lblTallerTitulo, lblTallerDato, null, lblTallerVer);
            AcomodarTarjeta(pnlIngresos, lblIngresosTitulo, lblIngresosDato, lblIngresosAclaracion, lblIngresosVer);
            AcomodarTarjeta(pnlAgencia, lblAgenciaTitulo, lblAgenciaDato, null, lblAgenciaVer);
        }

        /// <summary>
        /// Acomoda los textos de una tarjeta a su nuevo tamaño. Dejamos un margen de 4 píxeles
        /// a cada lado: antes los textos iban de borde a borde y tapaban el borde azul
        /// que se dibuja al pasar el mouse (por eso la tarjeta se veía "recortada").
        /// </summary>
        private static void AcomodarTarjeta(Panel tarjeta, Label titulo, Label dato, Label aclaracion, Label ver)
        {
            int ancho = tarjeta.ClientSize.Width - 2 * MargenTarjeta;

            titulo.SetBounds(MargenTarjeta, 25, ancho, 35);
            dato.SetBounds(MargenTarjeta, 85, ancho, 25);
            aclaracion?.SetBounds(MargenTarjeta, 110, ancho, 20);
            ver.SetBounds(MargenTarjeta, tarjeta.ClientSize.Height - 40, ancho, 22);
        }

        /// <summary>Barra de abajo: "Usuario: admin | Rol: Administrador" (traducido).</summary>
        private void MostrarUsuario()
        {
            var usuario = Sesion.UsuarioActual;
            lblUsuarioActual.Text = $"{"Usuario".Traducir()}: {usuario.NombreUsuario}  |  " +
                                    $"{"Rol".Traducir()}: {Sesion.TextoRol(usuario.Rol).Traducir()}";
        }

        // Idioma

        /// <summary>
        /// Arma el menú Sistema > Idioma con un ítem por cada archivo de la carpeta I18n
        /// (IdiomaService.ObtenerIdiomas()). Para sumar un idioma
        /// nuevo alcanza con agregar el archivo, sin tocar código.
        /// El idioma actual queda tildado.
        /// </summary>
        private void CargarIdiomas()
        {
            mnuIdioma.DropDownItems.Clear();

            try
            {
                foreach (CultureInfo idioma in IdiomaService.ObtenerIdiomas())
                {
                    // NativeName es el nombre del idioma en su propio idioma ("English (United States)").
                    var opcion = new ToolStripMenuItem(idioma.NativeName)
                    {
                        Tag = idioma,
                        Checked = idioma.Name == IdiomaService.IdiomaActual().Name
                    };
                    opcion.Click += Idioma_Click;
                    mnuIdioma.DropDownItems.Add(opcion);
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void Idioma_Click(object sender, EventArgs e)
        {
            var idioma = (CultureInfo)((ToolStripMenuItem)sender).Tag;

            try
            {
                // 1) Cambiamos el idioma  2) lo guardamos como preferido del usuario
                IdiomaService.CambiarIdioma(idioma.Name);
                _usuarioService.CambiarIdioma(idioma.Name);

                // 3) Volvemos a traducir esta pantalla (las demás se traducen solas al abrirse)
                Traductor.Traducir(this);
                MostrarUsuario();
                CargarIdiomas();
                CargarInicio();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        // Pantalla de Inicio

        /// <summary>
        /// Pide el resumen a la BLL y lo muestra en los recuadros.
        /// Se llama al abrir el sistema y cada vez que se cierra otra pantalla,
        /// así los números quedan al día (ej: después de registrar una compra).
        /// </summary>
        private void CargarInicio()
        {
            // El mecánico no ve los números del negocio: ve SU inicio (tarjetas y botones según sus permisos).
            bool puedeVer = _inicioService.PuedeVerResumen();
            pnlTaller.Visible = puedeVer;
            pnlIngresos.Visible = puedeVer;
            pnlAgencia.Visible = puedeVer;
            lblActividadTitulo.Visible = puedeVer;
            lstActividad.Visible = puedeVer;
            pnlMecanico.Visible = !puedeVer;
            MostrarSaludo();

            if (!puedeVer)
            {
                CargarInicioMecanico();
                return;
            }

            try
            {
                CargarPanelSistema();
                ResumenInicio resumen = _inicioService.ObtenerResumen();

                lblTallerDato.Text = $"{"Vehículos en taller".Traducir()}: {resumen.VehiculosEnTaller}";
                lblIngresosDato.Text = $"{"Chocados ingresados".Traducir()}: {resumen.ChocadosIngresadosUltimoAnio}";
                lblAgenciaDato.Text = $"{"Ventas (últimos 12 meses)".Traducir()}: {resumen.VentasUltimoAnio}";

                lstActividad.Items.Clear();
                _actividadResaltada = -1;
                if (resumen.Actividades.Count == 0)
                {
                    lstActividad.Items.Add(new ItemActividad { Texto = "Todavía no hay movimientos.".Traducir() });
                }
                else
                {
                    foreach (ActividadReciente actividad in resumen.Actividades)
                    {
                        lstActividad.Items.Add(new ItemActividad
                        {
                            Texto = $"{TextoFecha(actividad.Fecha)} - {actividad.Descripcion.TraducirMensaje()}",
                            IdVehiculo = actividad.IdVehiculo
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>
        /// Convierte una fecha en texto corto: "Hoy", "Ayer" o "25/09/2026".
        /// Nunca se muestra la hora: algunos movimientos se guardan con hora y otros no,
        /// y mezclados quedaba desprolijo ("26/09 19:01" al lado de "25/09").
        /// </summary>
        private static string TextoFecha(DateTime fecha)
        {
            if (fecha.Date == DateTime.Today)
                return "Hoy".Traducir();
            if (fecha.Date == DateTime.Today.AddDays(-1))
                return "Ayer".Traducir();
            return fecha.ToString("dd/MM/yyyy");
        }

        // Actividad reciente clickeable

        /// <summary>
        /// La lista se dibuja "a mano" (OwnerDraw) para que cada renglón parezca un link:
        /// manito y subrayado al pasar el mouse, y al hacer clic se abre la ficha del auto.
        /// </summary>
        private void PrepararActividad()
        {
            lstActividad.DrawMode = DrawMode.OwnerDrawFixed;
            lstActividad.ItemHeight = 26;
            lstActividad.DrawItem += lstActividad_DrawItem;
            lstActividad.MouseMove += lstActividad_MouseMove;
            lstActividad.MouseLeave += (sender, e) => ResaltarActividad(-1);
            lstActividad.MouseClick += lstActividad_MouseClick;
        }

        private void lstActividad_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            var item = (ItemActividad)lstActividad.Items[e.Index];
            bool resaltado = e.Index == _actividadResaltada && item.IdVehiculo.HasValue;

            using (var fondo = new SolidBrush(resaltado ? Tema.Seleccion : lstActividad.BackColor))
                e.Graphics.FillRectangle(fondo, e.Bounds);

            // Los renglones con auto van en azul (como un link); el aviso "no hay movimientos", en gris.
            Color color = item.IdVehiculo.HasValue ? Tema.Acento : Tema.TextoApagado;
            FontStyle estilo = resaltado ? FontStyle.Underline : FontStyle.Regular;
            using (var fuente = new Font(lstActividad.Font, estilo))
            {
                var area = new Rectangle(e.Bounds.X + 6, e.Bounds.Y, e.Bounds.Width - 6, e.Bounds.Height);
                TextRenderer.DrawText(e.Graphics, item.Texto, fuente, area, color,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            }
        }

        private void lstActividad_MouseMove(object sender, MouseEventArgs e)
        {
            ResaltarActividad(lstActividad.IndexFromPoint(e.Location));
        }

        /// <summary>Cambia el renglón resaltado y el cursor (manito solo sobre renglones con auto).</summary>
        private void ResaltarActividad(int indice)
        {
            bool tieneAuto = indice >= 0 && indice < lstActividad.Items.Count
                             && ((ItemActividad)lstActividad.Items[indice]).IdVehiculo.HasValue;
            if (!tieneAuto)
                indice = -1;

            lstActividad.Cursor = tieneAuto ? Cursors.Hand : Cursors.Default;
            if (indice == _actividadResaltada)
                return;

            _actividadResaltada = indice;
            lstActividad.Invalidate();
        }

        private void lstActividad_MouseClick(object sender, MouseEventArgs e)
        {
            int indice = lstActividad.IndexFromPoint(e.Location);
            if (indice < 0 || indice >= lstActividad.Items.Count)
                return;

            if (lstActividad.Items[indice] is ItemActividad { IdVehiculo: int idVehiculo })
                AbrirPantalla(new FrmFichaVehiculo(idVehiculo));
        }

        /// <summary>
        /// Abre una pantalla como ventana modal (hay que cerrarla para volver al menú)
        /// y al cerrarla actualiza los números del Inicio.
        /// </summary>
        private void AbrirPantalla(Form pantalla)
        {
            using (pantalla)
            {
                pantalla.ShowDialog(this);
            }
            CargarInicio();
        }

        // Tarjetas que funcionan como botones

        /// <summary>
        /// Convierte los tres recuadros en "botones": manito al pasar el mouse,
        /// borde azul resaltado y, al hacer clic, se abre el detalle de esa tarjeta.
        /// El Tag guarda qué tarjeta es (Taller / Ingresos / Agencia).
        /// </summary>
        private void PrepararTarjetas()
        {
            PrepararTarjeta(pnlTaller, TipoDetalleInicio.Taller);
            PrepararTarjeta(pnlIngresos, TipoDetalleInicio.Ingresos);
            PrepararTarjeta(pnlAgencia, TipoDetalleInicio.Agencia);

            foreach (Label ver in new[] { lblTallerVer, lblIngresosVer, lblAgenciaVer })
                ver.ForeColor = Tema.Acento;
        }

        private void PrepararTarjeta(Panel tarjeta, TipoDetalleInicio tipo)
        {
            tarjeta.Tag = tipo;
            tarjeta.Cursor = Cursors.Hand;
            tarjeta.Click += Tarjeta_Click;
            tarjeta.MouseEnter += Tarjeta_MouseEnter;
            tarjeta.MouseLeave += Tarjeta_MouseLeave;
            tarjeta.Paint += Tarjeta_Paint;

            // Si tocás el texto de adentro ("TALLER" o el número) abre lo mismo.
            foreach (Control hijo in tarjeta.Controls)
            {
                hijo.Cursor = Cursors.Hand;
                hijo.Click += Tarjeta_Click;
                hijo.MouseEnter += Tarjeta_MouseEnter;
                hijo.MouseLeave += Tarjeta_MouseLeave;
            }
        }

        /// <summary>Si el clic vino de un texto de adentro, devuelve el recuadro que lo contiene.</summary>
        private static Panel TarjetaDe(object sender)
        {
            return sender as Panel ?? (Panel)((Control)sender).Parent;
        }

        private void Tarjeta_Click(object sender, EventArgs e)
        {
            Panel tarjeta = TarjetaDe(sender);
            var tipo = (TipoDetalleInicio)tarjeta.Tag;

            _tarjetaResaltada = null;
            tarjeta.BackColor = Tema.Superficie;
            tarjeta.Invalidate();

            AbrirPantalla(new FrmDetalleInicio(tipo));
        }

        private void Tarjeta_MouseEnter(object sender, EventArgs e)
        {
            Panel tarjeta = TarjetaDe(sender);
            if (_tarjetaResaltada == tarjeta)
                return;

            _tarjetaResaltada = tarjeta;
            tarjeta.BackColor = FondoTarjetaResaltada;   // los textos de adentro toman el mismo color
            tarjeta.Invalidate();   // Invalidate hace que se vuelva a dibujar
        }

        private void Tarjeta_MouseLeave(object sender, EventArgs e)
        {
            Panel tarjeta = TarjetaDe(sender);

            // Pasar del recuadro a un texto de adentro también dispara MouseLeave,
            // así que solo apagamos el resaltado si el mouse salió de verdad.
            bool sigueAdentro = tarjeta.ClientRectangle.Contains(tarjeta.PointToClient(Cursor.Position));
            if (sigueAdentro)
                return;

            _tarjetaResaltada = null;
            tarjeta.BackColor = Tema.Superficie;
            tarjeta.Invalidate();
        }

        /// <summary>Dibuja un borde azul más grueso en la tarjeta que tiene el mouse encima.</summary>
        private void Tarjeta_Paint(object sender, PaintEventArgs e)
        {
            var tarjeta = (Panel)sender;
            if (tarjeta != _tarjetaResaltada)
                return;

            using (var lapiz = new Pen(Tema.Primario, 2))
                e.Graphics.DrawRectangle(lapiz, 1, 1, tarjeta.Width - 2, tarjeta.Height - 2);
        }

        // Inicio del mecánico

        /// <summary>
        /// Arma el Inicio del mecánico: 4 tarjetas (con cantidad y "Ver detalle") y botones grandes
        /// a sus funciones. Se crea siempre (oculto) y se muestra solo a quien no ve el resumen del negocio.
        /// Las tarjetas y los botones se muestran según los PERMISOS del usuario (Composite: Sesion/Funcion),
        /// no según el rol: si el administrador le saca una patente, el botón desaparece.
        /// </summary>
        private void CrearInicioMecanico()
        {
            pnlMecanico.Name = "pnlMecanico";
            pnlMecanico.Visible = false;
            pnlInicio.Controls.Add(pnlMecanico);

            AgregarTarjetaMecanico("pnlMecDiagnostico", "MIS AUTOS EN DIAGNÓSTICO", "(esperando diagnóstico en el taller)",
                TipoDetalleInicio.MecanicoDiagnostico, Funcion.ConsultarDiagnostico);
            AgregarTarjetaMecanico("pnlMecOrdenes", "MIS ÓRDENES DE TRABAJO", "(abiertas, con tareas mías)",
                TipoDetalleInicio.MecanicoOrdenes, Funcion.ConsultarOrdenesTrabajo);
            AgregarTarjetaMecanico("pnlMecPedidos", "PEDIDOS DE REPUESTOS PENDIENTES", "(de mis órdenes)",
                TipoDetalleInicio.MecanicoPedidos, Funcion.ConsultarPedidosRepuestos);
            AgregarTarjetaMecanico("pnlMecTareas", "TAREAS REALIZADAS ESTE MES", "(terminadas por mí)",
                TipoDetalleInicio.MecanicoTareas, Funcion.RegistrarTareasRealizadas);

            // Botones grandes: cada uno aparece solo si el usuario tiene el permiso correspondiente.
            AgregarBotonMecanico("btnMecDiagnosticos", "Diagnósticos",
                () => Sesion.TienePermiso(Funcion.ConsultarDiagnostico) || Sesion.TienePermiso(Funcion.RegistrarDiagnostico),
                () => AbrirPantalla(new FrmDiagnosticos(!Sesion.TienePermiso(Funcion.RegistrarDiagnostico))));
            AgregarBotonMecanico("btnMecOrdenes", "Órdenes de trabajo",
                () => Sesion.TienePermiso(Funcion.ConsultarOrdenesTrabajo) || Sesion.TienePermiso(Funcion.GestionarOrdenesTrabajo),
                () => AbrirPantalla(new FrmOrdenesTrabajo(Sesion.TienePermiso(Funcion.GestionarOrdenesTrabajo))));
            AgregarBotonMecanico("btnMecTareas", "Tareas realizadas",
                () => Sesion.TienePermiso(Funcion.RegistrarTareasRealizadas),
                () => AbrirPantalla(new FrmTareasRealizadas()));
            AgregarBotonMecanico("btnMecPedidos", "Pedidos de repuestos",
                () => Sesion.TienePermiso(Funcion.RegistrarPedidoRepuestos) || Sesion.TienePermiso(Funcion.ConsultarPedidosRepuestos),
                () => AbrirPantalla(new FrmPedidos(Sesion.TienePermiso(Funcion.RegistrarPedidoRepuestos)
                    ? ModoPedidos.Registrar : ModoPedidos.Consulta)));
            AgregarBotonMecanico("btnMecPerfil", "Mi perfil", () => true, () => mnuMiPerfil_Click(this, EventArgs.Empty));
            AgregarBotonMecanico("btnMecAyuda", "Ayuda", () => true, () => AyudaUI.Mostrar(this));

            // Listas con lo suyo (cada renglón abre la ficha del auto). Mismos permisos que las tarjetas.
            AgregarListaMecanico("MIS AUTOS ESPERANDO DIAGNÓSTICO", Funcion.ConsultarDiagnostico, () => _inicioService.ObtenerDetalleAutosEnDiagnostico());
            AgregarListaMecanico("MIS ÓRDENES ABIERTAS", Funcion.ConsultarOrdenesTrabajo, () => _inicioService.ObtenerDetalleMisOrdenes());
            AgregarListaMecanico("ÚLTIMAS TAREAS REALIZADAS", Funcion.RegistrarTareasRealizadas, () => _inicioService.ObtenerDetalleMisTareas());
            AgregarListaMecanico("PEDIDOS DE REPUESTOS PENDIENTES", Funcion.ConsultarPedidosRepuestos, () => _inicioService.ObtenerDetalleMisPedidos());
        }

        private readonly List<(Label Titulo, ListBox Lista, Funcion Permiso, Func<List<DetalleInicio>> Datos)> _listasMecanico =
            new List<(Label, ListBox, Funcion, Func<List<DetalleInicio>>)>();

        private void AgregarListaMecanico(string titulo, Funcion permiso, Func<List<DetalleInicio>> datos)
        {
            int numero = _listasMecanico.Count + 1;
            var lblTituloLista = new Label { Name = "lblListaMec" + numero + "Titulo", Text = titulo, AutoSize = false, Font = new Font("Segoe UI", 9.75F, FontStyle.Bold) };
            var lista = new ListBox { Name = "lstMec" + numero, IntegralHeight = false, SelectionMode = SelectionMode.None };
            PrepararListaConLinks(lista);
            pnlMecanico.Controls.Add(lblTituloLista);
            pnlMecanico.Controls.Add(lista);
            _listasMecanico.Add((lblTituloLista, lista, permiso, datos));
        }

        // Listas con renglones "link" (los usan las listas del mecánico): manito, subrayado y clic = ficha del auto.

        private readonly Dictionary<ListBox, int> _renglonResaltado = new Dictionary<ListBox, int>();

        private void PrepararListaConLinks(ListBox lista)
        {
            lista.DrawMode = DrawMode.OwnerDrawFixed;
            lista.ItemHeight = 24;
            _renglonResaltado[lista] = -1;
            lista.DrawItem += ListaLink_DrawItem;
            lista.MouseMove += (sender, e) => ResaltarRenglon(lista, lista.IndexFromPoint(e.Location));
            lista.MouseLeave += (sender, e) => ResaltarRenglon(lista, -1);
            lista.MouseClick += (sender, e) =>
            {
                int indice = lista.IndexFromPoint(e.Location);
                if (indice >= 0 && indice < lista.Items.Count && lista.Items[indice] is ItemActividad { IdVehiculo: int idVehiculo })
                    AbrirPantalla(new FrmFichaVehiculo(idVehiculo));
            };
        }

        private void ListaLink_DrawItem(object sender, DrawItemEventArgs e)
        {
            var lista = (ListBox)sender;
            if (e.Index < 0 || e.Index >= lista.Items.Count)
                return;

            var item = (ItemActividad)lista.Items[e.Index];
            bool resaltado = e.Index == _renglonResaltado[lista] && item.IdVehiculo.HasValue;
            using (var fondo = new SolidBrush(resaltado ? Tema.Seleccion : lista.BackColor))
                e.Graphics.FillRectangle(fondo, e.Bounds);

            Color color = item.IdVehiculo.HasValue ? Tema.Acento : Tema.TextoApagado;
            using var fuente = new Font(lista.Font, resaltado ? FontStyle.Underline : FontStyle.Regular);
            var area = new Rectangle(e.Bounds.X + 6, e.Bounds.Y, e.Bounds.Width - 6, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, item.Texto, fuente, area, color,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        private void ResaltarRenglon(ListBox lista, int indice)
        {
            bool tieneAuto = indice >= 0 && indice < lista.Items.Count && ((ItemActividad)lista.Items[indice]).IdVehiculo.HasValue;
            if (!tieneAuto)
                indice = -1;
            lista.Cursor = tieneAuto ? Cursors.Hand : Cursors.Default;
            if (_renglonResaltado[lista] == indice)
                return;
            _renglonResaltado[lista] = indice;
            lista.Invalidate();
        }

        /// <summary>Llena las listas del mecánico: "fecha · patente · auto · dato", hasta 15 renglones cada una.</summary>
        private void CargarListasMecanico()
        {
            foreach (var l in _listasMecanico)
            {
                bool ver = MostrarTarjetaMecanico(l.Permiso);
                l.Titulo.Visible = l.Lista.Visible = ver;
                if (!ver)
                    continue;

                l.Lista.Items.Clear();
                try
                {
                    foreach (DetalleInicio d in l.Datos().Take(15))
                    {
                        string dato = string.IsNullOrWhiteSpace(d.Persona) ? d.Extra.TraducirValor() : d.Persona;
                        l.Lista.Items.Add(new ItemActividad
                        {
                            Texto = $"{d.Fecha:dd/MM/yyyy}  ·  {d.Dominio}  ·  {d.Vehiculo}" + (string.IsNullOrWhiteSpace(dato) ? "" : "  ·  " + dato),
                            IdVehiculo = d.IdVehiculo
                        });
                    }
                }
                catch (Exception ex)
                {
                    ex.Handle();
                }
                if (l.Lista.Items.Count == 0)
                    l.Lista.Items.Add(new ItemActividad { Texto = "Sin registros.".Traducir() });
            }
        }

        private void AgregarTarjetaMecanico(string nombre, string titulo, string aclaracion, TipoDetalleInicio tipo, Funcion permiso)
        {
            // BorderStyle FixedSingle = Tema la dibuja como tarjeta (fondo blanco y franja azul arriba).
            var tarjeta = new Panel { Name = nombre, BorderStyle = BorderStyle.FixedSingle };
            var lblTitulo = new Label
            {
                Name = nombre + "Titulo", Text = titulo, TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };
            var lblDato = new Label
            {
                Name = nombre + "Dato", Text = "0", TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 28F, FontStyle.Bold), ForeColor = Tema.Acento
            };
            var lblAclaracion = new Label
            {
                Name = nombre + "Aclaracion", Text = aclaracion, TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Tema.TextoApagado
            };
            var lblVer = new Label { Name = nombre + "Ver", Text = "Ver detalle ›", TextAlign = ContentAlignment.MiddleCenter };

            tarjeta.Controls.AddRange(new Control[] { lblTitulo, lblDato, lblAclaracion, lblVer });
            pnlMecanico.Controls.Add(tarjeta);
            PrepararTarjeta(tarjeta, tipo);   // misma manito, resaltado y clic que las tarjetas del administrador
            lblVer.ForeColor = Tema.Acento;
            lblVer.Font = new Font(lblVer.Font, FontStyle.Underline);

            _tarjetasMecanico.Add((tarjeta, lblTitulo, lblDato, lblAclaracion, lblVer, permiso));
        }

        private void AgregarBotonMecanico(string nombre, string texto, Func<bool> visible, Action accion)
        {
            var boton = new Button
            {
                Name = nombre, Text = texto, Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            boton.Click += (sender, e) => accion();
            pnlMecanico.Controls.Add(boton);
            _botonesMecanico.Add((boton, visible));
        }

        /// <summary>Números de las tarjetas y qué tarjetas/botones se ven según los permisos.</summary>
        private void CargarInicioMecanico()
        {
            foreach (var t in _tarjetasMecanico)
                t.Tarjeta.Visible = MostrarTarjetaMecanico(t.Permiso);
            foreach (var b in _botonesMecanico)
                b.Boton.Visible = b.Visible();

            if (_inicioService.EsInicioDeMecanico())
            {
                try
                {
                    ResumenMecanico resumen = _inicioService.ObtenerResumenMecanico();
                    int[] valores = { resumen.AutosEnDiagnostico, resumen.OrdenesAbiertas, resumen.PedidosPendientes, resumen.TareasRealizadasMes };
                    for (int i = 0; i < _tarjetasMecanico.Count; i++)
                        _tarjetasMecanico[i].Dato.Text = valores[i].ToString();
                }
                catch (Exception ex)
                {
                    Mensajes.Error(ex);
                }
                CargarListasMecanico();
            }

            AcomodarInicio();
        }

        /// <summary>Una tarjeta del mecánico se ve si el usuario tiene un mecánico vinculado y el permiso de esa tarjeta.</summary>
        private bool MostrarTarjetaMecanico(Funcion permiso)
        {
            return Sesion.HayUsuarioLogueado && _inicioService.EsInicioDeMecanico() && Sesion.TienePermiso(permiso);
        }

        /// <summary>
        /// Reparte el Inicio del mecánico: arriba las tarjetas visibles en una fila;
        /// abajo los botones visibles en una grilla de 3 columnas.
        /// </summary>
        private void AcomodarInicioMecanico(int ancho, int alto, int arriba)
        {
            pnlMecanico.SetBounds(0, arriba, ancho, alto - arriba);

            // Medidas pensadas al 100 %: se multiplican por la escala de Windows (k).
            float k = DeviceDpi / 96f;
            int S(int valor) => (int)(valor * k);
            int separacion = S(16);

            // 1) Tarjetas: el título se mide (puede ocupar 1 o 2 renglones) y el número va debajo, sin taparlo.
            var tarjetas = _tarjetasMecanico.Where(t => MostrarTarjetaMecanico(t.Permiso)).ToList();
            int altoTarjetas = 0;
            if (tarjetas.Count > 0)
            {
                int anchoTarjeta = (ancho - (tarjetas.Count - 1) * separacion) / tarjetas.Count;
                int interior = anchoTarjeta - 2 * MargenTarjeta;
                int altoTitulo = tarjetas.Max(t => TextRenderer.MeasureText(t.Titulo.Text, t.Titulo.Font,
                    new Size(interior, 0), TextFormatFlags.WordBreak).Height) + S(4);
                altoTarjetas = S(14) + altoTitulo + S(52) + S(40) + S(36);
                for (int i = 0; i < tarjetas.Count; i++)
                {
                    var t = tarjetas[i];
                    t.Tarjeta.SetBounds(i * (anchoTarjeta + separacion), 0, anchoTarjeta, altoTarjetas);
                    t.Titulo.SetBounds(MargenTarjeta, S(14), interior, altoTitulo);
                    t.Dato.SetBounds(MargenTarjeta, t.Titulo.Bottom, interior, S(52));
                    t.Aclaracion.SetBounds(MargenTarjeta, t.Dato.Bottom, interior, S(40));   // puede ocupar 2 renglones
                    t.Ver.SetBounds(MargenTarjeta, altoTarjetas - S(32), interior, S(24));
                }
            }

            // 2) Botones a sus funciones, todos en una fila.
            // (Se pregunta por el permiso y no por Visible: Visible da false mientras la ventana no se mostró.)
            var botones = _botonesMecanico.Where(b => b.Visible()).Select(b => b.Boton).ToList();
            int arribaBotones = tarjetas.Count > 0 ? altoTarjetas + separacion : 0;
            int altoBoton = S(52);
            if (botones.Count > 0)
            {
                int anchoBoton = (ancho - (botones.Count - 1) * S(10)) / botones.Count;
                for (int i = 0; i < botones.Count; i++)
                {
                    botones[i].Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                    botones[i].SetBounds(i * (anchoBoton + S(10)), arribaBotones, anchoBoton, altoBoton);
                }
            }

            // 3) Listas en una grilla de 2 x 2 con todo el alto que queda.
            var listas = _listasMecanico.Where(l => MostrarTarjetaMecanico(l.Permiso)).ToList();
            if (listas.Count == 0)
                return;
            int arribaListas = arribaBotones + (botones.Count > 0 ? altoBoton + separacion : 0);
            int columnas = listas.Count == 1 ? 1 : 2;
            int filas = (listas.Count + columnas - 1) / columnas;
            int anchoLista = (ancho - (columnas - 1) * separacion) / columnas;
            int altoCelda = Math.Max(S(110), (pnlMecanico.Height - arribaListas - (filas - 1) * separacion) / filas);
            for (int i = 0; i < listas.Count; i++)
            {
                int x = (i % columnas) * (anchoLista + separacion);
                int y = arribaListas + (i / columnas) * (altoCelda + separacion);
                listas[i].Titulo.SetBounds(x, y, anchoLista, S(22));
                listas[i].Lista.ItemHeight = S(24);
                listas[i].Lista.SetBounds(x, y + S(24), anchoLista, altoCelda - S(24));
            }
        }

        // Permisos

        /// <summary>
        /// Muestra u oculta cada opción según los permisos del usuario (patrón Composite:
        /// Sesion.TienePermiso recorre sus familias y patentes).
        /// Se usa "Available" y no "Visible" porque Visible devuelve false mientras el menú está cerrado.
        /// </summary>
        private void AplicarPermisos()
        {
            // Seguridad
            mnuUsuarios.Available = Sesion.TienePermiso(Funcion.GestionarUsuarios);
            mnuPermisos.Available = Sesion.TienePermiso(Funcion.GestionarPermisos);
            mnuBitacora.Available = Sesion.TienePermiso(Funcion.VerBitacora);
            mnuBackup.Available = Sesion.TienePermiso(Funcion.RealizarBackup);
            mnuIntegridad.Available = Sesion.TienePermiso(Funcion.VerificarIntegridad);

            // Maestros
            mnuClientes.Available = Sesion.TienePermiso(Funcion.GestionarClientes);
            mnuMecanicos.Available = Sesion.TienePermiso(Funcion.GestionarMecanicos);
            mnuProveedores.Available = Sesion.TienePermiso(Funcion.GestionarProveedores);
            mnuAgencias.Available = Sesion.TienePermiso(Funcion.GestionarAgencias);

            // Compras y vehículos
            mnuRegistrarCompra.Available = Sesion.TienePermiso(Funcion.GestionarCompras);
            mnuConsultarCompras.Available = Sesion.TienePermiso(Funcion.GestionarCompras);
            mnuVehiculos.Available = Sesion.TienePermiso(Funcion.GestionarVehiculos);

            // Taller
            mnuRecepcion.Available = Sesion.TienePermiso(Funcion.RegistrarRecepcion);
            mnuRegistrarDiagnostico.Available = Sesion.TienePermiso(Funcion.RegistrarDiagnostico);
            mnuConsultarDiagnostico.Available = Sesion.TienePermiso(Funcion.ConsultarDiagnostico);
            mnuPresupuestos.Available = Sesion.TienePermiso(Funcion.GestionarPresupuestos);
            mnuOrdenesTrabajo.Available = Sesion.TienePermiso(Funcion.GestionarOrdenesTrabajo);
            mnuConsultarOrdenesTrabajo.Available = Sesion.TienePermiso(Funcion.ConsultarOrdenesTrabajo);
            mnuTareasRealizadas.Available = Sesion.TienePermiso(Funcion.RegistrarTareasRealizadas);

            // Repuestos
            mnuRegistrarPedido.Available = Sesion.TienePermiso(Funcion.RegistrarPedidoRepuestos);
            mnuEntregaPedido.Available = Sesion.TienePermiso(Funcion.RegistrarEntregaPedido);
            mnuConsultarPedidos.Available = Sesion.TienePermiso(Funcion.ConsultarPedidosRepuestos);

            // Comercial
            mnuConsignaciones.Available = Sesion.TienePermiso(Funcion.GestionarConsignaciones);
            mnuPublicaciones.Available = Sesion.TienePermiso(Funcion.GestionarPublicaciones);
            mnuVentas.Available = Sesion.TienePermiso(Funcion.GestionarVentas);

            // Reportes
            bool verReportes = Sesion.TienePermiso(Funcion.VerReportes);
            mnuReporteVehiculos.Available = verReportes;
            mnuReporteReparaciones.Available = verReportes;
            mnuReportePresupuestos.Available = verReportes;
            mnuReporteVentas.Available = verReportes;

            // Si un menú de arriba (por ejemplo Seguridad) se queda sin opciones, lo ocultamos.
            foreach (ToolStripMenuItem menu in menuPrincipal.Items)
            {
                if (menu == mnuSistema)
                    continue;
                menu.Available = TieneOpcionesVisibles(menu);
            }
        }

        private static bool TieneOpcionesVisibles(ToolStripMenuItem menu)
        {
            foreach (ToolStripItem opcion in menu.DropDownItems)
                if (opcion.Available)
                    return true;
            return false;
        }

        // Sistema

        private void mnuCerrarSesion_Click(object sender, EventArgs e)
        {
            if (!Mensajes.Confirmar("¿Desea cerrar la sesión?".Traducir()))
                return;

            _usuarioService.CerrarSesion();
            CerroSesion = true;
            Close();
        }

        private void mnuSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Mi perfil: cambiar mi contraseña, mi pregunta de seguridad y mi idioma.
        /// Al volver, se traduce de nuevo el menú por si cambió el idioma.
        /// </summary>
        private void mnuMiPerfil_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmMiPerfil())
                frm.ShowDialog(this);

            Traductor.Traducir(this);
            MostrarUsuario();
            CargarIdiomas();
            MarcarTema();   // por si cambió el tema desde Mi perfil
            CargarInicio();
        }

        private void mnuPedirAyuda_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmPedirAyuda());
        }

        /// <summary>Abre el índice de la ayuda (archivo PRAS.chm hecho con HTML Help Workshop).</summary>
        private void mnuAyuda_Click(object sender, EventArgs e)
        {
            AyudaUI.Mostrar(this);
        }

        // Seguridad

        private void mnuUsuarios_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmUsuarios());
        }

        private void mnuPermisos_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmPermisos());
        }

        private void mnuBitacora_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmBitacora());
        }

        private void mnuIntegridad_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmIntegridad(null));
        }

        /// <summary>
        /// Backup y restore. Si se restauró un backup, la base "volvió al pasado"
        /// (el usuario logueado podría ni existir), así que se cierra la sesión.
        /// </summary>
        private void mnuBackup_Click(object sender, EventArgs e)
        {
            bool seRestauro;
            using (var frm = new FrmBackup())
            {
                frm.ShowDialog(this);
                seRestauro = frm.SeRestauroBackup;
            }

            if (seRestauro)
            {
                Mensajes.Info("La base fue restaurada. Vuelva a iniciar sesión.".Traducir());
                _usuarioService.CerrarSesion();
                CerroSesion = true;
                Close();
                return;
            }

            CargarInicio();
        }

        // Maestros

        private void mnuClientes_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmClientes());
        }

        private void mnuMecanicos_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmMecanicos());
        }

        private void mnuProveedores_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmProveedores());
        }

        private void mnuAgencias_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmAgencias());
        }

        // Compras

        private void mnuRegistrarCompra_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmRegistrarCompra());
        }

        private void mnuConsultarCompras_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmCompras());
        }

        private void mnuVehiculos_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmBuscadorAutos());
        }

        // Taller

        private void mnuRecepcion_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmAsignacionPresupuesto());
        }

        private void mnuPresupuestos_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmCargaCostos());
        }

        private void mnuRegistrarDiagnostico_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmDiagnosticos(false));
        }

        private void mnuConsultarDiagnostico_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmDiagnosticos(true));
        }

        private void mnuOrdenesTrabajo_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmOrdenesTrabajo(true));
        }

        private void mnuConsultarOrdenesTrabajo_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmOrdenesTrabajo(false));
        }

        private void mnuTareasRealizadas_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmTareasRealizadas());
        }

        // Repuestos

        private void mnuRegistrarPedido_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmPedidos(ModoPedidos.Registrar));
        }

        private void mnuEntregaPedido_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmPedidos(ModoPedidos.Entrega));
        }

        private void mnuConsultarPedidos_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmPedidos(ModoPedidos.Consulta));
        }

        // Comercial

        private void mnuConsignaciones_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmConsignaciones());
        }

        private void mnuPublicaciones_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmPublicaciones());
        }

        private void mnuVentas_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmVentas());
        }

        // Reportes

        private void mnuReporteVehiculos_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmReporte(TipoReporte.Vehiculos));
        }

        private void mnuReporteReparaciones_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmReporte(TipoReporte.Reparaciones));
        }

        private void mnuReportePresupuestos_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmReporte(TipoReporte.Presupuestos));
        }

        private void mnuReporteVentas_Click(object sender, EventArgs e)
        {
            AbrirPantalla(new FrmReporte(TipoReporte.Ventas));
        }
    }
}
