CREATE PROCEDURE buscarEmpresaPorCriterio_sp
@cuit VARCHAR(20),
@razon_social VARCHAR(20),
@email VARCHAR(20)
AS
BEGIN
	SELECT razon_social, mail, coalesce(cuit,null) cuit, mail, calle, numero_calle, piso  FROM Empresas
	WHERE (razon_social LIKE '%' + @razon_social + '%'
		AND mail LIKE '%' + @email + '%'
		AND cuit = @cuit)
		OR (razon_social LIKE '%' + @razon_social + '%'
		AND mail LIKE '%' + @email + '%'
		AND @cuit LIKE '')
END


