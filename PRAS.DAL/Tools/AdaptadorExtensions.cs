using System.Data;
using PRAS.DAL.Interfaces;

namespace PRAS.DAL.Tools
{
    /// <summary>
    /// Ayudas para usar los adaptadores con un DataTable entero:
    /// así cada repositorio no repite el "foreach fila -> Fill".
    /// </summary>
    internal static class AdaptadorExtensions
    {
        /// <summary>Convierte todas las filas de la tabla con el adaptador.</summary>
        public static List<T> FillLista<T>(this IAdaptador<T> adaptador, DataTable tabla)
        {
            var lista = new List<T>();
            foreach (DataRow fila in tabla.Rows)
                lista.Add(adaptador.Fill(fila));
            return lista;
        }

        /// <summary>Convierte la primera fila, o devuelve null si la tabla vino vacía.</summary>
        public static T FillPrimero<T>(this IAdaptador<T> adaptador, DataTable tabla) where T : class
        {
            return tabla.Rows.Count == 0 ? null : adaptador.Fill(tabla.Rows[0]);
        }
    }
}
