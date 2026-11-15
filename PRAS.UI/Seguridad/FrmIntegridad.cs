using PRAS.BLL;
using PRAS.Services.DomainModel;
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
    /// Además de decir si la base está bien, muestra QUÉ se revisó (cada tabla, cuántos registros
    /// y si coinciden el DVH y el DVV), cuándo se verificó y cómo probarlo con SQL.
    /// </summary>
    public partial class FrmIntegridad : FormBase
    {
        private readonly IntegridadService _integridadService = new IntegridadService();
        private List<string> _errores;

        /// <summary>Resultado de cada tabla protegida (null si los errores vinieron del login).</summary>
        private List<ResultadoIntegridadTabla> _detalle;

        /// <summary>Fecha y hora de la última verificación.</summary>
        private DateTime _verificadoEl = DateTime.Now;

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
                _detalle = new List<ResultadoIntegridadTabla>();
                _errores = _integridadService.Verificar(_detalle);
                _verificadoEl = DateTime.Now;
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
                lblEstado.ForeColor = Tema.Exito;   // verde
                btnRecalcular.Enabled = false;
            }
            else
            {
                lblEstado.Text = $"{"Se detectaron cambios realizados por fuera del sistema".Traducir()} ({_errores.Count})";
                lblEstado.ForeColor = Tema.Peligro;
                btnRecalcular.Enabled = true;

                foreach (string error in _errores)
                    lstErrores.Items.Add("✖ " + error.TraducirMensaje());
                lstErrores.Items.Add("");
            }

            MostrarDetalle();
        }

        /// <summary>
        /// Muestra qué se controló en cada tabla. Así se ve que la verificación se hizo de verdad
        /// (no es solo un cartel): cantidad de registros revisados, DVH y DVV de cada tabla.
        /// Al final explica cómo probarlo: cambiar un dato con SQL y volver a verificar.
        /// </summary>
        private void MostrarDetalle()
        {
            if (_detalle == null)
                return;   // vino del login con los errores ya calculados

            lstErrores.Items.Add($"Verificación realizada el {_verificadoEl:dd/MM/yyyy HH:mm:ss}".TraducirMensaje());

            foreach (ResultadoIntegridadTabla tabla in _detalle)
            {
                string marcaDvh = tabla.RegistrosAlterados == 0 ? "✔" : "✖";
                string marcaDvv = tabla.DvvCorrecto ? "✔" : "✖";

                lstErrores.Items.Add($"Tabla {tabla.Tabla} (base {tabla.Base})".TraducirMensaje());
                lstErrores.Items.Add($"   {marcaDvh} " +
                    $"{tabla.Registros} registro(s) revisados uno por uno con el DVH, {tabla.RegistrosAlterados} alterado(s)".TraducirMensaje());
                lstErrores.Items.Add($"   {marcaDvv} " + (tabla.DvvCorrecto
                    ? "DVV correcto: no se agregaron ni borraron filas".TraducirMensaje()
                    : "DVV distinto: se agregaron o borraron filas por fuera del sistema".TraducirMensaje()));
            }

            lstErrores.Items.Add("");
            lstErrores.Items.Add("Cómo probarlo".Traducir() + ":");
            lstErrores.Items.Add("   1) " + "En SQL Server Management Studio cambie un dato de una tabla protegida, por ejemplo".Traducir());
            lstErrores.Items.Add("      UPDATE PRAS.dbo.Cliente SET Domicilio = 'Prueba' WHERE IdCliente = 1");
            lstErrores.Items.Add("   2) " + "Vuelva a presionar Verificar nuevamente: el sistema detecta el registro modificado (DVH)".TraducirMensaje());
            lstErrores.Items.Add("   3) " + "Si borra o agrega una fila con SQL, lo detecta el DVV de la tabla".Traducir());
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
