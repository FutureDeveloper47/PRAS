using System.Drawing.Drawing2D;
using System.Globalization;
using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade;
using PRAS.Services.Facade.Extensions;
using PRAS.BLL.Excepciones;

namespace PRAS.UI
{
    /// <summary>
    /// CU Seguridad.001 - Iniciar Sesión.
    /// Si el login es correcto, el formulario se cierra con DialogResult.OK
    /// y Program.cs abre el menú principal.
    ///
    /// Diseño: a la izquierda un panel con la marca PRAS (ícono, nombre y versión) y a la derecha
    /// el ingreso: idioma, usuario, contraseña (con "Mostrar" y aviso de Bloq Mayús), los botones
    /// y en una fila los tres links (¿Olvidó su contraseña? · Solicitar acceso · Usuarios de prueba).
    /// Los controles del diseñador se reacomodan por código; la lógica del ingreso es la misma de siempre
    /// (intentos fallidos, integridad, bitácora).
    /// </summary>
    public partial class FrmLogin : FormBase
    {
        private const int MaximoIntentos = 3;
        private readonly UsuarioService _usuarioService = new UsuarioService();
        private readonly IntegridadService _integridadService = new IntegridadService();
        private readonly ClienteService _clienteService = new ClienteService();
        private int _intentosFallidos = 0;
        private readonly DatosPruebaService _datosPruebaService = new DatosPruebaService();

        /// <summary>Link "Usuarios de prueba" (solo si App.config lo permite).</summary>
        private readonly LinkLabel lnkUsuariosPrueba = new LinkLabel();

        /// <summary>Link "Solicitar acceso" (CU.Seguridad.016): siempre visible.</summary>
        private readonly LinkLabel lnkSolicitarAcceso = new LinkLabel();

        // Controles nuevos del diseño
        private readonly Panel pnlMarca = new Panel();
        private readonly Label lblMarcaNombre = new Label();
        private readonly Label lblMarcaSubtitulo = new Label();
        private readonly Label lblMarcaLema = new Label();
        private readonly Label lblVersion = new Label();
        private readonly PictureBox picLogo = new PictureBox();
        private readonly Label lblIngresoTitulo = new Label();
        private readonly Label lblIngresoAyuda = new Label();
        private readonly Label lblIdioma = new Label();
        private readonly ComboBox cboIdioma = new ComboBox();
        private readonly CheckBox chkMostrar = new CheckBox();
        private readonly Label lblMayusculas = new Label();

        /// <summary>Opción del combo de idiomas: se ve con el nombre del idioma en su propio idioma.</summary>
        private sealed class OpcionIdiomaLogin
        {
            public CultureInfo Cultura { get; init; }
            public override string ToString() =>
                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Cultura.NativeName);
        }

        /// <summary>El login no lleva la franja azul de título: tiene su propio panel con la marca.</summary>
        public override bool MostrarEncabezado => false;

        public FrmLogin()
        {
            InitializeComponent();
            ArmarDisenio();
        }

        // Diseño

