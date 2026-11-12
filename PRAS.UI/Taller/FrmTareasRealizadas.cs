using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Taller - Registrar tareas realizadas.
    /// El mecánico ve SUS tareas de las órdenes abiertas (las más urgentes primero),
    /// las inicia y las marca como finalizadas. Un administrador ve las de todos.
    /// </summary>
    public partial class FrmTareasRealizadas : FormBase
    {
        private readonly OrdenTrabajoService _ordenService = new OrdenTrabajoService();

        public FrmTareasRealizadas()
        {
            InitializeComponent();
        }

        private void FrmTareasRealizadas_Load(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                dgvTareas.Rows.Clear();
                foreach (TareaOrdenTrabajo t in _ordenService.ListarTareasParaRegistrar(chkMostrarFinalizadas.Checked))
                {
                    int fila = dgvTareas.Rows.Add(t.PrioridadOrden.Traducir(), t.IdOrdenTrabajo, t.Vehiculo.Dominio,
                        $"{t.Vehiculo.Marca} {t.Vehiculo.Modelo}", t.Mecanico.NombreCompleto, t.DescripcionTarea,
                        t.EstadoTarea.Traducir(), t.FechaInicio?.ToString("dd/MM/yyyy HH:mm"), t.FechaFin?.ToString("dd/MM/yyyy HH:mm"));
                    dgvTareas.Rows[fila].Tag = t;
                }
                ActualizarBotones();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private TareaOrdenTrabajo TareaSeleccionada => dgvTareas.CurrentRow?.Tag as TareaOrdenTrabajo;

        /// <summary>Solo se habilita lo que tiene sentido según el estado de la tarea elegida.</summary>
        private void ActualizarBotones()
        {
            TareaOrdenTrabajo tarea = TareaSeleccionada;
            btnIniciarTarea.Enabled = tarea != null && tarea.EstadoTarea == Estados.TareaPendiente;
            btnRegistrarFinalizada.Enabled = tarea != null && tarea.EstadoTarea != Estados.TareaFinalizada;
        }

        private void dgvTareas_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotones();
        }

        private void chkMostrarFinalizadas_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void btnIniciarTarea_Click(object sender, EventArgs e)
        {
            TareaOrdenTrabajo tarea = TareaSeleccionada;
            if (tarea == null)
                return;

            try
            {
                _ordenService.IniciarTarea(tarea.IdTarea);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnRegistrarFinalizada_Click(object sender, EventArgs e)
        {
            TareaOrdenTrabajo tarea = TareaSeleccionada;
            if (tarea == null)
                return;

            if (!Mensajes.Confirmar($"¿Marcar como finalizada la tarea \"{tarea.DescripcionTarea}\" ({tarea.Vehiculo.Dominio})?"))
                return;

            try
            {
                _ordenService.FinalizarTarea(tarea.IdTarea);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
