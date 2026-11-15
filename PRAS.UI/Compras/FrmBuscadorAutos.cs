using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Pantalla C de EA - Buscador de autos.
    /// Filtra por estado y por patente (a medida que se escribe) y permite
    /// Ver Detalle, Modificar y Eliminar el auto elegido en la lista.
    /// </summary>
    public partial class FrmBuscadorAutos : FormBase
    {
        private readonly VehiculoService _vehiculoService = new VehiculoService();
        private bool _cargando = true;   // para que no busque varias veces mientras se arma la pantalla

        public FrmBuscadorAutos()
        {
            InitializeComponent();
        }

        private void FrmBuscadorAutos_Load(object sender, EventArgs e)
        {
            // Opción 0 es "Todos" y después un renglón por cada estado (1 a 7).
            cboEstado.Items.Add("Todos");
            foreach (EstadoVehiculo estado in Enum.GetValues<EstadoVehiculo>())
                cboEstado.Items.Add(VehiculoService.TextoEstado(estado));
            cboEstado.SelectedIndex = 0;

            _cargando = false;
            CargarGrilla();
        }

        /// <summary>
        /// Estado elegido en el combo, o null si eligió "Todos".
        /// Funciona porque el renglón N del combo corresponde al estado N del enum.
        /// </summary>
        private EstadoVehiculo? EstadoElegido()
        {
            if (cboEstado.SelectedIndex <= 0)
                return null;
            return (EstadoVehiculo)cboEstado.SelectedIndex;
        }

        private void CargarGrilla()
        {
            if (_cargando)
                return;

            try
            {
                List<Vehiculo> vehiculos = _vehiculoService.Listar(EstadoElegido(), txtPatente.Text);

                dgvAutos.Rows.Clear();
                foreach (Vehiculo vehiculo in vehiculos)
                {
                    int fila = dgvAutos.Rows.Add(
                        vehiculo.Dominio,
                        vehiculo.Marca,
                        vehiculo.Modelo,
                        vehiculo.Anio,
                        vehiculo.Color,
                        VehiculoService.TextoEstado(vehiculo.EstadoVehiculo));

                    // En el Tag de la fila guardamos el objeto, para saber cuál eligió.
                    dgvAutos.Rows[fila].Tag = vehiculo;
                }

                lblCantidad.Text = $"{vehiculos.Count} vehículo(s)".TraducirMensaje();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void cboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        /// <summary>Busca a medida que se escribe la patente.</summary>
        private void txtPatente_TextChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        /// <summary>Devuelve el auto de la fila seleccionada, o null si no hay ninguna.</summary>
        private Vehiculo VehiculoSeleccionado()
        {
            Vehiculo vehiculo = dgvAutos.CurrentRow?.Tag as Vehiculo;
            if (vehiculo == null)
                Mensajes.Advertencia("Seleccione un auto de la lista.");
            return vehiculo;
        }

        // Botones

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            VerDetalle();
        }

        /// <summary>Doble clic en una fila = Ver Detalle.</summary>
        private void dgvAutos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                VerDetalle();
        }

        private void VerDetalle()
        {
            Vehiculo vehiculo = VehiculoSeleccionado();
            if (vehiculo == null)
                return;

            using var frm = new FrmVehiculoEdicion(vehiculo, soloLectura: true);
            frm.ShowDialog(this);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Vehiculo vehiculo = VehiculoSeleccionado();
            if (vehiculo == null)
                return;

            using var frm = new FrmVehiculoEdicion(vehiculo, soloLectura: false);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Vehiculo vehiculo = VehiculoSeleccionado();
            if (vehiculo == null)
                return;

            if (!Mensajes.Confirmar(
                    $"¿Eliminar el vehículo {vehiculo.Descripcion}?\n\n" +
                    "También se borra su compra. Use esta opción solo para corregir una carga equivocada."))
                return;

            try
            {
                _vehiculoService.Eliminar(vehiculo.IdVehiculo);
                Mensajes.Info($"El vehículo {vehiculo.Dominio} fue eliminado.");
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
