using System.Globalization;
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
    ///
    /// A la derecha está la FICHA DEL AUTO (kilometraje, combustible, GNC, transmisión, puertas, dueños,
    /// VTV, service, aire, dirección, chapa y pintura), las reparaciones hechas en el taller y las
    /// observaciones. Con "Armar descripción" se arma el texto del aviso con esos datos (se puede editar).
    /// Las reglas (km ≥ 0, combos obligatorios, etc.) están en la BLL (PublicacionService.ValidarFicha).
    /// La ficha se arma por código para no rehacer el diseñador.
    /// </summary>
    public partial class FrmPublicacionEdicion : FormBase
    {
        private readonly PublicacionService _publicacionService = new PublicacionService();
        private readonly Publicacion _publicacion;   // null es alta
        private bool EsAlta => _publicacion == null;

        // Ficha del auto (armada por código)
        private readonly GroupBox grpFicha = new GroupBox();
        private readonly NumericUpDown numKilometraje = new NumericUpDown();
        private readonly ComboBox cboCombustible = new ComboBox();
        private readonly CheckBox chkGNC = new CheckBox();
        private readonly ComboBox cboTransmision = new ComboBox();
        private readonly NumericUpDown numPuertas = new NumericUpDown();
        private readonly NumericUpDown numDuenos = new NumericUpDown();
        private readonly ComboBox cboChapaPintura = new ComboBox();
        private readonly CheckBox chkVTV = new CheckBox();
        private readonly CheckBox chkService = new CheckBox();
        private readonly CheckBox chkAire = new CheckBox();
        private readonly CheckBox chkDireccion = new CheckBox();
        private readonly TextBox txtObservaciones = new TextBox();
        private readonly TextBox txtReparaciones = new TextBox();
        private readonly Button btnArmarDescripcion = new Button();

        /// <summary>Reparaciones del taller del auto elegido (salen de las tareas finalizadas de sus órdenes).</summary>
        private List<string> _reparaciones = new List<string>();

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
                btnGuardar.Text = "Guardar".Traducir();
            }

            ArmarFicha();
        }

        // Ficha del auto: armado de la pantalla

        /// <summary>
        /// Agranda la pantalla y pone la ficha a la derecha. Las medidas se multiplican por la escala
        /// de Windows (k) porque el resto de la pantalla ya fue agrandado por el diseñador.
        /// </summary>
        private void ArmarFicha()
        {
            float k = DeviceDpi / 96f;
            int S(int valor) => (int)(valor * k);

            int xFicha = txtDescripcion.Right + S(18);
            int anchoFicha = S(430);
            ClientSize = new Size(xFicha + anchoFicha + S(12), S(620));

            grpFicha.Text = "Ficha del auto";
            grpFicha.SetBounds(xFicha, cboConsignacion.Top - S(4), anchoFicha, ClientSize.Height - cboConsignacion.Top - S(60));
            grpFicha.Padding = new Padding(S(10));

            var tabla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, AutoScroll = true };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));

            numKilometraje.Maximum = 2000000;
            numKilometraje.ThousandsSeparator = true;
            numKilometraje.Increment = 1000;
            Renglon(tabla, "Kilometraje: *", numKilometraje);

            PrepararCombo(cboCombustible, PublicacionService.Combustibles);
            cboCombustible.SelectedIndexChanged += (s, e) =>
            {
                // Si el combustible es GNC, el auto tiene GNC sí o sí.
                if ((string)cboCombustible.SelectedItem == "GNC")
                    chkGNC.Checked = true;
            };
            Renglon(tabla, "Combustible: *", cboCombustible);

            chkGNC.Text = "Tiene GNC";
            Casillas(tabla, chkGNC);

            PrepararCombo(cboTransmision, PublicacionService.Transmisiones);
            Renglon(tabla, "Transmisión: *", cboTransmision);

            numPuertas.Minimum = 2;
            numPuertas.Maximum = 5;
            numPuertas.Value = 5;
            Renglon(tabla, "Puertas: *", numPuertas);

            numDuenos.Minimum = 1;
            numDuenos.Maximum = 20;
            numDuenos.Value = 1;
            Renglon(tabla, "Cantidad de dueños: *", numDuenos);

            PrepararCombo(cboChapaPintura, PublicacionService.EstadosChapaPintura);
            Renglon(tabla, "Chapa y pintura: *", cboChapaPintura);

            chkVTV.Text = "VTV vigente";
            chkService.Text = "Service oficial";
            Casillas(tabla, chkVTV, chkService);
            chkAire.Text = "Aire acondicionado";
            chkDireccion.Text = "Dirección asistida";
            Casillas(tabla, chkAire, chkDireccion);

            txtReparaciones.Name = "txtReparaciones";
            txtReparaciones.ReadOnly = true;
            txtReparaciones.Multiline = true;
            txtReparaciones.ScrollBars = ScrollBars.Vertical;
            txtReparaciones.Height = S(70);
            Bloque(tabla, "Reparaciones hechas en el taller:", txtReparaciones);

            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Multiline = true;
            txtObservaciones.MaxLength = 500;
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Height = S(60);
            Bloque(tabla, "Observaciones:", txtObservaciones);

            btnArmarDescripcion.Name = "btnArmarDescripcion";
            btnArmarDescripcion.Text = "Armar descripción";
            btnArmarDescripcion.Click += btnArmarDescripcion_Click;

            grpFicha.Controls.Add(tabla);
            Controls.Add(grpFicha);

            // La descripción ocupa todo el alto y los botones bajan al pie de la pantalla.
            int arribaBotones = ClientSize.Height - btnGuardar.Height - S(14);
            txtDescripcion.Height = arribaBotones - txtDescripcion.Top - S(12);
            btnCancelar.Location = new Point(grpFicha.Right - btnCancelar.Width, arribaBotones);
            btnGuardar.Location = new Point(btnCancelar.Left - btnGuardar.Width - S(8), arribaBotones);

            // "Armar descripción" va debajo de la descripción, porque es la que llena.
            btnArmarDescripcion.SetBounds(txtDescripcion.Left, arribaBotones, S(200), btnGuardar.Height);
            Controls.Add(btnArmarDescripcion);
        }

        /// <summary>Agrega un renglón que toma el alto de lo que tiene adentro (sin dejar espacios de más).</summary>
        private static int NuevaFila(TableLayoutPanel tabla)
        {
            tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            return tabla.RowCount++;
        }

        private static void PrepararCombo(ComboBox combo, string[] opciones)
        {
            // DropDownList: se elige de la lista. Se guarda en español y se ve traducido (ValoresTraducidos).
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Items.AddRange(opciones);
        }

        private static void Renglon(TableLayoutPanel tabla, string etiqueta, Control control)
        {
            var lbl = new Label { Text = etiqueta, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 6, 4, 6) };
            control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            control.Margin = new Padding(0, 3, 0, 3);
            int fila = NuevaFila(tabla);
            tabla.Controls.Add(lbl, 0, fila);
            tabla.Controls.Add(control, 1, fila);
        }

        private static void Casillas(TableLayoutPanel tabla, params CheckBox[] casillas)
        {
            int fila = NuevaFila(tabla);
            for (int i = 0; i < casillas.Length; i++)
            {
                casillas[i].AutoSize = true;
                casillas[i].Margin = new Padding(0, 4, 0, 4);
                tabla.Controls.Add(casillas[i], i, fila);
            }
        }

        private static void Bloque(TableLayoutPanel tabla, string etiqueta, Control control)
        {
            var lbl = new Label { Text = etiqueta, AutoSize = true, Margin = new Padding(0, 8, 0, 2) };
            int fila = NuevaFila(tabla);
            tabla.Controls.Add(lbl, 0, fila);
            tabla.SetColumnSpan(lbl, 2);

            control.Dock = DockStyle.Top;
            fila = NuevaFila(tabla);
            tabla.Controls.Add(control, 0, fila);
            tabla.SetColumnSpan(control, 2);
        }

        // Carga de datos

        private void FrmPublicacionEdicion_Load(object sender, EventArgs e)
        {
            // Las plataformas se guardan en español y se muestran traducidas;
            // el usuario también puede escribir otra que no esté en la lista.
            ValoresTraducidos.ConectarEditable(cboPlataforma);
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
        /// Se pueden cambiar plataforma, precio, descripción, observaciones y la ficha del auto.
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
                int indice = Array.IndexOf(PublicacionService.Plataformas, _publicacion.Plataforma);
                if (indice >= 0)
                    cboPlataforma.SelectedIndex = indice;   // una de la lista: se ve traducida
                else
                    cboPlataforma.Text = _publicacion.Plataforma;   // una que escribió el usuario
                numPrecio.Value = Math.Min(_publicacion.PrecioPublicado, numPrecio.Maximum);
                txtDescripcion.Text = _publicacion.Descripcion;
                txtObservaciones.Text = _publicacion.Observaciones;
                MostrarFicha(_publicacion.Vehiculo);
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Pone en pantalla la ficha que ya tiene el auto (si nunca se cargó, quedan los valores por defecto).</summary>
        private void MostrarFicha(Vehiculo v)
        {
            if (v.Kilometraje.HasValue)
                numKilometraje.Value = Math.Min(v.Kilometraje.Value, (int)numKilometraje.Maximum);
            Elegir(cboCombustible, v.Combustible);
            chkGNC.Checked = v.TieneGNC ?? false;
            Elegir(cboTransmision, v.Transmision);
            if (v.Puertas.HasValue)
                numPuertas.Value = Math.Max(numPuertas.Minimum, Math.Min(v.Puertas.Value, numPuertas.Maximum));
            if (v.CantidadDuenos.HasValue)
                numDuenos.Value = Math.Max(numDuenos.Minimum, Math.Min(v.CantidadDuenos.Value, numDuenos.Maximum));
            Elegir(cboChapaPintura, v.EstadoChapaPintura);
            chkVTV.Checked = v.VTVVigente ?? false;
            chkService.Checked = v.ServiceOficial ?? false;
            chkAire.Checked = v.AireAcondicionado ?? false;
            chkDireccion.Checked = v.DireccionAsistida ?? false;

            try
            {
                _reparaciones = _publicacionService.ObtenerReparacionesDelTaller(v.IdVehiculo);
                txtReparaciones.Text = _reparaciones.Count == 0
                    ? "Sin reparaciones registradas.".Traducir()
                    : string.Join(Environment.NewLine, _reparaciones.Select(r => "• " + r));
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private static void Elegir(ComboBox combo, string valor)
        {
            combo.SelectedIndex = valor == null ? -1 : combo.Items.IndexOf(valor);
        }

        /// <summary>
        /// Plataforma para guardar: si eligió una de la lista, el valor original en español
        /// (lo que se ve está traducido); si escribió otra, lo que escribió.
        /// </summary>
        private string PlataformaElegida()
        {
            bool eligioDeLaLista = cboPlataforma.SelectedIndex >= 0
                && cboPlataforma.Text == cboPlataforma.GetItemText(cboPlataforma.SelectedItem);
            return eligioDeLaLista ? (string)cboPlataforma.SelectedItem : cboPlataforma.Text;
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

            // En el alta, la ficha arranca con lo que el auto ya tenga cargado y sus reparaciones.
            if (EsAlta)
                MostrarFicha(consignacion.Vehiculo);

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                txtDescripcion.Text = ArmarDescripcion(consignacion.Vehiculo, conFicha: false);
        }

        // Armar descripción

        private void btnArmarDescripcion_Click(object sender, EventArgs e)
        {
            Consignacion consignacion = ConsignacionElegida();
            if (consignacion == null)
            {
                Mensajes.Advertencia("Seleccione el vehículo a publicar.".Traducir());
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtDescripcion.Text)
                && !Mensajes.Confirmar("¿Reemplazar la descripción actual por la que se arma con la ficha del auto?".Traducir()))
                return;

            txtDescripcion.Text = ArmarDescripcion(consignacion.Vehiculo, conFicha: true);
        }

        /// <summary>
        /// Texto del aviso, en el idioma elegido, con los datos de la ficha. Ejemplo:
        /// "Ford Ranger 2019 - Negro. Kilometraje: 85.000 km. Combustible: Diésel. ..."
        /// Es solo una ayuda para escribir: el texto queda en el cuadro y se puede editar antes de guardar.
        /// </summary>
        private string ArmarDescripcion(Vehiculo v, bool conFicha)
        {
            var renglones = new List<string> { $"{v.Marca} {v.Modelo} {v.Anio} - {v.Color}." };

            if (conFicha)
            {
                string combustible = (cboCombustible.Text == "" ? "-" : cboCombustible.Text)
                                     + (chkGNC.Checked && (string)cboCombustible.SelectedItem != "GNC" ? " + " + "GNC".Traducir() : "");
                renglones.Add($"{"Kilometraje".Traducir()}: {numKilometraje.Value.ToString("N0", CultureInfo.CurrentCulture)} km. " +
                              $"{"Combustible".Traducir()}: {combustible}. " +
                              $"{"Transmisión".Traducir()}: {(cboTransmision.Text == "" ? "-" : cboTransmision.Text)}.");
                renglones.Add($"{"Puertas".Traducir()}: {numPuertas.Value}. {"Cantidad de dueños".Traducir()}: {numDuenos.Value}.");

                var equipamiento = new List<string>();
                if (chkAire.Checked) equipamiento.Add("Aire acondicionado".Traducir());
                if (chkDireccion.Checked) equipamiento.Add("Dirección asistida".Traducir());
                if (chkVTV.Checked) equipamiento.Add("VTV vigente".Traducir());
                if (chkService.Checked) equipamiento.Add("Service oficial".Traducir());
                if (equipamiento.Count > 0)
                    renglones.Add(string.Join(", ", equipamiento) + ".");

                if (cboChapaPintura.Text != "")
                    renglones.Add($"{"Chapa y pintura".Traducir()}: {cboChapaPintura.Text}.");

                if (_reparaciones.Count > 0)
                    renglones.Add($"{"Reparado en nuestro taller".Traducir()}: {string.Join("; ", _reparaciones.Select(r => r.Trim().TrimEnd('.')))}.");

                if (!string.IsNullOrWhiteSpace(txtObservaciones.Text))
                    renglones.Add(txtObservaciones.Text.Trim());
            }

            renglones.Add("Papeles al día, listo para transferir.".Traducir());
            return string.Join(Environment.NewLine, renglones);
        }

        // Guardar

        /// <summary>Junta lo que se cargó en la ficha (los combos devuelven el valor en español).</summary>
        private Vehiculo FichaCargada(int idVehiculo)
        {
            return new Vehiculo
            {
                IdVehiculo = idVehiculo,
                Kilometraje = (int)numKilometraje.Value,
                Combustible = cboCombustible.SelectedItem as string,
                TieneGNC = chkGNC.Checked,
                Transmision = cboTransmision.SelectedItem as string,
                Puertas = (int)numPuertas.Value,
                CantidadDuenos = (int)numDuenos.Value,
                VTVVigente = chkVTV.Checked,
                ServiceOficial = chkService.Checked,
                AireAcondicionado = chkAire.Checked,
                DireccionAsistida = chkDireccion.Checked,
                EstadoChapaPintura = cboChapaPintura.SelectedItem as string
            };
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (EsAlta)
                {
                    Consignacion consignacion = ConsignacionElegida();
                    int idVehiculo = consignacion?.IdVehiculo ?? 0;
                    _publicacionService.Registrar(new Publicacion
                    {
                        IdVehiculo = idVehiculo,
                        Plataforma = PlataformaElegida(),
                        PrecioPublicado = numPrecio.Value,
                        Descripcion = txtDescripcion.Text,
                        Observaciones = txtObservaciones.Text,
                        Vehiculo = FichaCargada(idVehiculo)
                    });

                    Mensajes.Info("Publicación registrada. El auto quedó Publicado.".Traducir());
                }
                else
                {
                    // Mandamos solo lo que se puede cambiar; el resto lo vuelve a leer la BLL de la base
                    _publicacionService.Modificar(new Publicacion
                    {
                        IdPublicacion = _publicacion.IdPublicacion,
                        Plataforma = PlataformaElegida(),
                        PrecioPublicado = numPrecio.Value,
                        Descripcion = txtDescripcion.Text,
                        Observaciones = txtObservaciones.Text,
                        Vehiculo = FichaCargada(_publicacion.IdVehiculo)
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
