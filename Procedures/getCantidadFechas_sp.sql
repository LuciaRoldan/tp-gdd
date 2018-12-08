CREATE PROCEDURE getCantidadFechas_sp(
@id_publicacion INT
)
AS
BEGIN
	SELECT COUNT(DISTINCT id_publicacion) FROM Espectaculos WHERE (id_publicacion = @id_publicacion)
END