        private void ArmarDisenio()
        {
            float k = DeviceDpi / 96f;
            int S(int valor) => (int)(valor * k);

            SuspendLayout();
            ClientSize = new Size(S(820), S(460));

            // Panel de la marca (izquierda): degradé con los colores del tema, como la pantalla de carga.
            pnlMarca.Name = "pnlMarca";
            pnlMarca.Dock = DockStyle.Left;
            pnlMarca.Width = S(300);
            pnlMarca.Paint += PintarMarca;
            pnlMarca.Resize += (s, e) =>
            {
                // La versión va siempre abajo del panel (se acomoda si cambia el tamaño o la escala).
                lblVersion.Top = pnlMarca.Height - lblVersion.Height - S(18);
                pnlMarca.Invalidate();
            };

            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.BackColor = Color.Transparent;
            picLogo.SetBounds(S(36), S(60), S(72), S(72));
            try
            {
                using Icon icono = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                using var grande = new Icon(icono, 128, 128);
                picLogo.Image = grande.ToBitmap();
            }
            catch
            {
                // Sin ícono: queda solo el nombre.
            }

            ConfigurarTextoMarca(lblMarcaNombre, "PRAS", new Font("Segoe UI", 34F, FontStyle.Bold), S(36), S(140), S(240), S(64));
            ConfigurarTextoMarca(lblMarcaSubtitulo, "Programa de Registro Automotor Superior", new Font("Segoe UI", 12F), S(38), S(206), S(240), S(52));
            ConfigurarTextoMarca(lblMarcaLema, "Compra de autos chocados · Taller · Agencia", new Font("Segoe UI", 9.5F), S(38), S(264), S(240), S(40));
            lblMarcaLema.ForeColor = Color.FromArgb(205, 222, 238);
            ConfigurarTextoMarca(lblVersion, "", new Font("Segoe UI", 8.5F), S(38), S(450), S(240), S(22));
            lblVersion.ForeColor = Color.FromArgb(205, 222, 238);
            pnlMarca.Controls.AddRange(new Control[] { picLogo, lblMarcaNombre, lblMarcaSubtitulo, lblMarcaLema, lblVersion });

            // Lado derecho: una tabla de renglones que se acomodan solos.
            var tabla = new TableLayoutPanel
            {
                Name = "tlpIngreso",
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                Padding = new Padding(S(44), S(18), S(44), S(18))
            };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Idioma (arriba a la derecha)
            lblIdioma.Text = "Idioma:";
            lblIdioma.AutoSize = true;
            lblIdioma.Margin = new Padding(0, S(6), S(6), 0);
            cboIdioma.Name = "cboIdioma";
            cboIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
            cboIdioma.Width = S(190);
            var filaIdioma = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Dock = DockStyle.Fill, WrapContents = false, Margin = Padding.Empty };
            filaIdioma.Controls.Add(cboIdioma);
            filaIdioma.Controls.Add(lblIdioma);
            Agregar(tabla, filaIdioma);

            // Título y ayuda (el nombre termina en "Titulo": Tema lo pinta del azul del tema)
            lblIngresoTitulo.Name = "lblIngresoTitulo";
            lblIngresoTitulo.Text = "Iniciar sesión";
            lblIngresoTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblIngresoTitulo.AutoSize = true;
            lblIngresoTitulo.Margin = new Padding(0, S(10), 0, 0);
            Agregar(tabla, lblIngresoTitulo);

            lblIngresoAyuda.Text = "Ingrese su usuario y su contraseña.";
            lblIngresoAyuda.AutoSize = true;
            lblIngresoAyuda.ForeColor = Tema.TextoApagado;
            lblIngresoAyuda.Margin = new Padding(0, 0, 0, S(16));
            Agregar(tabla, lblIngresoAyuda);

            // Usuario
            lblUsuario.AutoSize = true;
            lblUsuario.Margin = new Padding(0, S(4), 0, S(4));
            Agregar(tabla, lblUsuario);
            txtUsuario.Dock = DockStyle.Fill;
            txtUsuario.Font = new Font("Segoe UI", 11F);
            txtUsuario.Margin = new Padding(0, 0, 0, S(10));
            Agregar(tabla, txtUsuario);

