CREATE PROCEDURE agregarUbicacionSinNumerar_sp(
@tipo_ubicacion NVARCHAR(20),
@cantidad INT,
@precio NUMERIC(18)
)
AS
BEGIN
	DECLARE @contador INT = 0
	DECLARE @id_ubicacion INT

	WHILE(@contador < @cantidad)
	BEGIN

		IF NOT EXISTS (SELECT * FROM TiposDeUbicacion WHERE descripcion = @tipo_ubicacion)
		BEGIN
			INSERT INTO TiposDeUbicacion(id_tipo_ubicacion, descripcion) values(
			(SELECT MAX(id_tipo_ubicacion) + 1 FROM TiposDeUbicacion), @tipo_ubicacion)
		END

		INSERT INTO Ubicaciones(fila, asiento, sin_numerar, precio, codigo_tipo_ubicacion)
		SELECT NULL, NULL, 1, @precio, id_tipo_ubicacion
		FROM TiposDeUbicacion WHERE descripcion = @tipo_ubicacion



		INSERT INTO #UbicacionesInsertadas
		SELECT SCOPE_IDENTITY()

		SET @contador +=1

	END

	SELECT * FROM #UbicacionesInsertadas
END




