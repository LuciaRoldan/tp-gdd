CREATE PROCEDURE agregarPublicacion_sp @nombre_empresa varchar(255), @grado_publicacion nvarchar(20), @rubro nvarchar(100), 
@descripcion varchar(255), @cantidad_asientos int, @direccion varchar(80), @fecha_inicio DATETIME, @fecha_evento DATETIME, 
@estado_espectaculo char(15), @id_publicacion int OUTPUT
AS
BEGIN

	DECLARE @id_empresa int = (SELECT id_empresa FROM Usuarios u JOIN Empresas e ON (u.id_usuario = e.id_usuario)
								WHERE username like @nombre_empresa)
	DECLARE @id_grado_publicacion int = (SELECT id_grado_publicacion FROM Grados_publicacion WHERE nombre like @grado_publicacion)
	DECLARE @id_rubro int = (SELECT id_rubro FROM Rubros WHERE descripcion like @rubro)
	
	INSERT INTO Publicaciones(id_empresa, id_grado_publicacion, id_rubro, descripcion, cantidad_asientos, direccion)
	VALUES(@id_empresa, @id_grado_publicacion, @id_rubro, @descripcion, @cantidad_asientos, @direccion)
	SET @id_publicacion = @@IDENTITY

	INSERT INTO Espectaculos(id_espectaculo, id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo)
	VALUES(1, @id_publicacion, @fecha_inicio, @fecha_evento, @estado_espectaculo)

END

DROP PROCEDURE agregarPublicacion_sp