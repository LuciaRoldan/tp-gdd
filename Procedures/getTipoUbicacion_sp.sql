CREATE PROCEDURE getTipoUbicacion_sp
AS
BEGIN
	SELECT DISTINCT descripcion FROM TiposDeUbicacion WHERE descripcion <> ''
END
