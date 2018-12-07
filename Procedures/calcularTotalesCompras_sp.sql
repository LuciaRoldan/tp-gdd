CREATE PROCEDURE calcularTotalesCompras_sp
AS
BEGIN
	UPDATE Compras
	SET importe = u.precio
	FROM Compras c
	JOIN UbicacionXEspectaculo uxe ON(uxe.id_compra = c.id_compra)
	JOIN Ubicaciones u ON(u.id_ubicacion = uxe.id_ubicacion)
END