using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.DomainModel.Composite;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Seguridad - Gestionar permisos con el patrón COMPOSITE.
    /// Solapa "Familias": se arman los grupos de permisos (familias con patentes y otras familias).
    /// Solapa "Usuarios": se le asignan familias y/o patentes a cada usuario y se ven
    /// sus permisos efectivos (todas las patentes que le quedan al recorrer el árbol).
    /// </summary>
    public partial class FrmPermisos : FormBase
    {
        private readonly PermisoService _permisoService = new PermisoService();

        private List<Familia> _familias = new List<Familia>();
        private List<Patente> _patentes = new List<Patente>();

        public FrmPermisos()
        {
            InitializeComponent();
        }

        private void FrmPermisos_Load(object sender, EventArgs e)
        {
            try
            {
                cboUsuario.DisplayMember = nameof(Usuario.NombreUsuario);
                cboUsuario.DataSource = _permisoService.ListarUsuarios();
                CargarTodo();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Vuelve a leer familias y patentes de la base y redibuja las dos solapas.</summary>
        private void CargarTodo()
        {
            Guid? seleccionada = (FamiliaDelNodo(tvFamilias.SelectedNode))?.Id;

            _familias = _permisoService.ListarFamilias();
            _patentes = _permisoService.ListarPatentes();

            // Árbol de familias: cada familia con todo lo que tiene adentro.
            tvFamilias.BeginUpdate();
            tvFamilias.Nodes.Clear();
            foreach (Familia familia in _familias)
                tvFamilias.Nodes.Add(CrearNodo(familia));
            tvFamilias.EndUpdate();

            if (seleccionada != null)
                SeleccionarFamilia(seleccionada.Value);

            CargarCombo(cboPatente, _patentes);
            CargarCombo(cboSubfamilia, _familias);
            CargarCombo(cboAsignarFamilia, _familias);
            CargarCombo(cboAsignarPatente, _patentes);

            MostrarFamiliaSeleccionada();
            CargarPermisosDelUsuario();
        }

        private static void CargarCombo<T>(ComboBox combo, List<T> lista) where T : Component
        {
            combo.DataSource = null;
            combo.DisplayMember = nameof(Component.Descripcion);
            combo.DataSource = new List<T>(lista);
        }

        /// <summary>
        /// Crea el nodo del árbol para un componente. Si es una familia, se llama a sí mismo
        /// para cada hijo (recursividad): así se dibuja el árbol completo del Composite.
        /// </summary>
        private static TreeNode CrearNodo(Component componente)
        {
            var nodo = new TreeNode(componente.Descripcion) { Tag = componente };

            if (componente is Familia familia)
            {
                nodo.Text = "[" + "Familia".Traducir() + "] " + familia.Nombre;
                nodo.NodeFont = new Font("Segoe UI", 9F, FontStyle.Bold);
                foreach (Component hijo in familia.GetHijos())
                    nodo.Nodes.Add(CrearNodo(hijo));
            }

            return nodo;
        }

        private void SeleccionarFamilia(Guid id)
        {
            foreach (TreeNode nodo in tvFamilias.Nodes)
            {
                if (nodo.Tag is Familia familia && familia.Id == id)
                {
                    tvFamilias.SelectedNode = nodo;
                    return;
                }
            }
        }

        // Solapa Familias

        /// <summary>
        /// Familia sobre la que se trabaja: si se eligió una familia, esa;
        /// si se eligió una patente, la familia que la contiene.
        /// </summary>
        private static Familia FamiliaDelNodo(TreeNode nodo)
        {
            if (nodo == null)
                return null;
            if (nodo.Tag is Familia familia)
                return familia;
            return nodo.Parent?.Tag as Familia;
        }

        private void tvFamilias_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MostrarFamiliaSeleccionada();
        }

        private void MostrarFamiliaSeleccionada()
        {
            Familia familia = FamiliaDelNodo(tvFamilias.SelectedNode);
            lblFamiliaSeleccionada.Text = familia == null ? "(ninguna)".Traducir() : familia.Nombre;
        }

        private void btnAgregarNuevaFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                Familia nueva = _permisoService.CrearFamilia(txtNuevaFamilia.Text);
                txtNuevaFamilia.Clear();
                CargarTodo();
                SeleccionarFamilia(nueva.Id);
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnAgregarPatente_Click(object sender, EventArgs e)
        {
            try
            {
                _permisoService.AgregarPatenteAFamilia(cboPatente.SelectedItem as Patente,
                                                       FamiliaDelNodo(tvFamilias.SelectedNode));
                CargarTodo();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnAgregarSubfamilia_Click(object sender, EventArgs e)
        {
            try
            {
                _permisoService.AgregarFamiliaAFamilia(cboSubfamilia.SelectedItem as Familia,
                                                       FamiliaDelNodo(tvFamilias.SelectedNode));
                CargarTodo();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Saca el elemento elegido de la familia que lo contiene (el nodo padre en el árbol).</summary>
        private void btnQuitarDeFamilia_Click(object sender, EventArgs e)
        {
            TreeNode nodo = tvFamilias.SelectedNode;
            if (nodo?.Parent == null)
            {
                Mensajes.Advertencia("Seleccione en el árbol el elemento (patente o familia) que quiere quitar de su familia.".Traducir());
                return;
            }

            var componente = (Component)nodo.Tag;
            var familia = (Familia)nodo.Parent.Tag;
            if (!Mensajes.Confirmar($"¿Quitar \"{componente.Descripcion}\" de la familia \"{familia.Nombre}\"?"))
                return;

            try
            {
                _permisoService.QuitarDeFamilia(componente, familia);
                CargarTodo();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnEliminarFamilia_Click(object sender, EventArgs e)
        {
            TreeNode nodo = tvFamilias.SelectedNode;
            if (nodo == null || nodo.Parent != null || !(nodo.Tag is Familia familia))
            {
                Mensajes.Advertencia("Seleccione una familia del primer nivel del árbol.".Traducir());
                return;
            }

            if (!Mensajes.Confirmar($"¿Eliminar la familia \"{familia.Nombre}\"? Los usuarios que la tengan asignada perderán esos permisos."))
                return;

            try
            {
                _permisoService.EliminarFamilia(familia);
                CargarTodo();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        // Solapa Usuarios

        private Usuario UsuarioSeleccionado => cboUsuario.SelectedItem as Usuario;

        private void cboUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPermisosDelUsuario();
        }

        /// <summary>
        /// Muestra lo que el usuario tiene asignado (árbol) y sus permisos efectivos:
        /// todas las patentes que le quedan al recorrer sus familias (sin repetir).
        /// </summary>
        private void CargarPermisosDelUsuario()
        {
            tvUsuario.Nodes.Clear();
            lstEfectivos.Items.Clear();

            Usuario usuario = UsuarioSeleccionado;
            if (usuario == null)
                return;

            try
            {
                List<Component> privilegios = _permisoService.ObtenerPrivilegios(usuario.IdUsuario);

                tvUsuario.BeginUpdate();
                foreach (Component componente in privilegios)
                    tvUsuario.Nodes.Add(CrearNodo(componente));
                tvUsuario.EndUpdate();

                foreach (Patente patente in _permisoService.ObtenerPatentesEfectivas(privilegios))
                    lstEfectivos.Items.Add(patente.Descripcion);
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void btnAgregarFamiliaUsuario_Click(object sender, EventArgs e)
        {
            Asignar(cboAsignarFamilia.SelectedItem as Component);
        }

        private void btnAgregarPatenteUsuario_Click(object sender, EventArgs e)
        {
            Asignar(cboAsignarPatente.SelectedItem as Component);
        }

        private void Asignar(Component componente)
        {
            try
            {
                _permisoService.AsignarAUsuario(componente, UsuarioSeleccionado);
                CargarPermisosDelUsuario();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        /// <summary>Solo se pueden quitar los elementos del primer nivel (lo asignado directamente).</summary>
        private void btnQuitarDeUsuario_Click(object sender, EventArgs e)
        {
            TreeNode nodo = tvUsuario.SelectedNode;
            if (nodo == null || nodo.Parent != null)
            {
                Mensajes.Advertencia("Seleccione un permiso del primer nivel del árbol (lo que está asignado directamente al usuario).".Traducir());
                return;
            }

            try
            {
                _permisoService.QuitarAUsuario((Component)nodo.Tag, UsuarioSeleccionado);
                CargarPermisosDelUsuario();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
