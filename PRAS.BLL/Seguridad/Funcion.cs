namespace PRAS.BLL
{
    /// <summary>
    /// Funciones del sistema a las que se les controla el acceso.
    /// Cada una es una PATENTE del patrón Composite: en la tabla Patente de PRAS_Seguridad
    /// la columna DataKey tiene exactamente este nombre (ej: "GestionarVentas").
    /// </summary>
    public enum Funcion
    {
        // Seguridad
        GestionarUsuarios,
        GestionarPermisos,   // familias y patentes
        VerBitacora,
        RealizarBackup,   // backup y restore
        VerificarIntegridad,   // dígitos verificadores

        // Maestros
        GestionarClientes,
        GestionarMecanicos,
        GestionarProveedores,
        GestionarAgencias,

        // Compras y vehículos
        GestionarCompras,
        GestionarVehiculos,

        // Taller
        RegistrarRecepcion,
        RegistrarDiagnostico,
        ConsultarDiagnostico,
        GestionarPresupuestos,
        GestionarOrdenesTrabajo,   // crear, asignar tareas y terminar la reparación
        ConsultarOrdenesTrabajo,
        RegistrarTareasRealizadas,

        // Repuestos
        RegistrarPedidoRepuestos,
        RegistrarEntregaPedido,
        ConsultarPedidosRepuestos,

        // Comercial
        GestionarConsignaciones,
        GestionarPublicaciones,
        GestionarVentas,

        // Reportes
        VerReportes
    }
}
