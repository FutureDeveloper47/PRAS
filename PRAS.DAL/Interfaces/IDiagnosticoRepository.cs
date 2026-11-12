using PRAS.Entidades;

namespace PRAS.DAL.Interfaces
{
    /// <summary>Repositorio de diagnósticos (tabla Diagnostico).</summary>
    public interface IDiagnosticoRepository : IGetByIdRepository<Diagnostico>
    {
        /// <summary>Último diagnóstico de cada auto del taller (incluirTodos = también los que ya salieron).</summary>
        List<Diagnostico> ListarUltimos(bool incluirTodos);

        void Update(Diagnostico diagnostico);

        /// <summary>
        /// Finaliza el diagnóstico. Si requiere reparación crea presupuesto, orden y tarea y el auto
        /// pasa a En reparación; si no, el auto queda Reparado. Todo en una transacción.
        /// </summary>
        void Finalizar(Diagnostico diagnostico, int? idMecanico, string prioridad);
    }
}