            // Contraseña + "Mostrar"
            lblContrasena.AutoSize = true;
            lblContrasena.Margin = new Padding(0, S(4), 0, S(4));
            Agregar(tabla, lblContrasena);
            var filaContrasena = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill, AutoSize = true, Margin = Padding.Empty };
            filaContrasena.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            filaContrasena.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            txtContrasena.Dock = DockStyle.Fill;
            txtContrasena.Font = new Font("Segoe UI", 11F);
            txtContrasena.Margin = Padding.Empty;
            chkMostrar.Name = "chkMostrar";
            chkMostrar.Text = "Mostrar";
            chkMostrar.AutoSize = true;
            chkMostrar.Margin = new Padding(S(10), S(4), 0, 0);
            chkMostrar.CheckedChanged += (s, e) => txtContrasena.UseSystemPasswordChar = !chkMostrar.Checked;
            filaContrasena.Controls.Add(txtContrasena, 0, 0);
            filaContrasena.Controls.Add(chkMostrar, 1, 0);
            Agregar(tabla, filaContrasena);

            // Aviso de Bloq Mayús (siempre ocupa su lugar, así la pantalla no "salta")
            lblMayusculas.Name = "lblMayusculas";
            lblMayusculas.AutoSize = true;
            lblMayusculas.ForeColor = Tema.Peligro;
            lblMayusculas.Margin = new Padding(0, S(4), 0, S(8));
            lblMayusculas.Text = " ";
            Agregar(tabla, lblMayusculas);

            // Botones
            var filaBotones = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill, AutoSize = true, Margin = Padding.Empty };
            filaBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
            filaBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
            btnIngresar.Dock = DockStyle.Fill;
            btnIngresar.Height = S(40);
            btnIngresar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnIngresar.Margin = new Padding(0, 0, S(8), 0);
            btnCancelar.Dock = DockStyle.Fill;
            btnCancelar.Height = S(40);
            btnCancelar.Margin = Padding.Empty;
            filaBotones.Controls.Add(btnIngresar, 0, 0);
            filaBotones.Controls.Add(btnCancelar, 1, 0);
            Agregar(tabla, filaBotones);

            // Los tres links en una fila, con la misma letra
            var filaLinks = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, WrapContents = true, Margin = new Padding(0, S(18), 0, 0) };
            lnkSolicitarAcceso.Name = "lnkSolicitarAcceso";
            lnkSolicitarAcceso.Text = "Solicitar acceso";
            lnkSolicitarAcceso.LinkClicked += lnkSolicitarAcceso_LinkClicked;
            lnkUsuariosPrueba.Name = "lnkUsuariosPrueba";
            lnkUsuariosPrueba.Text = "Usuarios de prueba";
            lnkUsuariosPrueba.LinkClicked += lnkUsuariosPrueba_LinkClicked;
            foreach (LinkLabel link in new[] { lnkOlvideContrasena, lnkSolicitarAcceso, lnkUsuariosPrueba })
            {
                link.AutoSize = true;
                link.Font = new Font("Segoe UI", 9.75F);
                link.LinkBehavior = LinkBehavior.HoverUnderline;
                link.LinkColor = Tema.Acento;
                link.ActiveLinkColor = Tema.Acento;
                link.VisitedLinkColor = Tema.Acento;
                link.Margin = new Padding(0, 0, S(18), S(4));
            }
            filaLinks.Controls.Add(lnkOlvideContrasena);
            filaLinks.Controls.Add(lnkSolicitarAcceso);
            // "Usuarios de prueba" solo si App.config lo permite (en una instalación real se apaga).
            if (_datosPruebaService.MostrarUsuariosDePrueba)
                filaLinks.Controls.Add(lnkUsuariosPrueba);
            Agregar(tabla, filaLinks);

            // El resto del alto queda libre abajo.
            tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            tabla.RowCount++;

            // El título viejo del diseñador ya no se usa (el título está en el panel de la derecha).
            Controls.Remove(lblTitulo);
            foreach (Control control in new Control[] { lblUsuario, txtUsuario, lblContrasena, txtContrasena, btnIngresar, btnCancelar, lnkOlvideContrasena })
                Controls.Remove(control);

            Controls.Add(tabla);
            Controls.Add(pnlMarca);   // se agrega último para que el Dock = Left quede afuera de la tabla

            // Bloq Mayús: se revisa al escribir y al volver a la ventana.
            txtUsuario.KeyUp += (s, e) => RevisarMayusculas();
            txtContrasena.KeyUp += (s, e) => RevisarMayusculas();
            txtContrasena.Enter += (s, e) => RevisarMayusculas();
            Activated += (s, e) => RevisarMayusculas();

            ResumeLayout(true);
        }

        private static void Agregar(TableLayoutPanel tabla, Control control)
        {
            tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tabla.Controls.Add(control, 0, tabla.RowCount++);
        }

        private static void ConfigurarTextoMarca(Label etiqueta, string texto, Font fuente, int x, int y, int ancho, int alto)
        {
            etiqueta.Text = texto;
            etiqueta.Font = fuente;
            etiqueta.ForeColor = Color.White;   // blanco sobre el azul: no lo cambia el tema
            etiqueta.BackColor = Color.Transparent;
            etiqueta.AutoSize = false;
            etiqueta.SetBounds(x, y, ancho, alto);
        }

        /// <summary>Fondo del panel de la marca: degradé del azul del tema y una línea fina como la ruta del splash.</summary>
        private void PintarMarca(object sender, PaintEventArgs e)
        {
            Rectangle area = pnlMarca.ClientRectangle;
            if (area.Width == 0 || area.Height == 0)
                return;
            using (var fondo = new LinearGradientBrush(area, Tema.PrimarioHover, Tema.PrimarioOscuro, 70f))
                e.Graphics.FillRectangle(fondo, area);
            using var linea = new Pen(Color.FromArgb(70, Color.White), 2) { DashStyle = DashStyle.Dash };
            int y = area.Height - (int)(70 * DeviceDpi / 96f);
            e.Graphics.DrawLine(linea, area.Left + 30, y, area.Right - 30, y);
        }

        // Carga

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);   // FormBase traduce y aplica el tema

            CargarIdiomas();
            MostrarVersion();

            // Recordamos el último usuario que entró en esta PC (nunca la contraseña).
            string ultimo = PreferenciasLocales.UltimoUsuario();
            if (!string.IsNullOrEmpty(ultimo) && txtUsuario.Text == "")
            {
                txtUsuario.Text = ultimo;
                ActiveControl = txtContrasena;
            }
            RevisarMayusculas();
        }

        private void MostrarVersion()
        {
            Version version = typeof(FrmLogin).Assembly.GetName().Version ?? new Version(1, 0);
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}".TraducirMensaje() + "  ·  UAI";
        }

        private void RevisarMayusculas()
        {
            lblMayusculas.Text = Control.IsKeyLocked(Keys.CapsLock) ? "Bloq Mayús está activado.".Traducir() : " ";
        }

        // Idioma del ingreso

        private bool _cargandoIdiomas;

        private void CargarIdiomas()
        {
            _cargandoIdiomas = true;
            cboIdioma.Items.Clear();
            try
            {
                foreach (CultureInfo idioma in IdiomaService.ObtenerIdiomas())
                {
                    cboIdioma.Items.Add(new OpcionIdiomaLogin { Cultura = idioma });
                    if (idioma.Name == IdiomaService.IdiomaActual().Name)
                        cboIdioma.SelectedIndex = cboIdioma.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
            _cargandoIdiomas = false;
            cboIdioma.SelectedIndexChanged -= cboIdioma_SelectedIndexChanged;
            cboIdioma.SelectedIndexChanged += cboIdioma_SelectedIndexChanged;
        }

        /// <summary>
        /// Cambia el idioma de la pantalla de ingreso. Al entrar, el sistema pasa igual al idioma
        /// que el usuario tiene guardado (el de Mi perfil).
        /// </summary>
        private void cboIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoIdiomas || cboIdioma.SelectedItem is not OpcionIdiomaLogin opcion)
                return;

            IdiomaService.CambiarIdioma(opcion.Cultura.Name);
            Traductor.Traducir(this);
            MostrarVersion();
            RevisarMayusculas();
        }

        // Links

        /// <summary>
        /// "Solicitar acceso": una persona sin usuario pide su cuenta. Queda deshabilitada hasta que
        /// un administrador la habilite (así nadie se puede crear un Administrador por su cuenta).
        /// </summary>
        private void lnkSolicitarAcceso_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var frm = new FrmSolicitarCuenta())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    txtUsuario.Text = frm.UsuarioSolicitado;
                    txtContrasena.Clear();
                }
            }
        }

        private void lnkUsuariosPrueba_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var frm = new FrmUsuariosDePrueba())
            {
                if (frm.ShowDialog(this) == DialogResult.OK && frm.UsuarioElegido != null)
                {
                    txtUsuario.Text = frm.UsuarioElegido;
                    txtContrasena.Clear();
                    txtContrasena.Focus();
                }
            }
        }

        // Ingresar (misma lógica de antes)

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = _usuarioService.IniciarSesion(txtUsuario.Text, txtContrasena.Text);

                // Pasamos al idioma que este usuario eligió la última vez
                // (si nunca eligió, queda en español).
                IdiomaService.CambiarIdioma(usuario.Idioma);

                // Antes de entrar revisamos los dígitos verificadores, para ver que nadie
                // haya tocado la base por afuera del sistema.
                if (!IntegridadCorrecta(usuario))
                {
                    _usuarioService.CerrarSesion();
                    IdiomaService.CambiarIdioma("es-AR");
                    txtContrasena.Clear();
                    return;
                }

                // Datos de clientes cargados antes del encriptado AES: se encriptan ahora (una sola vez).
                _clienteService.EncriptarDatosPendientes();

                // Se recuerda solo el NOMBRE de usuario en esta PC, para no tener que escribirlo la próxima vez.
                PreferenciasLocales.GuardarUltimoUsuario(usuario.NombreUsuario);

                DialogResult = DialogResult.OK;   // cierra el formulario
            }
            catch (ReglaNegocioException ex)
            {
                _intentosFallidos++;
                if (_intentosFallidos >= MaximoIntentos)
                {
                    Mensajes.Advertencia($"Superó los {MaximoIntentos} intentos permitidos. El sistema se cerrará.");
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                Mensajes.Advertencia(ex.Message);
                txtContrasena.Clear();
                txtContrasena.Focus();
            }
            catch (Exception ex)
            {
                if (Sesion.HayUsuarioLogueado)
                    _usuarioService.CerrarSesion();
                Mensajes.Error(ex);   // por ejemplo, no hay conexión con la base
            }
        }

        /// <summary>
        /// "¿Olvidó su contraseña?": abre Recuperar contraseña con el usuario que ya escribió.
        /// </summary>
        private void lnkOlvideContrasena_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var frm = new FrmRecuperarContrasena(txtUsuario.Text))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    txtContrasena.Clear();
                    txtContrasena.Focus();
                }
            }
        }

        /// <summary>
        /// Verifica los dígitos verificadores (DVH y DVV). Devuelve true si se puede entrar.
        /// - Si la base está bien: entra.
        /// - Si hay problemas y es Administrador: se le muestra la pantalla de Integridad
        ///   para que recalcule los dígitos o restaure un backup.
        /// - Si hay problemas y NO es Administrador: no puede entrar.
        /// </summary>
        private bool IntegridadCorrecta(Usuario usuario)
        {
            List<string> errores = _integridadService.Verificar();
            if (errores.Count == 0)
                return true;

            if (!Sesion.TienePermiso(Funcion.VerificarIntegridad))
            {
                Mensajes.Advertencia(
                    "Se detectaron cambios en la base de datos realizados por fuera del sistema.".Traducir() + "\n\n" +
                    "Solo un Administrador puede ingresar para revisarlo.".Traducir());
                return false;
            }

            using (var frm = new FrmIntegridad(errores))
            {
                frm.ShowDialog(this);

                if (frm.SeRestauroBackup)
                {
                    Mensajes.Info("La base fue restaurada. Vuelva a iniciar sesión.".Traducir());
                    return false;
                }
            }

            return true;
        }
    }
}
