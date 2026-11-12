using PRAS.BLL;
using PRAS.Entidades;

namespace PRAS.UI
{
    /// <summary>
    /// Pantalla D de EA - Asignación de presupuesto (ingreso del auto al taller).
    /// Se elige un auto "Comprado", el mecánico, se escribe el diagnóstico inicial y la prioridad.
    /// Al guardar, el sistema crea la recepción, el diagnóstico, el presupuesto (vacío),
    /// la orden de trabajo y la tarea del mecánico, y el auto pasa a "En reparación".
    /// </summary>
    public partial class FrmAsignacionPresupuesto : FormBase
    {
        private readonly TallerService _tallerService = new TallerService();

        public FrmAsignacionPresupuesto()
        {
            InitializeComponent();
        }

        private void FrmAsignacionPresupuesto_Load(object sender, EventArgs e)
        {
            try
            {
                List<Vehiculo> vehiculos = _tallerService.ListarVehiculosParaIngreso();
                List<Mecanico> mecanicos = _tallerService.ListarMecanicos();

                // DataSource es la lista, DisplayMember lo que se ve y ValueMember el Id que se guarda.
                cboVehiculo.DisplayMember = nameof(Vehiculo.Descripcion);
                cboVehiculo.ValueMember = nameof(Vehiculo.IdVehiculo);
                cboVehiculo.DataSource = vehiculos;
                cboVehiculo.SelectedIndex = -1;   // que el usuario elija a propósito

                cboMecanico.DisplayMember = nameof(Mecanico.NombreCompleto);
                cboMecanico.ValueMember = nameof(Mecanico.IdMecanico);
                cboMecanico.DataSource = mecanicos;
                cboMecanico.SelectedIndex = -1;

                if (vehiculos.Count == 0)
                {
                    Mensajes.Info("No hay autos en estado \"Comprado\" para ingresar al taller.\n" +
                                  "Primero registre una compra.");
                    btnGuardar.Enabled = false;
                }
                else if (mecanicos.Count == 0)
                {
                    Mensajes.Info("No hay mecánicos habilitados. Cargue uno desde Maestros > Mecánicos.");
                    btnGuardar.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
                btnGuardar.Enabled = false;
            }
        }

        /// <summary>Qué botón de prioridad está apretado (Baja / Alta / Media).</summary>
        private string PrioridadElegida()
        {
            if (rbBaja.Checked) return TallerService.PrioridadBaja;
            if (rbAlta.Checked) return TallerService.PrioridadAlta;
            return TallerService.PrioridadMedia;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // SelectedValue es el Id. Si no eligió nada viene null y mandamos 0 (la BLL avisa).
            int idVehiculo = cboVehiculo.SelectedValue is int v ? v : 0;
            int idMecanico = cboMecanico.SelectedValue is int m ? m : 0;

            try
            {
                _tallerService.RegistrarIngreso(idVehiculo, idMecanico, txtDiagnostico.Text, PrioridadElegida());

                Mensajes.Info($"El vehículo {cboVehiculo.Text} ingresó al taller.\n\n" +
                              $"Mecánico: {cboMecanico.Text}  -  Prioridad: {PrioridadElegida()}\n" +
                              "Estado: En reparación. Los costos se cargan en Taller > Carga de costos.");
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
