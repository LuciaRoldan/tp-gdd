CREATE TRIGGER insertarNuevoEspectaculo ON Espectaculos
INSTEAD OF INSERT
AS
BEGIN
DECLARE @id_publicacion INT, @fecha_inicio DATETIME, @fecha_evento DATETIME, @estado_espectaculo CHAR(15) 
DECLARE cur CURSOR FOR 
SELECT id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo FROM inserted
DECLARE @last_id INT
SET @last_id = (SELECT MAX(id_espectaculo) FROM Espectaculos) + 1
OPEN cur
FETCH NEXT FROM cur INTO @id_publicacion, @fecha_inicio, @fecha_evento, @estado_espectaculo
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO Espectaculos(id_espectaculo, id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo)
VALUES (@last_id, @id_publicacion, @fecha_inicio, @fecha_evento, @estado_espectaculo)
SET @last_id += 1
FETCH NEXT FROM cur INTO @id_publicacion, @fecha_inicio, @fecha_evento, @estado_espectaculo
END
CLOSE cur
DEALLOCATE cur
END

/* Prueba
INSERT INTO Espectaculos(id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo)
SELECT TOP 5 id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo
FROM Espectaculos
ORDER BY id_publicacion DESC
*/

