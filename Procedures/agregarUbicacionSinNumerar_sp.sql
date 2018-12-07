CREATE PROCEDURE agregarUbicacionSinNumerar_sp(
@id_publicacion INT,
@tipo_ubicacion NVARCHAR(20),
@cantidad INT,
@precio NUMERIC(18)
)
AS
BEGIN
	DECLARE @contador INT = 0

	WHILE(@contador < @cantidad)
	BEGIN

		INSERT INTO Ubicaciones(fila, asiento, sin_numerar, precio)
		VALUES(@tipo_ubicacion, NULL, NULL, 1, @precio)
		SET @contador +=1

	END
END