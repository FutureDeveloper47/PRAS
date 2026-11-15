using PRAS.Services.Facade.Extensions;
using PRAS.BLL;
using PRAS.Entidades;

namespace PRAS.UI
{
    /// <summary>
    /// Ficha de un proveedor: Alta (CU Proveedores.001) si se abre con null,
    /// Modificar (CU Proveedores.002) si se abre con el proveedor elegido.
    /// </summary>
    public partial class FrmProveedorEdicion : FormBase
    {
        private readonly ProveedorService _proveedorService = new ProveedorService();
        private readonly Proveedor _proveedor;   // null es alta
        private bool EsAlta => _proveedor == null;

        public FrmProveedorEdicion(Proveedor proveedor)
        {
            InitializeComponent();
            _proveedor = proveedor;
        }

        private void FrmProveedorEdicion_Load(object sender, EventArgs e)
        {
            if (EsAlta)
            {
                Text = "Alta de proveedor".Traducir();   // se traduce acá porque FormBase ya tradujo la pantalla antes del Load
                return;
            }

            Text = "Modificar proveedor".Traducir();   // se traduce acá porque FormBase ya tradujo la pantalla antes del Load
            txtRazonSocial.Text = _proveedor.RazonSocial;
            txtCUIT.Text = _proveedor.CUIT;
            txtDomicilio.Text = _proveedor.Domicilio;
            txtTelefono.Text = _proveedor.Telefono;
            txtEmail.Text = _proveedor.Email;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Armamos un objeto nuevo con lo cargado. Las validaciones las hace la BLL.
            var proveedor = new Proveedor
            {
                IdProveedor = EsAlta ? 0 : _proveedor.IdProveedor,
                RazonSocial = txtRazonSocial.Text,
                CUIT = txtCUIT.Text,
                Domicilio = txtDomicilio.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text
            };

            try
            {
                if (EsAlta)
                {
                    _proveedorService.Alta(proveedor);
                    Mensajes.Info("Proveedor dado de alta correctamente.");
                }
                else
                {
                    _proveedorService.Modificar(proveedor);
                    Mensajes.Info("Proveedor modificado correctamente.");
                }
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
