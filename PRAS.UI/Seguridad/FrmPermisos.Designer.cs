namespace PRAS.UI
{
    partial class FrmPermisos
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            tabPermisos = new TabControl();
            tabFamilias = new TabPage();
            lblArbolTitulo = new Label();
            tvFamilias = new TreeView();
            lblNuevaFamilia = new Label();
            txtNuevaFamilia = new TextBox();
            btnAgregarNuevaFamilia = new Button();
            lblSeleccionada = new Label();
            lblFamiliaSeleccionada = new Label();
            lblPatente = new Label();
            cboPatente = new ComboBox();
            btnAgregarPatente = new Button();
            lblSubfamilia = new Label();
            cboSubfamilia = new ComboBox();
            btnAgregarSubfamilia = new Button();
            btnQuitarDeFamilia = new Button();
            btnEliminarFamilia = new Button();
            lblAyuda = new Label();
            tabUsuarios = new TabPage();
            lblUsuario = new Label();
            cboUsuario = new ComboBox();
            lblAsignadosTitulo = new Label();
            tvUsuario = new TreeView();
            lblEfectivosTitulo = new Label();
            lstEfectivos = new ListBox();
            lblAsignarFamilia = new Label();
            cboAsignarFamilia = new ComboBox();
            btnAgregarFamiliaUsuario = new Button();
            lblAsignarPatente = new Label();
            cboAsignarPatente = new ComboBox();
            btnAgregarPatenteUsuario = new Button();
            btnQuitarDeUsuario = new Button();
            btnCerrar = new Button();
            tabPermisos.SuspendLayout();
            tabFamilias.SuspendLayout();
            tabUsuarios.SuspendLayout();
            SuspendLayout();
            //
            // tabPermisos
            //
            tabPermisos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            tabPermisos.SelectedIndex = 0;
            tabPermisos.Controls.Add(tabFamilias);
            tabPermisos.Controls.Add(tabUsuarios);
            tabPermisos.Location = new Point(12, 12);
            tabPermisos.Name = "tabPermisos";
            tabPermisos.Size = new Size(876, 500);
            tabPermisos.TabIndex = 0;
            //
            // tabFamilias
            //
            tabFamilias.Padding = new Padding(3);
            tabFamilias.Controls.Add(lblArbolTitulo);
            tabFamilias.Controls.Add(tvFamilias);
            tabFamilias.Controls.Add(lblNuevaFamilia);
            tabFamilias.Controls.Add(txtNuevaFamilia);
            tabFamilias.Controls.Add(btnAgregarNuevaFamilia);
            tabFamilias.Controls.Add(lblSeleccionada);
            tabFamilias.Controls.Add(lblFamiliaSeleccionada);
            tabFamilias.Controls.Add(lblPatente);
            tabFamilias.Controls.Add(cboPatente);
            tabFamilias.Controls.Add(btnAgregarPatente);
            tabFamilias.Controls.Add(lblSubfamilia);
            tabFamilias.Controls.Add(cboSubfamilia);
            tabFamilias.Controls.Add(btnAgregarSubfamilia);
            tabFamilias.Controls.Add(btnQuitarDeFamilia);
            tabFamilias.Controls.Add(btnEliminarFamilia);
            tabFamilias.Controls.Add(lblAyuda);
            tabFamilias.Location = new Point(4, 24);
            tabFamilias.Name = "tabFamilias";
            tabFamilias.Size = new Size(868, 472);
            tabFamilias.TabIndex = 0;
            tabFamilias.Text = "Familias";
            tabFamilias.UseVisualStyleBackColor = true;
            //
            // lblArbolTitulo
            //
            lblArbolTitulo.AutoSize = true;
            lblArbolTitulo.Location = new Point(10, 10);
            lblArbolTitulo.Name = "lblArbolTitulo";
            lblArbolTitulo.TabIndex = 0;
            lblArbolTitulo.Text = "Familias y sus permisos";
            //
            // tvFamilias
            //
            tvFamilias.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            tvFamilias.HideSelection = false;
            tvFamilias.Location = new Point(10, 32);
            tvFamilias.Name = "tvFamilias";
            tvFamilias.Size = new Size(420, 430);
            tvFamilias.TabIndex = 1;
            tvFamilias.AfterSelect += tvFamilias_AfterSelect;
            //
            // lblNuevaFamilia
            //
            lblNuevaFamilia.AutoSize = true;
            lblNuevaFamilia.Location = new Point(450, 32);
            lblNuevaFamilia.Name = "lblNuevaFamilia";
            lblNuevaFamilia.TabIndex = 2;
            lblNuevaFamilia.Text = "Nueva familia:";
            //
            // txtNuevaFamilia
            //
            txtNuevaFamilia.Location = new Point(450, 52);
            txtNuevaFamilia.Name = "txtNuevaFamilia";
            txtNuevaFamilia.Size = new Size(250, 23);
            txtNuevaFamilia.TabIndex = 3;
            //
            // btnAgregarNuevaFamilia
            //
            btnAgregarNuevaFamilia.Location = new Point(710, 51);
            btnAgregarNuevaFamilia.Name = "btnAgregarNuevaFamilia";
            btnAgregarNuevaFamilia.Size = new Size(140, 25);
            btnAgregarNuevaFamilia.TabIndex = 4;
            btnAgregarNuevaFamilia.Text = "Crear familia";
            btnAgregarNuevaFamilia.UseVisualStyleBackColor = true;
            btnAgregarNuevaFamilia.Click += btnAgregarNuevaFamilia_Click;
            //
            // lblSeleccionada
            //
            lblSeleccionada.AutoSize = true;
            lblSeleccionada.Location = new Point(450, 97);
            lblSeleccionada.Name = "lblSeleccionada";
            lblSeleccionada.TabIndex = 5;
            lblSeleccionada.Text = "Familia seleccionada:";
            //
            // lblFamiliaSeleccionada
            //
            lblFamiliaSeleccionada.AutoSize = true;
            lblFamiliaSeleccionada.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFamiliaSeleccionada.Location = new Point(590, 97);
            lblFamiliaSeleccionada.Name = "lblFamiliaSeleccionada";
            lblFamiliaSeleccionada.TabIndex = 6;
            lblFamiliaSeleccionada.Text = "(ninguna)";
            //
            // lblPatente
            //
            lblPatente.AutoSize = true;
            lblPatente.Location = new Point(450, 127);
            lblPatente.Name = "lblPatente";
            lblPatente.TabIndex = 7;
            lblPatente.Text = "Patente:";
            //
            // cboPatente
            //
            cboPatente.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPatente.FormattingEnabled = true;
            cboPatente.Location = new Point(450, 147);
            cboPatente.Name = "cboPatente";
            cboPatente.Size = new Size(250, 23);
            cboPatente.TabIndex = 8;
            //
            // btnAgregarPatente
            //
            btnAgregarPatente.Location = new Point(710, 146);
            btnAgregarPatente.Name = "btnAgregarPatente";
            btnAgregarPatente.Size = new Size(140, 25);
            btnAgregarPatente.TabIndex = 9;
            btnAgregarPatente.Text = "Agregar a la familia";
            btnAgregarPatente.UseVisualStyleBackColor = true;
            btnAgregarPatente.Click += btnAgregarPatente_Click;
            //
            // lblSubfamilia
            //
            lblSubfamilia.AutoSize = true;
            lblSubfamilia.Location = new Point(450, 182);
            lblSubfamilia.Name = "lblSubfamilia";
            lblSubfamilia.TabIndex = 10;
            lblSubfamilia.Text = "Familia:";
            //
            // cboSubfamilia
            //
            cboSubfamilia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSubfamilia.FormattingEnabled = true;
            cboSubfamilia.Location = new Point(450, 202);
            cboSubfamilia.Name = "cboSubfamilia";
            cboSubfamilia.Size = new Size(250, 23);
            cboSubfamilia.TabIndex = 11;
            //
            // btnAgregarSubfamilia
            //
            btnAgregarSubfamilia.Location = new Point(710, 201);
            btnAgregarSubfamilia.Name = "btnAgregarSubfamilia";
            btnAgregarSubfamilia.Size = new Size(140, 25);
            btnAgregarSubfamilia.TabIndex = 12;
            btnAgregarSubfamilia.Text = "Agregar a la familia";
            btnAgregarSubfamilia.UseVisualStyleBackColor = true;
            btnAgregarSubfamilia.Click += btnAgregarSubfamilia_Click;
            //
            // btnQuitarDeFamilia
            //
            btnQuitarDeFamilia.Location = new Point(450, 250);
            btnQuitarDeFamilia.Name = "btnQuitarDeFamilia";
            btnQuitarDeFamilia.Size = new Size(230, 30);
            btnQuitarDeFamilia.TabIndex = 13;
            btnQuitarDeFamilia.Text = "Quitar elemento seleccionado";
            btnQuitarDeFamilia.UseVisualStyleBackColor = true;
            btnQuitarDeFamilia.Click += btnQuitarDeFamilia_Click;
            //
            // btnEliminarFamilia
            //
            btnEliminarFamilia.Location = new Point(450, 290);
            btnEliminarFamilia.Name = "btnEliminarFamilia";
            btnEliminarFamilia.Size = new Size(230, 30);
            btnEliminarFamilia.TabIndex = 14;
            btnEliminarFamilia.Text = "Eliminar familia seleccionada";
            btnEliminarFamilia.UseVisualStyleBackColor = true;
            btnEliminarFamilia.Click += btnEliminarFamilia_Click;
            //
            // lblAyuda
            //
            lblAyuda.Location = new Point(450, 340);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(400, 110);
            lblAyuda.TabIndex = 15;
            lblAyuda.Text = "Patrón Composite. Una familia puede contener patentes (permisos concretos) y otras familias. Seleccione una familia en el árbol para agregarle o quitarle elementos. La familia Administrador está protegida.";
            //
            // tabUsuarios
            //
            tabUsuarios.Padding = new Padding(3);
            tabUsuarios.Controls.Add(lblUsuario);
            tabUsuarios.Controls.Add(cboUsuario);
            tabUsuarios.Controls.Add(lblAsignadosTitulo);
            tabUsuarios.Controls.Add(tvUsuario);
            tabUsuarios.Controls.Add(lblEfectivosTitulo);
            tabUsuarios.Controls.Add(lstEfectivos);
            tabUsuarios.Controls.Add(lblAsignarFamilia);
            tabUsuarios.Controls.Add(cboAsignarFamilia);
            tabUsuarios.Controls.Add(btnAgregarFamiliaUsuario);
            tabUsuarios.Controls.Add(lblAsignarPatente);
            tabUsuarios.Controls.Add(cboAsignarPatente);
            tabUsuarios.Controls.Add(btnAgregarPatenteUsuario);
            tabUsuarios.Controls.Add(btnQuitarDeUsuario);
            tabUsuarios.Location = new Point(4, 24);
            tabUsuarios.Name = "tabUsuarios";
            tabUsuarios.Size = new Size(868, 472);
            tabUsuarios.TabIndex = 1;
            tabUsuarios.Text = "Usuarios";
            tabUsuarios.UseVisualStyleBackColor = true;
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(10, 13);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "Usuario:";
            //
            // cboUsuario
            //
            cboUsuario.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUsuario.FormattingEnabled = true;
            cboUsuario.Location = new Point(70, 9);
            cboUsuario.Name = "cboUsuario";
            cboUsuario.Size = new Size(250, 23);
            cboUsuario.TabIndex = 1;
            cboUsuario.SelectedIndexChanged += cboUsuario_SelectedIndexChanged;
            //
            // lblAsignadosTitulo
            //
            lblAsignadosTitulo.AutoSize = true;
            lblAsignadosTitulo.Location = new Point(10, 45);
            lblAsignadosTitulo.Name = "lblAsignadosTitulo";
            lblAsignadosTitulo.TabIndex = 2;
            lblAsignadosTitulo.Text = "Permisos asignados";
            //
            // tvUsuario
            //
            tvUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            tvUsuario.HideSelection = false;
            tvUsuario.Location = new Point(10, 65);
            tvUsuario.Name = "tvUsuario";
            tvUsuario.Size = new Size(400, 397);
            tvUsuario.TabIndex = 3;
            //
            // lblEfectivosTitulo
            //
            lblEfectivosTitulo.AutoSize = true;
            lblEfectivosTitulo.Location = new Point(430, 45);
            lblEfectivosTitulo.Name = "lblEfectivosTitulo";
            lblEfectivosTitulo.TabIndex = 4;
            lblEfectivosTitulo.Text = "Permisos efectivos (todas sus patentes)";
            //
            // lstEfectivos
            //
            lstEfectivos.FormattingEnabled = true;
            lstEfectivos.ItemHeight = 15;
            lstEfectivos.Location = new Point(430, 65);
            lstEfectivos.Name = "lstEfectivos";
            lstEfectivos.Size = new Size(420, 199);
            lstEfectivos.TabIndex = 5;
            //
            // lblAsignarFamilia
            //
            lblAsignarFamilia.AutoSize = true;
            lblAsignarFamilia.Location = new Point(430, 280);
            lblAsignarFamilia.Name = "lblAsignarFamilia";
            lblAsignarFamilia.TabIndex = 6;
            lblAsignarFamilia.Text = "Familia:";
            //
            // cboAsignarFamilia
            //
            cboAsignarFamilia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAsignarFamilia.FormattingEnabled = true;
            cboAsignarFamilia.Location = new Point(430, 300);
            cboAsignarFamilia.Name = "cboAsignarFamilia";
            cboAsignarFamilia.Size = new Size(260, 23);
            cboAsignarFamilia.TabIndex = 7;
            //
            // btnAgregarFamiliaUsuario
            //
            btnAgregarFamiliaUsuario.Location = new Point(700, 299);
            btnAgregarFamiliaUsuario.Name = "btnAgregarFamiliaUsuario";
            btnAgregarFamiliaUsuario.Size = new Size(150, 25);
            btnAgregarFamiliaUsuario.TabIndex = 8;
            btnAgregarFamiliaUsuario.Text = "Asignar familia";
            btnAgregarFamiliaUsuario.UseVisualStyleBackColor = true;
            btnAgregarFamiliaUsuario.Click += btnAgregarFamiliaUsuario_Click;
            //
            // lblAsignarPatente
            //
            lblAsignarPatente.AutoSize = true;
            lblAsignarPatente.Location = new Point(430, 335);
            lblAsignarPatente.Name = "lblAsignarPatente";
            lblAsignarPatente.TabIndex = 9;
            lblAsignarPatente.Text = "Patente:";
            //
            // cboAsignarPatente
            //
            cboAsignarPatente.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAsignarPatente.FormattingEnabled = true;
            cboAsignarPatente.Location = new Point(430, 355);
            cboAsignarPatente.Name = "cboAsignarPatente";
            cboAsignarPatente.Size = new Size(260, 23);
            cboAsignarPatente.TabIndex = 10;
            //
            // btnAgregarPatenteUsuario
            //
            btnAgregarPatenteUsuario.Location = new Point(700, 354);
            btnAgregarPatenteUsuario.Name = "btnAgregarPatenteUsuario";
            btnAgregarPatenteUsuario.Size = new Size(150, 25);
            btnAgregarPatenteUsuario.TabIndex = 11;
            btnAgregarPatenteUsuario.Text = "Asignar patente";
            btnAgregarPatenteUsuario.UseVisualStyleBackColor = true;
            btnAgregarPatenteUsuario.Click += btnAgregarPatenteUsuario_Click;
            //
            // btnQuitarDeUsuario
            //
            btnQuitarDeUsuario.Location = new Point(430, 400);
            btnQuitarDeUsuario.Name = "btnQuitarDeUsuario";
            btnQuitarDeUsuario.Size = new Size(230, 30);
            btnQuitarDeUsuario.TabIndex = 12;
            btnQuitarDeUsuario.Text = "Quitar permiso seleccionado";
            btnQuitarDeUsuario.UseVisualStyleBackColor = true;
            btnQuitarDeUsuario.Click += btnQuitarDeUsuario_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(788, 522);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FrmPermisos
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(900, 562);
            Controls.Add(tabPermisos);
            Controls.Add(btnCerrar);
            MinimizeBox = false;
            Name = "FrmPermisos";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Permisos de usuarios (familias y patentes)";
            Load += FrmPermisos_Load;
            tabUsuarios.ResumeLayout(false);
            tabUsuarios.PerformLayout();
            tabFamilias.ResumeLayout(false);
            tabFamilias.PerformLayout();
            tabPermisos.ResumeLayout(false);
            tabPermisos.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabPermisos;
        private TabPage tabFamilias;
        private Label lblArbolTitulo;
        private TreeView tvFamilias;
        private Label lblNuevaFamilia;
        private TextBox txtNuevaFamilia;
        private Button btnAgregarNuevaFamilia;
        private Label lblSeleccionada;
        private Label lblFamiliaSeleccionada;
        private Label lblPatente;
        private ComboBox cboPatente;
        private Button btnAgregarPatente;
        private Label lblSubfamilia;
        private ComboBox cboSubfamilia;
        private Button btnAgregarSubfamilia;
        private Button btnQuitarDeFamilia;
        private Button btnEliminarFamilia;
        private Label lblAyuda;
        private TabPage tabUsuarios;
        private Label lblUsuario;
        private ComboBox cboUsuario;
        private Label lblAsignadosTitulo;
        private TreeView tvUsuario;
        private Label lblEfectivosTitulo;
        private ListBox lstEfectivos;
        private Label lblAsignarFamilia;
        private ComboBox cboAsignarFamilia;
        private Button btnAgregarFamiliaUsuario;
        private Label lblAsignarPatente;
        private ComboBox cboAsignarPatente;
        private Button btnAgregarPatenteUsuario;
        private Button btnQuitarDeUsuario;
        private Button btnCerrar;
    }
}
