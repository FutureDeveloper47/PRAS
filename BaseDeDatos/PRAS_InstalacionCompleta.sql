/* ==========================================================================
   PRAS - INSTALACIÓN COMPLETA DE LAS BASES DE DATOS (desde cero)

   Crea las DOS bases del sistema, ya en su versión final:
     1) PRAS            -> datos del negocio (clientes, autos, compras, taller, agencia, ventas)
     2) PRAS_Seguridad  -> usuarios, permisos (Composite), dígitos verificadores y bitácora
   y carga datos de prueba para poder usar el sistema enseguida.

   CÓMO SE USA:
     - Abrir este archivo en SQL Server Management Studio, conectado a (localdb)\MSSQLLocalDB
     - Ejecutar todo (F5).
   Si alguna de las dos bases ya existe, el script se detiene sin tocar nada.

   USUARIOS PARA INGRESAR AL SISTEMA:
     admin    / admin123     -> Administrador (todos los permisos)
     dueno    / dueno123     -> Dueño (todo menos Seguridad)
     mecanico / mecanico123  -> Mecánico (Carlos Ferreyra: solo lo del taller)
   (Cambiar las contraseñas después del primer ingreso.)

   Los dígitos verificadores NO se calculan acá: el sistema los calcula solo
   la primera vez que alguien inicia sesión.
   ========================================================================== */

USE master;
GO

IF DB_ID('PRAS') IS NOT NULL OR DB_ID('PRAS_Seguridad') IS NOT NULL
BEGIN
    PRINT 'La base PRAS o PRAS_Seguridad ya existe. No se hizo ningún cambio.';
    PRINT 'Si querés reinstalar, borrá primero las dos bases (se pierden los datos).';
    SET NOEXEC ON;
END
GO

/* ==========================================================================
   PARTE 1: BASE DEL NEGOCIO (PRAS)
   ========================================================================== */

CREATE DATABASE PRAS;
GO

USE PRAS;
GO

/* ---------------------------- TABLAS MAESTRAS ---------------------------- */

CREATE TABLE Taller (
    IdTaller      INT IDENTITY(1,1) PRIMARY KEY,
    RazonSocial   NVARCHAR(100) NOT NULL,
    CUIT          VARCHAR(13)   NOT NULL,
    Domicilio     NVARCHAR(100) NOT NULL,
    Localidad     NVARCHAR(50)  NOT NULL,
    Telefono      VARCHAR(20)   NULL,
    Email         NVARCHAR(100) NULL
);

CREATE TABLE Cliente (
    IdCliente     INT IDENTITY(1,1) PRIMARY KEY,
    Nombre        NVARCHAR(50)  NOT NULL,
    Apellido      NVARCHAR(50)  NOT NULL,
    DNI           VARCHAR(10)   NOT NULL CONSTRAINT UQ_Cliente_DNI UNIQUE,
    CUIT          VARCHAR(13)   NULL,
    Domicilio     NVARCHAR(100) NULL,
    Localidad     NVARCHAR(50)  NULL,
    Telefono      VARCHAR(100)  NULL,     -- encriptado con AES (texto "ENC:...")
    Email         NVARCHAR(300) NULL,     -- encriptado con AES (texto "ENC:...")
    Activo        BIT NOT NULL CONSTRAINT DF_Cliente_Activo DEFAULT 1,
    DVH           VARCHAR(32)   NULL      -- dígito verificador horizontal
);

CREATE TABLE Mecanico (
    IdMecanico    INT IDENTITY(1,1) PRIMARY KEY,
    Nombre        NVARCHAR(50)  NOT NULL,
    Apellido      NVARCHAR(50)  NOT NULL,
    Especialidad  NVARCHAR(50)  NULL,
    Telefono      VARCHAR(20)   NULL,
    Email         NVARCHAR(100) NULL,
    Activo        BIT NOT NULL CONSTRAINT DF_Mecanico_Activo DEFAULT 1
);

CREATE TABLE Proveedor (
    IdProveedor   INT IDENTITY(1,1) PRIMARY KEY,
    RazonSocial   NVARCHAR(100) NOT NULL,
    CUIT          VARCHAR(13)   NOT NULL CONSTRAINT UQ_Proveedor_CUIT UNIQUE,
    Domicilio     NVARCHAR(100) NULL,
    Telefono      VARCHAR(20)   NULL,
    Email         NVARCHAR(100) NULL,
    Activo        BIT NOT NULL CONSTRAINT DF_Proveedor_Activo DEFAULT 1
);

CREATE TABLE Agencia (
    IdAgencia     INT IDENTITY(1,1) PRIMARY KEY,
    RazonSocial   NVARCHAR(100) NOT NULL,
    CUIT          VARCHAR(13)   NOT NULL CONSTRAINT UQ_Agencia_CUIT UNIQUE,
    Domicilio     NVARCHAR(100) NULL,
    Localidad     NVARCHAR(50)  NULL,
    Telefono      VARCHAR(20)   NULL,
    Email         NVARCHAR(100) NULL,
    Activo        BIT NOT NULL CONSTRAINT DF_Agencia_Activo DEFAULT 1
);

/* ------------------------------- VEHÍCULO -------------------------------- */

