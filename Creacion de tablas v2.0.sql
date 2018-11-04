USE GD2C2018

---------------CREACION DE TABLAS---------------

CREATE TABLE usuario(
id_usuario INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE usuarioXrol(
id_usuarioXrol INT IDENTITY(1,1) PRIMARY KEY,
)

CREATE TABLE cliente(
id_cliente INT IDENTITY PRIMARY KEY
)

CREATE TABLE empresa(
id_empresa INT IDENTITY PRIMARY KEY
)

CREATE TABLE medioDePago(
id_medio_de_pag INT IDENTITY PRIMARY KEY
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

-----TABLAS QUE HAY QUE CREAR DESPUES-----

CREATE TABLE puntos(
id_usuario INT REFERENCES usuario,
cantidad_puntos BIGINT,
fecha_vencimiento DATE
)


CREATE TABLE funcionalidadXrol(
id_funcionalidadXrol INT IDENTITY PRIMARY KEY
)

CREATE TABLE factura(
id_factura INT IDENTITY PRIMARY KEY
)

SELECT * FROM usuario
SELECT * FROM cliente
SELECT * FROM empresa
SELECT * FROM domicilio

DROP TABLE usuario
DROP TABLE cliente
DROP TABLE empresa
DROP TABLE domicilio
DROP TABLE compra
DROP TABLE gradoPublicacion
DROP TABLE rolXfuncionalidad
DROP TABLE rol
DROP TABLE factura
DROP TABLE funcionalidad
DROP TABLE premio
DROP TABLE publicacion
DROP TABLE usuarioXrol

-----ALTER TABLES-----

ALTER TABLE usuario ADD
username VARCHAR(25),
password VARCHAR(25),
habilitado BIT,
alta_logica DATETIME

ALTER TABLE cliente ADD
id_usuario INT NOT NULL REFERENCES usuario,
nombre NVARCHAR(255),
apellido NVARCHAR(255),
tipo_documento CHAR(3), --?
documento NUMERIC(18,0),
cuil NUMERIC(18,0), --??
mail NVARCHAR(50),
telefono NUMERIC(15),
fecha_creacion DATETIME,
fecha_nacimiento DATE,
calle NVARCHAR(50),
numero_calle NUMERIC(18,0),
piso NUMERIC(18,0),
depto NVARCHAR(50),
codigo_postal NVARCHAR(50)

ALTER TABLE empresa ADD
id_usuario INT REFERENCES usuario,
razon_social NVARCHAR(255),
mail NVARCHAR(50),
cuit NVARCHAR(255),
fecha_creacion DATETIME,
calle NVARCHAR(50),
numero_calle NUMERIC(18,0),
piso NUMERIC(18,0),
depto NVARCHAR(50),
codigo_postal NVARCHAR(50)

ALTER TABLE usuarioXrol ADD
id_usuario INT REFERENCES usuario,
id_rol INT REFERENCES rol

ALTER TABLE medioDePago ADD
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
estado_publicacion CHAR(1) CHECK(estado_publicacion IN ('Borrador', 'Publicada', 'Finalizada')),
fecha_inicio DATETIME,
fecha_evento DATETIME,
cantidad_asientos INT,
direccion VARCHAR(80)

ALTER TABLE rubro ADD
descripcion NVARCHAR(100)

DROP TABLE gradoPublicacion

ALTER TABLE gradoPublicacion ADD
comision NUMERIC(3,3),
nombre NVARCHAR(20)

ALTER TABLE compra ADD
id_cliente INT REFERENCES cliente,
id_publicacion INT REFERENCES publicacion,
id_medio_de_pago INT REFERENCES medioDePago,
id_factura INT REFERENCES factura,
fecha DATETIME,
importe INT

ALTER TABLE ubicacion ADD
id_publicacion INT REFERENCES publicacion,
id_compra INT REFERENCES compra,
fila VARCHAR(3),
asiento NUMERIC(18),
tipo_ubicacion VARCHAR(100),
sin_numerar BIT, --esta asi en la BD... pero no puede ser NULL en fila? :(
--Por lo que lei BIT puede ser 0, 1 o tambien NULL
precio NUMERIC(18)

ALTER TABLE factura ADD
id_empresa INT REFERENCES empresa,
fecha_facturacion DATETIME,
importe_total NUMERIC(18,2) --lo de la factura esta turbio
--No se si la vamos a necesitar realmente

ALTER TABLE rol ADD
nombre CHAR(40)

ALTER TABLE funcionalidad ADD
nombre VARCHAR(100)

ALTER TABLE funcionalidadXrol ADD
id_funcionalidad INTEGER REFERENCES funcionalidad,
id_rol INTEGER REFERENCES rol

SELECT * FROM funcionalidadXrol