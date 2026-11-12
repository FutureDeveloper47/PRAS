using System.Data;
using Microsoft.Data.SqlClient;
using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DataAccess.Tools;
using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.DataAccess.Implementations
{
    /// <summary>
    /// Tabla de relación FamiliaPatente: las patentes de cada familia.
    /// </summary>
    internal class FamiliaPatenteRepository : IJoinRepository<Patente, Familia>
    {
        public void Add(Patente patente, Familia familia)
        {
            SqlHelper.ExecuteNonQuery(
                "INSERT INTO FamiliaPatente (IdFamilia, IdPatente) VALUES (@Padre, @Hijo)",
                CommandType.Text,
                new SqlParameter("@Padre", familia.Id),
                new SqlParameter("@Hijo", patente.Id));
        }

        public void AddRange(List<Patente> children, Familia parent)
        {
            foreach (Patente child in children)
                Add(child, parent);
        }

        public void Remove(Patente patente, Familia familia)
        {
            SqlHelper.ExecuteNonQuery(
                "DELETE FROM FamiliaPatente WHERE IdFamilia = @Padre AND IdPatente = @Hijo",
                CommandType.Text,
                new SqlParameter("@Padre", familia.Id),
                new SqlParameter("@Hijo", patente.Id));
        }

        /// <summary>Quita el elemento de TODOS los padres que lo tengan.</summary>
        public void Delete(Patente obj)
        {
            SqlHelper.ExecuteNonQuery(
                "DELETE FROM FamiliaPatente WHERE IdPatente = @Hijo",
                CommandType.Text,
                new SqlParameter("@Hijo", obj.Id));
        }

        public List<Patente> GetByObject(Familia obj)
        {
            // Primero leemos los Id y cerramos el lector; después buscamos cada patente.
            var ids = new List<Guid>();
            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(
                "SELECT IdPatente FROM FamiliaPatente WHERE IdFamilia = @Padre",
                CommandType.Text,
                new SqlParameter("@Padre", obj.Id)))
            {
                while (dataReader.Read())
                    ids.Add(dataReader.GetGuid(0));
            }

            var lista = new List<Patente>();
            foreach (Guid id in ids)
            {
                Patente item = new PatenteRepository().GetById(id);
                if (item != null)
                    lista.Add(item);
            }
            return lista;
        }
    }
}