-- EstadoVehiculo: 1 Comprado, 2 EnDiagnostico, 3 EnReparacion, 4 Reparado,
--                 5 EnAgencia, 6 Publicado, 7 Vendido (igual que el enum en C#)
CREATE TABLE Vehiculo (
    IdVehiculo      INT IDENTITY(1,1) PRIMARY KEY,
    Dominio         VARCHAR(10)  NOT NULL CONSTRAINT UQ_Vehiculo_Dominio UNIQUE,
    Marca           NVARCHAR(50) NOT NULL,
    Modelo          NVARCHAR(50) NOT NULL,
    Anio            INT          NOT NULL CONSTRAINT CK_Vehiculo_Anio CHECK (Anio BETWEEN 1950 AND 2100),
    Color           NVARCHAR(30) NULL,
    NumeroChasis    VARCHAR(30)  NULL,
    NumeroMotor     VARCHAR(30)  NULL,
    EstadoVehiculo  TINYINT      NOT NULL CONSTRAINT DF_Vehiculo_Estado DEFAULT 1
                                 CONSTRAINT CK_Vehiculo_Estado CHECK (EstadoVehiculo BETWEEN 1 AND 7)
);

/* -------------------------------- COMPRA --------------------------------- */

CREATE TABLE Compra (
    IdCompra        INT IDENTITY(1,1) PRIMARY KEY,
    IdVehiculo      INT NOT NULL CONSTRAINT UQ_Compra_Vehiculo UNIQUE
                        CONSTRAINT FK_Compra_Vehiculo REFERENCES Vehiculo(IdVehiculo),
    IdCliente       INT NOT NULL CONSTRAINT FK_Compra_Cliente REFERENCES Cliente(IdCliente),
    FechaCompra     DATETIME      NOT NULL,
    PrecioCompra    DECIMAL(18,2) NOT NULL CONSTRAINT CK_Compra_Precio CHECK (PrecioCompra > 0),
    FormaPago       NVARCHAR(30)  NOT NULL,
    Observaciones   NVARCHAR(500) NULL
);

/* -------------------------------- TALLER --------------------------------- */

CREATE TABLE RecepcionVehiculo (
    IdRecepcion      INT IDENTITY(1,1) PRIMARY KEY,
    IdVehiculo       INT NOT NULL CONSTRAINT FK_Recepcion_Vehiculo REFERENCES Vehiculo(IdVehiculo),
    FechaIngreso     DATETIME      NOT NULL,
    MotivoIngreso    NVARCHAR(200) NOT NULL,
    EstadoRecepcion  VARCHAR(20)   NOT NULL CONSTRAINT DF_Recepcion_Estado DEFAULT 'Pendiente',
    Observaciones    NVARCHAR(500) NULL
);

CREATE TABLE Diagnostico (
    IdDiagnostico       INT IDENTITY(1,1) PRIMARY KEY,
    IdRecepcion         INT NOT NULL CONSTRAINT FK_Diagnostico_Recepcion REFERENCES RecepcionVehiculo(IdRecepcion),
    IdVehiculo          INT NOT NULL CONSTRAINT FK_Diagnostico_Vehiculo REFERENCES Vehiculo(IdVehiculo),
    FechaDiagnostico    DATETIME       NOT NULL,
    DescripcionDanios   NVARCHAR(1000) NOT NULL,
    EstadoDiagnostico   VARCHAR(20)    NOT NULL CONSTRAINT DF_Diagnostico_Estado DEFAULT 'Pendiente',
    RequiereReparacion  BIT            NOT NULL CONSTRAINT DF_Diagnostico_Req DEFAULT 1,
    Observaciones       NVARCHAR(500)  NULL
);

CREATE TABLE EvidenciaVehiculo (
    IdEvidencia       INT IDENTITY(1,1) PRIMARY KEY,
    IdDiagnostico     INT NOT NULL CONSTRAINT FK_Evidencia_Diagnostico REFERENCES Diagnostico(IdDiagnostico),
    IdVehiculo        INT NOT NULL CONSTRAINT FK_Evidencia_Vehiculo REFERENCES Vehiculo(IdVehiculo),
    Descripcion       NVARCHAR(200) NULL,
    TipoEvidencia     VARCHAR(20)   NOT NULL,
    UbicacionArchivo  NVARCHAR(260) NOT NULL,
    FechaCarga        DATETIME      NOT NULL
);

CREATE TABLE Presupuesto (
    IdPresupuesto      INT IDENTITY(1,1) PRIMARY KEY,
    IdVehiculo         INT NOT NULL CONSTRAINT FK_Presupuesto_Vehiculo REFERENCES Vehiculo(IdVehiculo),
    FechaEmision       DATETIME      NOT NULL,
    FechaVencimiento   DATETIME      NOT NULL,
    EstadoPresupuesto  VARCHAR(20)   NOT NULL CONSTRAINT DF_Presupuesto_Estado DEFAULT 'Pendiente',
    MontoTotal         DECIMAL(18,2) NOT NULL CONSTRAINT DF_Presupuesto_Monto DEFAULT 0,
    Observaciones      NVARCHAR(500) NULL
);

CREATE TABLE DetallePresupuesto (
    IdDetallePresupuesto  INT IDENTITY(1,1) PRIMARY KEY,
    IdPresupuesto         INT NOT NULL CONSTRAINT FK_DetPresupuesto_Presupuesto REFERENCES Presupuesto(IdPresupuesto),
    DescripcionItem       NVARCHAR(200) NOT NULL,
    Cantidad              INT           NOT NULL CONSTRAINT CK_DetPresupuesto_Cant CHECK (Cantidad > 0),
    PrecioUnitario        DECIMAL(18,2) NOT NULL CONSTRAINT CK_DetPresupuesto_Precio CHECK (PrecioUnitario >= 0),
    Subtotal              DECIMAL(18,2) NOT NULL,
    Aprobado              BIT           NOT NULL CONSTRAINT DF_DetPresupuesto_Aprob DEFAULT 0
);

CREATE TABLE OrdenTrabajo (
    IdOrdenTrabajo  INT IDENTITY(1,1) PRIMARY KEY,
    IdRecepcion     INT NOT NULL CONSTRAINT FK_OT_Recepcion   REFERENCES RecepcionVehiculo(IdRecepcion),
    IdPresupuesto   INT NOT NULL CONSTRAINT FK_OT_Presupuesto REFERENCES Presupuesto(IdPresupuesto),
    IdVehiculo      INT NOT NULL CONSTRAINT FK_OT_Vehiculo    REFERENCES Vehiculo(IdVehiculo),
    IdTaller        INT NOT NULL CONSTRAINT FK_OT_Taller      REFERENCES Taller(IdTaller),
    FechaCreacion   DATETIME      NOT NULL,
    EstadoOrden     VARCHAR(20)   NOT NULL CONSTRAINT DF_OT_Estado DEFAULT 'Abierta',
    Prioridad       VARCHAR(10)   NOT NULL CONSTRAINT DF_OT_Prioridad DEFAULT 'Media',
    Observaciones   NVARCHAR(500) NULL
);

CREATE TABLE TareaOrdenTrabajo (
    IdTarea           INT IDENTITY(1,1) PRIMARY KEY,
    IdOrdenTrabajo    INT NOT NULL CONSTRAINT FK_Tarea_OT       REFERENCES OrdenTrabajo(IdOrdenTrabajo),
    IdMecanico        INT NOT NULL CONSTRAINT FK_Tarea_Mecanico REFERENCES Mecanico(IdMecanico),
    DescripcionTarea  NVARCHAR(300) NOT NULL,
    CostoEstimado     DECIMAL(18,2) NOT NULL CONSTRAINT CK_Tarea_Costo CHECK (CostoEstimado >= 0),
    EstadoTarea       VARCHAR(20)   NOT NULL CONSTRAINT DF_Tarea_Estado DEFAULT 'Pendiente',
    FechaInicio       DATETIME      NULL,
    FechaFin          DATETIME      NULL
);

/* ------------------------------- REPUESTOS ------------------------------- */

CREATE TABLE PedidoRepuesto (
    IdPedido        INT IDENTITY(1,1) PRIMARY KEY,
    IdProveedor     INT NOT NULL CONSTRAINT FK_Pedido_Proveedor REFERENCES Proveedor(IdProveedor),
    IdOrdenTrabajo  INT NOT NULL CONSTRAINT FK_Pedido_OT        REFERENCES OrdenTrabajo(IdOrdenTrabajo),
    FechaPedido     DATETIME      NOT NULL,
    FechaEntrega    DATETIME      NULL,
    EstadoPedido    VARCHAR(20)   NOT NULL CONSTRAINT DF_Pedido_Estado DEFAULT 'Pendiente',
    MontoTotal      DECIMAL(18,2) NOT NULL CONSTRAINT DF_Pedido_Monto DEFAULT 0
);

CREATE TABLE DetallePedidoRepuesto (
    IdDetallePedido      INT IDENTITY(1,1) PRIMARY KEY,
    IdPedido             INT NOT NULL CONSTRAINT FK_DetPedido_Pedido REFERENCES PedidoRepuesto(IdPedido),
    DescripcionRepuesto  NVARCHAR(200) NOT NULL,
    Cantidad             INT           NOT NULL CONSTRAINT CK_DetPedido_Cant CHECK (Cantidad > 0),
    PrecioUnitario       DECIMAL(18,2) NOT NULL CONSTRAINT CK_DetPedido_Precio CHECK (PrecioUnitario >= 0),
    Subtotal             DECIMAL(18,2) NOT NULL
);

/* ------------------------ CONSIGNACIÓN Y VENTA --------------------------- */

CREATE TABLE Consignacion (
    IdConsignacion      INT IDENTITY(1,1) PRIMARY KEY,
    IdVehiculo          INT NOT NULL CONSTRAINT FK_Consignacion_Vehiculo REFERENCES Vehiculo(IdVehiculo),
    IdAgencia           INT NOT NULL CONSTRAINT FK_Consignacion_Agencia  REFERENCES Agencia(IdAgencia),
    FechaEntrega        DATETIME      NOT NULL,
    FechaRetiro         DATETIME      NULL,
    PrecioMinimoVenta   DECIMAL(18,2) NOT NULL CONSTRAINT CK_Consignacion_Precio CHECK (PrecioMinimoVenta > 0),
    PorcentajeComision  DECIMAL(5,2)  NOT NULL CONSTRAINT CK_Consignacion_Comision CHECK (PorcentajeComision > 0 AND PorcentajeComision <= 100),
    EstadoConsignacion  VARCHAR(20)   NOT NULL CONSTRAINT DF_Consignacion_Estado DEFAULT 'Vigente'
);

CREATE TABLE Publicacion (
    IdPublicacion      INT IDENTITY(1,1) PRIMARY KEY,
    IdVehiculo         INT NOT NULL CONSTRAINT FK_Publicacion_Vehiculo REFERENCES Vehiculo(IdVehiculo),
    FechaPublicacion   DATETIME       NOT NULL,
    FechaFinalizacion  DATETIME       NULL,
    PrecioPublicado    DECIMAL(18,2)  NOT NULL CONSTRAINT CK_Publicacion_Precio CHECK (PrecioPublicado > 0),
    Plataforma         NVARCHAR(50)   NOT NULL,
    Descripcion        NVARCHAR(1000) NULL,
    EstadoPublicacion  VARCHAR(20)    NOT NULL CONSTRAINT DF_Publicacion_Estado DEFAULT 'Activa'
);

CREATE TABLE Venta (
    IdVenta         INT IDENTITY(1,1) PRIMARY KEY,
    IdConsignacion  INT NOT NULL CONSTRAINT UQ_Venta_Consignacion UNIQUE
                        CONSTRAINT FK_Venta_Consignacion REFERENCES Consignacion(IdConsignacion),
    IdCliente       INT NOT NULL CONSTRAINT FK_Venta_Cliente REFERENCES Cliente(IdCliente),
    FechaVenta      DATETIME      NOT NULL,
    PrecioVenta     DECIMAL(18,2) NOT NULL CONSTRAINT CK_Venta_Precio CHECK (PrecioVenta > 0),
    MontoComision   DECIMAL(18,2) NOT NULL CONSTRAINT CK_Venta_Comision CHECK (MontoComision >= 0),
    FormaPago       NVARCHAR(30)  NOT NULL
);
GO


/* ---------------------------- DATOS INICIALES ---------------------------- */

INSERT INTO Taller (RazonSocial, CUIT, Domicilio, Localidad, Telefono, Email)
VALUES (N'PRAS S.A.', '30-00000000-0', N'Completar domicilio', N'Completar localidad', NULL, NULL);
GO

PRINT 'Base PRAS creada.';
GO

/* ------------------------------ DATOS DE PRUEBA ------------------------------ */
-- 20 autos con historias realistas: recién comprados, en diagnóstico, en reparación,
-- reparados, en agencia, publicados y vendidos.

SET NOCOUNT ON;
SET XACT_ABORT ON;   -- ante cualquier error, se cancela toda la transacción

BEGIN TRANSACTION;

/* ------------------------- Mecánicos y agencias ------------------------- */

INSERT INTO Mecanico (Nombre, Apellido, Especialidad, Telefono, Email)
SELECT M.Nombre, M.Apellido, M.Especialidad, M.Telefono, M.Email
FROM (VALUES
    (N'Carlos', N'Ferreyra', N'Chapa y pintura',  '11-4658-2231', N'cferreyra@pras.com.ar'),
    (N'Jorge',  N'Quiroga',  N'Mecánica general', '11-5987-4410', N'jquiroga@pras.com.ar'),
    (N'Pablo',  N'Ledesma',  N'Electricidad',     '11-3321-7789', N'pledesma@pras.com.ar')
) AS M (Nombre, Apellido, Especialidad, Telefono, Email)
WHERE NOT EXISTS (SELECT 1 FROM Mecanico X WHERE X.Nombre = M.Nombre AND X.Apellido = M.Apellido);

INSERT INTO Agencia (RazonSocial, CUIT, Domicilio, Localidad, Telefono, Email)
SELECT A.RazonSocial, A.CUIT, A.Domicilio, A.Localidad, A.Telefono, A.Email
FROM (VALUES
    (N'Automotores del Oeste S.A.', '30-71458236-9', N'Av. Rivadavia 17850',       N'Morón', '11-4629-5500', N'ventas@autosdeloeste.com.ar'),
    (N'Haedo Usados S.R.L.',        '30-70896541-2', N'Av. Presidente Perón 1420', N'Haedo', '11-4443-8120', N'contacto@haedousados.com.ar')
) AS A (RazonSocial, CUIT, Domicilio, Localidad, Telefono, Email)
WHERE NOT EXISTS (SELECT 1 FROM Agencia X WHERE X.CUIT = A.CUIT);

/* ------------------------- Autos y sus compras ------------------------- */
-- Estado: 1 Comprado, 2 EnDiagnostico, 3 EnReparacion, 4 Reparado, 5 EnAgencia, 6 Publicado, 7 Vendido

CREATE TABLE #Autos (
    Dominio VARCHAR(10), Marca NVARCHAR(50), Modelo NVARCHAR(50), Anio INT, Color NVARCHAR(30),
    Chasis VARCHAR(30), Motor VARCHAR(30), Estado TINYINT,
    FechaCompra DATETIME, VendNombre NVARCHAR(50), VendApellido NVARCHAR(50), VendDNI VARCHAR(10),
    VendLocalidad NVARCHAR(50), VendTelefono VARCHAR(20),
    PrecioCompra DECIMAL(18,2), FormaPago NVARCHAR(30), Observaciones NVARCHAR(500)
);

