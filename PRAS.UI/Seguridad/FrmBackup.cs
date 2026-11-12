using PRAS.BLL;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// CU Seguridad - Realizar Backup / Restaurar Backup (solo Administrador).
    /// Backup  = copia de TODA la base en un archivo .bak.
    /// Restore = vuelve la base al estado que tenía cuando se hizo ese .bak
    ///           (se pierde todo lo cargado después).
    /// </summary>
    public partial class FrmBackup : FormBase
    {
        private readonly RespaldoService _respaldoService = new RespaldoService();

        /// <summary>true si se restauró un backup: quien abrió esta pantalla debe cerrar la sesión.</summary>
        public bool SeRestauroBackup { get; private set; }

        public FrmBackup()
        {
            InitializeComponent();
        }

        private void FrmBackup_Load(object sender, EventArgs e)
        {
            try
            {
                // Proponemos un nombre con fecha y hora: PRAS_2026-10-14_11-30.bak
                txtDestino.Text = Path.Combine(_respaldoService.CarpetaBackups(), _respaldoService.NombreSugerido());
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnElegirDestino_Click(object sender, EventArgs e)
        {
            using var dialogo = new SaveFileDialog
            {
                Filter = "Backup de SQL Server (*.bak)|*.bak",
                FileName = Path.GetFileName(txtDestino.Text),
                InitialDirectory = Path.GetDirectoryName(txtDestino.Text)
            };

            if (dialogo.ShowDialog(this) == DialogResult.OK)
                txtDestino.Text = dialogo.FileName;
        }

        private void btnRealizarBackup_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _respaldoService.RealizarBackup(txtDestino.Text);
                Cursor = Cursors.Default;

                Mensajes.Info("El backup se realizó correctamente.".Traducir() + "\n\n" + txtDestino.Text);

                // Para el próximo backup proponemos otro nombre con la hora nueva.
                txtDestino.Text = Path.Combine(Path.GetDirectoryName(txtDestino.Text), _respaldoService.NombreSugerido());
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                Mensajes.Error(ex);
            }
        }

        private void btnElegirOrigen_Click(object sender, EventArgs e)
        {
            using var dialogo = new OpenFileDialog
            {
                Filter = "Backup de SQL Server (*.bak)|*.bak",
                InitialDirectory = _respaldoService.CarpetaBackups()
            };

            if (dialogo.ShowDialog(this) == DialogResult.OK)
                txtOrigen.Text = dialogo.FileName;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOrigen.Text))
            {
                Mensajes.Advertencia("Seleccione el archivo de backup que quiere restaurar.".Traducir());
                return;
            }

            if (!Mensajes.Confirmar(
                    "Se van a reemplazar TODOS los datos actuales por los del backup.".Traducir() + "\n" +
                    "Lo que se cargó después de ese backup se pierde.".Traducir() + "\n\n" +
                    "¿Desea continuar?".Traducir()))
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _respaldoService.RealizarRestore(txtOrigen.Text);
                Cursor = Cursors.Default;

                SeRestauroBackup = true;
                Close();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                Mensajes.Error(ex);
            }
        }
    }
}
