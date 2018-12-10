CREATE PROCEDURE buscarAsientosParaFilaEspectaculo_sp
@id_espectaculo INT,
@fila CHAR(3)
AS
BEGIN
	SELECT DISTINCT asiento FROM Ubicaciones u
	JOIN UbicacionXEspectaculo uxe ON(uxe.id_ubicacion = u.id_ubicacion)
	WHERE uxe.id_espectaculo = 12364 --@id_espectaculo
			AND u.fila = 'A' --@fila
END