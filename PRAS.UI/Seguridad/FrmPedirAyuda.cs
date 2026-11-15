using System.Diagnostics;
using PRAS.BLL;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// CU Seguridad - Pedir ayuda.
    /// El usuario elige el tema de un desplegable, describe el problema y lo envía:
    /// el pedido queda registrado en la bitácora (el Administrador lo ve en Seguridad > Bitácora).
    /// Opcional: también se puede mandar por email al administrador.
    /// Si es urgente, abajo están los datos de contacto del administrador.
    /// </summary>
    public partial class FrmPedirAyuda : FormBase
    {
        private const int LargoMaximo = 1000;

        private readonly SoporteService _soporteService = new SoporteService();

        public FrmPedirAyuda()
        {
            InitializeComponent();
            txtDetalle.TextChanged += (sender, e) => lblCaracteres.Text = $"{txtDetalle.TextLength} / {LargoMaximo}";
        }

        private void FrmPedirAyuda_Load(object sender, EventArgs e)
        {
            // Los temas vienen de la BLL en español: se muestran traducidos y se guarda el original.
            foreach (string tema in _soporteService.TemasDeAyuda())
                cmbTema.Items.Add(new OpcionTraducida(tema));

            lblContacto.Text = FrmRecuperarContrasena.TextoContacto(_soporteService);
        }

        private string TemaElegido => (cmbTema.SelectedItem as OpcionTraducida)?.Original;

        private void btnRegistrarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                _soporteService.RegistrarPedido(TemaElegido, txtDetalle.Text);
                Mensajes.Info("Su pedido de ayuda quedó registrado. El administrador lo va a ver en la bitácora.");
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>
        /// Abre el programa de correo de la PC con el email del administrador, el asunto y el detalle ya escritos
        /// (link "mailto:"). Antes lo registra en la bitácora, así queda constancia aunque no se envíe el mail.
        /// </summary>
        private void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                _soporteService.RegistrarPedido(TemaElegido, txtDetalle.Text);

                string asunto = Uri.EscapeDataString("PRAS - " + "Pedido de ayuda".Traducir() + " - " + TemaElegido.TraducirMensaje());
                string cuerpo = Uri.EscapeDataString(txtDetalle.Text);
                string link = $"mailto:{_soporteService.EmailAdministrador()}?subject={asunto}&body={cuerpo}";

                Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
                DialogResult = DialogResult.OK;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // No hay un programa de correo configurado en la PC: el pedido igual quedó en la bitácora.
                Mensajes.Advertencia("No hay un programa de correo configurado en esta PC. Su pedido quedó registrado en la bitácora.");
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }

    /// <summary>
    /// Opción de un desplegable que se MUESTRA traducida pero GUARDA el texto original en español
    /// (así la BLL y la base siempre reciben el mismo valor, sin importar el idioma).
    /// </summary>
    public class OpcionTraducida
    {
        public string Original { get; }

        public OpcionTraducida(string original)
        {
            Original = original;
        }

        /// <summary>El ComboBox muestra lo que devuelve ToString().</summary>
        public override string ToString()
        {
            return Original.TraducirMensaje();
        }
    }
}
