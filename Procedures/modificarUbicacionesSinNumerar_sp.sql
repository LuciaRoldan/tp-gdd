CREATE PROCEDURE modificarUbicacionesSinNumerar_sp(
@id_publicacion INT,
@tipo_ubicacion NVARCHAR(20),
@precio NUMERIC(18),
@filas INT
)
AS
BEGIN
	DELETE FROM UbicacionXEspectaculo
	WHERE id_espectaculo IN (SELECT uxe.id_espectaculo FROM UbicacionXEspectaculo uxe
							JOIN Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
							JOIN Ubicaciones u ON(u.id_ubicacion = uxe.id_ubicacion)
							WHERE e.id_publicacion = @id_publicacion AND u.tipo_ubicacion = @tipo_ubicacion)

	DELETE FROM Ubicaciones
	WHERE id_ubicacion IN (SELECT uu.id_ubicacion FROM Ubicaciones uu
							JOIN UbicacionXEspectaculo uxe ON(uxe.id_ubicacion = uu.id_ubicacion)
							JOIN Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
							WHERE e.id_publicacion = @id_publicacion) AND tipo_ubicacion = @tipo_ubicacion

	EXEC agregarUbicacionSinNumerar_sp @id_publicacion, @tipo_ubicacion, @cantidad, @precio

	DECLARE @id_ubicacion INT
	DECLARE cursorUbicaciones CURSOR FOR SELECT id_ubicacion FROM Ubicaciones
						WHERE id_ubicacion IN (SELECT uu.id_ubicacion FROM Ubicaciones uu
							JOIN UbicacionXEspectaculo uxe ON(uxe.id_ubicacion = uu.id_ubicacion)
							JOIN Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
							WHERE e.id_publicacion = @id_publicacion) AND tipo_ubicacion = @tipo_ubicacion
	
	FETCH NEXT FROM cursorUbicaciones INTO @id_ubicacion
	WHILE @@FETCH_STATUS = 0
	BEGIN

		DECLARE @id_espectaculo INT
		DECLARE cursorEspectaculos CURSOR FOR SELECT id_espectaculo FROM Espectaculos WHERE id_publicacion = @id_publicacion

		FETCH NEXT FROM cursorFechas INTO @id_espectaculo
		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO UbicacionXEspectaculo(id_espectaculo, id_ubicacion, id_compra)
			VALUES (@id_espectaculo, @id_ubicacion, NULL)

			FETCH NEXT FROM cursorEspectaculos INTO @id_espectaculo
		END
	FETCH NEXT FROM cursorUbicaciones INTO @id_ubicacion
	END

	CLOSE cursorUbicaciones
	CLOSE cursorEspectaculos
	DEALLOCATE cursorUbicaciones
	DEALLOCATE cursorEspectaculos

END