CREATE PROCEDURE agregarEspectaculo_sp(
@id_publicacion INT,
@fecha DATETIME,
@estado_publicacion CHAR(20)
)
AS
BEGIN
	IF (@fecha < GETDATE())
		BEGIN
		RAISERROR('La fecha del evento no puede ser anterior a la fecha actual', 16, 1)
		END

	IF EXISTS (SELECT * FROM Espectaculos WHERE fecha_evento = @fecha AND @id_publicacion = id_publicacion)
		BEGIN
		RAISERROR('No pueden existir dos funciones para una publicacion con la misma fecha', 16, 1)
		END

	INSERT INTO Espectaculos(id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo)
	VALUES(@id_publicacion, GETDATE(), @fecha, @estado_publicacion)

	SELECT MAX(id_espectaculo) AS id_espectaculo FROM Espectaculos
END