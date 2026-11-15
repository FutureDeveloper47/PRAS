namespace PRAS.UI
{
    partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            menuPrincipal = new MenuStrip();
            mnuSistema = new ToolStripMenuItem();
            mnuIdioma = new ToolStripMenuItem();
            mnuMiPerfil = new ToolStripMenuItem();
            mnuSeparadorAyuda = new ToolStripSeparator();
            mnuPedirAyuda = new ToolStripMenuItem();
            mnuAyuda = new ToolStripMenuItem();
            mnuSeparadorSesion = new ToolStripSeparator();
            mnuCerrarSesion = new ToolStripMenuItem();
            mnuSalir = new ToolStripMenuItem();
            mnuSeguridad = new ToolStripMenuItem();
            mnuUsuarios = new ToolStripMenuItem();
            mnuPermisos = new ToolStripMenuItem();
            mnuBitacora = new ToolStripMenuItem();
            mnuBackup = new ToolStripMenuItem();
            mnuIntegridad = new ToolStripMenuItem();
            mnuMaestros = new ToolStripMenuItem();
            mnuClientes = new ToolStripMenuItem();
            mnuMecanicos = new ToolStripMenuItem();
            mnuProveedores = new ToolStripMenuItem();
            mnuAgencias = new ToolStripMenuItem();
            mnuCompras = new ToolStripMenuItem();
            mnuRegistrarCompra = new ToolStripMenuItem();
            mnuConsultarCompras = new ToolStripMenuItem();
            mnuVehiculos = new ToolStripMenuItem();
            mnuTaller = new ToolStripMenuItem();
            mnuRecepcion = new ToolStripMenuItem();
            mnuRegistrarDiagnostico = new ToolStripMenuItem();
            mnuConsultarDiagnostico = new ToolStripMenuItem();
            mnuPresupuestos = new ToolStripMenuItem();
            mnuOrdenesTrabajo = new ToolStripMenuItem();
            mnuConsultarOrdenesTrabajo = new ToolStripMenuItem();
            mnuTareasRealizadas = new ToolStripMenuItem();
            mnuRepuestos = new ToolStripMenuItem();
            mnuRegistrarPedido = new ToolStripMenuItem();
            mnuEntregaPedido = new ToolStripMenuItem();
            mnuConsultarPedidos = new ToolStripMenuItem();
            mnuComercial = new ToolStripMenuItem();
            mnuConsignaciones = new ToolStripMenuItem();
            mnuPublicaciones = new ToolStripMenuItem();
            mnuVentas = new ToolStripMenuItem();
            mnuReportes = new ToolStripMenuItem();
            mnuReporteVehiculos = new ToolStripMenuItem();
            mnuReporteReparaciones = new ToolStripMenuItem();
            mnuReportePresupuestos = new ToolStripMenuItem();
            mnuReporteVentas = new ToolStripMenuItem();
            barraEstado = new StatusStrip();
            lblUsuarioActual = new ToolStripStatusLabel();
            pnlInicio = new Panel();
            lblTitulo = new Label();
            pnlTaller = new Panel();
            lblTallerTitulo = new Label();
            lblTallerDato = new Label();
            lblTallerVer = new Label();
            pnlIngresos = new Panel();
            lblIngresosTitulo = new Label();
            lblIngresosDato = new Label();
            lblIngresosAclaracion = new Label();
            lblIngresosVer = new Label();
            pnlAgencia = new Panel();
            lblAgenciaTitulo = new Label();
            lblAgenciaDato = new Label();
            lblAgenciaVer = new Label();
            lblActividadTitulo = new Label();
            lstActividad = new ListBox();
            menuPrincipal.SuspendLayout();
            barraEstado.SuspendLayout();
            pnlInicio.SuspendLayout();
            pnlTaller.SuspendLayout();
            pnlIngresos.SuspendLayout();
            pnlAgencia.SuspendLayout();
            SuspendLayout();
            //
            // menuPrincipal
            //
            menuPrincipal.Items.AddRange(new ToolStripItem[] { mnuSistema, mnuSeguridad, mnuMaestros, mnuCompras, mnuTaller, mnuRepuestos, mnuComercial, mnuReportes });
            menuPrincipal.Location = new Point(0, 0);
            menuPrincipal.Name = "menuPrincipal";
            menuPrincipal.Size = new Size(984, 24);
            menuPrincipal.TabIndex = 0;
            //
            // mnuSistema
            //
            mnuSistema.DropDownItems.AddRange(new ToolStripItem[] { mnuIdioma, mnuMiPerfil, mnuSeparadorAyuda, mnuPedirAyuda, mnuAyuda, mnuSeparadorSesion, mnuCerrarSesion, mnuSalir });
            mnuSistema.Name = "mnuSistema";
            mnuSistema.Text = "&Sistema";
            //
            // mnuIdioma  (las opciones de cada idioma se agregan por código, según los archivos de I18n)
            // 
            mnuIdioma.Name = "mnuIdioma";
            mnuIdioma.Text = "Idioma";
            // 
            // mnuMiPerfil
            //
            mnuMiPerfil.Name = "mnuMiPerfil";
            mnuMiPerfil.Text = "Mi perfil";
            mnuMiPerfil.Click += mnuMiPerfil_Click;
            //
            // mnuSeparadorAyuda
            //
            mnuSeparadorAyuda.Name = "mnuSeparadorAyuda";
            //
            // mnuPedirAyuda
            //
            mnuPedirAyuda.Name = "mnuPedirAyuda";
            mnuPedirAyuda.Text = "Pedir ayuda";
            mnuPedirAyuda.Click += mnuPedirAyuda_Click;
            //
            // mnuAyuda
            //
            mnuAyuda.Name = "mnuAyuda";
            mnuAyuda.ShortcutKeyDisplayString = "F1";
            mnuAyuda.Text = "Ayuda";
            mnuAyuda.Click += mnuAyuda_Click;
            //
            // mnuSeparadorSesion
            //
            mnuSeparadorSesion.Name = "mnuSeparadorSesion";
            //
            // mnuCerrarSesion
            //
            mnuCerrarSesion.Name = "mnuCerrarSesion";
            mnuCerrarSesion.Text = "Cerrar sesión";
            mnuCerrarSesion.Click += mnuCerrarSesion_Click;
            //
            // mnuSalir
            //
            mnuSalir.Name = "mnuSalir";
            mnuSalir.Text = "Salir";
            mnuSalir.Click += mnuSalir_Click;
            //
            // mnuSeguridad
            //
            mnuSeguridad.DropDownItems.AddRange(new ToolStripItem[] { mnuUsuarios, mnuPermisos, mnuBitacora, mnuBackup, mnuIntegridad });
            mnuSeguridad.Name = "mnuSeguridad";
            mnuSeguridad.Text = "S&eguridad";
            //
            // mnuUsuarios
            //
            mnuUsuarios.Name = "mnuUsuarios";
            mnuUsuarios.Text = "Usuarios";
            mnuUsuarios.Click += mnuUsuarios_Click;
            //
            // mnuPermisos
            //
            mnuPermisos.Name = "mnuPermisos";
            mnuPermisos.Text = "Permisos (familias y patentes)";
            mnuPermisos.Click += mnuPermisos_Click;
            // 
            // mnuBitacora
            // 
            mnuBitacora.Name = "mnuBitacora";
            mnuBitacora.Text = "Bitácora";
            mnuBitacora.Click += mnuBitacora_Click;
            // 
            // mnuBackup
            // 
            mnuBackup.Name = "mnuBackup";
            mnuBackup.Text = "Backup y restore";
            mnuBackup.Click += mnuBackup_Click;
            // 
            // mnuIntegridad
            // 
            mnuIntegridad.Name = "mnuIntegridad";
            mnuIntegridad.Text = "Verificar integridad";
            mnuIntegridad.Click += mnuIntegridad_Click;
            //
            // mnuMaestros
            //
            mnuMaestros.DropDownItems.AddRange(new ToolStripItem[] { mnuClientes, mnuMecanicos, mnuProveedores, mnuAgencias });
            mnuMaestros.Name = "mnuMaestros";
            mnuMaestros.Text = "&Maestros";
            //
            // mnuClientes
            //
            mnuClientes.Name = "mnuClientes";
            mnuClientes.Text = "Clientes";
            mnuClientes.Click += mnuClientes_Click;
            //
            // mnuMecanicos
            //
            mnuMecanicos.Name = "mnuMecanicos";
            mnuMecanicos.Text = "Mecánicos";
            mnuMecanicos.Click += mnuMecanicos_Click;
            //
            // mnuProveedores
            //
            mnuProveedores.Name = "mnuProveedores";
            mnuProveedores.Text = "Proveedores";
            mnuProveedores.Click += mnuProveedores_Click;
            //
            // mnuAgencias
            //
            mnuAgencias.Name = "mnuAgencias";
            mnuAgencias.Text = "Agencias";
            mnuAgencias.Click += mnuAgencias_Click;
            //
            // mnuCompras
            //
            mnuCompras.DropDownItems.AddRange(new ToolStripItem[] { mnuRegistrarCompra, mnuConsultarCompras, mnuVehiculos });
            mnuCompras.Name = "mnuCompras";
            mnuCompras.Text = "&Compras";
            //
            // mnuRegistrarCompra
            //
            mnuRegistrarCompra.Name = "mnuRegistrarCompra";
            mnuRegistrarCompra.Text = "Registrar compra de vehículo";
            mnuRegistrarCompra.Click += mnuRegistrarCompra_Click;
            //
            // mnuConsultarCompras
            //
            mnuConsultarCompras.Name = "mnuConsultarCompras";
            mnuConsultarCompras.Text = "Consultar compras";
            mnuConsultarCompras.Click += mnuConsultarCompras_Click;
            //
            // mnuVehiculos
            //
            mnuVehiculos.Name = "mnuVehiculos";
            mnuVehiculos.Text = "Buscador de autos";
            mnuVehiculos.Click += mnuVehiculos_Click;
            //
            // mnuTaller
            //
            mnuTaller.DropDownItems.AddRange(new ToolStripItem[] { mnuRecepcion, mnuRegistrarDiagnostico, mnuConsultarDiagnostico, mnuPresupuestos, mnuOrdenesTrabajo, mnuConsultarOrdenesTrabajo, mnuTareasRealizadas });
            mnuTaller.Name = "mnuTaller";
            mnuTaller.Text = "&Taller";
            //
            // mnuRecepcion
            //
            mnuRecepcion.Name = "mnuRecepcion";
            mnuRecepcion.Text = "Asignación de presupuesto (ingreso a taller)";
            mnuRecepcion.Click += mnuRecepcion_Click;
            //
            // mnuRegistrarDiagnostico
            //
            mnuRegistrarDiagnostico.Name = "mnuRegistrarDiagnostico";
            mnuRegistrarDiagnostico.Text = "Registrar diagnóstico";
            mnuRegistrarDiagnostico.Click += mnuRegistrarDiagnostico_Click;
            //
            // mnuConsultarDiagnostico
            //
            mnuConsultarDiagnostico.Name = "mnuConsultarDiagnostico";
            mnuConsultarDiagnostico.Text = "Consultar diagnóstico";
            mnuConsultarDiagnostico.Click += mnuConsultarDiagnostico_Click;
            //
            // mnuPresupuestos
            //
            mnuPresupuestos.Name = "mnuPresupuestos";
            mnuPresupuestos.Text = "Carga de costos";
            mnuPresupuestos.Click += mnuPresupuestos_Click;
            //
            // mnuOrdenesTrabajo
            //
            mnuOrdenesTrabajo.Name = "mnuOrdenesTrabajo";
            mnuOrdenesTrabajo.Text = "Órdenes de trabajo";
            mnuOrdenesTrabajo.Click += mnuOrdenesTrabajo_Click;
            //
            // mnuConsultarOrdenesTrabajo
            //
            mnuConsultarOrdenesTrabajo.Name = "mnuConsultarOrdenesTrabajo";
            mnuConsultarOrdenesTrabajo.Text = "Consultar órdenes de trabajo";
            mnuConsultarOrdenesTrabajo.Click += mnuConsultarOrdenesTrabajo_Click;
            //
            // mnuTareasRealizadas
            //
            mnuTareasRealizadas.Name = "mnuTareasRealizadas";
            mnuTareasRealizadas.Text = "Registrar tareas realizadas";
            mnuTareasRealizadas.Click += mnuTareasRealizadas_Click;
            //
            // mnuRepuestos
            //
            mnuRepuestos.DropDownItems.AddRange(new ToolStripItem[] { mnuRegistrarPedido, mnuEntregaPedido, mnuConsultarPedidos });
            mnuRepuestos.Name = "mnuRepuestos";
            mnuRepuestos.Text = "&Repuestos";
            //
            // mnuRegistrarPedido
            //
            mnuRegistrarPedido.Name = "mnuRegistrarPedido";
            mnuRegistrarPedido.Text = "Registrar pedido";
            mnuRegistrarPedido.Click += mnuRegistrarPedido_Click;
            //
            // mnuEntregaPedido
            //
            mnuEntregaPedido.Name = "mnuEntregaPedido";
            mnuEntregaPedido.Text = "Registrar entrega de pedido";
            mnuEntregaPedido.Click += mnuEntregaPedido_Click;
            //
            // mnuConsultarPedidos
            //
            mnuConsultarPedidos.Name = "mnuConsultarPedidos";
            mnuConsultarPedidos.Text = "Consultar pedidos";
            mnuConsultarPedidos.Click += mnuConsultarPedidos_Click;
            //
            // mnuComercial
            //
            mnuComercial.DropDownItems.AddRange(new ToolStripItem[] { mnuConsignaciones, mnuPublicaciones, mnuVentas });
            mnuComercial.Name = "mnuComercial";
            mnuComercial.Text = "C&omercial";
            //
            // mnuConsignaciones
            //
            mnuConsignaciones.Name = "mnuConsignaciones";
            mnuConsignaciones.Text = "Consignaciones";
            mnuConsignaciones.Click += mnuConsignaciones_Click;
            //
            // mnuPublicaciones
            //
            mnuPublicaciones.Name = "mnuPublicaciones";
            mnuPublicaciones.Text = "Publicaciones";
            mnuPublicaciones.Click += mnuPublicaciones_Click;
            //
            // mnuVentas
            //
            mnuVentas.Name = "mnuVentas";
            mnuVentas.Text = "Ventas";
            mnuVentas.Click += mnuVentas_Click;
            //
            // mnuReportes
            //
            mnuReportes.DropDownItems.AddRange(new ToolStripItem[] { mnuReporteVehiculos, mnuReporteReparaciones, mnuReportePresupuestos, mnuReporteVentas });
            mnuReportes.Name = "mnuReportes";
            mnuReportes.Text = "Re&portes";
            //
            // mnuReporteVehiculos
            //
            mnuReporteVehiculos.Name = "mnuReporteVehiculos";
            mnuReporteVehiculos.Text = "Vehículos";
            mnuReporteVehiculos.Click += mnuReporteVehiculos_Click;
            //
            // mnuReporteReparaciones
            //
            mnuReporteReparaciones.Name = "mnuReporteReparaciones";
            mnuReporteReparaciones.Text = "Reparaciones";
            mnuReporteReparaciones.Click += mnuReporteReparaciones_Click;
            //
            // mnuReportePresupuestos
            //
            mnuReportePresupuestos.Name = "mnuReportePresupuestos";
            mnuReportePresupuestos.Text = "Presupuestos";
            mnuReportePresupuestos.Click += mnuReportePresupuestos_Click;
            //
            // mnuReporteVentas
            //
            mnuReporteVentas.Name = "mnuReporteVentas";
            mnuReporteVentas.Text = "Ventas";
            mnuReporteVentas.Click += mnuReporteVentas_Click;
            //
            // barraEstado
            //
            barraEstado.Items.AddRange(new ToolStripItem[] { lblUsuarioActual });
            barraEstado.Location = new Point(0, 539);
            barraEstado.Name = "barraEstado";
            barraEstado.Size = new Size(984, 22);
            barraEstado.TabIndex = 1;
            //
            // lblUsuarioActual
            //
            lblUsuarioActual.Name = "lblUsuarioActual";
            lblUsuarioActual.Size = new Size(50, 17);
            lblUsuarioActual.Text = "Usuario:";
            //
            // pnlInicio  (Pantalla A de EA: queda centrado aunque la ventana se maximice)
            //
            pnlInicio.Anchor = AnchorStyles.None;
            pnlInicio.Controls.Add(lblTitulo);
            pnlInicio.Controls.Add(pnlTaller);
            pnlInicio.Controls.Add(pnlIngresos);
            pnlInicio.Controls.Add(pnlAgencia);
            pnlInicio.Controls.Add(lblActividadTitulo);
            pnlInicio.Controls.Add(lstActividad);
            pnlInicio.Location = new Point(12, 46);
            pnlInicio.Name = "pnlInicio";
            pnlInicio.Size = new Size(960, 470);
            pnlInicio.TabIndex = 2;
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 5);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(960, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "PRAS: Programa de Registro Automotor Superior";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // pnlTaller  (tarjeta-botón: al tocarla abre el detalle del taller)
            //
            pnlTaller.Controls.Add(lblTallerTitulo);
            pnlTaller.Controls.Add(lblTallerDato);
            pnlTaller.Controls.Add(lblTallerVer);
            pnlTaller.BorderStyle = BorderStyle.FixedSingle;
            pnlTaller.Location = new Point(20, 85);
            pnlTaller.Name = "pnlTaller";
            pnlTaller.Size = new Size(220, 380);
            pnlTaller.TabIndex = 1;
            //
            // lblTallerTitulo
            //
            lblTallerTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTallerTitulo.Location = new Point(0, 25);
            lblTallerTitulo.Name = "lblTallerTitulo";
            lblTallerTitulo.Size = new Size(218, 35);
            lblTallerTitulo.TabIndex = 0;
            lblTallerTitulo.Text = "TALLER";
            lblTallerTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblTallerDato
            //
            lblTallerDato.Font = new Font("Segoe UI", 10F);
            lblTallerDato.Location = new Point(0, 90);
            lblTallerDato.Name = "lblTallerDato";
            lblTallerDato.Size = new Size(218, 25);
            lblTallerDato.TabIndex = 1;
            lblTallerDato.Text = "Vehículos en taller: 0";
            lblTallerDato.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblTallerVer
            //
            lblTallerVer.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            lblTallerVer.Location = new Point(0, 335);
            lblTallerVer.Name = "lblTallerVer";
            lblTallerVer.Size = new Size(218, 22);
            lblTallerVer.TabIndex = 2;
            lblTallerVer.Text = "Ver detalle ›";
            lblTallerVer.TextAlign = ContentAlignment.MiddleCenter;
            //
            // pnlIngresos  (tarjeta-botón: al tocarla abre las compras del mes)
            //
            pnlIngresos.Controls.Add(lblIngresosTitulo);
            pnlIngresos.Controls.Add(lblIngresosDato);
            pnlIngresos.Controls.Add(lblIngresosAclaracion);
            pnlIngresos.Controls.Add(lblIngresosVer);
            pnlIngresos.BorderStyle = BorderStyle.FixedSingle;
            pnlIngresos.Location = new Point(260, 85);
            pnlIngresos.Name = "pnlIngresos";
            pnlIngresos.Size = new Size(220, 380);
            pnlIngresos.TabIndex = 2;
            //
            // lblIngresosTitulo
            //
            lblIngresosTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblIngresosTitulo.Location = new Point(0, 25);
            lblIngresosTitulo.Name = "lblIngresosTitulo";
            lblIngresosTitulo.Size = new Size(218, 35);
            lblIngresosTitulo.TabIndex = 0;
            lblIngresosTitulo.Text = "INGRESOS";
            lblIngresosTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblIngresosDato
            //
            lblIngresosDato.Font = new Font("Segoe UI", 10F);
            lblIngresosDato.Location = new Point(0, 90);
            lblIngresosDato.Name = "lblIngresosDato";
            lblIngresosDato.Size = new Size(218, 25);
            lblIngresosDato.TabIndex = 1;
            lblIngresosDato.Text = "Chocados ingresados: 0";
            lblIngresosDato.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblIngresosAclaracion
            //
            lblIngresosAclaracion.Font = new Font("Segoe UI", 9F);
            lblIngresosAclaracion.ForeColor = SystemColors.GrayText;
            lblIngresosAclaracion.Location = new Point(0, 115);
            lblIngresosAclaracion.Name = "lblIngresosAclaracion";
            lblIngresosAclaracion.Size = new Size(218, 20);
            lblIngresosAclaracion.TabIndex = 2;
            lblIngresosAclaracion.Text = "(últimos 12 meses)";
            lblIngresosAclaracion.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblIngresosVer
            //
            lblIngresosVer.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            lblIngresosVer.Location = new Point(0, 335);
            lblIngresosVer.Name = "lblIngresosVer";
            lblIngresosVer.Size = new Size(218, 22);
            lblIngresosVer.TabIndex = 3;
            lblIngresosVer.Text = "Ver detalle ›";
            lblIngresosVer.TextAlign = ContentAlignment.MiddleCenter;
            //
            // pnlAgencia  (tarjeta-botón: al tocarla abre ventas y autos en agencia)
            //
            pnlAgencia.Controls.Add(lblAgenciaTitulo);
            pnlAgencia.Controls.Add(lblAgenciaDato);
            pnlAgencia.Controls.Add(lblAgenciaVer);
            pnlAgencia.BorderStyle = BorderStyle.FixedSingle;
            pnlAgencia.Location = new Point(500, 85);
            pnlAgencia.Name = "pnlAgencia";
            pnlAgencia.Size = new Size(440, 160);
            pnlAgencia.TabIndex = 3;
            //
            // lblAgenciaTitulo
            //
            lblAgenciaTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblAgenciaTitulo.Location = new Point(0, 20);
            lblAgenciaTitulo.Name = "lblAgenciaTitulo";
            lblAgenciaTitulo.Size = new Size(438, 35);
            lblAgenciaTitulo.TabIndex = 0;
            lblAgenciaTitulo.Text = "AGENCIA";
            lblAgenciaTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblAgenciaDato
            //
            lblAgenciaDato.Font = new Font("Segoe UI", 10F);
            lblAgenciaDato.Location = new Point(0, 70);
            lblAgenciaDato.Name = "lblAgenciaDato";
            lblAgenciaDato.Size = new Size(438, 25);
            lblAgenciaDato.TabIndex = 1;
            lblAgenciaDato.Text = "Ventas (últimos 12 meses): 0";
            lblAgenciaDato.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblAgenciaVer
            //
            lblAgenciaVer.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            lblAgenciaVer.Location = new Point(0, 120);
            lblAgenciaVer.Name = "lblAgenciaVer";
            lblAgenciaVer.Size = new Size(438, 22);
            lblAgenciaVer.TabIndex = 2;
            lblAgenciaVer.Text = "Ver detalle ›";
            lblAgenciaVer.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblActividadTitulo
            //
            lblActividadTitulo.AutoSize = true;
            lblActividadTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblActividadTitulo.Location = new Point(500, 260);
            lblActividadTitulo.Name = "lblActividadTitulo";
            lblActividadTitulo.Size = new Size(128, 15);
            lblActividadTitulo.TabIndex = 4;
            lblActividadTitulo.Text = "ACTIVIDAD RECIENTE";
            //
            // lstActividad  (más ancha y más alta: se lee cada renglón completo, sin scroll)
            //
            lstActividad.Font = new Font("Segoe UI", 9.75F);
            lstActividad.FormattingEnabled = true;
            lstActividad.HorizontalScrollbar = true;
            lstActividad.IntegralHeight = false;
            lstActividad.ItemHeight = 17;
            lstActividad.Location = new Point(500, 280);
            lstActividad.Name = "lstActividad";
            lstActividad.SelectionMode = SelectionMode.None;
            lstActividad.Size = new Size(440, 185);
            lstActividad.TabIndex = 5;
            //
            // FrmPrincipal
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 561);
            Controls.Add(pnlInicio);
            Controls.Add(barraEstado);
            Controls.Add(menuPrincipal);
            MainMenuStrip = menuPrincipal;
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SISTEMA - INICIO";
            WindowState = FormWindowState.Maximized;
            Load += FrmPrincipal_Load;
            menuPrincipal.ResumeLayout(false);
            menuPrincipal.PerformLayout();
            barraEstado.ResumeLayout(false);
            barraEstado.PerformLayout();
            pnlInicio.ResumeLayout(false);
            pnlInicio.PerformLayout();
            pnlTaller.ResumeLayout(false);
            pnlIngresos.ResumeLayout(false);
            pnlAgencia.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuPrincipal;
        private ToolStripMenuItem mnuSistema;
        private ToolStripMenuItem mnuIdioma;
        private ToolStripMenuItem mnuMiPerfil;
        private ToolStripSeparator mnuSeparadorAyuda;
        private ToolStripMenuItem mnuPedirAyuda;
        private ToolStripMenuItem mnuAyuda;
        private ToolStripSeparator mnuSeparadorSesion;
        private ToolStripMenuItem mnuCerrarSesion;
        private ToolStripMenuItem mnuSalir;
        private ToolStripMenuItem mnuSeguridad;
        private ToolStripMenuItem mnuUsuarios;
        private ToolStripMenuItem mnuPermisos;
        private ToolStripMenuItem mnuBitacora;
        private ToolStripMenuItem mnuBackup;
        private ToolStripMenuItem mnuIntegridad;
        private ToolStripMenuItem mnuMaestros;
        private ToolStripMenuItem mnuClientes;
        private ToolStripMenuItem mnuMecanicos;
        private ToolStripMenuItem mnuProveedores;
        private ToolStripMenuItem mnuAgencias;
        private ToolStripMenuItem mnuCompras;
        private ToolStripMenuItem mnuRegistrarCompra;
        private ToolStripMenuItem mnuConsultarCompras;
        private ToolStripMenuItem mnuVehiculos;
        private ToolStripMenuItem mnuTaller;
        private ToolStripMenuItem mnuRecepcion;
        private ToolStripMenuItem mnuRegistrarDiagnostico;
        private ToolStripMenuItem mnuConsultarDiagnostico;
        private ToolStripMenuItem mnuPresupuestos;
        private ToolStripMenuItem mnuOrdenesTrabajo;
        private ToolStripMenuItem mnuConsultarOrdenesTrabajo;
        private ToolStripMenuItem mnuTareasRealizadas;
        private ToolStripMenuItem mnuRepuestos;
        private ToolStripMenuItem mnuRegistrarPedido;
        private ToolStripMenuItem mnuEntregaPedido;
        private ToolStripMenuItem mnuConsultarPedidos;
        private ToolStripMenuItem mnuComercial;
        private ToolStripMenuItem mnuConsignaciones;
        private ToolStripMenuItem mnuPublicaciones;
        private ToolStripMenuItem mnuVentas;
        private ToolStripMenuItem mnuReportes;
        private ToolStripMenuItem mnuReporteVehiculos;
        private ToolStripMenuItem mnuReporteReparaciones;
        private ToolStripMenuItem mnuReportePresupuestos;
        private ToolStripMenuItem mnuReporteVentas;
        private StatusStrip barraEstado;
        private ToolStripStatusLabel lblUsuarioActual;
        private Panel pnlInicio;
        private Label lblTitulo;
        private Panel pnlTaller;
        private Label lblTallerTitulo;
        private Label lblTallerDato;
        private Label lblTallerVer;
        private Panel pnlIngresos;
        private Label lblIngresosTitulo;
        private Label lblIngresosDato;
        private Label lblIngresosAclaracion;
        private Label lblIngresosVer;
        private Panel pnlAgencia;
        private Label lblAgenciaTitulo;
        private Label lblAgenciaDato;
        private Label lblAgenciaVer;
        private Label lblActividadTitulo;
        private ListBox lstActividad;
    }
}
