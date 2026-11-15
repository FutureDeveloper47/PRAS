using System.Data;
using PRAS.DAL.Interfaces;
using PRAS.DAL.Tools;

namespace PRAS.DAL.Implementaciones.SqlServer
{
    /// <summary>
    /// Datos de prueba del negocio repartidos entre 2024 y 2026 (singleton): 18 autos con toda su historia
    /// (compra, taller, presupuesto, orden de trabajo, pedido de repuestos, agencia, publicación y venta),
    /// sus vendedores y compradores, y 3 proveedores. Sirve para que todas las pantallas y reportes
    /// muestren varios registros al defender la tesis.
    ///
    /// Se carga UNA sola vez: si ya existe alguno de estos autos, no hace nada (así, si se borra uno a mano,
    /// no vuelve a aparecer). Todo va en una transacción: o se carga entero o no se carga nada.
    /// Los clientes se insertan con teléfono en texto plano: la BLL los encripta con AES y recalcula
    /// sus dígitos verificadores enseguida (por eso este método devuelve los Id de los clientes nuevos).
    /// </summary>
    internal sealed class DatosPruebaRepository : IDatosPruebaRepository
    {
        #region Singleton
        private static readonly DatosPruebaRepository _instance = new DatosPruebaRepository();

        public static DatosPruebaRepository Current
        {
            get { return _instance; }
        }

        private DatosPruebaRepository()
        {
        }
        #endregion

