using PRAS.Services.Facade.Extensions;
using PRAS.BLL;
using PRAS.Entidades;

namespace PRAS.UI
{
    /// <summary>
    /// Ficha de un cliente. Se usa para las dos operaciones:
    /// - Alta (CU Clientes.001): se abre con cliente = null.
    /// - Modificar (CU Clientes.002): se abre con el cliente elegido en la grilla.
    /// </summary>
    public partial class FrmClienteEdicion : FormBase
    {
        private readonly ClienteService _clienteService = new ClienteService();
        private readonly Cliente _cliente;   // null es alta
        private bool EsAlta => _cliente == null;

        /// <summary>El cliente tal como quedó guardado (lo usa Registrar Compra al dar de alta al vendedor).</summary>
        public Cliente ClienteGuardado { get; private set; }

        public FrmClienteEdicion(Cliente cliente)
        {
            InitializeComponent();
            _cliente = cliente;
        }

        private void FrmClienteEdicion_Load(object sender, EventArgs e)
        {
            if (EsAlta)
            {
                Text = "Alta de cliente".Traducir();   // se traduce acá porque FormBase ya tradujo la pantalla antes del Load
                return;
            }

            Text = "Modificar cliente".Traducir();   // se traduce acá porque FormBase ya tradujo la pantalla antes del Load
            txtNombre.Text = _cliente.Nombre;
            txtApellido.Text = _cliente.Apellido;
            txtDNI.Text = _cliente.DNI;
            txtCUIT.Text = _cliente.CUIT;
            txtDomicilio.Text = _cliente.Domicilio;
            txtLocalidad.Text = _cliente.Localidad;
            txtTelefono.Text = _cliente.Telefono;
            txtEmail.Text = _cliente.Email;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Armamos un objeto nuevo con lo cargado. Las validaciones las hace la BLL.
            var cliente = new Cliente
            {
                IdCliente = EsAlta ? 0 : _cliente.IdCliente,
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                DNI = txtDNI.Text,
                CUIT = txtCUIT.Text,
                Domicilio = txtDomicilio.Text,
                Localidad = txtLocalidad.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text
            };

            try
            {
                if (EsAlta)
                {
                    _clienteService.Alta(cliente);
                    Mensajes.Info("Cliente dado de alta correctamente.");
                }
                else
                {
                    _clienteService.Modificar(cliente);
                    Mensajes.Info("Cliente modificado correctamente.");
                }
                ClienteGuardado = cliente;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
