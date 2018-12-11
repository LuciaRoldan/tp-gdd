CREATE PROCEDURE getGradosPublicaciones_sp
AS
BEGIN
	SELECT DISTINCT nombre FROM Grados_publicacion
END