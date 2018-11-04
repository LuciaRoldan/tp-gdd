SELECT * FROM empresa
SELECT * FROM usuario
SELECT * FROM cliente
SELECT * FROM domicilio

--.--.--.--.--.--.--
--.--.--.--.--.--.--
--.--.--.--.--.--.--
--A PARTIR DE ACA ESTA BIEN--
--.--.--.--.--.--.--
--.--.--.--.--.--.--
--.--.--.--.--.--.--

--Creacion de roles
INSERT INTO rol(nombre)
VALUES ('Administrativo'),('Empresa'),('Cliente')

SELECT * FROM rol
SELECT * FROM usuario
SELECT * FROM usuarioXrol

--Creacion de usuarios

INSERT INTO usuario (username, password, habilitado, alta_logica)
SELECT DISTINCT Cli_Dni, Cli_Dni, 1, GETDATE()
FROM gd_esquema.Maestra
WHERE Cli_Dni IS NOT NULL

INSERT INTO usuario (username, password, habilitado, alta_logica)
SELECT DISTINCT Espec_Empresa_Cuit, Espec_Empresa_Cuit, 1, GETDATE()
FROM gd_esquema.Maestra

DELETE FROM usuario
SELECT * FROM usuario

--Creacion de Empresa

SELECT * FROM empresa

INSERT INTO empresa(u.id_usuario, razon_social, mail, cuit, fecha_creacion, calle, nro_calle, piso, depto, codigo_postal)
SELECT DISTINCT Espec_Empresa_Razon_Social, Espec_Empresa_Mail, Espec_Empresa_Cuit, Espec_Empresa_Fecha_Creacion
FROM gd_esquema.Maestra gd
JOIN usuario u ON(u.username = gd.Espec_Empresa_Cuit)




--.--.--.--cliente--.--.--.--
--Falta el CUIL, por ahora en NULL
--No se si va GETDATE o que en fecha_creacion
INSERT INTO cliente(id_usuario, nombre, apellido, tipo_documento, documento, cuil, mail, fecha_creacion, fecha_nacimiento, calle, numero_calle, piso, depto, codigo_postal)
SELECT DISTINCT u.id_usuario, Cli_Nombre, Cli_Apeliido, 'DNI' as Tipo_DNI, Cli_Dni, NULL, Cli_Mail, GETDATE() AS fecha_creacion, Cli_Fecha_Nac, Cli_Dom_Calle, Cli_Nro_Calle, Cli_Piso, Cli_Depto, Cli_Cod_Postal
FROM gd_esquema.Maestra gd
JOIN usuario u ON(u.username = CAST(gd.Cli_Dni as varchar))
WHERE Cli_Dni IS NOT NULL
SELECT * FROM cliente
--.--.--.--empresa--.--.--.--
INSERT INTO empresa(id_usuario, razon_social, mail, cuit, fecha_creacion, calle, numero_calle, piso, depto, codigo_postal)
SELECT DISTINCT u.id_usuario, Espec_Empresa_Razon_Social ,Espec_Empresa_Mail, Espec_Empresa_Cuit, Espec_Empresa_Fecha_Creacion,
				Espec_Empresa_Dom_Calle, Espec_Empresa_Nro_Calle, Espec_Empresa_Piso, Espec_Empresa_Depto,
				Espec_Empresa_Cod_Postal
FROM gd_esquema.Maestra gd
JOIN usuario u ON(u.username = gd.Espec_Empresa_Cuit)
WHERE Espec_Empresa_Cuit IS NOT NULL

SELECT * FROM empresa
