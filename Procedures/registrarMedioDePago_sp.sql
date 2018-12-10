CREATE PROCEDURE registrarMedioDePago_sp
@id_cliente INT,
@numero_tarjeta INT,
@titular INT
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Medios_de_pago mp WHERE nro_tarjeta = @numero_tarjeta AND id_cliente = @id_cliente)
		BEGIN
		INSERT INTO Medios_de_pago(id_cliente, descripcion, nro_tarjeta, titular)
		VALUES(@id_cliente, 'Tarjeta', @numero_tarjeta, @titular)
		END
		ELSE
		BEGIN
		RAISERROR('Tarjeta ya registrada', 16, 1)
		END
END