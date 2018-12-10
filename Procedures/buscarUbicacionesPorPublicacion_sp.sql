--drop procedure buscarUbicacionessPorPublicacion_sp

create procedure buscarUbicacionessPorPublicacion_sp (@id_publicacion int) as begin
	SELECT tipo_ubicacion as tipo, sin_numerar, precio, count(distinct fila) as filas, count(*)/count(DISTINCT e.id_espectaculo) as asientos 
	FROM (SELECT * FROM Espectaculos
	where id_publicacion = @id_publicacion) AS e
	JOIN UbicacionXEspectaculo uxe ON (e.id_espectaculo = uxe.id_espectaculo)
	JOIN Ubicaciones u ON (uxe.id_ubicacion = u.id_ubicacion)
	GROUP BY tipo_ubicacion, sin_numerar, precio
end

SELECT * FROM Publicaciones ORDER BY 1 DESC
SELECT * FROM Usuarios
create procedure buscarUbicacionesPorPublicacion_sp (@id_publicacion int) as 
begin
	SELECT tipo_ubicacion as tipo, sin_numerar, fila
	--, precio, COUNT(DISTINCT u.fila), COUNT(*) AS asientos
	FROM Espectaculos ee 
	join dbo.UbicacionXEspectaculo e on e.id_espectaculo = ee.id_espectaculo 
	join Ubicaciones u ON (u.id_ubicacion = e.id_ubicacion)
	where id_publicacion = 7000
	order by tipo_ubicacion
	group by tipo_ubicacion, sin_numerar
end

SELECT * FROM Usuarios