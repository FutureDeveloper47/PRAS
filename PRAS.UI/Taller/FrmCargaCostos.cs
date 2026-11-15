using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Pantalla E de EA - Carga de costos.
    /// Se elige un auto "En reparación" y se le cargan gastos de Repuesto o Mano de obra.
    /// Cada gasto es un renglón del presupuesto del auto; el TOTAL ACUMULADO es la suma.
    /// "Agregar gasto" lo suma a la lista; recién con "Guardar" se graba en la base.
    /// "Finalizar reparación" pasa el auto a "Reparado".
    /// </summary>
    public partial class FrmCargaCostos : FormBase
    {
        private readonly TallerService _tallerService = new TallerService();

        private Presupuesto _presupuesto;   // auto elegido (con su presupuesto)
        private List<DetallePresupuesto> _guardados = new List<DetallePresupuesto>();   // ya están en la base
        private readonly List<DetallePresupuesto> _nuevos = new List<DetallePresupuesto>();   // falta tocar Guardar

        public FrmCargaCostos()
        {
            InitializeComponent();
        }

        private void FrmCargaCostos_Load(object sender, EventArgs e)
        {
            cboTipoGasto.Items.Add(TallerService.GastoRepuesto);
            cboTipoGasto.Items.Add(TallerService.GastoManoDeObra);
            cboTipoGasto.SelectedIndex = 0;

            cboAuto.DisplayMember = nameof(Presupuesto.DescripcionVehiculo);
            CargarAutos();
        }

        // Autos

        /// <summary>Llena el combo con los autos en reparación y elige el primero.</summary>
        private void CargarAutos()
        {
            try
            {
                List<Presupuesto> presupuestos = _tallerService.ListarPresupuestosEnReparacion();
                cboAuto.DataSource = presupuestos;

                if (presupuestos.Count == 0)
                {
                    cboAuto.SelectedIndex = -1;
                    _presupuesto = null;
                    Mensajes.Info("No hay autos \"En reparación\".\n" +
                                  "Primero ingrese uno al taller desde Taller > Asignación de presupuesto.");
                }
                else
                {
                    cboAuto.SelectedIndex = 0;
                    _presupuesto = presupuestos[0];
                }

                CargarGastos();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>
        /// El usuario eligió otro auto. Si había gastos sin guardar, se pregunta antes de perderlos.
        /// (SelectionChangeCommitted solo se dispara cuando lo cambia el usuario, no el código.)
        /// </summary>
        private void cboAuto_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_nuevos.Count > 0 &&
                !Mensajes.Confirmar("Hay gastos sin guardar para el auto anterior. ¿Descartarlos?"))
            {
                cboAuto.SelectedItem = _presupuesto;   // vuelve al auto que estaba antes
                return;
            }

            _presupuesto = cboAuto.SelectedItem as Presupuesto;
            _nuevos.Clear();
            CargarGastos();
        }

        // Gastos

        /// <summary>Trae de la base los gastos ya guardados del auto elegido.</summary>
        private void CargarGastos()
        {
            try
            {
                _guardados = _presupuesto == null
                    ? new List<DetallePresupuesto>()
                    : _tallerService.ListarGastos(_presupuesto.IdPresupuesto);
            }
            catch (Exception ex)
            {
                _guardados = new List<DetallePresupuesto>();
                Mensajes.Error(ex);
            }
            MostrarGastos();
        }

        /// <summary>Dibuja la grilla (guardados + nuevos) y el TOTAL ACUMULADO.</summary>
        private void MostrarGastos()
        {
            dgvGastos.Rows.Clear();
            decimal total = 0;

            foreach (DetallePresupuesto gasto in _guardados)
            {
                AgregarFila(gasto, "Guardado");
                total += gasto.Subtotal;
            }
            foreach (DetallePresupuesto gasto in _nuevos)
            {
                AgregarFila(gasto, "Nuevo (sin guardar)");
                total += gasto.Subtotal;
            }

            lblTotal.Text = $"{"TOTAL ACUMULADO".Traducir()} = {total:C2}";

            bool hayAuto = _presupuesto != null;
            btnAgregar.Enabled = hayAuto;
            btnGuardar.Enabled = hayAuto && _nuevos.Count > 0;
            btnQuitar.Enabled = _nuevos.Count > 0;
            btnFinalizar.Enabled = hayAuto;
        }

        private void AgregarFila(DetallePresupuesto gasto, string estado)
        {
            int fila = dgvGastos.Rows.Add(
                gasto.DescripcionItem,
                gasto.Cantidad,
                gasto.PrecioUnitario.ToString("C2"),
                gasto.Subtotal.ToString("C2"),
                estado);
            dgvGastos.Rows[fila].Tag = gasto;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // La BLL arma y valida el gasto. Todavía no se guarda en la base.
                DetallePresupuesto gasto = _tallerService.CrearGasto(
                    cboTipoGasto.Text, txtDescripcion.Text, (int)nudCantidad.Value, nudPrecio.Value);

                _nuevos.Add(gasto);
                MostrarGastos();

                // Dejamos los campos listos para el próximo gasto.
                txtDescripcion.Clear();
                nudCantidad.Value = 1;
                nudPrecio.Value = 0;
                txtDescripcion.Focus();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Quita de la lista un gasto nuevo (los ya guardados no se pueden quitar).</summary>
        private void btnQuitar_Click(object sender, EventArgs e)
        {
            DetallePresupuesto gasto = dgvGastos.CurrentRow?.Tag as DetallePresupuesto;
            if (gasto == null || !_nuevos.Contains(gasto))
            {
                Mensajes.Advertencia("Seleccione en la lista un gasto \"Nuevo (sin guardar)\".");
                return;
            }

            _nuevos.Remove(gasto);
            MostrarGastos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                _tallerService.GuardarGastos(_presupuesto.IdPresupuesto, _nuevos);
                _nuevos.Clear();
                CargarGastos();
                Mensajes.Info("Los gastos se guardaron correctamente.");
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        // Fin de la reparación

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (_nuevos.Count > 0)
            {
                Mensajes.Advertencia("Hay gastos sin guardar. Guárdelos o quítelos antes de finalizar.");
                return;
            }

            if (!Mensajes.Confirmar($"¿Finalizar la reparación de {_presupuesto.DescripcionVehiculo}?\n\n" +
                                    "El auto pasa a \"Reparado\" y ya no se le pueden cargar costos."))
                return;

            try
            {
                _tallerService.FinalizarReparacion(_presupuesto.IdVehiculo);
                Mensajes.Info($"La reparación de {_presupuesto.Vehiculo.Dominio} quedó finalizada. Estado: Reparado.");
                CargarAutos();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Al cerrar (Cancelar o la X), si quedaron gastos sin guardar se pregunta.</summary>
        private void FrmCargaCostos_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_nuevos.Count > 0 &&
                !Mensajes.Confirmar("Hay gastos sin guardar. ¿Salir igual y descartarlos?"))
            {
                e.Cancel = true;
            }
        }
    }
}
