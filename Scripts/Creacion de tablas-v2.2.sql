USE GD2C2018

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
id_compra INT PRIMARY KEY
)

CREATE TABLE Publicaciones(
id_publicacion INT PRIMARY KEY
)

CREATE TABLE Rubros(
id_rubro INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE Grados_publicacion(
id_grado_publicacion INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE Ubicaciones(
id_ubicacion INT IDENTITY(1,1) PRIMARY KEY
)

CREATE TABLE Premios(
id_premio INT IDENTITY(1,1) PRIMARY KEY
)

-----ALTER TABLES-----

ALTER TABLE Usuarios ADD
username VARCHAR(25),
password VARCHAR(25),
habilitado BIT,
alta_logica DATETIME;

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
nro_tarjeta NUMERIC(30),
titular NVARCHAR(50),
fecha_vencimiento DATE;

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
id_publicacion INT REFERENCES Publicaciones,
id_medio_de_pago INT REFERENCES Medios_de_pago,
id_factura INT REFERENCES Facturas,
fecha DATETIME,
importe INT;

ALTER TABLE Publicaciones ADD
id_empresa INT REFERENCES Empresas,
id_grado_publicacion INT REFERENCES Grados_publicacion,
id_rubro INT REFERENCES Rubros,
descripcion NVARCHAR(255),
estado_publicacion CHAR(15) CHECK(estado_publicacion IN ('Borrador', 'Publicada', 'Finalizada')),
fecha_inicio DATETIME,
fecha_evento DATETIME,
cantidad_asientos INT,
direccion VARCHAR(80);

ALTER TABLE Grados_publicacion ADD
comision NUMERIC(3,3),
nombre NVARCHAR(20);

ALTER TABLE Rubros ADD
descripcion NVARCHAR(100);

ALTER TABLE Ubicaciones ADD
id_publicacion INT REFERENCES Publicaciones,
codigo_tipo_ubicacion INT,
tipo_ubicacion NVARCHAR(20),
id_compra INT REFERENCES Compras,
fila VARCHAR(3),
asiento NUMERIC(18),
sin_numerar BIT,
precio NUMERIC(18);

ALTER TABLE Premios ADD
descripcion VARCHAR(110),
puntos BIGINT;
