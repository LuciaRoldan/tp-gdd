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

	WHILE(@contador_filas < @filas)
	BEGIN
		
		DECLARE @contador_asientos INT = 0

		WHILE(@contador_asientos < @asientos_por_fila)
		BEGIN

			INSERT INTO Ubicaciones(fila, asiento, sin_numerar, precio, tipo_ubicacion)
			VALUES(CHAR(ASCII('A')+@contador_filas), @contador_asientos, 0, @precio, @tipo_ubicacion) --esta truchito, mati va a hacer funcion

			INSERT INTO #UbicacionesInsertadas
			SELECT SCOPE_IDENTITY()

			SET @contador_asientos += 1
		END

		SET @contador_filas +=1
	END

	SELECT * FROM #UbicacionesInsertadas
END