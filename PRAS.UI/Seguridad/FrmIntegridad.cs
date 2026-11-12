using PRAS.BLL;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// CU Seguridad - Verificar Integridad (dígitos verificadores DVH y DVV).
    /// Se abre:
    /// - Solo, al iniciar sesión, si se detectó que la base fue modificada por fuera del sistema.
    /// - Desde el menú Seguridad > Verificar integridad.
    /// El Administrador decide: si los cambios son correctos, RECALCULA los dígitos;
    /// si no, RESTAURA un backup.
    /// </summary>
    public partial class FrmIntegridad : FormBase
    {
        private readonly IntegridadService _integridadService = new IntegridadService();
        private List<string> _errores;

        /// <summary>true si se restauró un backup: quien abrió esta pantalla debe cerrar la sesión.</summary>
        public bool SeRestauroBackup { get; private set; }

        /// <param name="errores">Problemas ya detectados (login). null = verificar al abrir.</param>
        public FrmIntegridad(List<string> errores)
        {
            InitializeComponent();
            _errores = errores;
        }

        private void FrmIntegridad_Load(object sender, EventArgs e)
        {
            lblTablas.Text = "Tablas protegidas".Traducir() + ": " + string.Join(", ", _integridadService.TablasProtegidas());

            if (_errores == null)
                Verificar();
            else
                MostrarResultado();
        }

        private void Verificar()
        {
            try
            {
                _errores = _integridadService.Verificar();
                MostrarResultado();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void MostrarResultado()
        {
            lstErrores.Items.Clear();

            if (_errores.Count == 0)
            {
                lblEstado.Text = "La base de datos está íntegra. No se detectaron cambios externos.".Traducir();
                lblEstado.ForeColor = Color.FromArgb(46, 125, 50);   // verde
                btnRecalcular.Enabled = false;
                return;
            }

            lblEstado.Text = $"{"Se detectaron cambios realizados por fuera del sistema".Traducir()} ({_errores.Count})";
            lblEstado.ForeColor = Tema.Peligro;
            btnRecalcular.Enabled = true;

            foreach (string error in _errores)
                lstErrores.Items.Add(error);
        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            Verificar();
        }

        /// <summary>
        /// Acepta los datos ACTUALES como correctos y vuelve a calcular los dígitos.
        /// Solo hay que usarlo si el cambio fue legítimo (ej: un script autorizado).
        /// </summary>
        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            if (!Mensajes.Confirmar(
                    "Se van a aceptar los datos actuales como correctos y se recalcularán todos los dígitos verificadores.".Traducir() + "\n\n" +
                    "¿Desea continuar?".Traducir()))
                return;

            try
            {
                _integridadService.Recalcular();
                Mensajes.Info("Los dígitos verificadores se recalcularon correctamente.".Traducir());
                Verificar();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmBackup())
            {
                frm.ShowDialog(this);
                if (frm.SeRestauroBackup)
                {
                    SeRestauroBackup = true;
                    Close();
                }
            }
        }
    }
}
