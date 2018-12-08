CREATE FUNCTION getCantidadEntradasPublicacion(@id_publicacion INT)
RETURNS INT
AS
BEGIN
	RETURN (SELECT COUNT(DISTINCT id_ubicacion_espectaculo) FROM UbicacionXEspectaculo uxe
			JOIN Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
			WHERE e.id_publicacion = @id_publicacion)
END