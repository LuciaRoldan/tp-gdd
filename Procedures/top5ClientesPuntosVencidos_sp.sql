CREATE PROCEDURE top5ClientesPuntosVencidos_sp(
@fecha_inicio VARCHAR,
@fecha_fin VARCHAR
)
AS
BEGIN
	
	SELECT TOP 5 nombre, apellido, SUM(cantidad_puntos) 'Puntos Vencidos' FROM Clientes c
	JOIN Puntos p ON(p.id_cliente = c.id_cliente)
	WHERE fecha_vencimiento < GETDATE() AND (fecha_vencimiento BETWEEN CONVERT(DATETIME, @fecha_inicio, 127) AND CONVERT(DATETIME, @fecha_fin, 127))
	GROUP BY nombre, apellido
	ORDER BY SUM(cantidad_puntos) DESC
END

drop procedure top5ClientesPuntosVencidos_sp

/*
INSERT INTO Puntos(cantidad_puntos, fecha_vencimiento, id_cliente)
VALUES(100, '2015-07-05 00:00:00.000', 100);

INSERT INTO Puntos(cantidad_puntos, fecha_vencimiento, id_cliente)
VALUES(500, '2019-07-05 00:00:00.000', 100);

INSERT INTO Puntos(cantidad_puntos, fecha_vencimiento, id_cliente)
VALUES(300, '2016-07-05 00:00:00.000', 200);

DECLARE @una_fecha2 DATETIME = '2014-07-05 00:00:00.000'
DECLARE @otra_fecha2 DATETIME = '2016-07-05 00:00:00.000'
EXEC top5ClientesPuntosVencidos_sp @una_fecha2, @otra_fecha2
*/

INSERT INTO Puntos(cantidad_puntos, fecha_vencimiento, id_cliente)
VALUES(500, '2018-01-05 00:00:00.000', 100);

INSERT INTO Puntos(cantidad_puntos, fecha_vencimiento, id_cliente)
VALUES(600, '2018-02-05 00:00:00.000', 200);

INSERT INTO Puntos(cantidad_puntos, fecha_vencimiento, id_cliente)
VALUES(300, '2017-07-05 00:00:00.000', 200);

INSERT INTO Puntos(cantidad_puntos, fecha_vencimiento, id_cliente)
VALUES(500, '2018-01-05 00:00:00.000', 300);

INSERT INTO Puntos(cantidad_puntos, fecha_vencimiento, id_cliente)
VALUES(600, '2018-02-05 00:00:00.000', 400);

INSERT INTO Puntos(cantidad_puntos, fecha_vencimiento, id_cliente)
VALUES(300, '2018-07-05 00:00:00.000', 500);

INSERT INTO Puntos(cantidad_puntos, fecha_vencimiento, id_cliente)
VALUES(300, '2018-02-20 00:00:00.000', 600);

