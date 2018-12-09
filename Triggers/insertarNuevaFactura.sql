
CREATE TRIGGER insertarNuevaFactura ON Facturas
INSTEAD OF INSERT
AS
BEGIN
DECLARE @fecha_facturacion DATETIME, @importe_total NUMERIC(18,2), @id_empresa INT 
DECLARE cur CURSOR FOR 
SELECT fecha_facturacion, importe_total, id_empresa FROM inserted
DECLARE @last_id INT
SET @last_id = (SELECT MAX(id_factura) FROM Facturas) + 1
OPEN cur
FETCH NEXT FROM cur INTO @fecha_facturacion, @importe_total, @id_empresa
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO Facturas(id_factura, fecha_facturacion, importe_total, id_empresa)
VALUES (@last_id, @fecha_facturacion, @importe_total, @id_empresa)
SET @last_id += 1
FETCH NEXT FROM cur INTO @fecha_facturacion, @importe_total, @id_empresa
END
CLOSE cur
DEALLOCATE cur
END