INSERT INTO #Autos VALUES
('AE530NS', N'Fiat',       N'Cronos',     2023, N'Blanco',        '8AP359AFGPU214587', '55282328',      1, '20260929', N'Rocío',     N'Giménez',  '42587963', N'Hurlingham',        '11-6234-1187',  9800000, N'Transferencia', N'Choque frontal leve. Airbags sin activar.'),
('AE214KP', N'Toyota',     N'Corolla',    2022, N'Blanco perla',  '9BRBD3HE3N0187452', '2ZRH458712',    1, '20260927', N'Martín',    N'Gómez',    '33512874', N'Ituzaingó',         '11-5412-7789', 11800000, N'Transferencia', N'Golpe lateral derecho: puertas delantera y trasera.'),
('AD903RT', N'Volkswagen', N'Amarok',     2020, N'Gris oscuro',   '8AWDB22H0LA024563', 'DDXA07854',     1, '20260924', N'Carolina',  N'Benítez',  '29874563', N'Castelar',          '11-4478-9921', 14500000, N'Transferencia', N'Vuelco sin pérdida total. Techo y parante A dañados.'),
('AB561MN', N'Chevrolet',  N'Onix',       2019, N'Rojo',          '9BGKS48U0KG145236', 'L4D125874',     1, '20260922', N'Facundo',   N'Ruiz',     '38456721', N'Morón',             '11-3698-4521',  5900000, N'Efectivo',      N'Choque trasero: baúl y paragolpes.'),
('AF072LD', N'Renault',    N'Sandero',    2023, N'Azul',          '93Y5SRBE4PJ658741', 'H4MA732R01458', 1, '20260919', N'Lucía',     N'Pereyra',  '40125896', N'San Justo',         '11-5874-3362',  8700000, N'Transferencia', N'Granizo fuerte en capot y techo.'),
('AF118CX', N'Jeep',       N'Renegade',   2023, N'Gris',          '98848111XPK254789', '552987412',     2, '20260918', N'Tomás',     N'Aguirre',  '37845126', N'Ramos Mejía',       '11-4125-6698', 13200000, N'Transferencia', N'Impacto frontal izquierdo: óptica y guardabarros.'),
('AC477WS', N'Fiat',       N'Toro',       2021, N'Blanco',        '9BD281A34MYW45871', '55263447',      2, '20260915', N'Hernán',    N'Castro',   '31789654', N'Merlo',             '11-6547-2210', 12300000, N'Transferencia', N'Choque en cadena: frente y parte trasera.'),
('AE688BF', N'Peugeot',    N'2008',       2022, N'Gris aluminio', '8AD2CN6AUNG021457', '10FJCG021457',  2, '20260911', N'Valeria',   N'Romero',   '36258147', N'Villa Tesei',       '11-3214-5587', 10200000, N'Cheque',        N'Rozamiento lateral izquierdo y espejo roto.'),
('AD150HJ', N'Ford',       N'Ranger',     2019, N'Negro',         '8AFAR23L4KJ125478', 'SA2QJ125478',   3, '20260905', N'Gustavo',   N'Medina',   '27456983', N'Moreno',            '11-4789-6325', 13900000, N'Transferencia', N'Choque frontal: radiador y paragolpes.'),
('AB320QS', N'Volkswagen', N'Polo',       2020, N'Plateado',      '9BWAL5BZ4LP047852', 'CWS047852',     3, '20260902', N'Sofía',     N'Herrera',  '39874125', N'Haedo',             '11-6589-4471',  7400000, N'Efectivo',      N'Golpe en puerta trasera derecha y zócalo.'),
('AC905GT', N'Toyota',     N'Hilux',      2018, N'Gris',          '8AJHA8CD3J2587412', '1GDG258741',    3, '20260826', N'Roberto',   N'Silva',    '25698741', N'Luján',             '11-5236-9874', 15200000, N'Transferencia', N'Despiste en ruta: tren delantero y frente.'),
('NTY384',  N'Volkswagen', N'Suran',      2014, N'Blanco',        '8AWPB45Z1EA521478', 'CFZ521478',     4, '20260818', N'Andrea',    N'Morales',  '32145698', N'William C. Morris', '11-4758-3321',  3900000, N'Efectivo',      N'Choque trasero leve.'),
('AA912PL', N'Chevrolet',  N'Cruze',      2017, N'Azul',          '8AGBN69S0HR125874', 'LDE125874',     4, '20260812', N'Nicolás',   N'Torres',   '34789125', N'Hurlingham',        '11-6632-1458',  6800000, N'Transferencia', N'Capot, óptica derecha y paragolpes.'),
('AB744DC', N'Renault',    N'Kangoo',     2019, N'Blanco',        '8A1FC1315KL254178', 'K9KB608254178', 5, '20260804', N'Marcelo',   N'Flores',   '26541237', N'Ituzaingó',         '11-4412-5896',  6200000, N'Transferencia', N'Puerta corrediza lateral hundida.'),
('OKM519',  N'Honda',      N'Fit',        2015, N'Gris',          '93HGE8850FZ104587', 'L15A7104587',   5, '20260728', N'Paula',     N'Díaz',     '37412589', N'Castelar',          '11-3987-2214',  4700000, N'Efectivo',      N'Golpe frontal leve.'),
('AC263VX', N'Nissan',     N'Kicks',      2021, N'Rojo',          '94DFCAP15MB478521', 'HR16478521',    6, '20260721', N'Sebastián', N'Álvarez',  '33698521', N'Morón',             '11-5521-7896',  9600000, N'Transferencia', N'Lateral izquierdo: dos puertas.'),
('AA388KT', N'Citroën',    N'C4 Cactus',  2018, N'Negro',         '935SXHMZ8JB521478', '10FJCB521478',  6, '20260714', N'Julieta',   N'Ramírez',  '35124789', N'El Palomar',        '11-4896-3325',  6900000, N'Transferencia', N'Granizo y parabrisas.'),
('AD611ZB', N'Toyota',     N'Yaris',      2021, N'Gris',          '9BRKC3F30M8125874', '2NRJ125874',    7, '20260709', N'Martina',   N'Suárez',   '41258963', N'Ramos Mejía',       '11-6325-1147',  8900000, N'Transferencia', N'Frente y óptica izquierda.'),
('AB097RM', N'Volkswagen', N'Vento',      2019, N'Blanco',        '3VWLL7AJ4KM214587', 'CZD214587',     7, '20260630', N'Ezequiel',  N'Molina',   '28963147', N'Haedo',             '11-4523-6698',  8100000, N'Efectivo',      N'Choque trasero y baúl.'),
('MLP226',  N'Peugeot',    N'308',        2014, N'Gris',          '8AD4C5FS0EG258741', '10FHBP258741',  7, '20260620', N'Walter',    N'Ortiz',    '24785963', N'Moreno',            '11-3365-4789',  3600000, N'Efectivo',      N'Guardabarros y puerta delantera derecha.');

