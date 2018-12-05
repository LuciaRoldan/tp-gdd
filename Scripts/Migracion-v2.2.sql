USE GD2C2018

--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.--A PARTIR DE ACA ESTA BIEN---.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--

--.--.--.--.--.--.--ROLES--.--.--.--.--.--.--
--tabla roles
INSERT INTO Roles(nombre, habilitado)
VALUES ('Administrativo', 1),('Empresa', 1),('Cliente', 1);

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
('ABM de categoria'),
('ABM grado de publicacion'),
('Generar publicacion'),
('Editar publicacion'),
('Comprar'),
('Historial del cliente'),
('Canje y administracion de puntos'),
('Generar pago de comisiones'),
('Listado estadistico');

--SELECT * FROM Funcionalidades

--.--.--.--.--.--.--FUNCIONALIDADXROL--.--.--.--.--.--.--
--tabla
INSERT INTO FuncionalidadXRol(id_rol, id_funcionalidad)
SELECT r.id_rol, f.id_funcionalidad 
FROM Roles r, Funcionalidades f
WHERE r.nombre = 'Cliente'
AND (f.nombre = 'Registro de usuario' OR f.nombre = 'Comprar');

--SELECT * FROM FuncionalidadXRol

--.--.--.--.--.--.--USUARIOS--.--.--.--.--.--.--

--usuarios cliente
INSERT INTO Usuarios (username, password, habilitado, alta_logica, intentos_fallidos)
SELECT DISTINCT Cli_Dni, Cli_Dni, 1, GETDATE(), 0
FROM gd_esquema.Maestra
WHERE Cli_Dni IS NOT NULL;

--usuarios empresa
INSERT INTO Usuarios (username, password, habilitado, alta_logica, intentos_fallidos)
SELECT DISTINCT Espec_Empresa_Cuit, Espec_Empresa_Cuit, 1, GETDATE(), 0
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
			cantidad_asientos, direccion)
SELECT DISTINCT e.id_empresa, NULL, 1, Espectaculo_Descripcion, NULL, NULL
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

--.--.--.--.--.--.--UBICACIONES--.--.--.--.--.--.--

INSERT INTO Ubicaciones(codigo_tipo_ubicacion, tipo_ubicacion, fila, asiento, sin_numerar, precio)
SELECT DISTINCT gd.Ubicacion_Tipo_Codigo, gd.Ubicacion_Tipo_Descripcion, gd.Ubicacion_Fila, gd.Ubicacion_Asiento, Ubicacion_Sin_numerar, Ubicacion_Precio
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
SELECT DISTINCT c.id_cliente, NULL, Forma_Pago_Desc
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

INSERT INTO Compras(id_compra, id_cliente, id_factura, id_medio_de_pago, fecha)
SELECT DISTINCT id_compra, id_cliente, id_factura, id_medio_de_pago, fecha
FROM #ComprasTemp

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

--SELECT * FROM premios

--select * from gd_esquema.Maestra