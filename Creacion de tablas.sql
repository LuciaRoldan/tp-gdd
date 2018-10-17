USE GD2C2018

---------------CREACION DE TABLAS---------------

CREATE TABLE usuario(
id_usuario INT IDENTITY PRIMARY KEY
)

CREATE TABLE cliente(
id_cliente INT IDENTITY PRIMARY KEY
)

CREATE TABLE empresa(
id_empresa INT IDENTITY PRIMARY KEY
)

CREATE TABLE domicilio(
id_domicilio INT IDENTITY PRIMARY KEY
)

CREATE TABLE tarjeta(
id_tarjeta INT IDENTITY PRIMARY KEY
)

CREATE TABLE rol(
id_rol INT IDENTITY PRIMARY KEY
)

CREATE TABLE funcionalidad(
id_funcionalidad INT IDENTITY PRIMARY KEY
)

CREATE TABLE premio(
id_premio INT IDENTITY PRIMARY KEY
)

CREATE TABLE publicacion(
id_publicacion INT IDENTITY PRIMARY KEY
)

CREATE TABLE rubro(
id_rubro INT IDENTITY PRIMARY KEY
)

CREATE TABLE gradoPublicacion(
id_grado_publicacion INT IDENTITY PRIMARY KEY
)

CREATE TABLE compra(
id_compra INT IDENTITY PRIMARY KEY
)

CREATE TABLE ubicacion(
id_ubicacion INT IDENTITY PRIMARY KEY
)

CREATE TABLE tipoUbicacion(
tipo_codigo INT PRIMARY KEY --es una PK que nos traemos de la bd... esta bueno eso?
)

-----TABLAS QUE HAY QUE CREAR DESPUES-----

CREATE TABLE puntos(
id_usuario INT REFERENCES usuario,
cantidad_puntos BIGINT,
fecha_vencimiento DATE
)

CREATE TABLE rolXfuncionalidad(
id_rol INT REFERENCES rol,
id_funcionalidad INT REFERENCES funcionalidad
)

CREATE TABLE factura(
id_factura INT IDENTITY PRIMARY KEY
)


SELECT * FROM usuario
SELECT * FROM cliente
SELECT * FROM empresa

DROP TABLE usuario
DROP TABLE cliente
DROP TABLE empresa


-----ALTER TABLES-----

ALTER TABLE usuario ADD
username VARCHAR(25),
password VARCHAR(25),
rol_asignado INT REFERENCES rol,
habilitado BIT,
alta_logica DATETIME

ALTER TABLE cliente ADD
id_usuario INT NOT NULL REFERENCES usuario,
nombre NVARCHAR(255),
apellido NVARCHAR(255),
domicilio INT REFERENCES domicilio,
tipo_documento CHAR(3), --?
documento NUMERIC(18,0),
cuil NUMERIC(18,0), --??
mail NVARCHAR(50),
telefono NUMERIC(15),
fecha_creacion DATETIME,
fecha_nacimiento DATE

ALTER TABLE empresa ADD
id_usuario INT REFERENCES usuario,
razon_social NVARCHAR(255),
domicilio INT REFERENCES domicilio,
mail NVARCHAR(50),
cuit NVARCHAR(255),
fecha_creacion DATETIME

ALTER TABLE domicilio ADD
calle NVARCHAR(50),
numero_calle NUMERIC(18,0),
piso NUMERIC(18,0),
depto NVARCHAR(50,0),
codigo_postal NVARCHAR(50)

ALTER TABLE tarjeta ADD
id_cliente INT REFERENCES cliente,
nro_tarjeta NUMERIC(30),
titular NVARCHAR(50),
fecha_vencimiento DATE

ALTER TABLE premio ADD
descripcion VARCHAR(110),
puntos BIGINT

ALTER TABLE publicacion ADD
id_duenio INT REFERENCES empresa,
id_grado_publicacion INT REFERENCES gradoPublicacion,
id_rubro INT REFERENCES rubro,
descripcion NVARCHAR(255),
--estado_publicacion CHAR(1) CHECK(estado_publicacion IN ('B', 'A', 'F')), dice que tiene que ser varchar de 255 :(
estado_publicacion NVARCHAR(255),
fecha_inicio DATETIME,
fecha_evento DATETIME,
cantidad_asientos INT,
direccion VARCHAR(80)

ALTER TABLE rubro ADD
descripcion NVARCHAR(255)

ALTER TABLE gradoPublicacion ADD
comision INT

ALTER TABLE compra ADD
id_cliente INT REFERENCES cliente,
id_publicacion INT REFERENCES publicacion,
id_tarjeta INT REFERENCES tarjeta,
id_factura INT REFERENCES factura,
fecha DATETIME,
importe INT

ALTER TABLE compra ADD
id_publicacion INT REFERENCES publicacion,
tipo_codigo INT REFERENCES tipoCodigo,
id_compra INT REFERENCES compra,
fila VARCHAR(3),
asiento NUMERIC(18),
sin_numerar BIT, --esta asi en la BD... pero no puede ser NULL en fila? :(
precio NUMERIC(18)

ALTER TABLE tipoUbicacion ADD
tipo_descripcion NVARCHAR(255)

ALTER TABLE factura ADD
id_empresa INT REFERENCES empresa,
fecha_facturacion DATETIME,
importe_total NUMERIC(18,2) --lo de la factura esta turbio

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