-- Vendedores (particulares que nos vendieron el auto chocado)
INSERT INTO Cliente (Nombre, Apellido, DNI, Localidad, Telefono)
SELECT A.VendNombre, A.VendApellido, A.VendDNI, A.VendLocalidad, A.VendTelefono
FROM #Autos A
WHERE NOT EXISTS (SELECT 1 FROM Cliente C WHERE C.DNI = A.VendDNI);

-- Vehículos: solo las patentes que todavía no existen. Se guardan en #Nuevos
-- para que todo lo que sigue (compra, taller, agencia, venta) se cargue solo para ellos.
CREATE TABLE #Nuevos (IdVehiculo INT, Dominio VARCHAR(10));

INSERT INTO Vehiculo (Dominio, Marca, Modelo, Anio, Color, NumeroChasis, NumeroMotor, EstadoVehiculo)
OUTPUT INSERTED.IdVehiculo, INSERTED.Dominio INTO #Nuevos (IdVehiculo, Dominio)
SELECT A.Dominio, A.Marca, A.Modelo, A.Anio, A.Color, A.Chasis, A.Motor, A.Estado
FROM #Autos A
WHERE NOT EXISTS (SELECT 1 FROM Vehiculo V WHERE V.Dominio = A.Dominio);

INSERT INTO Compra (IdVehiculo, IdCliente, FechaCompra, PrecioCompra, FormaPago, Observaciones)
SELECT N.IdVehiculo, C.IdCliente, A.FechaCompra, A.PrecioCompra, A.FormaPago, A.Observaciones
FROM #Nuevos N
INNER JOIN #Autos A  ON A.Dominio = N.Dominio
INNER JOIN Cliente C ON C.DNI = A.VendDNI;

