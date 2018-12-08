CREATE FUNCTION getCantidadEntradasVendidad(@id_espectaculo INT)
RETURNS INT
AS
BEGIN
	RETURN (SELECT COUNT(DISTINCT id_ubicacion_espectaculo) FROM UbicacionXEspectaculo uxe
			WHERE id_espectaculo = @id_espectaculo AND id_compra IS NOT NULL)
END