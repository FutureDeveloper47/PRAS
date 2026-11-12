using PRAS.BLL;
using PRAS.Entidades;
using PRAS.Services.Facade.Extensions;

namespace PRAS.UI
{
    /// <summary>
    /// Comercial - Gestionar publicaciones: avisos de venta en internet de los autos que están
    /// en las agencias. Se publica un auto (CU.Publicacion.001), se modifica el aviso (.002)
    /// o se da de baja sin venderlo (.003).
    /// </summary>
    public partial class FrmPublicaciones : FormBase
    {
        private readonly PublicacionService _publicacionService = new PublicacionService();

        public FrmPublicaciones()
        {
            InitializeComponent();
        }

        private void FrmPublicaciones_Load(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                dgvPublicaciones.Rows.Clear();
                foreach (Publicacion p in _publicacionService.Listar(chkSoloActivas.Checked))
                {
                    int fila = dgvPublicaciones.Rows.Add(p.FechaPublicacion.ToString("dd/MM/yyyy"), p.Vehiculo.Dominio,
                        $"{p.Vehiculo.Marca} {p.Vehiculo.Modelo} ({p.Vehiculo.Anio})", p.Plataforma,
                        p.PrecioPublicado.ToString("C2"), p.EstadoPublicacion.Traducir(), p.FechaFinalizacion?.ToString("dd/MM/yyyy"));
                    dgvPublicaciones.Rows[fila].Tag = p;
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }

        private void chkSoloActivas_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var frm = new FrmPublicacionEdicion();
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        /// <summary>CU.Publicacion.002 - abre la ficha con el aviso elegido para cambiar plataforma, precio o descripción.</summary>
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (!(dgvPublicaciones.CurrentRow?.Tag is Publicacion publicacion))
            {
                Mensajes.Advertencia("Seleccione una publicación de la lista.".Traducir());
                return;
            }

            // Un aviso finalizado ya no se puede tocar (la BLL igual lo controla y tira la excepción)
            if (publicacion.EstadoPublicacion != Estados.PublicacionActiva)
            {
                Mensajes.Advertencia("Solo se pueden modificar las publicaciones activas.".Traducir());
                return;
            }

            using var frm = new FrmPublicacionEdicion(publicacion);
            if (frm.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        /// <summary>Doble clic en una fila = Modificar (igual que en las otras grillas del sistema).</summary>
        private void dgvPublicaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                btnModificar_Click(sender, e);
        }

        private void btnQuitarPublicacion_Click(object sender, EventArgs e)
        {
            if (!(dgvPublicaciones.CurrentRow?.Tag is Publicacion publicacion))
            {
                Mensajes.Advertencia("Seleccione una publicación de la lista.".Traducir());
                return;
            }

            if (!Mensajes.Confirmar($"¿Dar de baja la publicación de {publicacion.Vehiculo.Dominio} en {publicacion.Plataforma}? " +
                                    "El auto sigue en la agencia."))
                return;

            try
            {
                _publicacionService.Finalizar(publicacion.IdPublicacion);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex);
            }
        }
    }
}
