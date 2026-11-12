using PRAS.BLL.Excepciones;
using PRAS.DAL.Factory;
using PRAS.DAL.Interfaces;
using PRAS.Entidades;
using PRAS.Services.Exceptions.DataAccess;
using PRAS.Services.Facade;
using PRAS.Services.Facade.ExtensionsMethods;

namespace PRAS.BLL
{
    /// <summary>
    /// Casos de uso Taller - Registrar diagnóstico y Consultar diagnóstico.
    /// El diagnóstico se completa con la descripción de los daños y las EVIDENCIAS
    /// (fotos, videos o documentos). Al finalizarlo, si hay que reparar, el auto pasa a reparación.
    /// </summary>
    public class DiagnosticoService
    {
        private const int LargoDanios = 1000;
        private const int LargoObservaciones = 500;
        private const int LargoDescripcionEvidencia = 200;

        /// <summary>Carpeta (al lado del .exe) donde se copian los archivos de las evidencias.</summary>
        public const string CarpetaEvidencias = "Evidencias";

        private static readonly string[] ExtensionesFoto = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
        private static readonly string[] ExtensionesVideo = { ".mp4", ".avi", ".mov", ".wmv" };

        private readonly IDiagnosticoRepository _repositorio = FactoryDataAccess.DiagnosticoRepository;
        private readonly IEvidenciaVehiculoRepository _evidenciaRepositorio = FactoryDataAccess.EvidenciaVehiculoRepository;
        private readonly IVehiculoRepository _vehiculoRepositorio = FactoryDataAccess.VehiculoRepository;
        private readonly IMecanicoRepository _mecanicoRepositorio = FactoryDataAccess.MecanicoRepository;

        // Consultas

        /// <summary>Último diagnóstico de cada auto que está en el taller.</summary>
        public List<Diagnostico> ListarParaRegistrar()
        {
            Sesion.ExigirPermiso(Funcion.RegistrarDiagnostico);
            return _repositorio.ListarUltimos(false);
        }

        /// <summary>incluirTodos = también los autos que ya salieron del taller.</summary>
        public List<Diagnostico> ListarParaConsulta(bool incluirTodos)
        {
            ExigirVerDiagnosticos();
            return _repositorio.ListarUltimos(incluirTodos);
        }

        /// <summary>El diagnóstico con sus evidencias.</summary>
        public Diagnostico Obtener(int idDiagnostico)
        {
            ExigirVerDiagnosticos();
            return _repositorio.GetById(idDiagnostico)
                ?? throw new DiagnosticoNoEncontradoException(idDiagnostico);
        }

        public List<Mecanico> ListarMecanicos()
        {
            Sesion.ExigirPermiso(Funcion.RegistrarDiagnostico);
            return _mecanicoRepositorio.Listar("", false);
        }

        // Registrar

        /// <summary>Guarda los cambios sin finalizar el diagnóstico.</summary>
        public void Guardar(Diagnostico diagnostico)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.RegistrarDiagnostico);
                ValidarDatos(diagnostico);

                Diagnostico actual = _repositorio.GetById(diagnostico.IdDiagnostico)
                    ?? throw new DiagnosticoNoEncontradoException(diagnostico.IdDiagnostico);
                diagnostico.EstadoDiagnostico = actual.EstadoDiagnostico;   // el estado no se cambia acá

