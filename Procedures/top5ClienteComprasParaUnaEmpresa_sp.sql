CREATE PROCEDURE top5ClienteComprasParaUnaEmpresa_sp
AS
BEGIN
	SELECT TOP 5 nombre, apellido, documento, COUNT(id_ubicacion) 'Cantidad de compras' 
	FROM Clientes cl
	JOIN Compras c ON(c.id_cliente = cl.id_cliente)
	JOIN UbicacionXEspectaculo uxe ON(uxe.id_compra = c.id_compra)
	JOIN Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
	JOIN Publicaciones p ON (p.id_publicacion = e.id_publicacion)

	GROUP BY id_empresa, nombre, apellido, documento
	ORDER BY COUNT(id_ubicacion) DESC
END
