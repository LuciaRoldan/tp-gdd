CREATE PROCEDURE agregarEspectaculoYUbicaciones_sp(
@id_publicacion INT,
@fecha DATETIME,
@estado_publicacion CHAR(20),
@tipo_ubicacion NVARCHAR(20),
@cantidad INT,
@filas INT,
@precio NUMERIC(18)
)
AS
BEGIN
	IF (@fecha < GETDATE())
		BEGIN
		RAISERROR('La fecha del evento no puede ser anterior a la fecha actual', 16, 1)
		END

	IF EXISTS (SELECT * FROM Espectaculos WHERE fecha_evento = @fecha AND id_publicacion = @id_publicacion)
		BEGIN
		RAISERROR('No pueden existir dos funciones para una publicacion con la misma fecha', 16, 1)
		END

	INSERT INTO Espectaculos(id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo)
	VALUES(@id_publicacion, GETDATE(), @fecha, @estado_publicacion)

	DECLARE @id_espectaculo INT = (SELECT MAX(id_espectaculo) FROM Espectaculos)

	IF(@filas > 0) -- Caso numerado
	BEGIN
		EXEC agregarUbicacionNumerada_sp @id_espectaculo, @tipo_ubicacion, @cantidad, @filas, @precio
	END
	ELSE
		EXEC agregarUbicacionSinNumerar_sp @id_espectaculo, @tipo_ubicacion, @cantidad, @precio
END

/*
SELECT * FROM UbicacionXEspectaculo WHERE id_ubicacion BETWEEN 212274 AND 212323
SELECT * FROM Ubicaciones WHERE id_ubicacion BETWEEN 212274 AND 212323

DECLARE @date DATETIME = '2019-01-10'
EXEC agregarEspectaculoYUbicaciones_sp 7803, @date, 'Borrador', 'UnTipo', 50, 10, 100
*/