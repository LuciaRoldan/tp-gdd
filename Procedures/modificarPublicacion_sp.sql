CREATE PROCEDURE modificarPublicacion_sp(
@id_publicacion INT,
@descripcion NVARCHAR(255),
@rubro_nombre NVARCHAR(100),
@direccion VARCHAR(80) --cambiar?
)
AS
BEGIN

	UPDATE Publicaciones
	SET descripcion = @descripcion,
		id_rubro = (SELECT id_rubro FROM Rubros WHERE descripcion = @rubro_nombre),
		direccion = @direccion
	FROM Publicaciones
	WHERE id_publicacion = @id_publicacion
	
END