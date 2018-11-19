CREATE PROCEDURE buscarUsuarioPorCriterio_sp
@nombre VARCHAR(255),
@apellido VARCHAR(255),
@dni NUMERIC(18,0),
@email VARCHAR(255)
AS
BEGIN
	SELECT nombre, apellido, documento FROM Clientes
	WHERE nombre LIKE @nombre + '%'
		AND apellido LIKE @apellido + '%'
		AND documento LIKE @dni + '%'
		AND mail LIKE @email + '%'
END