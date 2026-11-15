using System.Data;
using System.Globalization;
using System.Text;
using Microsoft.Data.SqlClient;
using PRAS.Services.DataAccess;
using PRAS.Services.DataAccess.Tools;
using PRAS.Services.DomainModel;
using PRAS.Services.Logic.Infraestructure;

namespace PRAS.Services.Logic
{
    /// <summary>
    /// Dígitos verificadores: sirven para detectar si alguien tocó la base "por afuera"
    /// del sistema (por ejemplo con un UPDATE en SQL Server Management Studio).
    ///
    /// - DVH (Dígito Verificador HORIZONTAL): un hash de TODOS los campos de UNA fila.
    ///   Si cambian un dato de esa fila (ej: el teléfono), el DVH guardado ya no coincide.
    /// - DVV (Dígito Verificador VERTICAL): un hash de todos los DVH de UNA tabla.
    ///   Si borran o agregan una fila entera, el DVV ya no coincide.
    ///
    /// Las tablas protegidas pueden estar en cualquiera de las dos bases.
    /// Los DVV de todas se guardan en la tabla DigitoVerificador de PRAS_Seguridad.
    /// </summary>
    internal static class DigitoVerificadorLogic
    {
        /// <summary>Una tabla protegida: su nombre, su columna Id y en qué base está.</summary>
        private class TablaProtegida
        {
            public string Nombre { get; set; }
            public string ColumnaId { get; set; }
            public string NombreBase { get; set; }
            public string CadenaConexion { get; set; }
        }

        /// <summary>
        /// Tablas protegidas:
        /// - Usuario -> base de SEGURIDAD (PRAS_Seguridad)
        /// - Cliente -> base del NEGOCIO (PRAS)
        /// Para proteger otra tabla: agregarle la columna DVH (VARCHAR(32) NULL) y sumarla acá.
        /// </summary>
        private static readonly List<TablaProtegida> tablasProtegidas = new List<TablaProtegida>
        {
            new TablaProtegida { Nombre = "Usuario", ColumnaId = "IdUsuario", CadenaConexion = SqlHelper.ConnectionString },
            new TablaProtegida { Nombre = "Cliente", ColumnaId = "IdCliente", CadenaConexion = SqlHelper.ConnectionStringNegocio }
        };

        static DigitoVerificadorLogic()
        {
            foreach (TablaProtegida tabla in tablasProtegidas)
            {
                tabla.NombreBase = new SqlConnectionStringBuilder(tabla.CadenaConexion).InitialCatalog;
            }
        }

        /// <summary>Ej: "Usuario (PRAS_Seguridad)", "Cliente (PRAS)".</summary>
        public static List<string> TablasProtegidas()
        {
            return tablasProtegidas.Select(t => $"{t.Nombre} ({t.NombreBase})").ToList();
        }

        /// <summary>
        /// DVH de una fila: junta todos los valores (menos el propio DVH) separados por "|"
        /// y les calcula el hash MD5 con el servicio de criptografía.
        /// </summary>
        public static string CalcularDVH(DataRow fila)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn columna in fila.Table.Columns)
            {
                if (columna.ColumnName == "DVH")
                {
                    continue;
                }

                // Usamos InvariantCulture para que el mismo dato dé el mismo texto en cualquier idioma
                // (si no, 1500,50 y 1500.50 darían hashes distintos).
                sb.Append(Convert.ToString(fila[columna], CultureInfo.InvariantCulture));
                sb.Append('|');
            }

