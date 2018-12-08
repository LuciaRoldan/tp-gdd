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

	IF EXISTS (SELECT * FROM Espectaculos WHERE fecha_evento = @fecha)
		BEGIN
		RAISERROR('No pueden existir dos funciones para una publicacion con la misma fecha', 16, 1)
		END

	INSERT INTO Espectaculos(id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo)
	VALUES(@id_publicacion, GETDATE(), @fecha, @estado_publicacion)

	DECLARE @id_espectaculo INT = SCOPE_IDENTITY()

	INSERT INTO UbicacionXEspectaculo(id_espectaculo, id_ubicacion, id_compra)
	SELECT @id_espectaculo, id_ubicacion, NULL
			FROM Ubicaciones WHERE id_ubicacion IN (SELECT id_ubicacion FROM Ubicaciones u
													JOIN UbicacionXEspectaculo uxe ON(uxe.id_ubicacion = u.id_ubicacion)
													JOIN Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
													WHERE e.id_publicacion = @id_publicacion)
END