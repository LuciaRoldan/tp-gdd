CREATE PROCEDURE borrarPuntos_sp
@cantidad int,
@username varchar(30)
AS
BEGIN

	DECLARE @puntos int
	DECLARE @fecha_vencimiento DATETIME
	DECLARE @id_cliente int = (SELECT id_cliente FROM Clientes c JOIN Usuarios u ON (u.id_usuario = c.id_usuario)
											 WHERE username like @username)
	

	DECLARE administradorPuntos CURSOR FOR 
	SELECT cantidad_puntos, fecha_vencimiento FROM Puntos p JOIN Clientes c ON (p.id_cliente = c.id_cliente)
												WHERE c.id_cliente = @id_cliente
												GROUP BY cantidad_puntos, fecha_vencimiento
												ORDER BY fecha_vencimiento

	OPEN administradorPuntos
	FETCH NEXT FROM administradorPuntos
	INTO @puntos, @fecha_vencimiento

	WHILE(@@FETCH_STATUS = 0)
	BEGIN
		WHILE(@cantidad != 0)
		BEGIN
			IF (@puntos < @cantidad)
			BEGIN
				SET @cantidad = @cantidad - @puntos
				DELETE  
				FROM Puntos
				WHERE fecha_vencimiento = @fecha_vencimiento
			END
			ELSE 
			BEGIN
				UPDATE Puntos 
				SET cantidad_puntos = cantidad_puntos - @cantidad
				WHERE fecha_vencimiento = @fecha_vencimiento

				SET @cantidad = 0
			END
					 
		END

		FETCH NEXT FROM administradorPuntos
		INTO @puntos, @fecha_vencimiento
	END

	CLOSE administradorPuntos
	DEALLOCATE administradorPuntos
END