                _repositorio.Update(diagnostico);
                LoggerService.Info($"Modificó el diagnóstico {diagnostico.IdDiagnostico} del vehículo {actual.Vehiculo.Dominio}.");
            }
            catch (ReglaNegocioException ex)
            {
                // Aviso de negocio: queda en la bitácora y sigue hasta la pantalla.
                ExceptionManager.HandleException(ex);
                throw;
            }
            catch (DataAccessException)
            {
                // Ya lo registró la DAL.
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(ex);
                throw;
            }
        }

        /// <summary>
        /// Finaliza el diagnóstico. Si requiere reparación hay que elegir mecánico y prioridad
        /// (se crea la orden de trabajo); si no, el auto queda Reparado.
        /// </summary>
        public void Finalizar(Diagnostico diagnostico, int? idMecanico, string prioridad)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.RegistrarDiagnostico);
                ValidarDatos(diagnostico);

                Diagnostico actual = _repositorio.GetById(diagnostico.IdDiagnostico)
                    ?? throw new DiagnosticoNoEncontradoException(diagnostico.IdDiagnostico);

                if (actual.EstadoDiagnostico == Estados.DiagnosticoFinalizado)
                    throw new DiagnosticoFinalizadoException(actual.IdDiagnostico);

                Vehiculo vehiculo = _vehiculoRepositorio.GetById(actual.IdVehiculo)
                    ?? throw new VehiculoNoEncontradoException(actual.IdVehiculo);
                if (vehiculo.EstadoVehiculo != EstadoVehiculo.EnDiagnostico)
                    throw new EstadoVehiculoInvalidoException(vehiculo, "finalizar su diagnóstico");

                diagnostico.IdRecepcion = actual.IdRecepcion;
                diagnostico.IdVehiculo = actual.IdVehiculo;

                if (diagnostico.RequiereReparacion)
                {
                    TallerService.ObtenerMecanicoActivo(idMecanico ?? 0);
                    TallerService.ValidarPrioridad(prioridad);
                }

                _repositorio.Finalizar(diagnostico, idMecanico, prioridad);

                LoggerService.Info(diagnostico.RequiereReparacion
                    ? $"Finalizó el diagnóstico del vehículo {vehiculo.Dominio}: pasa a reparación (prioridad {prioridad})."
                    : $"Finalizó el diagnóstico del vehículo {vehiculo.Dominio}: no requiere reparación, pasa a Reparado.");
            }
            catch (ReglaNegocioException ex)
            {
                // Aviso de negocio: queda en la bitácora y sigue hasta la pantalla.
                ExceptionManager.HandleException(ex);
                throw;
            }
            catch (DataAccessException)
            {
                // Ya lo registró la DAL.
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(ex);
                throw;
            }
        }

        // Evidencias

        /// <summary>
        /// Copia el archivo a la carpeta Evidencias del programa y lo registra en la base
        /// (en la base se guarda solo la ruta).
        /// </summary>
        public EvidenciaVehiculo AgregarEvidencia(int idDiagnostico, string rutaArchivo, string descripcion)
        {
            try
            {
                Sesion.ExigirPermiso(Funcion.RegistrarDiagnostico);

                Diagnostico diagnostico = _repositorio.GetById(idDiagnostico)
                    ?? throw new DiagnosticoNoEncontradoException(idDiagnostico);

                if (string.IsNullOrWhiteSpace(rutaArchivo) || !File.Exists(rutaArchivo))
                    throw new ArchivoInvalidoException("Seleccione un archivo que exista.");

                descripcion = Validar.Opcional(descripcion, "Descripción", LargoDescripcionEvidencia);

                string relativa = Path.Combine(CarpetaEvidencias, diagnostico.Vehiculo.Dominio,
                    $"{DateTime.Now:yyyyMMdd_HHmmss}_{Path.GetFileName(rutaArchivo)}");
                if (relativa.Length > 260)
                    throw new ArchivoInvalidoException("El nombre del archivo es demasiado largo.");

                CopiarArchivo(rutaArchivo, relativa);

                var evidencia = new EvidenciaVehiculo
                {
                    IdDiagnostico = idDiagnostico,
                    IdVehiculo = diagnostico.IdVehiculo,
                    Descripcion = descripcion,
                    TipoEvidencia = TipoSegunArchivo(rutaArchivo),
                    UbicacionArchivo = relativa,
                    FechaCarga = DateTime.Now
                };
                _evidenciaRepositorio.Add(evidencia);

                LoggerService.Info($"Agregó una evidencia ({evidencia.TipoEvidencia}) al diagnóstico del vehículo {diagnostico.Vehiculo.Dominio}.");
                return evidencia;
            }
            catch (ReglaNegocioException ex)
            {
                // Aviso de negocio: queda en la bitácora y sigue hasta la pantalla.
                ExceptionManager.HandleException(ex);
                throw;
            }
            catch (DataAccessException)
            {
                // Ya lo registró la DAL.
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(ex);
                throw;
            }
        }

        /// <summary>Ruta completa del archivo de la evidencia (para abrirlo).</summary>
        public string RutaCompleta(EvidenciaVehiculo evidencia)
        {
            if (Path.IsPathRooted(evidencia.UbicacionArchivo))
                return evidencia.UbicacionArchivo;
            return Path.Combine(AppContext.BaseDirectory, evidencia.UbicacionArchivo);
        }

        // Apoyo

        private static void CopiarArchivo(string origen, string relativa)
        {
            try
            {
                string destino = Path.Combine(AppContext.BaseDirectory, relativa);
                Directory.CreateDirectory(Path.GetDirectoryName(destino));
                File.Copy(origen, destino);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                LoggerService.Error("No se pudo copiar el archivo de una evidencia.", ex);
                throw new ArchivoInvalidoException("No se pudo copiar el archivo: " + ex.Message);
            }
        }

        private static string TipoSegunArchivo(string ruta)
        {
            string extension = Path.GetExtension(ruta).ToLower();
            if (ExtensionesFoto.Contains(extension)) return "Foto";
            if (ExtensionesVideo.Contains(extension)) return "Video";
            return "Documento";
        }

        private static void ValidarDatos(Diagnostico diagnostico)
        {
            if (diagnostico == null || diagnostico.IdDiagnostico <= 0)
                throw new SeleccionObligatoriaException("un diagnóstico de la lista");

            diagnostico.DescripcionDanios = Validar.Requerido(diagnostico.DescripcionDanios, "Descripción de daños", LargoDanios);
            diagnostico.Observaciones = Validar.Opcional(diagnostico.Observaciones, "Observaciones", LargoObservaciones);
        }

        /// <summary>Puede ver los diagnósticos quien los registra o quien los consulta.</summary>
        private static void ExigirVerDiagnosticos()
        {
            if (!Sesion.TienePermiso(Funcion.RegistrarDiagnostico))
                Sesion.ExigirPermiso(Funcion.ConsultarDiagnostico);
        }
    }
}
