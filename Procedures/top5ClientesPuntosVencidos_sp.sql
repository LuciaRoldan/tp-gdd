CREATE PROCEDURE top5ClientesPuntosVencidos_sp(
@fecha_inicio DATETIME,
@fecha_fin DATETIME
)
AS
BEGIN
	SELECT TOP 5 nombre, apellido, SUM(cantidad_puntos) 'Puntos Vencidos' FROM Clientes c
	JOIN Puntos p ON(p.id_cliente = c.id_cliente)
	WHERE CONVERT(DATETIME, CONVERT(VARCHAR(8), fecha_vencimiento, 112)) < CONVERT(DATETIME,CONVERT(VARCHAR(8),GETDATE(),112))
	AND CONVERT(DATETIME, CONVERT(VARCHAR(8), fecha_vencimiento, 112)) > CONVERT(DATETIME, CONVERT(VARCHAR(8), @fecha_inicio, 112))
	AND CONVERT(DATETIME, CONVERT(VARCHAR(8), fecha_vencimiento, 112)) < CONVERT(DATETIME, CONVERT(VARCHAR(8), @fecha_fin, 112))
	--WHERE fecha_vencimiento < GETDATE() AND fecha_vencimiento > @fecha_inicio AND fecha_vencimiento < @fecha_fin
	GROUP BY nombre, apellido
	ORDER BY SUM(cantidad_puntos) DESC
END

drop procedure top5ClientesPuntosVencidos_sp


DECLARE @una_fecha DATETIME = (SELECT fecha FROM Compras WHERE id_compra=1)
DECLARE @otra_fecha DATETIME = (SELECT fecha FROM Compras WHERE id_compra=2)
EXEC top5ClientesPuntosVencidos_sp @una_fecha, @otra_fecha

--INSERT INTO Puntos(id_cliente, cantidad_puntos, fecha_vencimiento) VALUES(1, 250, GETDATE())