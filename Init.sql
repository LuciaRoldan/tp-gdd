DROP TABLE UsuarioXRol
DROP TABLE FuncionalidadXRol
DROP TABLE Roles
DROP TABLE Funcionalidades
DROP TABLE Premios
DROP TABLE Puntos
DROP TABLE UbicacionXEspectaculo
DROP TABLE Espectaculos
DROP TABLE Ubicaciones
DROP TABLE TiposDeUbicacion
DROP TABLE Compras
DROP TABLE Facturas
DROP TABLE Publicaciones
DROP TABLE Rubros
DROP TABLE Grados_publicacion
DROP TABLE Empresas
DROP TABLE Medios_de_pago
DROP TABLE Clientes
DROP TABLE Usuarios

---------------CREACION DE TABLAS---------------

CREATE TABLE Usuarios(
id_usuario INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE UsuarioXRol(
id_usuarioXrol INT IDENTITY(1,1) PRIMARY KEY,
)

CREATE TABLE Roles(
id_rol INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE FuncionalidadXRol(
id_funcionalidadXrol INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE Funcionalidades(
id_funcionalidad INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE Clientes(
id_cliente INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE Medios_de_pago(
id_medio_de_pago INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE Puntos(
id_punto INT IDENTITY(1,1) PRIMARY KEY,
)

CREATE TABLE Empresas(
id_empresa INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE Facturas(
id_factura INT PRIMARY KEY
)

CREATE TABLE Compras(
id_compra INT IDENTITY PRIMARY KEY
)

CREATE TABLE Publicaciones(
id_publicacion INT IDENTITY PRIMARY KEY
)

CREATE TABLE Espectaculos(
id_espectaculo INT PRIMARY KEY
)

CREATE TABLE UbicacionXEspectaculo(
id_ubicacion_espectaculo INT IDENTITY PRIMARY KEY
)

CREATE TABLE Rubros(
id_rubro INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE Grados_publicacion(
id_grado_publicacion INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE TiposDeUbicacion(
id_tipo_ubicacion INT IDENTITY PRIMARY KEY
)

CREATE TABLE Ubicaciones(
id_ubicacion INT IDENTITY PRIMARY KEY
)

CREATE TABLE Premios(
id_premio INT IDENTITY(1,1) PRIMARY KEY
)

-----ALTER TABLES-----

ALTER TABLE Usuarios ADD
username VARCHAR(255),
password VARCHAR(255),
habilitado BIT,
alta_logica DATETIME,
intentos_fallidos INT,
debe_cambiar_pass BIT;

ALTER TABLE UsuarioXRol ADD
id_usuario INT REFERENCES Usuarios,
id_rol INT REFERENCES Roles;

ALTER TABLE Roles ADD
nombre CHAR(40),
habilitado BIT;

ALTER TABLE FuncionalidadXRol ADD
id_funcionalidad INT REFERENCES Funcionalidades,
id_rol INT REFERENCES Roles;

ALTER TABLE Funcionalidades ADD
nombre VARCHAR(100);

ALTER TABLE Clientes ADD
id_usuario INT NOT NULL REFERENCES Usuarios,
nombre NVARCHAR(255),
apellido NVARCHAR(255),
tipo_documento CHAR(3), --?
documento NUMERIC(18,0),
cuil NUMERIC(18,0), --??
mail NVARCHAR(50),
telefono NUMERIC(15),
fecha_creacion DATETIME,
fecha_nacimiento DATETIME,
calle NVARCHAR(255),
numero_calle NUMERIC(18,0),
piso NUMERIC(18,0),
depto NVARCHAR(255),
codigo_postal NVARCHAR(50);

ALTER TABLE Medios_de_pago ADD
id_cliente INT REFERENCES Clientes,
descripcion VARCHAR(10) CHECK(descripcion IN ('Efectivo', 'Tarjeta')),
nro_tarjeta NUMERIC(30),
titular NVARCHAR(50)

ALTER TABLE Puntos ADD
id_cliente INT REFERENCES Clientes,
cantidad_puntos BIGINT,
fecha_vencimiento DATE;

ALTER TABLE Empresas ADD
id_usuario INT REFERENCES Usuarios,
razon_social NVARCHAR(255),
mail NVARCHAR(50),
cuit NVARCHAR(255),
fecha_creacion DATETIME,
calle NVARCHAR(50),
numero_calle NUMERIC(18,0),
piso NUMERIC(18,0),
depto NVARCHAR(50),
codigo_postal NVARCHAR(50);

ALTER TABLE Facturas ADD
id_empresa INT REFERENCES Empresas,
fecha_facturacion DATETIME,
importe_total NUMERIC(18,2);

ALTER TABLE Compras ADD
id_cliente INT REFERENCES Clientes,
id_medio_de_pago INT REFERENCES Medios_de_pago,
id_factura INT REFERENCES Facturas,
comision NUMERIC(3,3),
fecha DATETIME,
importe INT;

--ALTER TABLE Publicaciones ADD
--id_empresa INT REFERENCES Empresas,
--id_grado_publicacion INT REFERENCES Grados_publicacion,
--id_rubro INT REFERENCES Rubros,
--descripcion NVARCHAR(255),
--estado_publicacion CHAR(15) CHECK(estado_publicacion IN ('Borrador', 'Publicada', 'Finalizada')),
--fecha_inicio DATETIME,
--fecha_evento DATETIME,
--cantidad_asientos INT,
--direccion VARCHAR(80);

ALTER TABLE Publicaciones ADD
id_empresa INT REFERENCES Empresas,
id_grado_publicacion INT REFERENCES Grados_publicacion,
id_rubro INT REFERENCES Rubros,
descripcion NVARCHAR(255),
direccion VARCHAR(80);

ALTER TABLE Espectaculos ADD
id_publicacion INT REFERENCES Publicaciones,
fecha_inicio DATETIME,
fecha_evento DATETIME,
estado_espectaculo CHAR(15) CHECK(estado_espectaculo IN ('Borrador', 'Publicada', 'Finalizada'))

ALTER TABLE UbicacionXEspectaculo ADD
id_espectaculo INT REFERENCES Espectaculos,
id_ubicacion INT REFERENCES Ubicaciones,
id_compra INT REFERENCES Compras

ALTER TABLE Grados_publicacion ADD
comision NUMERIC(3,3),
nombre NVARCHAR(20);

ALTER TABLE Rubros ADD
descripcion NVARCHAR(100);

--ALTER TABLE Ubicaciones ADD
--id_publicacion INT REFERENCES Publicaciones,
--codigo_tipo_ubicacion INT,
--tipo_ubicacion NVARCHAR(20),
--id_compra INT REFERENCES Compras,
--fila VARCHAR(3),
--asiento NUMERIC(18),
--sin_numerar BIT,
--precio NUMERIC(18);

ALTER TABLE TiposDeUbicacion ADD
descripcion NVARCHAR(255)

ALTER TABLE Ubicaciones ADD
codigo_tipo_ubicacion INT,
tipo_ubicacion NVARCHAR(20),
fila VARCHAR(3),
asiento NUMERIC(18),
sin_numerar BIT,
precio NUMERIC(18);

ALTER TABLE Premios ADD
descripcion VARCHAR(110),
puntos BIGINT;

--.--.--.--.--.--.--ROLES--.--.--.--.--.--.--
--tabla roles
INSERT INTO Roles(nombre, habilitado)
VALUES ('Administrativo', 1),('Empresa', 1),('Cliente', 1),('adminOP', 1);

--SELECT * FROM Roles

--.--.--.--.--.--.--FUNCIONALIDADES--.--.--.--.--.--.--
--tabla funcionalidades
INSERT INTO Funcionalidades
VALUES
('Login y seguridad'),
('ABM de rol'),
('Registro de usuario'),
('ABM de cliente'),
('ABM de empresa de espectaculos'),
('ABM de rubro'),
('ABM grado de publicacion'),
('Generar publicacion'),
('Editar publicacion'),
('Comprar'),
('Historial del cliente'),
('Canje y administracion de puntos'),
('Generar pago de comisiones'),
('Listado estadistico');

--SELECT * FROM Funcionalidades

--.--.--.--.--.--.--USUARIOS--.--.--.--.--.--.--

--usuarios del sistema
INSERT INTO Usuarios(username, password, habilitado, alta_logica, intentos_fallidos, debe_cambiar_pass)
VALUES('admin', 'w23e', 1, GETDATE(), 0, 0)

INSERT INTO Usuarios(username, password, habilitado, alta_logica, intentos_fallidos, debe_cambiar_pass)
VALUES('sa', 'gestiondedatos', 1, GETDATE(), 0, 0)

--usuarios cliente
INSERT INTO Usuarios (username, password, habilitado, alta_logica, intentos_fallidos, debe_cambiar_pass)
SELECT DISTINCT Cli_Dni, Cli_Dni, 1, GETDATE(), 0, 0
FROM gd_esquema.Maestra
WHERE Cli_Dni IS NOT NULL;

--usuarios empresa
INSERT INTO Usuarios (username, password, habilitado, alta_logica, intentos_fallidos, debe_cambiar_pass)
SELECT DISTINCT Espec_Empresa_Cuit, Espec_Empresa_Cuit, 1, GETDATE(), 0, 0
FROM gd_esquema.Maestra;

--SELECT * FROM Usuarios

--.--.--.--.--.--.--CLIENTES--.--.--.--.--.--.--

--Falta el CUIL, por ahora en NULL
--No se si va GETDATE o que en fecha_creacion
INSERT INTO Clientes(id_usuario, nombre, apellido, tipo_documento, documento, cuil, mail, fecha_creacion, fecha_nacimiento, calle, numero_calle, piso, depto, codigo_postal)
SELECT DISTINCT u.id_usuario, Cli_Nombre, Cli_Apeliido, 'DNI' as Tipo_DNI, Cli_Dni, NULL, Cli_Mail, GETDATE() AS fecha_creacion, Cli_Fecha_Nac, Cli_Dom_Calle, Cli_Nro_Calle, Cli_Piso, Cli_Depto, Cli_Cod_Postal
FROM gd_esquema.Maestra gd
JOIN Usuarios u ON(u.username = CAST(gd.Cli_Dni as varchar))
WHERE Cli_Dni IS NOT NULL;

--SELECT * FROM Clientes

--.--.--.--.--.--.--EMPRESAS--.--.--.--.--.--.--

INSERT INTO Empresas(id_usuario, razon_social, mail, cuit, fecha_creacion, calle, numero_calle, piso, depto, codigo_postal)
SELECT DISTINCT u.id_usuario, Espec_Empresa_Razon_Social ,Espec_Empresa_Mail, Espec_Empresa_Cuit, Espec_Empresa_Fecha_Creacion,
				Espec_Empresa_Dom_Calle, Espec_Empresa_Nro_Calle, Espec_Empresa_Piso, Espec_Empresa_Depto,
				Espec_Empresa_Cod_Postal
FROM gd_esquema.Maestra gd
JOIN Usuarios u ON(u.username = gd.Espec_Empresa_Cuit)
WHERE Espec_Empresa_Cuit IS NOT NULL;

--SELECT * FROM Empresas

--.--.--.--.--.--.--ROLXUSUARIO--.--.--.--.--.--.--

--truchito
INSERT INTO UsuarioXRol(id_usuario, id_rol)
SELECT c.id_usuario, r.id_rol
FROM Clientes c, Roles r
WHERE r.nombre = 'Cliente';

INSERT INTO UsuarioXRol(id_usuario, id_rol)
SELECT e.id_usuario, r.id_rol
FROM Empresas e, Roles r
WHERE r.nombre = 'Empresa';

--SELECT * FROM UsuarioXRol

INSERT INTO dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM dbo.Usuarios WHERE username like 'admin'), 4)
INSERT INTO dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM dbo.Usuarios WHERE username like 'sa'), 1)


INSERT INTO Usuarios(username, password, habilitado, alta_logica, intentos_fallidos, debe_cambiar_pass)
VALUES('ro', 'lol', 1, GETDATE(), 0, 0)
INSERT INTO dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM dbo.Usuarios WHERE username like 'ro'), 3)
DECLARE @id INT = SCOPE_IDENTITY()
INSERT INTO Clientes(id_usuario, nombre, apellido, tipo_documento, documento, cuil, mail, fecha_creacion, fecha_nacimiento, calle, numero_calle, piso, depto, codigo_postal)
VALUES(@id, 'Ro', 'Chi', 'DNI', 40747111, 40747134, 'ro@chi.com', GETDATE(), GETDATE(), 'Gurru', 2215, 4, 'B', 1422)

--delete from UsuarioXRol where id_usuario = 785
--DELETE FROM Usuarios where id_usuario = 785
--delete from clientes where id_usuario = 785

--SELECT *  FROM clientes where nombre = 'ro'

INSERT INTO Usuarios(username, password, habilitado, alta_logica, intentos_fallidos, debe_cambiar_pass)
VALUES('roco', 'lol', 1, GETDATE(), 0, 0)
INSERT INTO dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM dbo.Usuarios WHERE username like 'roco'), 2)
DECLARE @id2 INT = SCOPE_IDENTITY()
INSERT INTO Empresas(id_usuario, razon_social, mail, cuit, fecha_creacion, calle, numero_calle, piso, depto, codigo_postal)
VALUES(@id2, 'Ro Co.', 'ro@co.com', 58555661, GETDATE(), 'Gur', 52, 1, 'B', 523)

--select * from usuarios where username = 'roco'
--delete from UsuarioXRol where id_usuario = 791
--delete from empresas where id_usuario = 791
--delete from usuarios where id_usuario = 791
--.--.--.--.--.--.--FUNCIONALIDADXROL--.--.--.--.--.--.--

INSERT INTO dbo.FuncionalidadXRol(id_rol, id_funcionalidad)
SELECT 1, id_funcionalidad
FROM Funcionalidades
WHERE nombre IN('ABM de cliente', 'ABM de empresa de espectaculos', 'Generar pago de comisiones', 'Listado estadistico', 'Registro de usuario')

INSERT INTO dbo.FuncionalidadXRol(id_rol, id_funcionalidad)
SELECT 2, id_funcionalidad
FROM Funcionalidades
WHERE nombre IN('ABM de categoria', 'ABM grado de publicacion', 'Editar publicacion', 'Generar publicacion')

INSERT INTO dbo.FuncionalidadXRol(id_rol, id_funcionalidad)
SELECT 3, id_funcionalidad
FROM Funcionalidades
WHERE nombre IN('Canje y administracion de puntos', 'Comprar', 'Historial del cliente')

INSERT INTO dbo.FuncionalidadXRol(id_rol, id_funcionalidad)
SELECT 4, id_funcionalidad
FROM Funcionalidades

--SELECT * FROM FuncionalidadXRol

--.--.--.--.--.--.--RUBROS--.--.--.--.--.--.--
INSERT INTO Rubros
SELECT DISTINCT Espectaculo_Rubro_Descripcion
FROM gd_esquema.Maestra;

INSERT INTO Rubros
VALUES
('Concierto'),
('Obra infantil'),
('Musical'),
('Stand-up'),
('Familiar')


--SELECT * FROM Rubros

--.--.--.--.--.--.--GRADOS--.--.--.--.--.--.--
INSERT INTO Grados_publicacion(comision, nombre)
VALUES
(0.5, 'Alto'),
(0.35, 'Medio'),
(0.2, 'Bajo');

--select * from Grados_publicacion

--.--.--.--.--.--.--PUBLICACION--.--.--.--.--.--.--
/*INSERT INTO Publicaciones(id_publicacion, id_empresa, id_grado_publicacion, id_rubro, descripcion, estado_publicacion,
			fecha_inicio, fecha_evento, cantidad_asientos, direccion)
SELECT DISTINCT Espectaculo_Cod, e.id_empresa, NULL, ru.id_rubro, Espectaculo_Descripcion, Espectaculo_Estado, Espectaculo_Fecha,
		Espectaculo_Fecha_Venc, NULL, NULL
FROM gd_esquema.Maestra gd
JOIN Empresas e ON (e.razon_social = gd.Espec_Empresa_Razon_Social)
JOIN rubros ru ON(gd.Espectaculo_Rubro_Descripcion = ru.descripcion);*/

/*INSERT INTO Publicaciones(id_empresa, id_grado_publicacion, id_rubro, descripcion,
			fecha_inicio, fecha_evento, cantidad_asientos, direccion)
SELECT DISTINCT Espectaculo_Cod, e.id_empresa, NULL, ru.id_rubro, Espectaculo_Descripcion, Espectaculo_Estado, Espectaculo_Fecha,
		Espectaculo_Fecha_Venc, NULL, NULL
FROM gd_esquema.Maestra gd
JOIN Empresas e ON (e.razon_social = gd.Espec_Empresa_Razon_Social)
JOIN rubros ru ON(gd.Espectaculo_Rubro_Descripcion = ru.descripcion);*/

INSERT INTO Publicaciones(id_empresa, id_grado_publicacion, id_rubro, descripcion,
			direccion)
SELECT DISTINCT e.id_empresa, NULL, 1, Espectaculo_Descripcion, NULL
FROM gd_esquema.Maestra gd
JOIN Empresas e ON (e.razon_social = gd.Espec_Empresa_Razon_Social)

--select * from gd_esquema.Maestra order by Espectaculo_Cod

--select * from Publicaciones

--.--.--.--.--.--.--ESPECTÁCULOS--.--.--.--.--.--.--
INSERT INTO Espectaculos(id_espectaculo, id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo)
SELECT DISTINCT gd.Espectaculo_Cod, p.id_publicacion, Espectaculo_Fecha, Espectaculo_Fecha_Venc, Espectaculo_Estado
FROM gd_esquema.Maestra gd
JOIN Publicaciones p ON (p.descripcion = gd.Espectaculo_Descripcion)

--SELECT * FROM Espectaculos

--.--.--.--.--.--.--TIPOSDEUBICACIONES--.--.--.--.--.--.--

SET IDENTITY_INSERT TiposDeUbicacion ON
INSERT INTO TiposDeUbicacion(id_tipo_ubicacion, descripcion)
SELECT DISTINCT Ubicacion_Tipo_Codigo, Ubicacion_Tipo_Descripcion
FROM gd_esquema.Maestra
SET IDENTITY_INSERT TiposDeUbicaciones OFF

--.--.--.--.--.--.--UBICACIONES--.--.--.--.--.--.--

INSERT INTO Ubicaciones(codigo_tipo_ubicacion, fila, asiento, sin_numerar, precio)
SELECT DISTINCT gd.Ubicacion_Tipo_Codigo, gd.Ubicacion_Fila, gd.Ubicacion_Asiento, Ubicacion_Sin_numerar, Ubicacion_Precio
FROM gd_esquema.Maestra gd

--select * from Ubicaciones

--.--.--.--.--.--.--FACTURAS--.--.--.--.--.--.--
INSERT INTO Facturas(id_factura, id_empresa, fecha_facturacion, importe_total)
SELECT DISTINCT Factura_Nro, e.id_empresa, Factura_Fecha, Factura_Total
FROM gd_esquema.Maestra gd
JOIN Empresas e ON(e.razon_social = gd.Espec_Empresa_Razon_Social)
WHERE Factura_Nro IS NOT NULL

--select * from Facturas

--.--.--.--.--.--.--MEDIODEPAGO--.--.--.--.--.--.--
INSERT INTO Medios_de_pago(c.id_cliente, descripcion, nro_tarjeta, titular)
SELECT DISTINCT c.id_cliente, Forma_Pago_Desc, NULL, NULL
FROM gd_esquema.Maestra gd
JOIN Clientes c ON(c.documento = gd.Cli_Dni) --truchito porue en el titular dice 'Efectivo'
WHERE gd.Item_Factura_Monto IS NOT NULL

--SELECT * FROM Medios_de_pago

--.--.--.--.--.--.--COMPRA--.--.--.--.--.--.--
--NO migro la descripcion de las facturas que lit dice "Rendicion de comisiones" porque es eso.

/*CREATE TABLE #ComprasTemp(
id_compra NUMERIC IDENTITY(1,1) PRIMARY KEY,
id_cliente INT REFERENCES Clientes,
id_medio_de_pago INT REFERENCES Medios_de_pago,
id_factura INT REFERENCES Facturas,
fecha DATETIME,
)*/

/*--INSERT INTO #ComprasTemp(id_cliente, id_medio_de_pago, id_factura, fecha)
INSERT INTO Compras(id_cliente, id_medio_de_pago, id_factura, fecha)
SELECT c.id_cliente, mp.id_medio_de_pago, f.id_factura, Compra_Fecha
FROM gd_esquema.Maestra gd
JOIN Clientes c ON(gd.Cli_Dni = c.documento)
JOIN Facturas f ON(f.id_factura = gd.Factura_Nro)
JOIN Medios_de_pago mp ON (c.id_cliente = mp.id_cliente)
WHERE gd.Forma_Pago_Desc IS NOT NULL*/

--Acá ver lo del @@SCOPE_IDENTITY
--INSERT INTO Compras(id_compra, id_cliente, id_medio_de_pago, id_factura, fecha)
--SELECT id_compra, id_cliente, id_medio_de_pago, id_factura, fecha
--FROM #ComprasTemp

--select * from #ComprasTemp
--SELECT * FROM Medios_de_pago
--select * from Compras

CREATE TABLE #ComprasTemp(
id_compra NUMERIC IDENTITY(1,1) PRIMARY KEY,
id_cliente INT REFERENCES Clientes,
id_espectaculo INT REFERENCES Espectaculos,
id_medio_de_pago INT REFERENCES Medios_de_pago,
id_factura INT REFERENCES Facturas,
fecha DATETIME,
asiento INT,
fila CHAR(1),
tipo_codigo INT
)

