CREATE PROCEDURE buscarUsuarioPorCriterio_sp
@nombre VARCHAR(255),
@apellido VARCHAR(255),
@dni NUMERIC(18,0),
@email VARCHAR(255)
AS
BEGIN
	SELECT nombre, apellido, coalesce(cuil,0) cuil, mail, coalesce(telefono,0) telefono, tipo_documento, fecha_nacimiento, fecha_creacion, coalesce(documento,0) documento, calle, coalesce(numero_calle,0) numero_calle FROM Clientes
	WHERE nombre LIKE '%' + @nombre + '%'
		AND apellido LIKE '%' + @apellido + '%'
		AND documento = @dni
		AND mail LIKE '%' + @email + '%'
END

drop procedure buscarUsuarioPorCriterio_sp
EXEC dbo.buscarUsuarioPorCriterio_sp '', '', '45023700', ''

exec buscarUsuarioPorCriterio_sp 'INDA', 'Navarro', '45023700', 'inda_Navarro@gmail.com'

