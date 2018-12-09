CREATE PROCEDURE actualizarGradoPublicacion_sp
@id_publicacion INT,
@grado VARCHAR(10)
AS
BEGIN
	IF EXISTS (SELECT descripcion FROM Publicaciones WHERE id_publicacion = @id_publicacion)
	BEGIN
		UPDATE Publicaciones 
		SET id_grado_publicacion = (SELECT id_grado_publicacion FROM Grados_publicacion WHERE nombre like @grado)
		WHERE id_publicacion like @id_publicacion
	END
	ELSE
	BEGIN
		RAISERROR('Publicacion inexistente', 16, 1)
	END
END

	
