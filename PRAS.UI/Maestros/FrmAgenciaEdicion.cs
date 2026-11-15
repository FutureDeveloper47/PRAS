using PRAS.Services.Facade.Extensions;
using PRAS.BLL;
using PRAS.Entidades;

namespace PRAS.UI
{
    /// <summary>
    /// Ficha de una agencia: Alta (CU Agencias.001) si se abre con null,
    /// Modificar (CU Agencias.002) si se abre con la agencia elegida.
    /// </summary>
    public partial class FrmAgenciaEdicion : FormBase
    {
        private readonly AgenciaService _agenciaService = new AgenciaService();
        private readonly Agencia _agencia;   // null es alta
        private bool EsAlta => _agencia == null;

        public FrmAgenciaEdicion(Agencia agencia)
        {
            InitializeComponent();
            _agencia = agencia;
        }

        private void FrmAgenciaEdicion_Load(object sender, EventArgs e)
        {
            if (EsAlta)
            {
                Text = "Alta de agencia".Traducir();   // se traduce acá porque FormBase ya tradujo la pantalla antes del Load
                return;
            }

            Text = "Modificar agencia".Traducir();   // se traduce acá porque FormBase ya tradujo la pantalla antes del Load
            txtRazonSocial.Text = _agencia.RazonSocial;
            txtCUIT.Text = _agencia.CUIT;
            txtDomicilio.Text = _agencia.Domicilio;
            txtLocalidad.Text = _agencia.Localidad;
            txtTelefono.Text = _agencia.Telefono;
            txtEmail.Text = _agencia.Email;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Armamos un objeto nuevo con lo cargado. Las validaciones las hace la BLL.
            var agencia = new Agencia
            {
                IdAgencia = EsAlta ? 0 : _agencia.IdAgencia,
                RazonSocial = txtRazonSocial.Text,
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
                    _agenciaService.Alta(agencia);
                    Mensajes.Info("Agencia dada de alta correctamente.");
                }
                else
                {
                    _agenciaService.Modificar(agencia);
                    Mensajes.Info("Agencia modificada correctamente.");
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
