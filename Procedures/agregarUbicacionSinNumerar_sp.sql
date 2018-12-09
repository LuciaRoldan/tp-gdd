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

		INSERT INTO Ubicaciones(fila, asiento, sin_numerar, precio, tipo_ubicacion)
		VALUES(NULL, NULL, 1, @precio, @tipo_ubicacion)

		INSERT INTO #UbicacionesInsertadas
		SELECT SCOPE_IDENTITY()

		SET @contador +=1

	END

	SELECT * FROM #UbicacionesInsertadas
END