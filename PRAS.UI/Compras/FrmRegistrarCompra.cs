using PRAS.BLL;
using PRAS.Entidades;
using PRAS.BLL.Excepciones;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Pantalla B de EA - Registrar vehículo (CU Compras.001 - Registrar Compra de Vehículo).
    /// Flujo: 1) cargar los datos del auto, 2) buscar al vendedor por DNI (o darlo de alta),
    /// 3) cargar fecha y precio, 4) GUARDAR Y ENVIAR.
    /// Al registrarse, el auto entra al sistema en estado "Comprado".
    /// Los avisos se muestran en los dos carteles de la derecha, como en el diseño.
    /// </summary>
    public partial class FrmRegistrarCompra : FormBase
    {
        private readonly CompraService _compraService = new CompraService();
        private readonly ClienteService _clienteService = new ClienteService();
        private Cliente _vendedor;   // el vendedor elegido (null si todavía no eligió)

        public FrmRegistrarCompra()
        {
            InitializeComponent();
        }

        private void FrmRegistrarCompra_Load(object sender, EventArgs e)
        {
            nudAnio.Maximum = DateTime.Today.Year + 1;
            nudAnio.Value = DateTime.Today.Year;
            dtpFecha.Value = DateTime.Today;
            dtpFecha.MaxDate = DateTime.Today;   // no deja elegir una fecha futura
            txtFormaPago.Text = CompraService.FormaPagoCompra;

            // El estado no lo elige el usuario: todo auto nuevo entra como Comprado.
            txtEstado.Text = EstadoVehiculo.Comprado.ToString();

            MostrarVendedor();
        }

        // Carteles de aviso

        /// <summary>Cartel de arriba: qué dato falta o está mal.</summary>
        private void MostrarFaltanDatos(string detalle)
        {
            pnlExito.Visible = false;
            lblFaltanDatos.Text = "FALTAN COMPLETAR DATOS PARA SU REGISTRO".Traducir() + "\n\n" + detalle.TraducirMensaje();
            pnlFaltanDatos.Visible = true;
        }

        /// <summary>Cartel de abajo: la compra se guardó bien.</summary>
        private void MostrarExito(string detalle)
        {
            pnlFaltanDatos.Visible = false;
            lblExito.Text = "AUTO REGISTRADO EXITOSAMENTE!".Traducir() + "\n\n" + detalle.TraducirMensaje();
            pnlExito.Visible = true;
        }

        private void OcultarCarteles()
        {
            pnlFaltanDatos.Visible = false;
            pnlExito.Visible = false;
        }

        // Vendedor

        private void btnBuscarVendedor_Click(object sender, EventArgs e)
        {
            BuscarVendedor();
        }

        /// <summary>Enter en el DNI = Buscar.</summary>
        private void txtDNIVendedor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;   // para que no suene el "ding" de Windows
                BuscarVendedor();
            }
        }

        /// <summary>Si se cambia el DNI escrito, el vendedor elegido deja de valer.</summary>
        private void txtDNIVendedor_TextChanged(object sender, EventArgs e)
        {
            if (_vendedor != null && txtDNIVendedor.Text.Trim() != _vendedor.DNI)
            {
                _vendedor = null;
                MostrarVendedor();
            }
        }

        private void BuscarVendedor()
        {
            try
            {
                Cliente cliente = _clienteService.ObtenerPorDNI(txtDNIVendedor.Text);

                if (cliente == null)
                {
                    // Si el vendedor no está cargado, le ofrecemos darlo de alta.
                    if (Mensajes.Confirmar("No existe un cliente con ese DNI.\n¿Desea darlo de alta ahora?"))
                        AltaVendedor();
                    return;
                }

                if (!cliente.Activo)
                {
                    MostrarFaltanDatos($"El cliente {cliente.NombreCompleto} está deshabilitado. " +
                                       "Habilítelo desde Maestros > Clientes para poder comprarle.");
                    return;
                }

                _vendedor = cliente;
                MostrarVendedor();
                OcultarCarteles();
                dtpFecha.Focus();
            }
            catch (ReglaNegocioException ex)
            {
                // Por ejemplo, el DNI está vacío o tiene letras.
                MostrarFaltanDatos(ex.Message);
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnNuevoCliente_Click(object sender, EventArgs e)
        {
            AltaVendedor();
        }

        /// <summary>Abre el alta de cliente (la misma pantalla del ABM) y, si se guarda, lo toma como vendedor.</summary>
        private void AltaVendedor()
        {
            using var frm = new FrmClienteEdicion(null);
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;

            _vendedor = frm.ClienteGuardado;
            txtDNIVendedor.Text = _vendedor.DNI;
            MostrarVendedor();
            OcultarCarteles();
            dtpFecha.Focus();
        }

        private void MostrarVendedor()
        {
            if (_vendedor == null)
            {
                lblVendedor.Text = "Sin seleccionar".Traducir();
                lblVendedor.ForeColor = Tema.TextoApagado;
            }
            else
            {
                lblVendedor.Text = _vendedor.NombreCompleto;
                lblVendedor.ForeColor = Tema.Texto;
            }
        }

        // Guardar

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            OcultarCarteles();

            if (_vendedor == null)
            {
                MostrarFaltanDatos("Busque y seleccione al vendedor (DNI VENDEDOR).");
                txtDNIVendedor.Focus();
                return;
            }

            // Armamos la compra con lo cargado. Las validaciones las hace la BLL.
            var compra = new Compra
            {
                IdCliente = _vendedor.IdCliente,
                FechaCompra = dtpFecha.Value.Date,
                PrecioCompra = nudPrecio.Value,
                Observaciones = txtObservaciones.Text,
                Vehiculo = new Vehiculo
                {
                    Dominio = txtDominio.Text,
                    Marca = txtMarca.Text,
                    Modelo = txtModelo.Text,
                    Anio = (int)nudAnio.Value,
                    Color = txtColor.Text,
                    NumeroChasis = txtChasis.Text,
                    NumeroMotor = txtMotor.Text
                }
            };

            if (!Mensajes.Confirmar(
                    $"¿Confirma la compra del vehículo {txtDominio.Text.Trim()} ({txtMarca.Text.Trim()} {txtModelo.Text.Trim()})\n" +
                    $"a {_vendedor.NombreCompleto} por {nudPrecio.Value:C2}?"))
                return;

            try
            {
                _compraService.RegistrarCompra(compra);

                MostrarExito($"{compra.Vehiculo.Dominio} ingresó al sistema en estado \"Comprado\".");
                BloquearPantalla();
            }
            catch (ReglaNegocioException ex)
            {
                // Falta un dato o está mal (lo detectó la BLL), avisamos en el cartel.
                MostrarFaltanDatos(ex.Message);
            }
            catch (Exception ex)
            {
                // Error inesperado (por ejemplo se cayó la base), mostramos la ventana de error.
                Mensajes.Error(ex);
            }
        }

        /// <summary>
        /// Después de guardar, la pantalla queda solo para leer el cartel de éxito:
        /// se bloquean los campos y CANCELAR pasa a ser CERRAR.
        /// Al cerrar devuelve OK, así la pantalla que la abrió se actualiza.
        /// </summary>
        private void BloquearPantalla()
        {
            foreach (Control control in Controls)
            {
                bool esDato = control is TextBox || control is NumericUpDown
                              || control is DateTimePicker || control is Button;
                if (esDato && control != btnCancelar)
                    control.Enabled = false;
            }

            btnCancelar.Text = "CERRAR".Traducir();
            btnCancelar.DialogResult = DialogResult.OK;
            btnCancelar.Focus();
        }
    }
}
