using System.Data;
using Microsoft.Data.SqlClient;
using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DataAccess.Tools;
using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.DataAccess.Implementations
{
    /// <summary>
    /// Tabla de relación UsuarioFamilia: las familias asignadas a cada usuario.
    /// El usuario se identifica por su Id (INT): la tabla Usuario es la de PRAS_Seguridad.
    /// </summary>
    internal class UsuarioFamiliaRepository : IJoinRepository<Familia, int>
    {
        public void Add(Familia familia, int idUsuario)
        {
            SqlHelper.ExecuteNonQuery(
                "INSERT INTO UsuarioFamilia (IdUsuario, IdFamilia) VALUES (@Padre, @Hijo)",
                CommandType.Text,
                new SqlParameter("@Padre", idUsuario),
                new SqlParameter("@Hijo", familia.Id));
        }

        public void AddRange(List<Familia> children, int parent)
        {
            foreach (Familia child in children)
                Add(child, parent);
        }

        public void Remove(Familia familia, int idUsuario)
        {
            SqlHelper.ExecuteNonQuery(
                "DELETE FROM UsuarioFamilia WHERE IdUsuario = @Padre AND IdFamilia = @Hijo",
                CommandType.Text,
                new SqlParameter("@Padre", idUsuario),
                new SqlParameter("@Hijo", familia.Id));
        }

        /// <summary>Quita el elemento de TODOS los padres que lo tengan.</summary>
        public void Delete(Familia obj)
        {
            SqlHelper.ExecuteNonQuery(
                "DELETE FROM UsuarioFamilia WHERE IdFamilia = @Hijo",
                CommandType.Text,
                new SqlParameter("@Hijo", obj.Id));
        }

        public List<Familia> GetByObject(int obj)
        {
            // Primero leemos los Id y cerramos el lector; después buscamos cada familia.
            var ids = new List<Guid>();
            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(
                "SELECT IdFamilia FROM UsuarioFamilia WHERE IdUsuario = @Padre",
                CommandType.Text,
                new SqlParameter("@Padre", obj)))
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
