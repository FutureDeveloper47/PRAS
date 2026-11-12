/* ==========================================================================
   PRAS - ACTUALIZAR UNA BASE EXISTENTE: PERMISOS con el patrón COMPOSITE

   Agrega las tablas de familias y patentes
   y carga los permisos iniciales:
     - Una patente por cada función del sistema.
     - Familias: Seguridad, Maestros, Compras, Taller, Repuestos, Comercial, Reportes,
                 Dueño (contiene 6 familias), Administrador (Dueño + Seguridad) y Mecánico.
     - A cada usuario existente se le asigna la familia de su rol.

   SOLO para quien ya tenía las bases de la 3ra entrega (scripts 01 a 05 ejecutados).
   En una instalación nueva NO hace falta: PRAS_InstalacionCompleta.sql ya lo incluye.
   Ejecutar completo en SQL Server Management Studio (F5).
   No borra datos. Se puede ejecutar más de una vez sin problema.
   ========================================================================== */

USE PRAS_Seguridad;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ---------------------- PERMISOS: patrón COMPOSITE ---------------------- */
-- Permisos con patrón Composite (tablas Familia, Patente y sus relaciones):
--   Patente        = un permiso concreto (DataKey = nombre de la función en C#, enum Funcion)
--   Familia        = grupo de permisos
--   FamiliaFamilia = una familia dentro de otra (así se arma el árbol)
--   FamiliaPatente = patentes de cada familia
--   UsuarioFamilia / UsuarioPatente = permisos asignados a cada usuario
IF OBJECT_ID('Patente', 'U') IS NULL
BEGIN
    CREATE TABLE Patente (
        IdPatente   UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Patente PRIMARY KEY DEFAULT NEWID(),
        DataKey     VARCHAR(100)     NOT NULL CONSTRAINT UQ_Patente_DataKey UNIQUE,
        TipoAcceso  INT              NOT NULL,   -- 0 Pantalla, 1 CasoUso, 2 Servicio, 3 SP, 4 Tabla
        Nombre      NVARCHAR(100)    NULL
    );
    PRINT 'Tabla Patente creada.';
END
GO

IF OBJECT_ID('Familia', 'U') IS NULL
BEGIN
    CREATE TABLE Familia (
        IdFamilia  UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Familia PRIMARY KEY DEFAULT NEWID(),
        Nombre     NVARCHAR(100)    NOT NULL CONSTRAINT UQ_Familia_Nombre UNIQUE
    );
    PRINT 'Tabla Familia creada.';
END
GO

IF OBJECT_ID('FamiliaFamilia', 'U') IS NULL
BEGIN
    CREATE TABLE FamiliaFamilia (
        IdFamiliaPadre  UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_FamiliaFamilia_Padre REFERENCES Familia(IdFamilia),
        IdFamiliaHijo   UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_FamiliaFamilia_Hijo  REFERENCES Familia(IdFamilia),
        CONSTRAINT PK_FamiliaFamilia PRIMARY KEY (IdFamiliaPadre, IdFamiliaHijo),
        CONSTRAINT CK_FamiliaFamilia_Distintas CHECK (IdFamiliaPadre <> IdFamiliaHijo)
    );
    PRINT 'Tabla FamiliaFamilia creada.';
END
GO

IF OBJECT_ID('FamiliaPatente', 'U') IS NULL
BEGIN
    CREATE TABLE FamiliaPatente (
        IdFamilia  UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_FamiliaPatente_Familia REFERENCES Familia(IdFamilia),
        IdPatente  UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_FamiliaPatente_Patente REFERENCES Patente(IdPatente),
        CONSTRAINT PK_FamiliaPatente PRIMARY KEY (IdFamilia, IdPatente)
    );
    PRINT 'Tabla FamiliaPatente creada.';
END
GO

IF OBJECT_ID('UsuarioFamilia', 'U') IS NULL
BEGIN
    CREATE TABLE UsuarioFamilia (
        IdUsuario  INT              NOT NULL CONSTRAINT FK_UsuarioFamilia_Usuario REFERENCES Usuario(IdUsuario),
        IdFamilia  UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_UsuarioFamilia_Familia REFERENCES Familia(IdFamilia),
        CONSTRAINT PK_UsuarioFamilia PRIMARY KEY (IdUsuario, IdFamilia)
    );
    PRINT 'Tabla UsuarioFamilia creada.';
END
GO

IF OBJECT_ID('UsuarioPatente', 'U') IS NULL
BEGIN
    CREATE TABLE UsuarioPatente (
        IdUsuario  INT              NOT NULL CONSTRAINT FK_UsuarioPatente_Usuario REFERENCES Usuario(IdUsuario),
        IdPatente  UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_UsuarioPatente_Patente REFERENCES Patente(IdPatente),
        CONSTRAINT PK_UsuarioPatente PRIMARY KEY (IdUsuario, IdPatente)
    );
    PRINT 'Tabla UsuarioPatente creada.';
END
GO

/* -------- Patentes: una por cada función del sistema (enum Funcion) -------- */
INSERT INTO Patente (DataKey, TipoAcceso, Nombre)
SELECT P.DataKey, 1, P.Nombre
FROM (VALUES
    ('GestionarUsuarios',         N'Gestionar usuarios'),
    ('GestionarPermisos',         N'Gestionar permisos (familias y patentes)'),
    ('VerBitacora',               N'Consultar bitácora'),
    ('RealizarBackup',            N'Backup y restore'),
    ('VerificarIntegridad',       N'Verificar integridad (dígitos verificadores)'),
    ('GestionarClientes',         N'Gestionar clientes'),
    ('GestionarMecanicos',        N'Gestionar mecánicos'),
    ('GestionarProveedores',      N'Gestionar proveedores'),
    ('GestionarAgencias',         N'Gestionar agencias'),
    ('GestionarCompras',          N'Registrar y consultar compras'),
    ('GestionarVehiculos',        N'Buscador de autos'),
    ('RegistrarRecepcion',        N'Ingreso de autos al taller'),
    ('RegistrarDiagnostico',      N'Registrar diagnóstico'),
    ('ConsultarDiagnostico',      N'Consultar diagnóstico'),
    ('GestionarPresupuestos',     N'Carga de costos (presupuestos)'),
    ('GestionarOrdenesTrabajo',   N'Gestionar órdenes de trabajo'),
    ('ConsultarOrdenesTrabajo',   N'Consultar órdenes de trabajo'),
    ('RegistrarTareasRealizadas', N'Registrar tareas realizadas'),
    ('RegistrarPedidoRepuestos',  N'Registrar pedido de repuestos'),
    ('RegistrarEntregaPedido',    N'Registrar entrega de pedido'),
    ('ConsultarPedidosRepuestos', N'Consultar pedidos de repuestos'),
    ('GestionarConsignaciones',   N'Gestionar consignaciones'),
    ('GestionarPublicaciones',    N'Gestionar publicaciones'),
    ('GestionarVentas',           N'Registrar y consultar ventas'),
    ('VerReportes',               N'Ver reportes')
) AS P (DataKey, Nombre)
WHERE NOT EXISTS (SELECT 1 FROM Patente X WHERE X.DataKey = P.DataKey);
GO

/* ------------------------------- Familias ------------------------------- */
INSERT INTO Familia (Nombre)
SELECT F.Nombre
FROM (VALUES (N'Seguridad'), (N'Maestros'), (N'Compras'), (N'Taller'), (N'Repuestos'),
             (N'Comercial'), (N'Reportes'), (N'Dueño'), (N'Administrador'), (N'Mecánico')) AS F (Nombre)
WHERE NOT EXISTS (SELECT 1 FROM Familia X WHERE X.Nombre = F.Nombre);
GO

-- Patentes de cada familia "temática"
INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
SELECT F.IdFamilia, P.IdPatente
FROM (VALUES
    (N'Seguridad', 'GestionarUsuarios'), (N'Seguridad', 'GestionarPermisos'), (N'Seguridad', 'VerBitacora'),
    (N'Seguridad', 'RealizarBackup'),    (N'Seguridad', 'VerificarIntegridad'),
    (N'Maestros',  'GestionarClientes'), (N'Maestros',  'GestionarMecanicos'),
    (N'Maestros',  'GestionarProveedores'), (N'Maestros', 'GestionarAgencias'),
    (N'Compras',   'GestionarCompras'),  (N'Compras',   'GestionarVehiculos'),
    (N'Taller',    'RegistrarRecepcion'), (N'Taller', 'RegistrarDiagnostico'), (N'Taller', 'ConsultarDiagnostico'),
    (N'Taller',    'GestionarPresupuestos'), (N'Taller', 'GestionarOrdenesTrabajo'),
    (N'Taller',    'ConsultarOrdenesTrabajo'), (N'Taller', 'RegistrarTareasRealizadas'),
    (N'Repuestos', 'RegistrarPedidoRepuestos'), (N'Repuestos', 'RegistrarEntregaPedido'),
    (N'Repuestos', 'ConsultarPedidosRepuestos'),
    (N'Comercial', 'GestionarConsignaciones'), (N'Comercial', 'GestionarPublicaciones'), (N'Comercial', 'GestionarVentas'),
    (N'Reportes',  'VerReportes'),
    -- El mecánico solo tiene patentes sueltas (lo que necesita en el taller)
    (N'Mecánico',  'ConsultarOrdenesTrabajo'), (N'Mecánico', 'RegistrarTareasRealizadas'),
    (N'Mecánico',  'ConsultarDiagnostico'),    (N'Mecánico', 'RegistrarPedidoRepuestos'),
    (N'Mecánico',  'ConsultarPedidosRepuestos')
) AS X (Familia, DataKey)
INNER JOIN Familia F ON F.Nombre = X.Familia
INNER JOIN Patente P ON P.DataKey = X.DataKey
WHERE NOT EXISTS (SELECT 1 FROM FamiliaPatente FP WHERE FP.IdFamilia = F.IdFamilia AND FP.IdPatente = P.IdPatente);
GO

-- Familias dentro de familias (acá se ve el Composite):
--   Dueño         = Maestros + Compras + Taller + Repuestos + Comercial + Reportes
--   Administrador = Dueño + Seguridad
INSERT INTO FamiliaFamilia (IdFamiliaPadre, IdFamiliaHijo)
SELECT PADRE.IdFamilia, HIJO.IdFamilia
FROM (VALUES
    (N'Dueño', N'Maestros'), (N'Dueño', N'Compras'), (N'Dueño', N'Taller'),
    (N'Dueño', N'Repuestos'), (N'Dueño', N'Comercial'), (N'Dueño', N'Reportes'),
    (N'Administrador', N'Dueño'), (N'Administrador', N'Seguridad')
) AS X (Padre, Hijo)
INNER JOIN Familia PADRE ON PADRE.Nombre = X.Padre
INNER JOIN Familia HIJO  ON HIJO.Nombre  = X.Hijo
WHERE NOT EXISTS (SELECT 1 FROM FamiliaFamilia FF WHERE FF.IdFamiliaPadre = PADRE.IdFamilia AND FF.IdFamiliaHijo = HIJO.IdFamilia);
GO

-- Cada usuario que todavía no tiene permisos recibe la familia de su rol
-- (Rol: 1 = Dueño, 2 = Administrador, 3 = Mecánico)
INSERT INTO UsuarioFamilia (IdUsuario, IdFamilia)
SELECT U.IdUsuario, F.IdFamilia
FROM Usuario U
INNER JOIN Familia F ON F.Nombre = CASE U.Rol WHEN 1 THEN N'Dueño' WHEN 2 THEN N'Administrador' ELSE N'Mecánico' END
WHERE NOT EXISTS (SELECT 1 FROM UsuarioFamilia UF WHERE UF.IdUsuario = U.IdUsuario)
  AND NOT EXISTS (SELECT 1 FROM UsuarioPatente UP WHERE UP.IdUsuario = U.IdUsuario);
GO

PRINT 'Script 06 finalizado.';
GO

/* --------------------------------------------------------------------------
   CONSULTAS DE CONTROL (ejecutar aparte):

   -- Árbol de familias
   SELECT P.Nombre AS Padre, H.Nombre AS Hija
   FROM PRAS_Seguridad.dbo.FamiliaFamilia FF
   INNER JOIN PRAS_Seguridad.dbo.Familia P ON P.IdFamilia = FF.IdFamiliaPadre
   INNER JOIN PRAS_Seguridad.dbo.Familia H ON H.IdFamilia = FF.IdFamiliaHijo;

   -- Familias de cada usuario
   SELECT U.NombreUsuario, F.Nombre AS Familia
   FROM PRAS_Seguridad.dbo.UsuarioFamilia UF
   INNER JOIN PRAS_Seguridad.dbo.Usuario U ON U.IdUsuario = UF.IdUsuario
   INNER JOIN PRAS_Seguridad.dbo.Familia F ON F.IdFamilia = UF.IdFamilia;
   -------------------------------------------------------------------------- */
