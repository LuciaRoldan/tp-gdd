CREATE PROCEDURE top5EmpresasLocalidadesNoVendidas_sp
@grado VARCHAR(20),
--@mes_publi INT,
--@anio_publi INT,
@fecha_inicio DATETIME,
@fecha_fin DATETIME
AS
BEGIN
	SELECT TOP 5 razon_social 'Razon social', COUNT(id_ubicacion) 'Ubicaciones no vendidas' FROM Publicaciones p
	JOIN Espectaculos e ON (e.id_publicacion = p.id_publicacion)
	JOIN UbicacionXEspectaculo uxe ON(uxe.id_espectaculo = e.id_espectaculo)
	JOIN Grados_publicacion gp ON(gp.id_grado_publicacion = p.id_grado_publicacion)
	JOIN Empresas emp ON(emp.id_empresa = p.id_empresa)
	WHERE uxe.id_compra IS NULL
		AND gp.nombre = @grado
		AND e.fecha_evento>@fecha_inicio AND e.fecha_evento<@fecha_fin
	GROUP BY razon_social, p.id_publicacion, fecha_evento, comision
	ORDER BY fecha_evento ASC, comision DESC --truchito para q ordene de alto a bajo
END

--EXEC top5EmpresasLocalidadesNoVendidas_sp 'Alto', 1, 2015