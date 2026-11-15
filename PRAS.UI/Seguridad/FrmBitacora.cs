using PRAS.BLL;
using PRAS.Services.DomainModel;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// CU Seguridad - Consultar Bitácora (solo Administrador).
    /// Muestra lo que fue grabando LoggerService: inicios de sesión, altas, modificaciones,
    /// errores, backups... Se puede filtrar por fecha, nivel, usuario y texto.
    /// Colores: amarillo = advertencia (Warn), rojo = error (Error / Fatal).
    /// </summary>
    public partial class FrmBitacora : FormBase
    {
        private readonly BitacoraService _bitacoraService = new BitacoraService();

        public FrmBitacora()
        {
            InitializeComponent();
            dgvBitacora.AutoGenerateColumns = false;   // usamos solo las columnas del diseñador
        }

        private void FrmBitacora_Load(object sender, EventArgs e)
        {
            dtpDesde.Value = FiltroFechas.DesdePorDefecto;
            dtpHasta.Value = FiltroFechas.HastaPorDefecto;

            // Combo de niveles: "(Todos)" y cada valor del enum LogLevel
            cmbNivel.Items.Add("(Todos)".Traducir());
            foreach (LogLevel nivel in Enum.GetValues(typeof(LogLevel)))
                cmbNivel.Items.Add(nivel);
            cmbNivel.SelectedIndex = 0;

            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                // Si eligió "(Todos)" (índice 0) mandamos null y no filtramos por nivel
                LogLevel? nivel = cmbNivel.SelectedIndex <= 0 ? null : (LogLevel)cmbNivel.SelectedItem;

                List<LogEntry> registros = _bitacoraService.Listar(
                    dtpDesde.Value, dtpHasta.Value, nivel, txtUsuario.Text, txtTexto.Text);

                dgvBitacora.DataSource = registros;
                lblCantidad.Text = $"{registros.Count} " + "registros".Traducir();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dtpDesde.Value = FiltroFechas.DesdePorDefecto;
            dtpHasta.Value = FiltroFechas.HastaPorDefecto;
            cmbNivel.SelectedIndex = 0;
            txtUsuario.Clear();
            txtTexto.Clear();
            CargarGrilla();
        }

        /// <summary>Doble clic: muestra el registro completo (los mensajes largos no entran en la grilla).</summary>
        private void dgvBitacora_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvBitacora.Rows[e.RowIndex].DataBoundItem is not LogEntry registro)
                return;

            string texto = $"{registro.TimeStamp:dd/MM/yyyy HH:mm:ss}  [{registro.LogLevel}]\n" +
                           $"{"Usuario".Traducir()}: {registro.Usuario}\n\n" +
                           registro.Message.TraducirMensaje();

            if (registro.Detalle != "")
                texto += "\n\n" + registro.Detalle;

            Mensajes.Info(texto);
        }

        /// <summary>
        /// Pinta cada fila según la gravedad del registro y muestra el mensaje en el idioma elegido.
        /// En la base el mensaje queda grabado en español (la bitácora tiene el idioma de la aplicación,
        /// como explicó el profesor); acá solo cambiamos cómo se MUESTRA, no lo que está guardado.
        /// </summary>
        private void dgvBitacora_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvBitacora.Rows[e.RowIndex].DataBoundItem is not LogEntry registro)
                return;

            bool esTexto = dgvBitacora.Columns[e.ColumnIndex] == colMensaje || dgvBitacora.Columns[e.ColumnIndex] == colDetalle;
            if (esTexto && e.Value is string mensaje)
            {
                e.Value = mensaje.TraducirMensaje();
                e.FormattingApplied = true;
            }

            switch (registro.LogLevel)
            {
                case LogLevel.Warn:
                    e.CellStyle.BackColor = Tema.AdvertenciaFondo;   // amarillo suave (oscuro en modo oscuro)
                    break;
                case LogLevel.Error:
                case LogLevel.Fatal:
                    e.CellStyle.BackColor = Tema.PeligroFondo;   // rojo suave
                    e.CellStyle.ForeColor = Tema.Peligro;
                    break;
            }
        }
    }
}
