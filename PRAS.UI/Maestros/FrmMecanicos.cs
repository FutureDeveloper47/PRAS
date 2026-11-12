using PRAS.BLL;
using PRAS.Entidades;

namespace PRAS.UI
{
    /// <summary>
    /// CU Mecanicos.004 - Listar. Desde acá se llega a Alta (.001), Modificar (.002)
    /// y Deshabilitar (.003). Misma estructura que FrmClientes.
    /// </summary>
    public partial class FrmMecanicos : FormBase
    {
        private readonly MecanicoService _mecanicoService = new MecanicoService();

        public FrmMecanicos()
        {
            InitializeComponent();
            dgvMecanicos.AutoGenerateColumns = false;   // usamos solo las columnas del diseñador
        }

        private void FrmMecanicos_Load(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                dgvMecanicos.DataSource = _mecanicoService.Listar(txtBuscar.Text, chkMostrarDeshabilitados.Checked);
                ActualizarBotonDeshabilitar();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Devuelve el mecánico de la fila seleccionada, o null si no hay ninguna.</summary>
        private Mecanico MecanicoSeleccionado()
        {
            return dgvMecanicos.CurrentRow?.DataBoundItem as Mecanico;
        }

        /// <summary>El mismo botón sirve para deshabilitar o volver a habilitar, según la fila elegida.</summary>
        private void ActualizarBotonDeshabilitar()
        {
            Mecanico mecanico = MecanicoSeleccionado();
            btnDeshabilitar.Text = (mecanico != null && !mecanico.Activo) ? "Habilitar" : "Deshabilitar";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void chkMostrarDeshabilitados_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var frm = new FrmMecanicoEdicion(null);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Mecanico mecanico = MecanicoSeleccionado();
            if (mecanico == null)
            {
                Mensajes.Advertencia("Seleccione un mecánico de la lista.");
                return;
            }

            using var frm = new FrmMecanicoEdicion(mecanico);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            Mecanico mecanico = MecanicoSeleccionado();
            if (mecanico == null)
            {
                Mensajes.Advertencia("Seleccione un mecánico de la lista.");
                return;
            }

            try
            {
                if (mecanico.Activo)
                {
                    if (!Mensajes.Confirmar($"¿Deshabilitar al mecánico {mecanico.NombreCompleto}?"))
                        return;
                    _mecanicoService.Deshabilitar(mecanico.IdMecanico);
                }
                else
                {
                    if (!Mensajes.Confirmar($"¿Volver a habilitar al mecánico {mecanico.NombreCompleto}?"))
                        return;
                    _mecanicoService.Habilitar(mecanico.IdMecanico);
                }
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void dgvMecanicos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                btnModificar_Click(sender, e);
        }

        private void dgvMecanicos_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotonDeshabilitar();
        }
    }
}