            return CryptographyLogic.HashMd5(sb.ToString());
        }

        /// <summary>DVV de una tabla: hash de todos los DVH de sus filas, en orden de Id.</summary>
        private static string CalcularDVV(DataTable registros)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataRow fila in registros.Rows)
            {
                sb.Append(fila["DVH"] as string);
            }

            return CryptographyLogic.HashMd5(sb.ToString());
        }

        /// <summary>
        /// Recalcula el DVH de UNA fila y el DVV de su tabla.
        /// Se llama cada vez que el sistema inserta o modifica una fila protegida.
        /// </summary>
        public static void RecalcularRegistro(string nombreTabla, int id)
        {
            TablaProtegida tabla = Buscar(nombreTabla);

            DataTable registro = DigitoVerificadorDal.ObtenerRegistro(tabla.CadenaConexion, tabla.Nombre, tabla.ColumnaId, id);
            if (registro.Rows.Count > 0)
            {
                DigitoVerificadorDal.ActualizarDVH(tabla.CadenaConexion, tabla.Nombre, tabla.ColumnaId, id, CalcularDVH(registro.Rows[0]));
            }

            RecalcularDVV(tabla);
        }

        /// <summary>Recalcula TODOS los DVH de una tabla y su DVV.</summary>
        private static void RecalcularTabla(TablaProtegida tabla)
        {
            DataTable registros = DigitoVerificadorDal.ObtenerRegistros(tabla.CadenaConexion, tabla.Nombre, tabla.ColumnaId);
            foreach (DataRow fila in registros.Rows)
            {
                int id = Convert.ToInt32(fila[tabla.ColumnaId]);
                DigitoVerificadorDal.ActualizarDVH(tabla.CadenaConexion, tabla.Nombre, tabla.ColumnaId, id, CalcularDVH(fila));
            }

            RecalcularDVV(tabla);
        }

        public static void RecalcularTodo()
        {
            foreach (TablaProtegida tabla in tablasProtegidas)
            {
                RecalcularTabla(tabla);
            }
        }

        /// <summary>
        /// Revisa todas las tablas protegidas y devuelve la lista de problemas encontrados.
        /// Lista vacía = las bases están íntegras.
        /// La primera vez que se usa (todavía no hay DVV guardado) calcula los dígitos
        /// a partir de los datos actuales.
        /// </summary>
        public static List<string> VerificarIntegridad()
        {
            return VerificarIntegridad(new List<ResultadoIntegridadTabla>());
        }

        /// <summary>
        /// Igual que VerificarIntegridad(), pero además completa "detalle" con el resultado de
        /// cada tabla (cuántos registros se revisaron y cuántos están alterados).
        /// </summary>
        public static List<string> VerificarIntegridad(List<ResultadoIntegridadTabla> detalle)
        {
            List<string> errores = new List<string>();

            foreach (TablaProtegida tabla in tablasProtegidas)
            {
                string dvvGuardado = DigitoVerificadorDal.ObtenerDVV(tabla.Nombre);

                if (dvvGuardado == null)
                {
                    // Primera vez: todavía no calculamos los dígitos de esta tabla.
                    RecalcularTabla(tabla);
                    dvvGuardado = DigitoVerificadorDal.ObtenerDVV(tabla.Nombre);
                }

                DataTable registros = DigitoVerificadorDal.ObtenerRegistros(tabla.CadenaConexion, tabla.Nombre, tabla.ColumnaId);
                var resultado = new ResultadoIntegridadTabla
                {
                    Tabla = tabla.Nombre,
                    Base = tabla.NombreBase,
                    Registros = registros.Rows.Count
                };

                // 1) DVH: fila por fila
                foreach (DataRow fila in registros.Rows)
                {
                    string dvhGuardado = fila["DVH"] as string;
                    if (dvhGuardado != CalcularDVH(fila))
                    {
                        resultado.RegistrosAlterados++;
                        errores.Add($"Base {tabla.NombreBase}, tabla {tabla.Nombre}, registro {tabla.ColumnaId} = {fila[tabla.ColumnaId]}: " +
                                    "sus datos fueron modificados por fuera del sistema (DVH).");
                    }
                }

                // 2) DVV: la tabla entera (así detectamos filas borradas o agregadas)
                resultado.DvvCorrecto = dvvGuardado == CalcularDVV(registros);
                if (!resultado.DvvCorrecto)
                {
                    errores.Add($"Base {tabla.NombreBase}, tabla {tabla.Nombre}: se agregaron o borraron filas por fuera del sistema (DVV).");
                }

                detalle.Add(resultado);
            }

            return errores;
        }

        private static void RecalcularDVV(TablaProtegida tabla)
        {
            DataTable registros = DigitoVerificadorDal.ObtenerRegistros(tabla.CadenaConexion, tabla.Nombre, tabla.ColumnaId);
            DigitoVerificadorDal.GuardarDVV(tabla.Nombre, CalcularDVV(registros));
        }

        private static TablaProtegida Buscar(string nombreTabla)
        {
            TablaProtegida tabla = tablasProtegidas.FirstOrDefault(t => t.Nombre == nombreTabla);
            if (tabla == null)
            {
                throw new ArgumentException($"La tabla {nombreTabla} no tiene dígitos verificadores.");
            }
            return tabla;
        }
    }
}
