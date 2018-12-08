CREATE PROCEDURE modificarUbicacionesSinNumerar_sp(
@id_publicacion INT,
@tipo_ubicacion NVARCHAR(20),
@precio NUMERIC(18),
@filas INT
)
AS
BEGIN
	DELETE FROM Ubicaciones
	WHERE id_ubicacion IN (SELECT uu.id_ubicacion FROM Ubicaciones uu
							JOIN UbicacionXEspectaculo uxe ON(uxe.id_ubicacion = uu.id_ubicacion)
							JOIN Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
							WHERE e.id_publicacion = @id_publicacion) AND tipo_ubicacion = @tipo_ubicacion

	DELETE FROM UbicacionXEspectaculo
	WHERE id_espectaculo IN (SELECT id_espectaculo FROM UbicacionXEspectaculo uxe
							JOIN Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
							JOIN Ubicaciones u ON(u.id_ubicacion = uxe.id_ubicacion)
							WHERE e.id_publicacion = @id_publicacion AND u.tipo_ubicacion = @tipo_ubicacion)

	EXEC agregarUbicacionSinNumerar_sp @id_publicacion, @tipo_ubicacion, @cantidad, @precio
END