/* ------------------------------- Taller ------------------------------- */
-- Todos los autos que ya no están "Comprado" pasaron por el taller.
-- Presupuesto NULL = todavía no se cargaron los costos (quedan para "Carga de costos").

CREATE TABLE #Taller (
    Dominio VARCHAR(10), FechaIngreso DATETIME, Prioridad VARCHAR(10),
    Danios NVARCHAR(500), Presupuesto DECIMAL(18,2) NULL
);

INSERT INTO #Taller VALUES
('AF118CX', '20260922', 'Media', N'Óptica delantera izquierda rota, guardabarros y paragolpes hundidos. Revisar radiador.', NULL),
('AC477WS', '20260917', 'Alta',  N'Frente: capot, paragolpes y radiador. Trasera: portón y paragolpes. Revisar chasis.', NULL),
('AE688BF', '20260914', 'Baja',  N'Rayones profundos en puertas izquierdas y espejo exterior roto.', NULL),
('AD150HJ', '20260908', 'Alta',  N'Radiador perforado, parrilla y paragolpes rotos, soporte de motor dañado.', NULL),
('AB320QS', '20260904', 'Media', N'Puerta trasera derecha hundida y zócalo doblado.', NULL),
('AC905GT', '20260829', 'Alta',  N'Parrilla de suspensión y extremos de dirección dañados. Frente completo.', NULL),
('NTY384',  '20260820', 'Baja',  N'Paragolpes trasero y portón.', 850000),
('AA912PL', '20260814', 'Media', N'Capot, óptica derecha y paragolpes delantero.', 1450000),
('AB744DC', '20260806', 'Media', N'Puerta corrediza y guía de la puerta.', 980000),
('OKM519',  '20260730', 'Baja',  N'Paragolpes delantero y óptica.', 620000),
('AC263VX', '20260723', 'Media', N'Dos puertas izquierdas: enderezado y pintura.', 1380000),
('AA388KT', '20260716', 'Baja',  N'Parabrisas y sacabollos por granizo.', 890000),
('AD611ZB', '20260711', 'Media', N'Frente y óptica izquierda.', 1150000),
('AB097RM', '20260702', 'Media', N'Baúl, paragolpes y luces traseras.', 1020000),
('MLP226',  '20260623', 'Baja',  N'Guardabarros y puerta delantera derecha.', 540000);

