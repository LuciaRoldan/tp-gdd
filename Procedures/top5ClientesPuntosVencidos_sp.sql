CREATE PROCEDURE top5ClientesPuntosVencidos_sp
AS
BEGIN
	SELECT TOP 5 nombre, apellido, SUM(cantidad_puntos) 'Puntos Vencidos' FROM Clientes c
	JOIN Puntos p ON(p.id_cliente = c.id_cliente)
	WHERE fecha_vencimiento < GETDATE()
	GROUP BY nombre, apellido
	ORDER BY SUM(cantidad_puntos) DESC
END