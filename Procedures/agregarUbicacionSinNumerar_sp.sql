CREATE PROCEDURE agregarUbicacionSinNumerar_sp(
@id_espectaculo INT,
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

		SET @id_ubicacion = SCOPE_IDENTITY()

		INSERT INTO UbicacionXEspectaculo(id_espectaculo, id_ubicacion, id_compra)
		VALUES (@id_espectaculo, @id_ubicacion, NULL)

		SET @contador +=1

	END
END