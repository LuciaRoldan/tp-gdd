CREATE PROCEDURE getMediosDePago_sp
@id_cliente INT
AS
BEGIN
	SELECT id_medio_de_pago, RIGHT(nro_tarjeta, 4) 'Ultimos 4 digitos' FROM Medios_de_pago WHERE id_cliente =  @id_cliente
END