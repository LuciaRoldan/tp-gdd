CREATE PROCEDURE agregarUbicacionNumerada_sp(
@tipo_ubicacion NVARCHAR(20),
@cantidad INT,
@filas INT,
@precio NUMERIC(18)
)
AS
BEGIN
	DECLARE @contador_filas INT = 0
	DECLARE @asientos_por_fila INT = @cantidad / @filas
	DECLARE @id_ubicacion INT

	IF NOT EXISTS (SELECT * FROM TiposDeUbicacion WHERE descripcion = @tipo_ubicacion)
		BEGIN
			INSERT INTO TiposDeUbicacion(id_tipo_ubicacion, descripcion)
			SELECT MAX(id_tipo_ubicacion)+1, descripcion FROM TiposDeUbicacion group by descripcion
		END

	WHILE(@contador_filas < @filas)
	BEGIN
		
		DECLARE @contador_asientos INT = 0

		WHILE(@contador_asientos < @asientos_por_fila)
		BEGIN

			INSERT INTO Ubicaciones(fila, asiento, sin_numerar, precio, codigo_tipo_ubicacion)
			SELECT CHAR(ASCII('A')+@contador_filas), @contador_asientos, 0, @precio, id_tipo_ubicacion 
			FROM TiposDeUbicacion WHERE descripcion = @tipo_ubicacion

			INSERT INTO #UbicacionesInsertadas
			SELECT SCOPE_IDENTITY()

			SET @contador_asientos += 1
		END

		SET @contador_filas +=1
	END

	SELECT * FROM #UbicacionesInsertadas
END



