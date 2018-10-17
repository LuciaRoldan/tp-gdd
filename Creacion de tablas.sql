USE GD2C2018

---------------CREACION DE TABLAS---------------

CREATE TABLE usuario
(
id_usuario INT IDENTITY PRIMARY KEY,
username VARCHAR(25), --yo
password VARCHAR(25), --yo
rol_asignado INT, --?
habilitado BIT,
alta_logica DATE
)

SELECT * FROM usuario

CREATE TABLE cliente
(
id_cliente INT IDENTITY PRIMARY KEY,
id_usuario INT,
nombre NVARCHAR(255),
apellido NVARCHAR(255),
tipo_documento CHAR(3), --?
documento NUMERIC(18,0),
cuil NUMERIC(18,0), --??
mail NVARCHAR(50),
telefono NUMERIC(15),
fecha_creacion DATETIME,
fecha_nacimiento DATE
)

CREATE TABLE empresa
(
id_empresa INT IDENTITY PRIMARY KEY,
id_usuario INT,
razon_social NVARCHAR(255),
mail NVARCHAR(50),
cuit NVARCHAR(255),
fecha_creacion DATETIME
)

CREATE TABLE administrativo
(
id_administrativo INT IDENTITY PRIMARY KEY
)




---------------MIGRACION DE TABLAS---------------

--.--.--.--EMPRESA--.--.--.--

CREATE PROCEDURE MigrarEmpresas
AS
BEGIN
	BEGIN TRY
	DECLARE
	@razon_social NVARCHAR(255),
	@mail NVARCHAR(50),
	@cuit NVARCHAR(255),
	@fecha_creacion DATETIME

	DECLARE EmpresaCursor CURSOR FOR
	SELECT Espec_Empresa_Razon_Social, Espec_Empresa_Mail, Espec_Empresa_Cuit, Espec_Empresa_Fecha_Creacion
	FROM gd_esquema.Maestra
	
	OPEN EmpresaCursor
	FETCH NEXT FROM EmpresaCursor
	INTO @razon_social, @mail, @cuit, @fecha_creacion

	BEGIN TRANSACTION
		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO empresa (razon_social, mail, cuit, fecha_creacion)
			VALUES (@razon_social, @mail, @cuit, @fecha_creacion)
		
		FETCH NEXT FROM EmpresaCursor
		INTO @razon_social, @mail, @cuit, @fecha_creacion

		END
	COMMIT TRANSACTION
	CLOSE EmpresaCursor
	DEALLOCATE EmpresaCursor
	
	END TRY
	
	BEGIN CATCH
	ROLLBACK TRANSACTION
	DECLARE @errorDescripcion VARCHAR(100)
	SELECT @errorDescripcion = 'Error en Empresa ' + @razon_social
	RAISERROR(@errorDescripcion,14,1)
	END CATCH
END

EXEC MigrarEmpresas

SELECT * FROM empresa