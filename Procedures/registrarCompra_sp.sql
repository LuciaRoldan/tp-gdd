CREATE PROCEDURE registrarCompra_sp
@id_cliente INT,
@id_medio_de_pago INT,
@importe BIGINT
AS
BEGIN
	INSERT INTO Compras(id_cliente, id_medio_de_pago, id_factura, fecha, importe)
	VALUES(@id_cliente, @id_medio_de_pago, NULL, GETDATE(), @importe)
END


go
CREATE PROCEDURE registrarCompraExU_sp (@id_compra INT, @id_ubicacion INT,@id_espectaculo BIGINT)
AS
BEGIN
	declare @id_ubicacion_espectaculo int
	set @id_ubicacion_espectaculo = (select id_ubicacion_espectaculo from UbicacionXEspectaculo where id_ubicacion = @id_ubicacion and id_espectaculo = @id_espectaculo)
	
	UPDATE UbicacionXEspectaculo
	SET id_compra = @id_compra
	WHERE id_ubicacion_espectaculo = @id_ubicacion_espectaculo
END
