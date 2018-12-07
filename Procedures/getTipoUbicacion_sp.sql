CREATE PROCEDURE getTipoUbicacion_sp
AS
BEGIN
	SELECT DISTINCT tipo_ubicacion FROM Ubicaciones WHERE tipo_ubicacion <> ''
END
