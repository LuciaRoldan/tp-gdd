CREATE PROCEDURE getUbicacionesDisponiblesParaEspectaculo_sp
@id_espectaculo INT
AS
BEGIN
	SELECT DISTINCT descripcion FROM UbicacionXEspectaculo uxe
	JOIN Ubicaciones u ON(u.id_ubicacion = uxe.id_ubicacion)
	JOIN TiposDeUbicacion tu ON(tu.id_tipo_ubicacion = u.codigo_tipo_ubicacion)
	WHERE uxe.id_espectaculo = @id_espectaculo AND id_compra IS NULL
END