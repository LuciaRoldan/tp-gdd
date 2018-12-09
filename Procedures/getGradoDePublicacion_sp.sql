CREATE PROCEDURE getGradoDePublicacion_sp
@id_publicacion int
AS
BEGIN
	SELECT nombre FROM Grados_publicacion gp JOIN Publicaciones p ON (gp.id_grado_publicacion = p.id_grado_publicacion)
				  WHERE id_publicacion = @id_publicacion
END
