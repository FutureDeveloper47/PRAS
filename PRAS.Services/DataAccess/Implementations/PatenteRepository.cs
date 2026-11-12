using System.Data;
using Microsoft.Data.SqlClient;
using PRAS.Services.DataAccess.Implementations.Adapters;
using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DataAccess.Tools;
using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.DataAccess.Implementations
{
    /// <summary>Tabla Patente de la base PRAS_Seguridad.</summary>
    internal class PatenteRepository : IPatenteRepository
    {
        private const string SelectBase = "SELECT IdPatente, DataKey, TipoAcceso, Nombre FROM Patente";

        public void Add(Patente entity)
        {
            entity.Id = Guid.NewGuid();
            SqlHelper.ExecuteNonQuery(
                "INSERT INTO Patente (IdPatente, DataKey, TipoAcceso, Nombre) VALUES (@IdPatente, @DataKey, @TipoAcceso, @Nombre)",
                CommandType.Text,
                new SqlParameter("@IdPatente", entity.Id),
                new SqlParameter("@DataKey", entity.DataKey),
                new SqlParameter("@TipoAcceso", (int)entity.TipoAcceso),
                new SqlParameter("@Nombre", (object)entity.Nombre));
        }

        public void Update(Patente entity)
        {
            SqlHelper.ExecuteNonQuery(
                "UPDATE Patente SET DataKey = @DataKey, TipoAcceso = @TipoAcceso, Nombre = @Nombre WHERE IdPatente = @IdPatente",
                CommandType.Text,
                new SqlParameter("@IdPatente", entity.Id),
                new SqlParameter("@DataKey", entity.DataKey),
                new SqlParameter("@TipoAcceso", (int)entity.TipoAcceso),
                new SqlParameter("@Nombre", (object)entity.Nombre));
        }

        public void Delete(Guid id)
        {
            // Las patentes son fijas (una por función), no se borran desde el programa.
            throw new NotSupportedException("Las patentes del sistema no se pueden eliminar.");
        }

        public Patente GetById(Guid id)
        {
            List<Patente> lista = Leer($"{SelectBase} WHERE IdPatente = @IdPatente", new SqlParameter("@IdPatente", id));
            return lista.Count == 0 ? null : lista[0];
        }

        public Patente GetByDataKey(string dataKey)
        {
            List<Patente> lista = Leer($"{SelectBase} WHERE DataKey = @DataKey", new SqlParameter("@DataKey", dataKey));
            return lista.Count == 0 ? null : lista[0];
        }

        public List<Patente> GetAll()
        {
            return Leer($"{SelectBase} ORDER BY Nombre");
        }

        private static List<Patente> Leer(string sql, params SqlParameter[] parametros)
        {
            var lista = new List<Patente>();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(sql, CommandType.Text, parametros))
            {
                while (reader.Read())
                {
                    object[] data = new object[reader.FieldCount];
                    reader.GetValues(data);
                    lista.Add(PatenteAdapter.Current.Get(data));
                }
            }
            return lista;
        }
    }
}
