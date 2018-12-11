CREATE PROCEDURE getFechasPublicacion_sp
@id_publicacion INT
AS
BEGIN
	SELECT fecha_evento FROM Espectaculos e
	WHERE e.id_publicacion = @id_publicacion
END