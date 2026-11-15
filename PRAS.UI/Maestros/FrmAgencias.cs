using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// CU Agencias.004 - Listar. Desde acá se llega a Alta (.001), Modificar (.002)
    /// y Deshabilitar (.003). Misma estructura que FrmClientes.
    /// </summary>
    public partial class FrmAgencias : FormBase
    {
        private readonly AgenciaService _agenciaService = new AgenciaService();

        public FrmAgencias()
        {
            InitializeComponent();
            dgvAgencias.AutoGenerateColumns = false;   // usamos solo las columnas del diseñador
        }

        private void FrmAgencias_Load(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                dgvAgencias.DataSource = _agenciaService.Listar(txtBuscar.Text, chkMostrarDeshabilitadas.Checked);
                ActualizarBotonDeshabilitar();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Devuelve la agencia de la fila seleccionada, o null si no hay ninguna.</summary>
        private Agencia AgenciaSeleccionada()
        {
            return dgvAgencias.CurrentRow?.DataBoundItem as Agencia;
        }

        /// <summary>El mismo botón sirve para deshabilitar o volver a habilitar, según la fila elegida.</summary>
        private void ActualizarBotonDeshabilitar()
        {
            Agencia agencia = AgenciaSeleccionada();
            btnDeshabilitar.Text = ((agencia != null && !agencia.Activo) ? "Habilitar" : "Deshabilitar").Traducir();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void chkMostrarDeshabilitadas_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var frm = new FrmAgenciaEdicion(null);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Agencia agencia = AgenciaSeleccionada();
            if (agencia == null)
            {
                Mensajes.Advertencia("Seleccione una agencia de la lista.");
                return;
            }

            using var frm = new FrmAgenciaEdicion(agencia);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            Agencia agencia = AgenciaSeleccionada();
            if (agencia == null)
            {
                Mensajes.Advertencia("Seleccione una agencia de la lista.");
                return;
            }

            try
            {
                if (agencia.Activo)
                {
                    if (!Mensajes.Confirmar($"¿Deshabilitar la agencia {agencia.RazonSocial}?"))
                        return;
                    _agenciaService.Deshabilitar(agencia.IdAgencia);
                }
                else
                {
                    if (!Mensajes.Confirmar($"¿Volver a habilitar la agencia {agencia.RazonSocial}?"))
                        return;
                    _agenciaService.Habilitar(agencia.IdAgencia);
                }
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void dgvAgencias_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                btnModificar_Click(sender, e);
        }

        private void dgvAgencias_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotonDeshabilitar();
        }
    }
}
