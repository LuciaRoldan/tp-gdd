USE GD2C2018

--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.--A PARTIR DE ACA ESTA BIEN---.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--

--.--.--.--.--.--.--ROLES--.--.--.--.--.--.--
--tabla roles
INSERT INTO Roles(nombre)
VALUES ('Administrativo'),('Empresa'),('Cliente');

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
INSERT INTO Usuarios (username, password, habilitado, alta_logica)
SELECT DISTINCT Cli_Dni, Cli_Dni, 1, GETDATE()
FROM gd_esquema.Maestra
WHERE Cli_Dni IS NOT NULL;

--usuarios empresa
INSERT INTO Usuarios (username, password, habilitado, alta_logica)
SELECT DISTINCT Espec_Empresa_Cuit, Espec_Empresa_Cuit, 1, GETDATE()
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

--SELECT * FROM UsuarioXRol

--.--.--.--.--.--.--RUBROS--.--.--.--.--.--.--
INSERT INTO Rubros
SELECT DISTINCT Espectaculo_Rubro_Descripcion
FROM gd_esquema.Maestra;

--SELECT * FROM Rubros

--.--.--.--.--.--.--GRADOS--.--.--.--.--.--.--
INSERT INTO Grados_publicacion(comision, nombre)
VALUES
(0.5, 'Alto'),
(0.35, 'Medio'),
(0.2, 'Bajo');

--select * from Grados_publicacion

--.--.--.--.--.--.--PUBLICACION--.--.--.--.--.--.--
INSERT INTO Publicaciones(id_publicacion, id_empresa, id_grado_publicacion, id_rubro, descripcion, estado_publicacion,
			fecha_inicio, fecha_evento, cantidad_asientos, direccion)
SELECT DISTINCT Espectaculo_Cod, e.id_empresa, NULL, ru.id_rubro, Espectaculo_Descripcion, Espectaculo_Estado, Espectaculo_Fecha,
		Espectaculo_Fecha_Venc, NULL, NULL
FROM gd_esquema.Maestra gd
JOIN Empresas e ON (e.razon_social = gd.Espec_Empresa_Razon_Social)
JOIN rubros ru ON(gd.Espectaculo_Rubro_Descripcion = ru.descripcion);

--select * from Publicaciones

--.--.--.--.--.--.--FACTURAS--.--.--.--.--.--.--
INSERT INTO Facturas(id_factura, id_empresa, fecha_facturacion, importe_total)
SELECT DISTINCT Factura_Nro, e.id_empresa, Factura_Fecha, Factura_Total
FROM gd_esquema.Maestra gd
JOIN Empresas e ON(e.razon_social = gd.Espec_Empresa_Razon_Social)
WHERE Factura_Nro IS NOT NULL

--select * from Facturas

--.--.--.--.--.--.--MEDIODEPAGO--.--.--.--.--.--.--
INSERT INTO Medios_de_pago(c.id_cliente, nro_tarjeta, titular, fecha_vencimiento)
SELECT DISTINCT c.id_cliente, NULL, Forma_Pago_Desc, NULL
FROM gd_esquema.Maestra gd
JOIN Clientes c ON(c.documento = gd.Cli_Dni) --truchito porue en el titular dice 'Efectivo'

--.--.--.--.--.--.--COMPRA--.--.--.--.--.--.--
--NO migro la descripcion de las facturas que lit dice "Rendicion de comisiones" porque es eso.

INSERT INTO Compras(id_cliente, id_publicacion, id_medio_de_pago, id_factura, fecha)
SELECT DISTINCT c.id_cliente, p.id_publicacion, mp.id_medio_de_pago, f.id_factura, Compra_Fecha
FROM gd_esquema.Maestra gd
JOIN Clientes c ON(gd.Cli_Dni = c.documento)
JOIN Publicaciones p ON(gd.Espectaculo_Cod = p.id_publicacion)
JOIN Medios_de_pago mp ON(mp.titular = gd.Forma_Pago_Desc)
JOIN Facturas f ON(f.id_factura = gd.Factura_Nro)



--select * from Compras

--.--.--.--.--.--.--UBICACIONES--.--.--.--.--.--.--

INSERT INTO Ubicaciones(id_publicacion, id_compra, fila, asiento, tipo_ubicacion, sin_numerar, precio)
SELECT DISTINCT Espectaculo_Cod, c.id_compra, Ubicacion_Fila, Ubicacion_Asiento, Ubicacion_Sin_numerar, Ubicacion_Precio---
FROM gd_esquema.Maestra gd
JOIN Compras c ON(c.id_publicacion = gd.Espectaculo_Cod) --anda porque vendo de a UN asiento


--select * from ubicacion


--select * from gd_esquema.Maestra