CREATE PROCEDURE top5EmpresasLocalidadesNoVendidas_sp
@grado VARCHAR(20),
@mes INT,
@anio INT
AS
BEGIN
	SELECT TOP 5 razon_social 'Razon social', COUNT(id_ubicacion) 'Ubicaciones no vendidas' FROM Publicaciones p
	JOIN Ubicaciones u ON(u.id_publicacion = p.id_publicacion)
	JOIN Grados_publicacion gp ON(gp.id_grado_publicacion = p.id_grado_publicacion)
	JOIN Empresas e ON(e.id_empresa = p.id_empresa)
	WHERE u.id_compra IS NULL
		AND gp.nombre = @grado
		AND MONTH(p.fecha_evento) = @mes
		AND YEAR(p.fecha_evento) = @anio
	GROUP BY razon_social, p.id_publicacion, fecha_evento, comision
	ORDER BY fecha_evento ASC, comision DESC --truchito para q ordene de alto a bajo
END

DROP PROCEDURE top5EmpresasLocalidadesNoVendidas_sp

EXEC top5EmpresasLocalidadesNoVendidas_sp 'Alto', 1, 2015