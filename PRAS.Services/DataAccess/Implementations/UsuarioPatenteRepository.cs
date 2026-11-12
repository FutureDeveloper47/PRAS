using System.Data;
using Microsoft.Data.SqlClient;
using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DataAccess.Tools;
using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.DataAccess.Implementations
{
    /// <summary>
    /// Tabla de relación UsuarioPatente: las patentes asignadas directamente a cada usuario (sin familia).
    /// El usuario se identifica por su Id (INT): la tabla Usuario es la de PRAS_Seguridad.
    /// </summary>
    internal class UsuarioPatenteRepository : IJoinRepository<Patente, int>
    {
        public void Add(Patente patente, int idUsuario)
        {
            SqlHelper.ExecuteNonQuery(
                "INSERT INTO UsuarioPatente (IdUsuario, IdPatente) VALUES (@Padre, @Hijo)",
                CommandType.Text,
                new SqlParameter("@Padre", idUsuario),
                new SqlParameter("@Hijo", patente.Id));
        }

        public void AddRange(List<Patente> children, int parent)
        {
            foreach (Patente child in children)
                Add(child, parent);
        }

        public void Remove(Patente patente, int idUsuario)
        {
            SqlHelper.ExecuteNonQuery(
                "DELETE FROM UsuarioPatente WHERE IdUsuario = @Padre AND IdPatente = @Hijo",
                CommandType.Text,
                new SqlParameter("@Padre", idUsuario),
                new SqlParameter("@Hijo", patente.Id));
        }

        /// <summary>Quita el elemento de TODOS los padres que lo tengan.</summary>
        public void Delete(Patente obj)
        {
            SqlHelper.ExecuteNonQuery(
                "DELETE FROM UsuarioPatente WHERE IdPatente = @Hijo",
                CommandType.Text,
                new SqlParameter("@Hijo", obj.Id));
        }

        public List<Patente> GetByObject(int obj)
        {
            // Primero leemos los Id y cerramos el lector; después buscamos cada patente.
            var ids = new List<Guid>();
            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(
                "SELECT IdPatente FROM UsuarioPatente WHERE IdUsuario = @Padre",
                CommandType.Text,
                new SqlParameter("@Padre", obj)))
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
