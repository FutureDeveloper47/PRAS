using System.Data;
using Microsoft.Data.SqlClient;
using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DataAccess.Tools;
using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.DataAccess.Implementations
{
    /// <summary>
    /// Tabla de relación FamiliaFamilia: una familia dentro de otra familia.
    /// </summary>
    internal class FamiliaFamiliaRepository : IJoinRepository<Familia, Familia>
    {
        public void Add(Familia hijo, Familia padre)
        {
            SqlHelper.ExecuteNonQuery(
                "INSERT INTO FamiliaFamilia (IdFamiliaPadre, IdFamiliaHijo) VALUES (@Padre, @Hijo)",
                CommandType.Text,
                new SqlParameter("@Padre", padre.Id),
                new SqlParameter("@Hijo", hijo.Id));
        }

        public void AddRange(List<Familia> children, Familia parent)
        {
            foreach (Familia child in children)
                Add(child, parent);
        }

        public void Remove(Familia hijo, Familia padre)
        {
            SqlHelper.ExecuteNonQuery(
                "DELETE FROM FamiliaFamilia WHERE IdFamiliaPadre = @Padre AND IdFamiliaHijo = @Hijo",
                CommandType.Text,
                new SqlParameter("@Padre", padre.Id),
                new SqlParameter("@Hijo", hijo.Id));
        }

        /// <summary>Quita el elemento de TODOS los padres que lo tengan.</summary>
        public void Delete(Familia obj)
        {
            SqlHelper.ExecuteNonQuery(
                "DELETE FROM FamiliaFamilia WHERE IdFamiliaHijo = @Hijo",
                CommandType.Text,
                new SqlParameter("@Hijo", obj.Id));
        }

        public List<Familia> GetByObject(Familia obj)
        {
            // Primero leemos los Id y cerramos el lector; después buscamos cada familia.
            var ids = new List<Guid>();
            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(
                "SELECT IdFamiliaHijo FROM FamiliaFamilia WHERE IdFamiliaPadre = @Padre",
                CommandType.Text,
                new SqlParameter("@Padre", obj.Id)))
            {
                while (dataReader.Read())
                    ids.Add(dataReader.GetGuid(0));
            }

            var lista = new List<Familia>();
            foreach (Guid id in ids)
            {
                Familia item = new FamiliaRepository().GetById(id);
                if (item != null)
                    lista.Add(item);
            }
            return lista;
        }
    }
}
