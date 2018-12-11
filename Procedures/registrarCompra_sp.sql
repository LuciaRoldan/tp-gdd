CREATE PROCEDURE registrarCompra_sp
@id_cliente INT,
@id_medio_de_pago INT,
@importe BIGINT,
@id_ubicacion_espectaculo INT
AS
BEGIN
	INSERT INTO Compras(id_cliente, id_medio_de_pago, id_factura, fecha, importe)
	VALUES(@id_cliente, @id_medio_de_pago, NULL, GETDATE(), @importe)

	DECLARE @id_compra INT = SCOPE_IDENTITY()

	UPDATE UbicacionXEspectaculo
	SET id_compra = @id_compra
	WHERE id_ubicacion_espectaculo = @id_ubicacion_espectaculo

END