-- Recepción (ingreso al taller)
INSERT INTO RecepcionVehiculo (IdVehiculo, FechaIngreso, MotivoIngreso, EstadoRecepcion)
SELECT N.IdVehiculo, T.FechaIngreso, N'Reparación de vehículo comprado chocado',
       CASE WHEN V.EstadoVehiculo IN (2, 3) THEN 'Pendiente' ELSE 'Finalizada' END
FROM #Nuevos N
INNER JOIN #Taller T  ON T.Dominio = N.Dominio
INNER JOIN Vehiculo V ON V.IdVehiculo = N.IdVehiculo;

-- Diagnóstico inicial
INSERT INTO Diagnostico (IdRecepcion, IdVehiculo, FechaDiagnostico, DescripcionDanios, EstadoDiagnostico, RequiereReparacion)
SELECT R.IdRecepcion, N.IdVehiculo, DATEADD(DAY, 1, T.FechaIngreso), T.Danios,
       CASE WHEN V.EstadoVehiculo = 2 THEN 'Pendiente' ELSE 'Finalizado' END, 1
FROM #Nuevos N
INNER JOIN #Taller T           ON T.Dominio = N.Dominio
INNER JOIN Vehiculo V          ON V.IdVehiculo = N.IdVehiculo
INNER JOIN RecepcionVehiculo R ON R.IdVehiculo = N.IdVehiculo;

-- Desde "En reparación" en adelante ya tienen presupuesto, orden de trabajo y tarea.
INSERT INTO Presupuesto (IdVehiculo, FechaEmision, FechaVencimiento, EstadoPresupuesto, MontoTotal)
SELECT N.IdVehiculo, DATEADD(DAY, 1, T.FechaIngreso), DATEADD(DAY, 16, T.FechaIngreso),
       CASE WHEN T.Presupuesto IS NULL THEN 'Pendiente' ELSE 'Aprobado' END,
       ISNULL(T.Presupuesto, 0)
FROM #Nuevos N
INNER JOIN #Taller T  ON T.Dominio = N.Dominio
INNER JOIN Vehiculo V ON V.IdVehiculo = N.IdVehiculo
WHERE V.EstadoVehiculo >= 3;

INSERT INTO DetallePresupuesto (IdPresupuesto, DescripcionItem, Cantidad, PrecioUnitario, Subtotal, Aprobado)
SELECT P.IdPresupuesto, N'Chapa, pintura, repuestos y mano de obra', 1, P.MontoTotal, P.MontoTotal, 1
FROM #Nuevos N
INNER JOIN Presupuesto P ON P.IdVehiculo = N.IdVehiculo
WHERE P.MontoTotal > 0;

INSERT INTO OrdenTrabajo (IdRecepcion, IdPresupuesto, IdVehiculo, IdTaller, FechaCreacion, EstadoOrden, Prioridad)
SELECT R.IdRecepcion, P.IdPresupuesto, N.IdVehiculo,
       (SELECT TOP 1 IdTaller FROM Taller ORDER BY IdTaller),
       DATEADD(DAY, 1, T.FechaIngreso),
       CASE WHEN V.EstadoVehiculo = 3 THEN 'Abierta' ELSE 'Finalizada' END,
       T.Prioridad
FROM #Nuevos N
INNER JOIN #Taller T           ON T.Dominio = N.Dominio
INNER JOIN Vehiculo V          ON V.IdVehiculo = N.IdVehiculo
INNER JOIN RecepcionVehiculo R ON R.IdVehiculo = N.IdVehiculo
INNER JOIN Presupuesto P       ON P.IdVehiculo = N.IdVehiculo;

-- Cada orden queda asignada a uno de los mecánicos activos (repartidos, no siempre el mismo).
INSERT INTO TareaOrdenTrabajo (IdOrdenTrabajo, IdMecanico, DescripcionTarea, CostoEstimado, EstadoTarea, FechaInicio, FechaFin)
SELECT OT.IdOrdenTrabajo, MEC.IdMecanico, LEFT(N'Reparar: ' + T.Danios, 300),
       ISNULL(T.Presupuesto, 0),
       CASE WHEN V.EstadoVehiculo = 3 THEN 'En curso' ELSE 'Finalizada' END,
       DATEADD(DAY, 2, T.FechaIngreso),
       CASE WHEN V.EstadoVehiculo = 3 THEN NULL ELSE DATEADD(DAY, 12, T.FechaIngreso) END
FROM #Nuevos N
INNER JOIN #Taller T       ON T.Dominio = N.Dominio
INNER JOIN Vehiculo V      ON V.IdVehiculo = N.IdVehiculo
INNER JOIN OrdenTrabajo OT ON OT.IdVehiculo = N.IdVehiculo
CROSS APPLY (SELECT TOP 1 M.IdMecanico FROM Mecanico M WHERE M.Activo = 1
             ORDER BY ABS(CHECKSUM(N.Dominio, M.IdMecanico))) MEC;

/* --------------------- Agencia: consignación y publicación --------------------- */