INSERT INTO #ComprasTemp(id_cliente, id_espectaculo, id_medio_de_pago, id_factura, fecha, asiento, fila, tipo_codigo)
SELECT c.id_cliente, gd.Espectaculo_Cod, mp.id_medio_de_pago, f.id_factura, gd.Compra_Fecha, gd.Ubicacion_Asiento, gd.Ubicacion_Fila, gd.Ubicacion_Tipo_Codigo
FROM gd_esquema.Maestra gd
JOIN Clientes c ON(gd.Cli_Dni = c.documento)
JOIN Facturas f ON(f.id_factura = gd.Factura_Nro)
JOIN Medios_de_pago mp ON(c.id_cliente = mp.id_cliente)
WHERE gd.Forma_Pago_Desc IS NOT NULL

--.--.--.--.--.--.--UBICACIONXESPECTACULO--.--.--.--.--.--.--

SET IDENTITY_INSERT Compras ON
INSERT INTO Compras(id_compra, id_cliente, id_factura, id_medio_de_pago, fecha)
SELECT DISTINCT id_compra, id_cliente, id_factura, id_medio_de_pago, fecha
FROM #ComprasTemp
SET IDENTITY_INSERT Compras OFF

INSERT INTO UbicacionXEspectaculo(id_espectaculo, id_ubicacion, id_compra)
--Falta agregar el id_compra
SELECT DISTINCT gd.Espectaculo_Cod, u.id_ubicacion, ct.id_compra
FROM gd_esquema.Maestra gd
JOIN Ubicaciones u ON (gd.Ubicacion_Tipo_Codigo = u.codigo_tipo_ubicacion
	AND gd.Ubicacion_Tipo_Descripcion = u.tipo_ubicacion AND gd.Ubicacion_Fila = u.fila
	AND gd.Ubicacion_Asiento = u.asiento AND gd.Ubicacion_Sin_numerar = u.sin_numerar
	AND gd.Ubicacion_Precio = u.precio)
