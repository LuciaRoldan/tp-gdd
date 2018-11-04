SELECT * FROM empresa
SELECT * FROM usuario
SELECT * FROM cliente
SELECT * FROM domicilio

--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.--A PARTIR DE ACA ESTA BIEN---.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--
--.--.--.--.--.--.----.--.--.--.--.--.----.--.--.--.--.--.--

--.--.--.--.--.--.--ROLES--.--.--.--.--.--.--
--tabla roles
INSERT INTO rol(nombre)
VALUES ('Administrativo'),('Empresa'),('Cliente')

SELECT * FROM rol
SELECT * FROM usuario
SELECT * FROM usuarioXrol

--.--.--.--.--.--.--FUNCIOALIDADES--.--.--.--.--.--.--
--tabla funcionalidades
INSERT INTO funcionalidad
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
('Listado estadistico')

SELECT * FROM funcionalidad

--.--.--.--.--.--.--FUNCIONALIDADXROL--.--.--.--.--.--.--
--tabla
INSERT INTO funcionalidadXrol(id_rol, id_funcionalidad)
SELECT r.id_rol, f.id_funcionalidad FROM rol r, funcionalidad f
WHERE r.nombre = 'Cliente'
AND (f.nombre = 'Registro de usuario' OR f.nombre = 'Comprar')

--.--.--.--.--.--.--USUARIOS--.--.--.--.--.--.--

--usuarios cliente
INSERT INTO usuario (username, password, habilitado, alta_logica)
SELECT DISTINCT Cli_Dni, Cli_Dni, 1, GETDATE()
FROM gd_esquema.Maestra
WHERE Cli_Dni IS NOT NULL

--usuarios empresa
INSERT INTO usuario (username, password, habilitado, alta_logica)
SELECT DISTINCT Espec_Empresa_Cuit, Espec_Empresa_Cuit, 1, GETDATE()
FROM gd_esquema.Maestra

DELETE FROM usuario
SELECT * FROM usuario


--.--.--.--.--.--.--CLIENTES--.--.--.--.--.--.--

--Falta el CUIL, por ahora en NULL
--No se si va GETDATE o que en fecha_creacion
INSERT INTO cliente(id_usuario, nombre, apellido, tipo_documento, documento, cuil, mail, fecha_creacion, fecha_nacimiento, calle, numero_calle, piso, depto, codigo_postal)
SELECT DISTINCT u.id_usuario, Cli_Nombre, Cli_Apeliido, 'DNI' as Tipo_DNI, Cli_Dni, NULL, Cli_Mail, GETDATE() AS fecha_creacion, Cli_Fecha_Nac, Cli_Dom_Calle, Cli_Nro_Calle, Cli_Piso, Cli_Depto, Cli_Cod_Postal
FROM gd_esquema.Maestra gd
JOIN usuario u ON(u.username = CAST(gd.Cli_Dni as varchar))
WHERE Cli_Dni IS NOT NULL
SELECT * FROM cliente


--.--.--.--.--.--.--EMPRESAS--.--.--.--.--.--.--

INSERT INTO empresa(id_usuario, razon_social, mail, cuit, fecha_creacion, calle, numero_calle, piso, depto, codigo_postal)
SELECT DISTINCT u.id_usuario, Espec_Empresa_Razon_Social ,Espec_Empresa_Mail, Espec_Empresa_Cuit, Espec_Empresa_Fecha_Creacion,
				Espec_Empresa_Dom_Calle, Espec_Empresa_Nro_Calle, Espec_Empresa_Piso, Espec_Empresa_Depto,
				Espec_Empresa_Cod_Postal
FROM gd_esquema.Maestra gd
JOIN usuario u ON(u.username = gd.Espec_Empresa_Cuit)
WHERE Espec_Empresa_Cuit IS NOT NULL

SELECT * FROM empresa

--.--.--.--.--.--.--ROLXUSUARIO--.--.--.--.--.--.--

INSERT INTO usuarioXrol(id_usuario, id_rol)
SELECT id_usuario, id_rol
FROM usuarios u, rol r
WHERE 