CREATE TABLE #Agencia (
    Dominio VARCHAR(10), FechaEntrega DATETIME, AgenciaCUIT VARCHAR(13),
    PrecioMinimo DECIMAL(18,2), Comision DECIMAL(5,2),
    FechaPublicacion DATETIME NULL, PrecioPublicado DECIMAL(18,2) NULL, Plataforma NVARCHAR(50) NULL
);

INSERT INTO #Agencia VALUES
('AB744DC', '20260910', '30-71458236-9', 12500000, 6.00, NULL,       NULL,     NULL),
('OKM519',  '20260906', '30-70896541-2',  9800000, 5.00, NULL,       NULL,     NULL),
('AC263VX', '20260829', '30-71458236-9', 19500000, 6.00, '20260830', 21500000, N'MercadoLibre'),
('AA388KT', '20260822', '30-70896541-2', 15000000, 5.00, '20260823', 16900000, N'Facebook Marketplace'),
('AD611ZB', '20260814', '30-71458236-9', 18000000, 6.00, '20260815', 19900000, N'MercadoLibre'),
('AB097RM', '20260805', '30-70896541-2', 16500000, 5.00, '20260806', 18900000, N'MercadoLibre'),
('MLP226',  '20260720', '30-71458236-9',  8500000, 6.00, '20260721',  9900000, N'Instagram');

/* ------------------------------- Ventas ------------------------------- */

CREATE TABLE #Ventas (
    Dominio VARCHAR(10), FechaVenta DATETIME, PrecioVenta DECIMAL(18,2),
    CompNombre NVARCHAR(50), CompApellido NVARCHAR(50), CompDNI VARCHAR(10),
    CompLocalidad NVARCHAR(50), CompTelefono VARCHAR(20), FormaPago NVARCHAR(30)
);

INSERT INTO #Ventas VALUES
('AD611ZB', '20260926', 19800000, N'Alejandro', N'Vega',  '30987456', N'Morón',      '11-5698-1234', N'Transferencia'),
('AB097RM', '20260917', 18500000, N'Florencia', N'Rojas', '36987412', N'Castelar',   '11-4785-9632', N'Financiado'),
('MLP226',  '20260908',  9600000, N'Matías',    N'Luna',  '38521476', N'Hurlingham', '11-3698-7412', N'Efectivo');

-- Consignación: el auto se entrega a la agencia. Si ya se vendió, queda cerrada.
INSERT INTO Consignacion (IdVehiculo, IdAgencia, FechaEntrega, FechaRetiro, PrecioMinimoVenta, PorcentajeComision, EstadoConsignacion)
SELECT N.IdVehiculo, AG.IdAgencia, X.FechaEntrega, VE.FechaVenta, X.PrecioMinimo, X.Comision,
       CASE WHEN VE.Dominio IS NULL THEN 'Vigente' ELSE 'Vendida' END
FROM #Nuevos N
INNER JOIN #Agencia X ON X.Dominio = N.Dominio
INNER JOIN Agencia AG ON AG.CUIT = X.AgenciaCUIT
LEFT JOIN #Ventas VE  ON VE.Dominio = N.Dominio;

-- Publicación en internet (los que están "Publicado" o ya se vendieron)
INSERT INTO Publicacion (IdVehiculo, FechaPublicacion, FechaFinalizacion, PrecioPublicado, Plataforma, Descripcion, EstadoPublicacion)
SELECT N.IdVehiculo, X.FechaPublicacion, VE.FechaVenta, X.PrecioPublicado, X.Plataforma,
       V.Marca + N' ' + V.Modelo + N' ' + CAST(V.Anio AS NVARCHAR(4)) + N' - ' + ISNULL(V.Color, N'') +
       N'. Reparado a nuevo, papeles al día, listo para transferir.',
       CASE WHEN VE.Dominio IS NULL THEN 'Activa' ELSE 'Finalizada' END
FROM #Nuevos N
INNER JOIN #Agencia X ON X.Dominio = N.Dominio
INNER JOIN Vehiculo V ON V.IdVehiculo = N.IdVehiculo
LEFT JOIN #Ventas VE  ON VE.Dominio = N.Dominio
WHERE X.FechaPublicacion IS NOT NULL;

-- Compradores
INSERT INTO Cliente (Nombre, Apellido, DNI, Localidad, Telefono)
SELECT VE.CompNombre, VE.CompApellido, VE.CompDNI, VE.CompLocalidad, VE.CompTelefono
FROM #Ventas VE
WHERE NOT EXISTS (SELECT 1 FROM Cliente C WHERE C.DNI = VE.CompDNI);

-- Venta: la comisión de la agencia sale del porcentaje de la consignación.
INSERT INTO Venta (IdConsignacion, IdCliente, FechaVenta, PrecioVenta, MontoComision, FormaPago)
SELECT CO.IdConsignacion, C.IdCliente, VE.FechaVenta, VE.PrecioVenta,
       ROUND(VE.PrecioVenta * CO.PorcentajeComision / 100, 2), VE.FormaPago
FROM #Nuevos N
INNER JOIN #Ventas VE      ON VE.Dominio = N.Dominio
INNER JOIN Consignacion CO ON CO.IdVehiculo = N.IdVehiculo
INNER JOIN Cliente C       ON C.DNI = VE.CompDNI;

COMMIT TRANSACTION;

/* ------------------------------ Resumen ------------------------------ */

