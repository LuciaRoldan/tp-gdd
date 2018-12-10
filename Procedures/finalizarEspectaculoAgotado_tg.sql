CREATE TRIGGER finalizarEspectaculoAgotado_tg
ON Compras
AFTER INSERT
AS
BEGIN
	DECLARE @id_espectaculo INT = (SELECT DISTINCT e.id_espectaculo FROM Espectaculos e
								JOIN UbicacionXEspectaculo uxe ON(uxe.id_espectaculo = e.id_espectaculo)
								WHERE uxe.id_compra = (SELECT id_compra FROM INSERTED))
	DECLARE @id_publicacion INT = (SELECT id_publicacion FROM Espectaculos WHERE id_espectaculo = @id_espectaculo)
	IF (dbo.getCantidadEntradasPublicacion(@id_publicacion) = dbo.getCantidadEntradasVendidas(@id_espectaculo))
	BEGIN
		UPDATE Espectaculos
		SET estado_espectaculo = 'Finalizado'
	END
END