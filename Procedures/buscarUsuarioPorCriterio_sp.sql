CREATE PROCEDURE buscarUsuarioPorCriterio_sp
@nombre VARCHAR(255),
@apellido VARCHAR(255),
@dni VARCHAR(18),
@email NVARCHAR(50)
AS
BEGIN
	SELECT nombre, apellido, coalesce(cuil,0) cuil, mail, coalesce(telefono,0) telefono, tipo_documento, fecha_nacimiento, fecha_creacion, coalesce(documento,0) documento, calle, coalesce(numero_calle,0) numero_calle FROM Clientes
	WHERE (nombre LIKE '%' + @nombre + '%'
		AND apellido LIKE '%' + @apellido + '%'
		AND documento = CAST(@dni AS INT)
		AND mail LIKE '%' + @email + '%')
		OR (nombre LIKE '%' + @nombre + '%'
		AND apellido LIKE '%' + @apellido + '%'
		AND @dni LIKE ''
		AND mail LIKE '%' + @email + '%')
END