DECLARE @AutosNuevos INT = (SELECT COUNT(*) FROM #Nuevos);
PRINT 'Datos de prueba cargados. Autos nuevos: ' + CAST(@AutosNuevos AS VARCHAR(10));

SELECT V.Dominio, V.Marca, V.Modelo, V.Anio,
       CASE V.EstadoVehiculo
            WHEN 1 THEN 'Comprado' WHEN 2 THEN 'En diagnóstico' WHEN 3 THEN 'En reparación'
            WHEN 4 THEN 'Reparado' WHEN 5 THEN 'En agencia'     WHEN 6 THEN 'Publicado'
            ELSE 'Vendido' END AS Estado
FROM Vehiculo V
ORDER BY V.EstadoVehiculo, V.Dominio;

DROP TABLE #Autos;
DROP TABLE #Nuevos;
DROP TABLE #Taller;
DROP TABLE #Agencia;
DROP TABLE #Ventas;
GO

/* ==========================================================================
   PARTE 2: BASE DE SEGURIDAD (PRAS_Seguridad)
   ========================================================================== */

CREATE DATABASE PRAS_Seguridad;
GO

USE PRAS_Seguridad;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ------------------------------- USUARIO -------------------------------- */
-- Rol: 1 = Dueño, 2 = Administrador, 3 = Mecánico (igual que el enum Rol en C#)
IF OBJECT_ID('Usuario', 'U') IS NULL
BEGIN
    CREATE TABLE Usuario (
        IdUsuario       INT IDENTITY(1,1) PRIMARY KEY,
        NombreUsuario   NVARCHAR(50) NOT NULL CONSTRAINT UQ_Usuario_Nombre UNIQUE,
        ContrasenaHash  VARCHAR(64)  NOT NULL,
        Rol             TINYINT      NOT NULL CONSTRAINT CK_Usuario_Rol CHECK (Rol BETWEEN 1 AND 3),
        Activo          BIT          NOT NULL CONSTRAINT DF_Usuario_Activo DEFAULT 1,
        -- IdMecanico apunta a la tabla Mecanico de la base PRAS. Como está en OTRA base,
        -- SQL Server no permite una clave foránea: el control lo hace la BLL (UsuarioService).
        IdMecanico      INT          NULL,
        Idioma          VARCHAR(10)  NULL,
        DVH             VARCHAR(32)  NULL,
        CONSTRAINT CK_Usuario_Mecanico CHECK (IdMecanico IS NULL OR Rol = 3)
    );

    -- Un mecánico no puede tener dos usuarios (índice "filtrado": ignora los NULL).
    CREATE UNIQUE INDEX UQ_Usuario_IdMecanico ON Usuario(IdMecanico) WHERE IdMecanico IS NOT NULL;

    PRINT 'Tabla Usuario creada.';
END
GO

/* -------------------------- DIGITO VERIFICADOR -------------------------- */
IF OBJECT_ID('DigitoVerificador', 'U') IS NULL
BEGIN
    CREATE TABLE DigitoVerificador (
        Tabla  VARCHAR(50) NOT NULL PRIMARY KEY,
        DVV    VARCHAR(32) NOT NULL
    );
    PRINT 'Tabla DigitoVerificador creada.';
END
GO

/* ------------------------------- BITACORA ------------------------------- */
-- Nivel: Debug, Info, Warn, Error, Fatal (igual que el enum LogLevel en C#)
IF OBJECT_ID('Bitacora', 'U') IS NULL
BEGIN
    CREATE TABLE Bitacora (
        IdBitacora        INT IDENTITY(1,1) PRIMARY KEY,
        Fecha             DATETIME       NOT NULL,
        Nivel             VARCHAR(10)    NOT NULL,
        Usuario           NVARCHAR(50)   NULL,
        Mensaje           NVARCHAR(MAX)  NOT NULL,
        Metodo            NVARCHAR(200)  NULL,
        TipoExcepcion     NVARCHAR(200)  NULL,
        MensajeExcepcion  NVARCHAR(MAX)  NULL,
        StackTrace        NVARCHAR(MAX)  NULL
    );

    CREATE INDEX IX_Bitacora_Fecha ON Bitacora(Fecha);
    PRINT 'Tabla Bitacora creada.';
END
GO


/* ------------------------------ USUARIOS ------------------------------ */
-- Contraseñas guardadas como hash SHA-256 (nunca en texto plano).
-- Rol: 1 = Dueño, 2 = Administrador, 3 = Mecánico
INSERT INTO Usuario (NombreUsuario, ContrasenaHash, Rol, Activo, IdMecanico)
VALUES (N'admin', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'admin123'), 2), 2, 1, NULL),
       (N'dueno', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'dueno123'), 2), 1, 1, NULL);

-- El usuario mecánico queda vinculado al mecánico Carlos Ferreyra (tabla Mecanico de la base PRAS).
INSERT INTO Usuario (NombreUsuario, ContrasenaHash, Rol, Activo, IdMecanico)
SELECT N'mecanico', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'mecanico123'), 2), 3, 1, M.IdMecanico
FROM PRAS.dbo.Mecanico M
WHERE M.Nombre = N'Carlos' AND M.Apellido = N'Ferreyra';
GO

/* ------------------------ USUARIO RECUPERACION ------------------------- */
-- Pregunta de seguridad de cada usuario, para recuperar la contraseña desde la pantalla de ingreso.
-- Va en una tabla aparte (y no en Usuario) para no cambiar las columnas que protege el DVH.
-- La respuesta se guarda como hash SHA-256 (en minúsculas y sin espacios de más), nunca en texto plano.
IF OBJECT_ID('UsuarioRecuperacion', 'U') IS NULL
BEGIN
    CREATE TABLE UsuarioRecuperacion (
        IdUsuario          INT            NOT NULL PRIMARY KEY
                           CONSTRAINT FK_UsuarioRecuperacion_Usuario REFERENCES Usuario(IdUsuario),
        PreguntaSeguridad  NVARCHAR(200)  NOT NULL,
        RespuestaHash      VARCHAR(64)    NOT NULL
    );
    PRINT 'Tabla UsuarioRecuperacion creada.';
END
GO

-- Pregunta de prueba para el admin (respuesta: pras), así se puede mostrar "Recuperar contraseña".
INSERT INTO UsuarioRecuperacion (IdUsuario, PreguntaSeguridad, RespuestaHash)
SELECT U.IdUsuario, N'¿Cuál es el nombre del sistema?', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'pras'), 2)
FROM Usuario U
WHERE U.NombreUsuario = N'admin'
  AND NOT EXISTS (SELECT 1 FROM UsuarioRecuperacion R WHERE R.IdUsuario = U.IdUsuario);
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

PRINT '------------------------------------------------------------';
PRINT 'Instalación terminada. Bases PRAS y PRAS_Seguridad creadas.';
PRINT 'Ingresar al sistema con admin / admin123';
PRINT '------------------------------------------------------------';
GO

SET NOEXEC OFF;
GO
