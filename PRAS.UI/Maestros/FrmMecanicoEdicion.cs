using PRAS.BLL;
using PRAS.Entidades;

namespace PRAS.UI
{
    /// <summary>
    /// Ficha de un mecánico: Alta (CU Mecanicos.001) si se abre con null,
    /// Modificar (CU Mecanicos.002) si se abre con el mecánico elegido.
    /// </summary>
    public partial class FrmMecanicoEdicion : FormBase
    {
        private readonly MecanicoService _mecanicoService = new MecanicoService();
        private readonly Mecanico _mecanico;   // null es alta
        private bool EsAlta => _mecanico == null;

        public FrmMecanicoEdicion(Mecanico mecanico)
        {
            InitializeComponent();
            _mecanico = mecanico;
        }

        private void FrmMecanicoEdicion_Load(object sender, EventArgs e)
        {
            if (EsAlta)
            {
                Text = "Alta de mecánico";
                return;
            }

            Text = "Modificar mecánico";
            txtNombre.Text = _mecanico.Nombre;
            txtApellido.Text = _mecanico.Apellido;
            txtEspecialidad.Text = _mecanico.Especialidad;
            txtTelefono.Text = _mecanico.Telefono;
            txtEmail.Text = _mecanico.Email;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Armamos un objeto nuevo con lo cargado. Las validaciones las hace la BLL.
            var mecanico = new Mecanico
            {
                IdMecanico = EsAlta ? 0 : _mecanico.IdMecanico,
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Especialidad = txtEspecialidad.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text
            };

            try
            {
                if (EsAlta)
                {
                    _mecanicoService.Alta(mecanico);
                    Mensajes.Info("Mecánico dado de alta correctamente.");
                }
                else
                {
                    _mecanicoService.Modificar(mecanico);
                    Mensajes.Info("Mecánico modificado correctamente.");
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
