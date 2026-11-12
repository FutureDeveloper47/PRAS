using System.Globalization;
using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade;
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

        /// <summary>true si el usuario eligió "Cerrar sesión" (Program.cs vuelve a mostrar el login).</summary>
        public bool CerroSesion { get; private set; }

        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            MostrarUsuario();
            CargarIdiomas();
            AplicarPermisos();
            PrepararTarjetas();
            CargarInicio();
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
            // El mecánico no ve los números del negocio, solo el título.
            bool puedeVer = _inicioService.PuedeVerResumen();
            pnlTaller.Visible = puedeVer;
            pnlIngresos.Visible = puedeVer;
            pnlAgencia.Visible = puedeVer;
            lblActividadTitulo.Visible = puedeVer;
            lstActividad.Visible = puedeVer;

            if (!puedeVer)
                return;

            try
            {
                ResumenInicio resumen = _inicioService.ObtenerResumen();

                lblTallerDato.Text = $"{"Vehículos en taller".Traducir()}: {resumen.VehiculosEnTaller}";
                lblIngresosDato.Text = $"{"Chocados ingresados".Traducir()}: {resumen.ChocadosIngresadosMes}";
                lblAgenciaDato.Text = $"{"Ventas del mes".Traducir()}: {resumen.VentasMes}";

                lstActividad.Items.Clear();
                if (resumen.Actividades.Count == 0)
                {
                    lstActividad.Items.Add("Todavía no hay movimientos.".Traducir());
                }
                else
                {
                    foreach (ActividadReciente actividad in resumen.Actividades)
                        lstActividad.Items.Add($"{TextoFecha(actividad.Fecha)} - {actividad.Descripcion}");
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>
        /// Convierte una fecha en texto corto como en el diseño: "Hoy 10:00", "Ayer", "25/09".
        /// La hora se muestra solo si la fecha la tiene (las compras se guardan sin hora).
        /// </summary>
        private static string TextoFecha(DateTime fecha)
        {
            string dia;
            if (fecha.Date == DateTime.Today)
                dia = "Hoy".Traducir();
            else if (fecha.Date == DateTime.Today.AddDays(-1))
                dia = "Ayer".Traducir();
            else
                dia = fecha.ToString("dd/MM");

            bool tieneHora = fecha.TimeOfDay != TimeSpan.Zero;
            return tieneHora ? $"{dia} {fecha:HH:mm}" : dia;
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
                ver.ForeColor = Tema.Primario;
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
            tarjeta.Invalidate();

            AbrirPantalla(new FrmDetalleInicio(tipo));
        }

        private void Tarjeta_MouseEnter(object sender, EventArgs e)
        {
            Panel tarjeta = TarjetaDe(sender);
            if (_tarjetaResaltada == tarjeta)
                return;

            _tarjetaResaltada = tarjeta;
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
