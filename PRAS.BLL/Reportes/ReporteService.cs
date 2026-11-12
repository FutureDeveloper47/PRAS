using System.Data;
using System.Globalization;
using System.Text;
using PRAS.BLL.Excepciones;
using PRAS.DAL.Factory;
using PRAS.DAL.Interfaces;
using PRAS.Services.Facade;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PRAS.BLL
{
    /// <summary>Reportes del sistema (menú Reportes).</summary>
    public enum TipoReporte
    {
        Vehiculos,
        Reparaciones,
        Presupuestos,
        Ventas
    }

    /// <summary>
    /// Casos de uso Reportes: Vehículos (stock e inversión), Reparaciones, Presupuestos y Ventas
    /// (con la ganancia de cada auto). Cada reporte se puede exportar a PDF (para imprimir o
    /// mandar) o a CSV (se abre con Excel).
    /// </summary>
    public class ReporteService
    {
        private readonly IReporteRepository _repositorio = FactoryDataAccess.ReporteRepository;

        /// <summary>true si el reporte se filtra por fechas (el de Vehículos no: muestra el stock actual).</summary>
        public static bool UsaFechas(TipoReporte tipo) => tipo != TipoReporte.Vehiculos;

        public static string Titulo(TipoReporte tipo)
        {
            switch (tipo)
            {
                case TipoReporte.Vehiculos: return "Reporte de vehículos";
                case TipoReporte.Reparaciones: return "Reporte de reparaciones";
                case TipoReporte.Presupuestos: return "Reporte de presupuestos";
                default: return "Reporte de ventas";
            }
        }

        public DataTable Generar(TipoReporte tipo, DateTime desde, DateTime hasta)
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);

            if (UsaFechas(tipo))
                Validar.RangoFechas(desde, hasta);

            DataTable tabla;
            switch (tipo)
            {
                case TipoReporte.Vehiculos: tabla = _repositorio.Vehiculos(null); break;
                case TipoReporte.Reparaciones: tabla = _repositorio.Reparaciones(desde, hasta); break;
                case TipoReporte.Presupuestos: tabla = _repositorio.Presupuestos(desde, hasta); break;
                default: tabla = _repositorio.Ventas(desde, hasta); break;
            }

            LoggerService.Info(UsaFechas(tipo)
                ? $"Generó el {Titulo(tipo).ToLower()} del {desde:dd/MM/yyyy} al {hasta:dd/MM/yyyy}."
                : $"Generó el {Titulo(tipo).ToLower()}.");
            return tabla;
        }

        /// <summary>Resumen del stock: cantidad de autos e inversión por estado (debajo del reporte de vehículos).</summary>
        public DataTable StockPorEstado()
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);
            return _repositorio.StockPorEstado();
        }

        /// <summary>Suma de una columna numérica (para los totales de abajo del reporte).</summary>
        public static decimal Sumar(DataTable tabla, string columna)
        {
            if (!tabla.Columns.Contains(columna))
                return 0;
            return tabla.AsEnumerable().Sum(f => f.IsNull(columna) ? 0 : Convert.ToDecimal(f[columna]));
        }

        /// <summary>
        /// Exporta el reporte a un archivo CSV separado por ";" (Excel en español lo abre directo).
        /// Se guarda con UTF-8 + BOM para que Excel muestre bien las tildes y la ñ.
        /// </summary>
        public void ExportarCsv(DataTable tabla, IList<string> encabezados, string rutaArchivo)
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);

            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new SeleccionObligatoriaException("dónde guardar el archivo");

            var cultura = CultureInfo.GetCultureInfo("es-AR");
            var sb = new StringBuilder();
            sb.AppendLine(string.Join(";", encabezados.Select(Escapar)));

            foreach (DataRow fila in tabla.Rows)
            {
                var valores = new List<string>();
                foreach (DataColumn columna in tabla.Columns)
                {
                    object valor = fila[columna];
                    string texto = valor switch
                    {
                        DBNull => "",
                        DateTime fecha => fecha.ToString("dd/MM/yyyy", cultura),
                        decimal numero => numero.ToString("0.00", cultura),
                        _ => Convert.ToString(valor, cultura)
                    };
                    valores.Add(Escapar(texto));
                }
                sb.AppendLine(string.Join(";", valores));
            }

            try
            {
                File.WriteAllText(rutaArchivo, sb.ToString(), new UTF8Encoding(true));
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                LoggerService.Error("No se pudo exportar el reporte.", ex);
                throw new ArchivoInvalidoException("No se pudo guardar el archivo (¿está abierto en Excel?): " + ex.Message);
            }

            LoggerService.Info($"Exportó un reporte a {Path.GetFileName(rutaArchivo)} ({tabla.Rows.Count} filas).");
        }

        /// <summary>
        /// Exporta el reporte a PDF (hoja A4 horizontal) con QuestPDF:
        /// - Encabezado: nombre del sistema, título del reporte, período, usuario y fecha.
        /// - Cuerpo: la misma tabla que se ve en pantalla (los montos alineados a la derecha).
        ///   Si no entra en una hoja, sigue en la siguiente y repite los títulos de las columnas.
        /// - Pie: los totales y "Página X de Y".
        /// </summary>
        public void ExportarPdf(DataTable tabla, IList<string> encabezados, string titulo, string periodo,
                                string totales, string rutaArchivo)
        {
            Sesion.ExigirPermiso(Funcion.VerReportes);

            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new SeleccionObligatoriaException("dónde guardar el archivo");

            // Licencia gratuita de QuestPDF (uso académico / empresa chica)
            QuestPDF.Settings.License = LicenseType.Community;

            var cultura = CultureInfo.GetCultureInfo("es-AR");
            string usuario = Sesion.UsuarioActual?.NombreUsuario ?? "";

            try
            {
                Document.Create(documento =>
                {
                    documento.Page(pagina =>
                    {
                        pagina.Size(PageSizes.A4.Landscape());
                        pagina.Margin(25);
                        pagina.DefaultTextStyle(estilo => estilo.FontSize(8));

                        // --- Encabezado
                        pagina.Header().Column(columna =>
                        {
                            columna.Item().Text("PRAS - Programa de Registro Automotor Superior").FontSize(9).FontColor(Colors.Grey.Darken1);
                            columna.Item().Text(titulo).FontSize(16).Bold();
                            columna.Item().Text($"{periodo}   |   Generado por {usuario} el {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(9);
                            columna.Item().PaddingTop(4).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                        });

                        // --- Tabla
                        pagina.Content().PaddingVertical(8).Table(t =>
                        {
                            t.ColumnsDefinition(definicion =>
                            {
                                for (int i = 0; i < tabla.Columns.Count; i++)
                                    definicion.RelativeColumn();
                            });

                            // Los títulos de las columnas se repiten en cada hoja
                            t.Header(cabecera =>
                            {
                                for (int i = 0; i < tabla.Columns.Count; i++)
                                {
                                    string texto = i < encabezados.Count ? encabezados[i] : tabla.Columns[i].ColumnName;
                                    cabecera.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text(texto).Bold();
                                }
                            });

                            foreach (DataRow fila in tabla.Rows)
                            {
                                foreach (DataColumn col in tabla.Columns)
                                {
                                    var celda = t.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3);
                                    string texto = Formatear(fila[col], cultura, true);
                                    if (col.DataType == typeof(decimal) || col.DataType == typeof(int))
                                        celda.AlignRight().Text(texto);
                                    else
                                        celda.Text(texto);
                                }
                            }
                        });

                        // --- Pie: totales y número de página
                        pagina.Footer().Row(fila =>
                        {
                            fila.RelativeItem().Text(totales ?? "").FontSize(8);
                            fila.ConstantItem(90).AlignRight().Text(texto =>
                            {
                                texto.Span("Página ");
                                texto.CurrentPageNumber();
                                texto.Span(" de ");
                                texto.TotalPages();
                            });
                        });
                    });
                }).GeneratePdf(rutaArchivo);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                LoggerService.Error("No se pudo exportar el reporte a PDF.", ex);
                throw new ArchivoInvalidoException("No se pudo guardar el archivo (¿está abierto en otro programa?): " + ex.Message);
            }

            LoggerService.Info($"Exportó un reporte a PDF: {Path.GetFileName(rutaArchivo)} ({tabla.Rows.Count} filas).");
        }

        /// <summary>Texto de una celda: fechas dd/MM/yyyy y montos con 2 decimales (con $ si es para PDF).</summary>
        private static string Formatear(object valor, CultureInfo cultura, bool conMoneda)
        {
            return valor switch
            {
                DBNull => "",
                DateTime fecha => fecha.ToString("dd/MM/yyyy", cultura),
                decimal numero => conMoneda ? numero.ToString("C2", cultura) : numero.ToString("0.00", cultura),
                _ => Convert.ToString(valor, cultura)
            };
        }

        private static string Escapar(string texto)
        {
            texto ??= "";
            if (texto.Contains(';') || texto.Contains('"') || texto.Contains('\n'))
                return "\"" + texto.Replace("\"", "\"\"") + "\"";
            return texto;
        }
    }
}
