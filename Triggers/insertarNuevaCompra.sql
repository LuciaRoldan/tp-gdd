CREATE TRIGGER insertarNuevaCompra ON Compras
INSTEAD OF INSERT
AS
BEGIN
	DECLARE @id_cliente INT, @id_medio_de_pago INT, @fecha DATETIME, @importe BIGINT
	DECLARE cur CURSOR FOR 
	SELECT id_cliente, id_medio_de_pago, fecha, importe FROM inserted
	DECLARE @last_id INT
	SET @last_id = (SELECT MAX(id_compra) FROM Compras) + 1
	OPEN cur
		FETCH NEXT FROM cur INTO @id_cliente, @id_medio_de_pago, @fecha, @importe
		WHILE @@FETCH_STATUS = 0
		BEGIN
		INSERT INTO Compras(id_cliente, id_medio_de_pago, id_factura, fecha, importe, id_compra)
		VALUES (@id_cliente, @id_medio_de_pago, NULL, @fecha, @importe, @last_id)
		SET @last_id += 1
		FETCH NEXT FROM cur INTO @id_cliente, @id_medio_de_pago, @fecha, @importe
		END
	CLOSE cur
	DEALLOCATE cur
	select @last_id-1 id
END
