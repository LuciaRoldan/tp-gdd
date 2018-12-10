CREATE PROCEDURE top5ClientesPuntosVencidos_sp(
@fecha_inicio DATETIME,
@fecha_fin DATETIME
)
AS
BEGIN
	SELECT TOP 5 nombre, apellido, SUM(cantidad_puntos) 'Puntos Vencidos' FROM Clientes c
	JOIN Puntos p ON(p.id_cliente = c.id_cliente)
	WHERE fecha_vencimiento < GETDATE() AND fecha_vencimiento>@fecha_inicio AND fecha_vencimiento<@fecha_fin
	GROUP BY nombre, apellido
	ORDER BY SUM(cantidad_puntos) DESC
END

--INSERT INTO Puntos(id_cliente, cantidad_puntos, fecha_vencimiento) VALUES(1, 250, GETDATE())