CREATE PROCEDURE eliminarTablas_sp
AS
BEGIN
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
END

--exec eliminarTablas_sp