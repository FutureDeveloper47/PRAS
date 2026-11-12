using System.Data;
using Microsoft.Data.SqlClient;
using PRAS.Services.DataAccess.Implementations.Adapters;
using PRAS.Services.DataAccess.Interfaces;
using PRAS.Services.DataAccess.Tools;
using PRAS.Services.DomainModel.Composite;

namespace PRAS.Services.DataAccess.Implementations
{
    /// <summary>Tabla Familia de la base PRAS_Seguridad.</summary>
    internal class FamiliaRepository : IFamiliaRepository
    {
        private const string SelectBase = "SELECT IdFamilia, Nombre FROM Familia";

        public void Add(Familia entity)
        {
            entity.Id = Guid.NewGuid();
            SqlHelper.ExecuteNonQuery(
                "INSERT INTO Familia (IdFamilia, Nombre) VALUES (@IdFamilia, @Nombre)",
                CommandType.Text,
                new SqlParameter("@IdFamilia", entity.Id),
                new SqlParameter("@Nombre", entity.Nombre));
        }

        public void Update(Familia entity)
        {
            SqlHelper.ExecuteNonQuery(
                "UPDATE Familia SET Nombre = @Nombre WHERE IdFamilia = @IdFamilia",
                CommandType.Text,
                new SqlParameter("@IdFamilia", entity.Id),
                new SqlParameter("@Nombre", entity.Nombre));
        }

        /// <summary>
        /// Borra la familia y todas sus relaciones (con otras familias, patentes y usuarios),
        /// en una sola instrucción por lotes para que no quede nada a medias.
        /// </summary>
        public void Delete(Guid id)
        {
            SqlHelper.ExecuteNonQuery(
                @"SET XACT_ABORT ON;
                  BEGIN TRANSACTION;
                  DELETE FROM FamiliaFamilia WHERE IdFamiliaPadre = @IdFamilia OR IdFamiliaHijo = @IdFamilia;
                  DELETE FROM FamiliaPatente WHERE IdFamilia = @IdFamilia;
                  DELETE FROM UsuarioFamilia WHERE IdFamilia = @IdFamilia;
                  DELETE FROM Familia WHERE IdFamilia = @IdFamilia;
                  COMMIT TRANSACTION;",
                CommandType.Text,
                new SqlParameter("@IdFamilia", id));
        }

        public Familia GetById(Guid id)
        {
            List<Familia> lista = Leer($"{SelectBase} WHERE IdFamilia = @IdFamilia", new SqlParameter("@IdFamilia", id));
            return lista.Count == 0 ? null : lista[0];
        }

        public Familia GetByNombre(string nombre)
        {
            List<Familia> lista = Leer($"{SelectBase} WHERE Nombre = @Nombre", new SqlParameter("@Nombre", nombre));
            return lista.Count == 0 ? null : lista[0];
        }

        public List<Familia> GetAll()
        {
            return Leer($"{SelectBase} ORDER BY Nombre");
        }

        /// <summary>
        /// Primero se leen las filas y se cierra el lector; recién después se "hidratan"
        /// los hijos de cada familia (así no quedan dos lectores abiertos al mismo tiempo).
        /// </summary>
        private static List<Familia> Leer(string sql, params SqlParameter[] parametros)
        {
            var filas = new List<object[]>();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(sql, CommandType.Text, parametros))
            {
                while (reader.Read())
                {
                    object[] data = new object[reader.FieldCount];
                    reader.GetValues(data);
                    filas.Add(data);
                }
            }

            return filas.Select(f => FamiliaAdapter.Current.Get(f)).ToList();
        }
    }
}
