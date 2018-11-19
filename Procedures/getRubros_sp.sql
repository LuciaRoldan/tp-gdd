CREATE PROCEDURE getRubros_sp
AS
BEGIN
	SELECT descripcion FROM Rubros WHERE descripcion <> ''