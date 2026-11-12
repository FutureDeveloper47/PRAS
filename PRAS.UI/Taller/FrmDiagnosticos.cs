using System.Diagnostics;
using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Taller - Registrar diagnóstico / Consultar diagnóstico (la misma pantalla en dos modos).
    /// A la izquierda los autos del taller con su último diagnóstico; a la derecha el detalle,
    /// las evidencias (fotos, videos o documentos) y la finalización del diagnóstico.
    /// En modo consulta todo queda de solo lectura (lo usa, por ejemplo, el mecánico).
    /// </summary>
    public partial class FrmDiagnosticos : FormBase
    {
        private readonly DiagnosticoService _diagnosticoService = new DiagnosticoService();
        private readonly bool _soloLectura;
        private Diagnostico _actual;

        public FrmDiagnosticos(bool soloLectura)
        {
            InitializeComponent();
            _soloLectura = soloLectura;

            // El título va acá y no en el Load porque FormBase lo traduce y arma
            // la franja azul antes de que corra el Load.
            if (soloLectura)
                Text = "Consultar diagnóstico";
        }

        private void FrmDiagnosticos_Load(object sender, EventArgs e)
        {
            if (_soloLectura)
            {
                txtDanios.ReadOnly = true;
                txtObservaciones.ReadOnly = true;
                chkRequiereReparacion.Enabled = false;
                btnGuardar.Visible = false;
                txtDescripcionEvidencia.Visible = false;
                btnAgregarEvidencia.Visible = false;
                grpFinalizar.Visible = false;
            }
            else
            {
                chkMostrarTodos.Visible = false;
                try
                {
                    cboMecanico.DisplayMember = nameof(Mecanico.NombreCompleto);
                    cboMecanico.ValueMember = nameof(Mecanico.IdMecanico);
                    cboMecanico.DataSource = _diagnosticoService.ListarMecanicos();
                    cboPrioridad.Items.AddRange(OrdenTrabajoService.Prioridades);
                    cboPrioridad.SelectedItem = TallerService.PrioridadMedia;
                }
                catch (Exception ex)
                {
                    Mensajes.Error(ex);
                }
            }

            CargarGrilla(null);
        }

        private void CargarGrilla(int? idSeleccionar)
        {
            try
            {
                List<Diagnostico> lista = _soloLectura
                    ? _diagnosticoService.ListarParaConsulta(chkMostrarTodos.Checked)
                    : _diagnosticoService.ListarParaRegistrar();

                dgvDiagnosticos.Rows.Clear();
                foreach (Diagnostico d in lista)
                {
                    int fila = dgvDiagnosticos.Rows.Add(
                        d.Vehiculo.Dominio,
                        $"{d.Vehiculo.Marca} {d.Vehiculo.Modelo} ({d.Vehiculo.Anio})",
                        d.FechaDiagnostico.ToString("dd/MM/yyyy"),
                        d.EstadoDiagnostico,
                        VehiculoService.TextoEstado(d.Vehiculo.EstadoVehiculo));
                    dgvDiagnosticos.Rows[fila].Tag = d.IdDiagnostico;
                    if (d.IdDiagnostico == idSeleccionar)
                        dgvDiagnosticos.CurrentCell = dgvDiagnosticos.Rows[fila].Cells[0];
                }

                if (lista.Count == 0)
                    MostrarDetalle(null);
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void chkMostrarTodos_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla(null);
        }

        private void dgvDiagnosticos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDiagnosticos.CurrentRow?.Tag is int id)
            {
                try
                {
                    MostrarDetalle(_diagnosticoService.Obtener(id));
                }
                catch (Exception ex)
                {
                    Mensajes.Error(ex);
                }
            }
        }

        private void MostrarDetalle(Diagnostico diagnostico)
        {
            _actual = diagnostico;
            dgvEvidencias.Rows.Clear();

            if (diagnostico == null)
            {
                lblVehiculo.Text = "Seleccione un vehículo de la lista.".Traducir();
                txtDanios.Clear();
                txtObservaciones.Clear();
                return;
            }

            lblVehiculo.Text = $"{diagnostico.Vehiculo.Descripcion}  -  {VehiculoService.TextoEstado(diagnostico.Vehiculo.EstadoVehiculo).Traducir()}";
            txtDanios.Text = diagnostico.DescripcionDanios;
            txtObservaciones.Text = diagnostico.Observaciones;
            chkRequiereReparacion.Checked = diagnostico.RequiereReparacion;

            foreach (EvidenciaVehiculo evidencia in diagnostico.Evidencias)
            {
                int fila = dgvEvidencias.Rows.Add(evidencia.TipoEvidencia, evidencia.Descripcion,
                    evidencia.FechaCarga.ToString("dd/MM/yyyy HH:mm"), Path.GetFileName(evidencia.UbicacionArchivo));
                dgvEvidencias.Rows[fila].Tag = evidencia;
            }

            // Solo se puede terminar un diagnóstico pendiente de un auto que está En diagnóstico.
            if (!_soloLectura)
            {
                grpFinalizar.Enabled = diagnostico.EstadoDiagnostico != Estados.DiagnosticoFinalizado
                                       && diagnostico.Vehiculo.EstadoVehiculo == EstadoVehiculo.EnDiagnostico;
            }
        }

        /// <summary>Arma el diagnóstico con lo que el usuario escribió en pantalla.</summary>
        private Diagnostico LeerPantalla()
        {
            return new Diagnostico
            {
                IdDiagnostico = _actual?.IdDiagnostico ?? 0,
                DescripcionDanios = txtDanios.Text,
                Observaciones = txtObservaciones.Text,
                RequiereReparacion = chkRequiereReparacion.Checked
            };
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Diagnostico diagnostico = LeerPantalla();
                _diagnosticoService.Guardar(diagnostico);
                Mensajes.Info("Diagnóstico guardado.".Traducir());
                CargarGrilla(diagnostico.IdDiagnostico);
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnRegistrarFinalizacion_Click(object sender, EventArgs e)
        {
            if (_actual == null)
            {
                Mensajes.Advertencia("Seleccione un vehículo de la lista.".Traducir());
                return;
            }

            string pregunta = chkRequiereReparacion.Checked
                ? $"¿Finalizar el diagnóstico de {_actual.Vehiculo.Dominio}? Se crea la orden de trabajo y el auto pasa a \"En reparación\"."
                : $"¿Finalizar el diagnóstico de {_actual.Vehiculo.Dominio}? Como no requiere reparación, el auto pasa directo a \"Reparado\".";
            if (!Mensajes.Confirmar(pregunta))
                return;

            try
            {
                _diagnosticoService.Finalizar(LeerPantalla(), cboMecanico.SelectedValue as int?, cboPrioridad.SelectedItem as string);
                Mensajes.Info("Diagnóstico finalizado.".Traducir());
                CargarGrilla(null);
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        // Evidencias

        private void btnAgregarEvidencia_Click(object sender, EventArgs e)
        {
            if (_actual == null)
            {
                Mensajes.Advertencia("Seleccione un vehículo de la lista.".Traducir());
                return;
            }

            using var dialogo = new OpenFileDialog
            {
                Title = "Seleccionar evidencia".Traducir(),
                Filter = "Fotos, videos y documentos|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.mp4;*.avi;*.mov;*.wmv;*.pdf;*.doc;*.docx|Todos los archivos|*.*"
            };
            if (dialogo.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                _diagnosticoService.AgregarEvidencia(_actual.IdDiagnostico, dialogo.FileName, txtDescripcionEvidencia.Text);
                txtDescripcionEvidencia.Clear();
                MostrarDetalle(_diagnosticoService.Obtener(_actual.IdDiagnostico));
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnVerEvidencia_Click(object sender, EventArgs e)
        {
            if (!(dgvEvidencias.CurrentRow?.Tag is EvidenciaVehiculo evidencia))
            {
                Mensajes.Advertencia("Seleccione una evidencia de la lista.".Traducir());
                return;
            }

            string ruta = _diagnosticoService.RutaCompleta(evidencia);
            if (!File.Exists(ruta))
            {
                Mensajes.Advertencia("No se encontró el archivo de la evidencia:".Traducir() + "\n" + ruta);
                return;
            }

            try
            {
                // Lo abre con el programa que Windows tenga para ese archivo (visor de fotos, reproductor, etc.).
                Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Mensajes.Error(new PRAS.Services.Exceptions.Base.UIException("No se pudo abrir el archivo de la evidencia.", ex));
            }
        }

        private void dgvEvidencias_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                btnVerEvidencia_Click(sender, e);
        }
    }
}
