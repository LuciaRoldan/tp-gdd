CREATE PROCEDURE top5ClienteComprasParaUnaEmpresa_sp
(@razon_social NVARCHAR(50),
@fecha_inicio DATETIME,
@fecha_fin DATETIME
)
AS
BEGIN
	SELECT TOP 5 nombre, apellido, documento, COUNT(id_ubicacion) 'Cantidad de compras', razon_social
	FROM Clientes cl
	JOIN Compras c ON(c.id_cliente = cl.id_cliente)
	JOIN UbicacionXEspectaculo uxe ON(uxe.id_compra = c.id_compra)
	JOIN Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
	JOIN Publicaciones p ON (p.id_publicacion = e.id_publicacion)
	JOIN Empresas ee ON(p.id_empresa = ee.id_empresa)

	WHERE c.fecha>@fecha_inicio AND fecha<@fecha_fin AND ee.razon_social = @razon_social
	GROUP BY ee.id_empresa, razon_social, nombre, apellido, documento
	ORDER BY COUNT(id_ubicacion) DESC
END