        private const string CargarOperacionesStatement = @"
SET NOCOUNT ON;
SET XACT_ABORT ON;   -- ante cualquier error se cancela todo

-- Si ya se cargaron (alguno de estos autos existe), no se hace nada.
IF EXISTS (SELECT 1 FROM Vehiculo WHERE Dominio IN ('AA741RT', 'AE450RS', 'AF718WD'))
BEGIN
    SELECT CAST(NULL AS INT) AS IdCliente WHERE 1 = 0;
    RETURN;
END

BEGIN TRANSACTION;

-- Agencias y proveedores (solo si no están)
INSERT INTO Agencia (RazonSocial, CUIT, Domicilio, Localidad, Telefono, Email)
SELECT A.RazonSocial, A.CUIT, A.Domicilio, A.Localidad, A.Telefono, A.Email
FROM (VALUES
    (N'Automotores del Oeste S.A.', '30-71458236-9', N'Av. Rivadavia 17850',       N'Morón', '11-4629-5500', N'ventas@autosdeloeste.com.ar'),
    (N'Haedo Usados S.R.L.',        '30-70896541-2', N'Av. Presidente Perón 1420', N'Haedo', '11-4443-8120', N'contacto@haedousados.com.ar')
) AS A (RazonSocial, CUIT, Domicilio, Localidad, Telefono, Email)
WHERE NOT EXISTS (SELECT 1 FROM Agencia X WHERE X.CUIT = A.CUIT);

INSERT INTO Proveedor (RazonSocial, CUIT, Domicilio, Telefono, Email)
SELECT P.RazonSocial, P.CUIT, P.Domicilio, P.Telefono, P.Email
FROM (VALUES
    (N'Repuestos del Oeste S.A.',      '30-71602348-1', N'Av. Gaona 3520, Ramos Mejía', '11-4654-2210', N'ventas@repuestosdeloeste.com.ar'),
    (N'Pinturería Haedo S.R.L.',       '30-70711259-6', N'Av. Rivadavia 15240, Haedo',  '11-4659-3381', N'pedidos@pintureriahaedo.com.ar'),
    (N'Electricidad Automotor Morón',  '30-71388420-3', N'Belgrano 410, Morón',         '11-4627-9045', N'taller@electricidadmoron.com.ar')
) AS P (RazonSocial, CUIT, Domicilio, Telefono, Email)
WHERE NOT EXISTS (SELECT 1 FROM Proveedor X WHERE X.CUIT = P.CUIT);

-- Autos: compra, taller, agencia y (si se vendió) venta, todo en una fila por auto.
-- Estado: 5 En agencia, 6 Publicado, 7 Vendido.
CREATE TABLE #Datos (
    Dominio VARCHAR(10), Marca NVARCHAR(50), Modelo NVARCHAR(50), Anio INT, Color NVARCHAR(30), Estado TINYINT,
    FechaCompra DATETIME, PrecioCompra DECIMAL(18,2), FormaPagoCompra NVARCHAR(30), Danios NVARCHAR(300),
    VendNombre NVARCHAR(50), VendApellido NVARCHAR(50), VendDNI VARCHAR(10), VendLocalidad NVARCHAR(50), VendTelefono VARCHAR(20),
    Prioridad VARCHAR(10), Presupuesto DECIMAL(18,2), AgenciaCUIT VARCHAR(13), FechaEntrega DATETIME,
    PrecioMinimo DECIMAL(18,2), Comision DECIMAL(5,2), FechaPublicacion DATETIME NULL, PrecioPublicado DECIMAL(18,2) NULL,
    Plataforma NVARCHAR(50) NULL, FechaVenta DATETIME NULL, PrecioVenta DECIMAL(18,2) NULL, FormaPagoVenta NVARCHAR(30) NULL,
    CompNombre NVARCHAR(50) NULL, CompApellido NVARCHAR(50) NULL, CompDNI VARCHAR(10) NULL, CompLocalidad NVARCHAR(50) NULL,
    CompTelefono VARCHAR(20) NULL, ProveedorCUIT VARCHAR(13) NULL, Repuesto NVARCHAR(200) NULL, MontoRepuesto DECIMAL(18,2) NULL
);

INSERT INTO #Datos VALUES
-- 2024
('AA741RT', N'Chevrolet',  N'Prisma',     2016, N'Gris',   7, '20240212',  3200000, N'Transferencia', N'Choque frontal: paragolpes, capot y óptica derecha.',
 N'Daniel',   N'Acosta',    '22814503', N'Morón',          '11-4512-7730', 'Media', 680000,  '30-71458236-9', '20240305',  6000000, 6.00, '20240306',  7200000, N'MercadoLibre',          '20240410',  6900000, N'Financiado',
 N'Verónica', N'Paz',       '25481976', N'Castelar',       '11-5236-1094', '30-71602348-1', N'Óptica delantera derecha', 185000),
('AB205LM', N'Ford',       N'Ka',         2018, N'Rojo',   7, '20240318',  2900000, N'Efectivo',      N'Golpe lateral izquierdo: puerta y zócalo.',
 N'Silvina',  N'Ortega',    '23905117', N'Ituzaingó',      '11-6021-4458', 'Baja',  540000,  '30-70896541-2', '20240410',  5500000, 5.00, '20240411',  6400000, N'Facebook Marketplace',  '20240525',  6200000, N'Efectivo',
 N'Federico', N'Ibáñez',    '26739102', N'Haedo',          '11-4785-2210', NULL, NULL, NULL),
('AC118PQ', N'Toyota',     N'Etios',      2019, N'Blanco', 7, '20240506',  4100000, N'Transferencia', N'Choque trasero: baúl, paragolpes y luces.',
 N'Ricardo',  N'Benedetti', '24718339', N'Ramos Mejía',    '11-3398-6621', 'Media', 820000,  '30-71458236-9', '20240601',  7800000, 6.00, '20240602',  8900000, N'MercadoLibre',          '20240720',  8600000, N'Transferencia',
 N'Gabriela', N'Ferro',     '27915843', N'Morón',          '11-6654-0932', '30-70711259-6', N'Pintura poliuretánica (4 litros)', 210000),
('AB933HN', N'Renault',    N'Logan',      2017, N'Azul',   7, '20240702',  2700000, N'Efectivo',      N'Granizo en techo y capot.',
 N'Mónica',   N'Villalba',  '26150482', N'San Justo',      '11-4420-7815', 'Baja',  450000,  '30-70896541-2', '20240725',  5000000, 5.00, '20240726',  5900000, N'Instagram',             '20240830',  5700000, N'Efectivo',
 N'Leandro',  N'Quintana',  '29083416', N'Hurlingham',     '11-5874-6603', NULL, NULL, NULL),
('AD027KS', N'Volkswagen', N'Gol',        2020, N'Gris',   7, '20240909',  3600000, N'Transferencia', N'Vuelco leve: techo, parante y parabrisas.',
 N'Alberto',  N'Sosa',      '27390615', N'Moreno',         '11-3365-1170', 'Alta',  1100000, '30-71458236-9', '20241004',  7200000, 6.00, '20241005',  8300000, N'MercadoLibre',          '20241118',  8000000, N'Transferencia',
 N'Camila',   N'Rossi',     '30471259', N'Castelar',       '11-4908-3326', '30-71602348-1', N'Parabrisas laminado', 320000),
('AC650BV', N'Peugeot',    N'208',        2019, N'Negro',  7, '20241104',  4300000, N'Cheque',        N'Impacto frontal izquierdo y guardabarros.',
 N'Patricia', N'Luque',     '28604271', N'Villa Tesei',    '11-6120-9947', 'Media', 950000,  '30-70896541-2', '20241128',  8200000, 5.00, '20241129',  9400000, N'Autocosmos',            '20250110',  9100000, N'Financiado',
 N'Esteban',  N'Correa',    '31659820', N'Ituzaingó',      '11-3017-5582', NULL, NULL, NULL),
-- 2025
('AE210DF', N'Fiat',       N'Argo',       2021, N'Rojo',   7, '20250203',  5200000, N'Transferencia', N'Lateral derecho: dos puertas y espejo.',
 N'Hugo',     N'Medrano',   '29517846', N'Morón',          '11-4471-8839', 'Media', 1050000, '30-71458236-9', '20250301',  9800000, 6.00, '20250302', 11200000, N'MercadoLibre',          '20250415', 10900000, N'Transferencia',
 N'Lorena',   N'Bianchi',   '32904573', N'Haedo',          '11-5590-2214', '30-70711259-6', N'Masilla, fondo y pintura', 175000),
('AD845TR', N'Chevrolet',  N'Tracker',    2020, N'Blanco', 7, '20250317',  7600000, N'Transferencia', N'Choque frontal fuerte: radiador, frente y airbags.',
 N'Jorgelina',N'Paredes',   '31086253', N'Merlo',          '11-6398-4127', 'Alta',  1600000, '30-70896541-2', '20250415', 13500000, 5.00, '20250416', 15400000, N'MercadoLibre',          '20250602', 15000000, N'Financiado',
 N'Andrés',   N'Molina',    '34172806', N'Ramos Mejía',    '11-4236-7790', '30-71602348-1', N'Radiador y electroventilador', 540000),
('AE377GC', N'Nissan',     N'Versa',      2022, N'Gris',   7, '20250512',  8100000, N'Transferencia', N'Choque trasero y portón.',
 N'Marcos',   N'Funes',     '32741908', N'Hurlingham',     '11-3712-0045', 'Media', 1250000, '30-71458236-9', '20250610', 13900000, 6.00, '20250611', 15800000, N'Facebook Marketplace',  '20250722', 15300000, N'Transferencia',
 N'Natalia',  N'Ferreira',  '35588231', N'Morón',          '11-6874-3315', NULL, NULL, NULL),
('AC902MW', N'Ford',       N'EcoSport',   2018, N'Azul',   7, '20250707',  5400000, N'Efectivo',      N'Rozamiento lateral y llanta rota.',
 N'Cecilia',  N'Maldonado', '33958162', N'Castelar',       '11-4590-6612', 'Baja',  760000,  '30-70896541-2', '20250801',  9700000, 5.00, '20250802', 11000000, N'Instagram',             '20250915', 10600000, N'Efectivo',
 N'Oscar',    N'Giménez',   '36740915', N'Haedo',          '11-5120-7748', '30-71388420-3', N'Sensor de estacionamiento', 95000),
('AF041JB', N'Renault',    N'Duster',     2023, N'Gris',   7, '20250825',  9800000, N'Transferencia', N'Despiste: tren delantero y paragolpes.',
 N'Gonzalo',  N'Peralta',   '34205719', N'Luján',          '11-6731-2296', 'Alta',  1900000, '30-71458236-9', '20250925', 17500000, 6.00, '20250926', 19900000, N'MercadoLibre',          '20251105', 19400000, N'Transferencia',
 N'Florencia',N'Navarro',   '37291648', N'Moreno',         '11-4402-8857', '30-71602348-1', N'Parrilla de suspensión y extremos', 610000),
('AE699YN', N'Toyota',     N'Yaris',      2022, N'Rojo',   7, '20251006',  8800000, N'Transferencia', N'Golpe en puerta delantera izquierda.',
 N'Laura',    N'Cabrera',   '35670384', N'Ituzaingó',      '11-3869-5524', 'Media', 1150000, '30-70896541-2', '20251104', 14800000, 5.00, '20251105', 16700000, N'Sitio web de la agencia','20251220', 16200000, N'Financiado',
 N'Pablo',    N'Domínguez', '38625097', N'Castelar',       '11-6015-9381', NULL, NULL, NULL),
-- 2026 (enero a mayo; las de junio en adelante ya están en la instalación)
('AF305QT', N'Volkswagen', N'T-Cross',    2023, N'Blanco', 7, '20251201', 12500000, N'Transferencia', N'Choque frontal: capot, ópticas y paragolpes.',
 N'Sergio',   N'Vallejos',  '36419527', N'Morón',          '11-4733-6190', 'Media', 1700000, '30-71458236-9', '20260105', 21000000, 6.00, '20260106', 23900000, N'MercadoLibre',          '20260218', 23200000, N'Transferencia',
 N'Mariana',  N'Lucero',    '39874310', N'Haedo',          '11-5307-2468', '30-71388420-3', N'Ópticas LED (par)', 720000),
('AE118LP', N'Chevrolet',  N'Onix Plus',  2022, N'Gris',   7, '20260112',  7900000, N'Efectivo',      N'Granizo y parabrisas.',
 N'Ramiro',   N'Toledo',    '37802641', N'San Justo',      '11-6280-1173', 'Baja',  980000,  '30-70896541-2', '20260210', 13600000, 5.00, '20260211', 15500000, N'Facebook Marketplace',  '20260325', 15100000, N'Efectivo',
 N'Julián',   N'Arias',     '40216583', N'Hurlingham',     '11-3945-6627', NULL, NULL, NULL),
('AF552NR', N'Peugeot',    N'2008',       2023, N'Azul',   7, '20260209', 11200000, N'Transferencia', N'Vuelco: techo, parantes y puertas.',
 N'Adriana',  N'Moyano',    '38156093', N'Merlo',          '11-4115-3389', 'Alta',  2100000, '30-71458236-9', '20260310', 19500000, 6.00, '20260311', 22400000, N'MercadoLibre',          '20260428', 21800000, N'Financiado',
 N'Diego',    N'Herrera',   '41539076', N'Morón',          '11-6542-8801', '30-70711259-6', N'Pintura completa y barniz', 480000),
('AE903KH', N'Fiat',       N'Cronos',     2022, N'Gris',   7, '20260316',  7300000, N'Transferencia', N'Golpe trasero: paragolpes y baúl.',
 N'Hernán',   N'Ríos',      '39407285', N'Ramos Mejía',    '11-3528-4410', 'Media', 1050000, '30-70896541-2', '20260415', 12800000, 5.00, '20260416', 14600000, N'Instagram',             '20260530', 14200000, N'Transferencia',
 N'Agustina', N'Castro',    '42087314', N'Castelar',       '11-4886-1052', NULL, NULL, NULL),
-- Sin vender todavía
('AF718WD', N'Honda',      N'HR-V',       2021, N'Negro',  6, '20260420', 10800000, N'Transferencia', N'Lateral izquierdo: tres paneles.',
 N'Claudio',  N'Benítez',   '40683152', N'Moreno',         '11-6177-3304', 'Media', 1500000, '30-71458236-9', '20260520', 18500000, 6.00, '20260521', 21000000, N'MercadoLibre',          NULL, NULL, NULL,
 NULL, NULL, NULL, NULL, NULL, '30-71602348-1', N'Moldura y manijas', 130000),
('AE450RS', N'Toyota',     N'Etios',      2020, N'Blanco', 5, '20260505',  5100000, N'Efectivo',      N'Paragolpes delantero y óptica.',
 N'Elena',    N'Sánchez',   '41927364', N'Ituzaingó',      '11-5063-7718', 'Baja',  700000,  '30-70896541-2', '20260601',  9200000, 5.00, NULL, NULL, NULL,                           NULL, NULL, NULL,
 NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- Clientes: vendedores y compradores (con teléfono en texto plano; la BLL lo encripta enseguida)
CREATE TABLE #ClientesNuevos (IdCliente INT);

INSERT INTO Cliente (Nombre, Apellido, DNI, Localidad, Telefono)
OUTPUT INSERTED.IdCliente INTO #ClientesNuevos (IdCliente)
SELECT C.Nombre, C.Apellido, C.DNI, C.Localidad, C.Telefono
FROM (
    SELECT VendNombre AS Nombre, VendApellido AS Apellido, VendDNI AS DNI, VendLocalidad AS Localidad, VendTelefono AS Telefono FROM #Datos
    UNION
    SELECT CompNombre, CompApellido, CompDNI, CompLocalidad, CompTelefono FROM #Datos WHERE CompDNI IS NOT NULL
) AS C
WHERE NOT EXISTS (SELECT 1 FROM Cliente X WHERE X.DNI = C.DNI);

-- Vehículos y compras
CREATE TABLE #Nuevos (IdVehiculo INT, Dominio VARCHAR(10));

INSERT INTO Vehiculo (Dominio, Marca, Modelo, Anio, Color, EstadoVehiculo)
OUTPUT INSERTED.IdVehiculo, INSERTED.Dominio INTO #Nuevos (IdVehiculo, Dominio)
SELECT D.Dominio, D.Marca, D.Modelo, D.Anio, D.Color, D.Estado
FROM #Datos D
WHERE NOT EXISTS (SELECT 1 FROM Vehiculo V WHERE V.Dominio = D.Dominio);

INSERT INTO Compra (IdVehiculo, IdCliente, FechaCompra, PrecioCompra, FormaPago, Observaciones)
SELECT N.IdVehiculo, C.IdCliente, D.FechaCompra, D.PrecioCompra, D.FormaPagoCompra, D.Danios
FROM #Nuevos N
INNER JOIN #Datos D  ON D.Dominio = N.Dominio
INNER JOIN Cliente C ON C.DNI = D.VendDNI;

-- Taller: ingreso 2 días después de la compra, diagnóstico al día siguiente, todo finalizado.
INSERT INTO RecepcionVehiculo (IdVehiculo, FechaIngreso, MotivoIngreso, EstadoRecepcion)
SELECT N.IdVehiculo, DATEADD(DAY, 2, D.FechaCompra), N'Reparación de vehículo comprado chocado', 'Finalizada'
FROM #Nuevos N INNER JOIN #Datos D ON D.Dominio = N.Dominio;

INSERT INTO Diagnostico (IdRecepcion, IdVehiculo, FechaDiagnostico, DescripcionDanios, EstadoDiagnostico, RequiereReparacion)
SELECT R.IdRecepcion, N.IdVehiculo, DATEADD(DAY, 3, D.FechaCompra), D.Danios, 'Finalizado', 1
FROM #Nuevos N
INNER JOIN #Datos D            ON D.Dominio = N.Dominio
INNER JOIN RecepcionVehiculo R ON R.IdVehiculo = N.IdVehiculo;

INSERT INTO Presupuesto (IdVehiculo, FechaEmision, FechaVencimiento, EstadoPresupuesto, MontoTotal)
SELECT N.IdVehiculo, DATEADD(DAY, 3, D.FechaCompra), DATEADD(DAY, 18, D.FechaCompra), 'Aprobado', D.Presupuesto
FROM #Nuevos N INNER JOIN #Datos D ON D.Dominio = N.Dominio;

INSERT INTO DetallePresupuesto (IdPresupuesto, DescripcionItem, Cantidad, PrecioUnitario, Subtotal, Aprobado)
SELECT P.IdPresupuesto, N'Chapa, pintura, repuestos y mano de obra', 1, P.MontoTotal, P.MontoTotal, 1
FROM #Nuevos N INNER JOIN Presupuesto P ON P.IdVehiculo = N.IdVehiculo;

INSERT INTO OrdenTrabajo (IdRecepcion, IdPresupuesto, IdVehiculo, IdTaller, FechaCreacion, EstadoOrden, Prioridad)
SELECT R.IdRecepcion, P.IdPresupuesto, N.IdVehiculo, (SELECT TOP 1 IdTaller FROM Taller ORDER BY IdTaller),
       DATEADD(DAY, 4, D.FechaCompra), 'Finalizada', D.Prioridad
FROM #Nuevos N
INNER JOIN #Datos D            ON D.Dominio = N.Dominio
INNER JOIN RecepcionVehiculo R ON R.IdVehiculo = N.IdVehiculo
INNER JOIN Presupuesto P       ON P.IdVehiculo = N.IdVehiculo;

-- Cada orden queda asignada a uno de los mecánicos activos (repartidos, no siempre el mismo).
INSERT INTO TareaOrdenTrabajo (IdOrdenTrabajo, IdMecanico, DescripcionTarea, CostoEstimado, EstadoTarea, FechaInicio, FechaFin)
SELECT OT.IdOrdenTrabajo, MEC.IdMecanico, LEFT(N'Reparar: ' + D.Danios, 300), D.Presupuesto, 'Finalizada',
       DATEADD(DAY, 5, D.FechaCompra), DATEADD(DAY, 15, D.FechaCompra)
FROM #Nuevos N
INNER JOIN #Datos D        ON D.Dominio = N.Dominio
INNER JOIN OrdenTrabajo OT ON OT.IdVehiculo = N.IdVehiculo
CROSS APPLY (SELECT TOP 1 M.IdMecanico FROM Mecanico M WHERE M.Activo = 1
             ORDER BY ABS(CHECKSUM(N.Dominio, M.IdMecanico))) MEC;

-- Pedidos de repuestos (entregados) de algunas órdenes
INSERT INTO PedidoRepuesto (IdProveedor, IdOrdenTrabajo, FechaPedido, FechaEntrega, EstadoPedido, MontoTotal)
SELECT PR.IdProveedor, OT.IdOrdenTrabajo, DATEADD(DAY, 5, D.FechaCompra), DATEADD(DAY, 9, D.FechaCompra), 'Entregado', D.MontoRepuesto
FROM #Nuevos N
INNER JOIN #Datos D        ON D.Dominio = N.Dominio
INNER JOIN OrdenTrabajo OT ON OT.IdVehiculo = N.IdVehiculo
INNER JOIN Proveedor PR    ON PR.CUIT = D.ProveedorCUIT
WHERE D.ProveedorCUIT IS NOT NULL;

INSERT INTO DetallePedidoRepuesto (IdPedido, DescripcionRepuesto, Cantidad, PrecioUnitario, Subtotal)
SELECT PE.IdPedido, D.Repuesto, 1, D.MontoRepuesto, D.MontoRepuesto
FROM #Nuevos N
INNER JOIN #Datos D          ON D.Dominio = N.Dominio
INNER JOIN OrdenTrabajo OT   ON OT.IdVehiculo = N.IdVehiculo
INNER JOIN PedidoRepuesto PE ON PE.IdOrdenTrabajo = OT.IdOrdenTrabajo
WHERE D.ProveedorCUIT IS NOT NULL;

-- Agencia: consignación (cerrada si se vendió) y publicación
INSERT INTO Consignacion (IdVehiculo, IdAgencia, FechaEntrega, FechaRetiro, PrecioMinimoVenta, PorcentajeComision, EstadoConsignacion)
SELECT N.IdVehiculo, AG.IdAgencia, D.FechaEntrega, D.FechaVenta, D.PrecioMinimo, D.Comision,
       CASE WHEN D.FechaVenta IS NULL THEN 'Vigente' ELSE 'Vendida' END
FROM #Nuevos N
INNER JOIN #Datos D ON D.Dominio = N.Dominio
CROSS APPLY (SELECT TOP 1 IdAgencia FROM Agencia WHERE CUIT = D.AgenciaCUIT ORDER BY IdAgencia) AG;

INSERT INTO Publicacion (IdVehiculo, FechaPublicacion, FechaFinalizacion, PrecioPublicado, Plataforma, Descripcion, EstadoPublicacion)
SELECT N.IdVehiculo, D.FechaPublicacion, D.FechaVenta, D.PrecioPublicado, D.Plataforma,
       D.Marca + N' ' + D.Modelo + N' ' + CAST(D.Anio AS NVARCHAR(4)) + N' - ' + D.Color +
       N'. Reparado a nuevo, papeles al día, listo para transferir.',
       CASE WHEN D.FechaVenta IS NULL THEN 'Activa' ELSE 'Finalizada' END
FROM #Nuevos N INNER JOIN #Datos D ON D.Dominio = N.Dominio
WHERE D.FechaPublicacion IS NOT NULL;

-- Venta: la comisión de la agencia sale del porcentaje de la consignación
INSERT INTO Venta (IdConsignacion, IdCliente, FechaVenta, PrecioVenta, MontoComision, FormaPago)
SELECT CO.IdConsignacion, C.IdCliente, D.FechaVenta, D.PrecioVenta, ROUND(D.PrecioVenta * CO.PorcentajeComision / 100, 2), D.FormaPagoVenta
FROM #Nuevos N
INNER JOIN #Datos D        ON D.Dominio = N.Dominio
INNER JOIN Consignacion CO ON CO.IdVehiculo = N.IdVehiculo
INNER JOIN Cliente C       ON C.DNI = D.CompDNI
WHERE D.FechaVenta IS NOT NULL;

COMMIT TRANSACTION;

SELECT IdCliente FROM #ClientesNuevos;";

        /// <summary>Carga los datos de prueba del negocio y devuelve los Id de los clientes nuevos.</summary>
        public List<int> CargarOperacionesDePrueba()
        {
            DataTable tabla = SqlHelper.ExecuteDataTable(CargarOperacionesStatement);
            return tabla.Rows.Cast<DataRow>().Select(fila => (int)fila["IdCliente"]).ToList();
        }
    }
}
