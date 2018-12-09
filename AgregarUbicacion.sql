CREATE PROCEDURE agregarUbicaciones_sp(
@tipo_ubicacion NVARCHAR(20),
@cantidad INT,
@filas INT,
@precio NUMERIC(18)
)
AS
BEGIN
	
	CREATE TABLE #UbicacionesInsertadas(
	id_ubicacion INT)

	IF(@filas > 0) -- Caso numerado
	BEGIN
		EXEC agregarUbicacionNumerada_sp @tipo_ubicacion, @cantidad, @filas, @precio
	END
	ELSE
	BEGIN
		EXEC agregarUbicacionSinNumerar_sp @tipo_ubicacion, @cantidad, @precio
	END

	DROP TABLE #UbicacionesInsertadas
END