LEFT JOIN #ComprasTemp ct ON(ct.id_espectaculo = gd.Espectaculo_Cod
		AND ct.asiento = gd.Ubicacion_Asiento
		AND ct.fila = gd.Ubicacion_Fila
		AND ct.tipo_codigo = gd.Ubicacion_Tipo_Codigo)

DROP TABLE #ComprasTemp

--Select * from UbicacionXEspectaculo
--select * from UbicacionXEspectaculo ORDER BY id_compra

--select * from gd_esquema.Maestra
--INSERT INTO UbicacionXEspectaculo(id_compra)
--Compra de 18 ubicaciones con este Espectaculo_Cod pero me está tirando menos? => Me devuelve las 8 que no fueron compradas, tienen Cli_Dni en NULL
--ME ESTÁ RETORNANDO LAS QUE TIENEN Cli_Dni en NULL realmente
--SELECT * FROM gd_esquema.Maestra WHERE Espectaculo_Cod = 12353 OR Espectaculo_Cod = 12355

--.--.--.--.--.--.--PUNTOS--.--.--.--.--.--.--

INSERT INTO Puntos(id_cliente, cantidad_puntos, fecha_vencimiento)
SELECT DISTINCT id_cliente, 0, NULL
FROM Clientes

--.--.--.--.--.--.--PREMIOS--.--.--.--.--.--.--

INSERT INTO Premios(descripcion, puntos)
VALUES
('Plancha', 800),
('2x1 en la proxima compra', 300),
('Set de 6 platos', 500),
('SEGA', 2000),
('Fin de semana en Tandil', 8000),
('Batidora', 1000)


--.--.--.--.--.--.--ENCRIPTACION--.--.--.--.--.--.--
BEGIN TRANSACTION
update Usuarios set password = LOWER(CONVERT(char(100),HASHBYTES('SHA2_256', password),2))
COMMIT