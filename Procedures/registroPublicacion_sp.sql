CREATE PROCEDURE registroPublicacion_sp(
@id_empresa int, @id_grado int, @id_rubro int, @descripcion varchar(255), @estado_publicacion char(15), @fecha_inicio DATETIME,
@fecha_evento DATETIME, @cantidad_asientos int, @direccion varchar(80))
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Publicaciones u JOIN dbo.Empresas e ON (u.id_usuario = e.id_usuario) 
	INSERT INTO Publicaciones(id_empresa, id_grado_publicacion, id_rubro, descripcion, estado_publicacion, fecha_inicio, 
	fecha_evento, cantidad_asientos, direccion)
	VALUES(@id_empresa, @id_grado,


select * from Publicaciones