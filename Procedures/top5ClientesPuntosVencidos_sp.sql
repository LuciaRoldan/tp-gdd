CREATE PROCEDURE top5ClientesPuntosVencidos_sp(
@fecha_inicio DATETIME,
@fecha_fin DATETIME
)
AS
BEGIN
	
	SELECT TOP 5 nombre, apellido, SUM(cantidad_puntos) 'Puntos Vencidos' FROM Clientes c
	JOIN Puntos p ON(p.id_cliente = c.id_cliente)
	WHERE fecha_vencimiento < GETDATE() AND (fecha_vencimiento BETWEEN CONVERT(DATETIME, @fecha_inicio, 127) AND CONVERT(DATETIME, @fecha_fin, 127))
	GROUP BY nombre, apellido
	ORDER BY SUM(cantidad_puntos) DESC
END


