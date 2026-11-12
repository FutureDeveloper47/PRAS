using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Taller - Órdenes de trabajo (gestionar) y Consultar órdenes de trabajo (la misma pantalla).
    /// Arriba las órdenes; abajo las tareas de la orden elegida.
    /// En modo gestión además se cambia la prioridad, se agregan tareas y se finaliza la orden.
    /// </summary>
    public partial class FrmOrdenesTrabajo : FormBase
    {
        private const string FiltroAbiertas = "Abiertas";
        private const string FiltroFinalizadas = "Finalizadas";
        private const string FiltroTodas = "Todas";

        private readonly OrdenTrabajoService _ordenService = new OrdenTrabajoService();
        private readonly bool _gestionar;

        public FrmOrdenesTrabajo(bool gestionar)
        {
            InitializeComponent();
            _gestionar = gestionar;

            // El título va acá y no en el Load porque FormBase lo traduce y arma
            // la franja azul antes de que corra el Load.
            if (!gestionar)
                Text = "Consultar órdenes de trabajo";
        }

        private void FrmOrdenesTrabajo_Load(object sender, EventArgs e)
        {
            if (!_gestionar)
            {
                grpGestion.Visible = false;
                dgvTareas.Height += grpGestion.Height + 10;
            }
            else
            {
                try
                {
                    cboPrioridad.Items.AddRange(OrdenTrabajoService.Prioridades);
                    cboMecanico.DisplayMember = nameof(Mecanico.NombreCompleto);
                    cboMecanico.ValueMember = nameof(Mecanico.IdMecanico);
                    cboMecanico.DataSource = _ordenService.ListarMecanicos();
                }
                catch (Exception ex)
                {
                    Mensajes.Error(ex);
                }
            }

            cboEstado.Items.AddRange(new object[] { FiltroAbiertas.Traducir(), FiltroFinalizadas.Traducir(), FiltroTodas.Traducir() });
            cboEstado.SelectedIndex = 0;   // esto carga la grilla
        }

        private string EstadoElegido()
        {
            switch (cboEstado.SelectedIndex)
            {
                case 0: return Estados.OrdenAbierta;
                case 1: return Estados.OrdenFinalizada;
                default: return null;
            }
        }

        private void CargarOrdenes(int? idSeleccionar = null)
        {
            try
            {
                List<OrdenTrabajo> ordenes = _ordenService.Listar(EstadoElegido());

                dgvOrdenes.Rows.Clear();
                foreach (OrdenTrabajo o in ordenes)
                {
                    int fila = dgvOrdenes.Rows.Add(o.IdOrdenTrabajo, o.FechaCreacion.ToString("dd/MM/yyyy"),
                        o.Vehiculo.Dominio, $"{o.Vehiculo.Marca} {o.Vehiculo.Modelo} ({o.Vehiculo.Anio})",
                        o.Prioridad.Traducir(), o.EstadoOrden.Traducir(), o.Mecanicos, o.MontoPresupuesto.ToString("C2"));
                    dgvOrdenes.Rows[fila].Tag = o;
                    if (o.IdOrdenTrabajo == idSeleccionar)
                        dgvOrdenes.CurrentCell = dgvOrdenes.Rows[fila].Cells[0];
                }

                if (ordenes.Count == 0)
                    dgvTareas.Rows.Clear();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private OrdenTrabajo OrdenSeleccionada => dgvOrdenes.CurrentRow?.Tag as OrdenTrabajo;

        private void cboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarOrdenes();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarOrdenes(OrdenSeleccionada?.IdOrdenTrabajo);
        }

        private void dgvOrdenes_SelectionChanged(object sender, EventArgs e)
        {
            OrdenTrabajo orden = OrdenSeleccionada;
            dgvTareas.Rows.Clear();
            if (orden == null)
                return;

            if (_gestionar)
            {
                cboPrioridad.SelectedItem = orden.Prioridad;
                grpGestion.Enabled = orden.EstadoOrden == Estados.OrdenAbierta;
            }

            try
            {
                foreach (TareaOrdenTrabajo t in _ordenService.ListarTareas(orden.IdOrdenTrabajo))
                {
                    dgvTareas.Rows.Add(t.IdTarea, t.Mecanico.NombreCompleto, t.DescripcionTarea,
                        t.CostoEstimado.ToString("C2"), t.EstadoTarea.Traducir(),
                        t.FechaInicio?.ToString("dd/MM/yyyy"), t.FechaFin?.ToString("dd/MM/yyyy"));
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private bool HayOrdenSeleccionada()
        {
            if (OrdenSeleccionada != null)
                return true;
            Mensajes.Advertencia("Seleccione una orden de trabajo de la lista.".Traducir());
            return false;
        }

        private void btnCambiarPrioridad_Click(object sender, EventArgs e)
        {
            if (!HayOrdenSeleccionada())
                return;

            try
            {
                int id = OrdenSeleccionada.IdOrdenTrabajo;
                _ordenService.CambiarPrioridad(id, cboPrioridad.SelectedItem as string);
                CargarOrdenes(id);
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnAgregarTarea_Click(object sender, EventArgs e)
        {
            if (!HayOrdenSeleccionada())
                return;

            try
            {
                int id = OrdenSeleccionada.IdOrdenTrabajo;
                _ordenService.AgregarTarea(id, cboMecanico.SelectedValue as int? ?? 0, txtDescripcionTarea.Text, numCosto.Value);
                txtDescripcionTarea.Clear();
                numCosto.Value = 0;
                CargarOrdenes(id);
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnRegistrarFinOrden_Click(object sender, EventArgs e)
        {
            if (!HayOrdenSeleccionada())
                return;

            OrdenTrabajo orden = OrdenSeleccionada;
            if (!Mensajes.Confirmar($"¿Finalizar la OT {orden.IdOrdenTrabajo} ({orden.Vehiculo.Dominio})? El auto queda \"Reparado\"."))
                return;

            try
            {
                _ordenService.Finalizar(orden.IdOrdenTrabajo);
                Mensajes.Info("Orden de trabajo finalizada. El auto quedó Reparado.".Traducir());
                CargarOrdenes();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
