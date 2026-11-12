using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Comercial - Ficha de una publicación. Se usa para dos casos de uso:
    /// - CU.Publicacion.001 Publicar auto: se abre sin publicación (alta).
    /// - CU.Publicacion.002 Modificar publicación: se abre con la publicación elegida en la grilla.
    /// Muestra el precio mínimo de la consignación: el precio publicado no puede ser menor.
    /// </summary>
    public partial class FrmPublicacionEdicion : FormBase
    {
        private readonly PublicacionService _publicacionService = new PublicacionService();
        private readonly Publicacion _publicacion;   // null es alta
        private bool EsAlta => _publicacion == null;

        /// <summary>Alta: publicar un auto que está en una agencia.</summary>
        public FrmPublicacionEdicion() : this(null)
        {
        }

        /// <summary>Si viene una publicación, la pantalla abre en modo Modificar.</summary>
        public FrmPublicacionEdicion(Publicacion publicacion)
        {
            InitializeComponent();
            _publicacion = publicacion;

            // Los textos se cambian acá (antes del Load) porque FormBase los traduce
            // y arma la franja del título justo antes de mostrar la pantalla.
            if (!EsAlta)
            {
                Text = "Modificar publicación";
                btnGuardar.Text = "Guardar";
            }
        }

        private void FrmPublicacionEdicion_Load(object sender, EventArgs e)
        {
            cboPlataforma.Items.AddRange(PublicacionService.Plataformas);
            cboPlataforma.SelectedIndex = 0;

            if (EsAlta)
                CargarAlta();
            else
                CargarModificacion();
        }

        /// <summary>Alta: el combo trae los autos que están en una agencia y todavía no se publicaron.</summary>
        private void CargarAlta()
        {
            try
            {
                List<Consignacion> consignaciones = _publicacionService.ListarConsignacionesParaPublicar();
                cboConsignacion.DisplayMember = "Texto";
                cboConsignacion.DataSource = consignaciones
                    .Select(c => new { Texto = $"{c.Vehiculo.Descripcion} - {c.Agencia.RazonSocial}", Consignacion = c })
                    .ToList();

                if (consignaciones.Count == 0)
                    Mensajes.Info("No hay autos en agencias sin publicar.".Traducir());
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>
        /// Modificar: el auto ya está publicado y NO se puede cambiar (queda fijo en el combo, deshabilitado).
        /// Se pueden cambiar plataforma, precio y descripción.
        /// </summary>
        private void CargarModificacion()
        {
            try
            {
                // Buscamos la consignación para mostrar el precio mínimo de la agencia
                Consignacion consignacion = _publicacionService.ObtenerConsignacionVigente(_publicacion.IdVehiculo);

                cboConsignacion.DisplayMember = "Texto";
                cboConsignacion.DataSource = new[]
                {
                    new { Texto = $"{consignacion.Vehiculo.Descripcion} - {consignacion.Agencia.RazonSocial}", Consignacion = consignacion }
                }.ToList();
                cboConsignacion.Enabled = false;

                // Ahora sí cargamos los datos actuales del aviso (pisan lo que sugirió el combo)
                cboPlataforma.Text = _publicacion.Plataforma;
                numPrecio.Value = Math.Min(_publicacion.PrecioPublicado, numPrecio.Maximum);
                txtDescripcion.Text = _publicacion.Descripcion;
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private Consignacion ConsignacionElegida()
        {
            object item = cboConsignacion.SelectedItem;
            return item?.GetType().GetProperty("Consignacion")?.GetValue(item) as Consignacion;
        }

        private void cboConsignacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            Consignacion consignacion = ConsignacionElegida();
            if (consignacion == null)
            {
                lblDatosConsignacion.Text = "";
                return;
            }

            lblDatosConsignacion.Text = $"{"Precio mínimo de venta".Traducir()}: {consignacion.PrecioMinimoVenta:C2}";
            if (numPrecio.Value < consignacion.PrecioMinimoVenta)
                numPrecio.Value = Math.Min(consignacion.PrecioMinimoVenta, numPrecio.Maximum);

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                Vehiculo v = consignacion.Vehiculo;
                txtDescripcion.Text = $"{v.Marca} {v.Modelo} {v.Anio} - {v.Color}. Reparado a nuevo, papeles al día, listo para transferir.";
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (EsAlta)
                {
                    Consignacion consignacion = ConsignacionElegida();
                    _publicacionService.Registrar(new Publicacion
                    {
                        IdVehiculo = consignacion?.IdVehiculo ?? 0,
                        Plataforma = cboPlataforma.Text,
                        PrecioPublicado = numPrecio.Value,
                        Descripcion = txtDescripcion.Text
                    });

                    Mensajes.Info("Publicación registrada. El auto quedó Publicado.".Traducir());
                }
                else
                {
                    // Mandamos solo lo que se puede cambiar; el resto lo vuelve a leer la BLL de la base
                    _publicacionService.Modificar(new Publicacion
                    {
                        IdPublicacion = _publicacion.IdPublicacion,
                        Plataforma = cboPlataforma.Text,
                        PrecioPublicado = numPrecio.Value,
                        Descripcion = txtDescripcion.Text
                    });

                    Mensajes.Info("Publicación modificada.".Traducir());
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
