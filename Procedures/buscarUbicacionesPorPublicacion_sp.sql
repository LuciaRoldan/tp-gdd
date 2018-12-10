--drop procedure buscarUbicacionessPorPublicacion_sp

create procedure buscarUbicacionessPorPublicacion_sp (@id_publicacion int) as begin
	SELECT tipo_ubicacion as tipo, sin_numerar, precio, count(distinct fila) as filas, count(*)/count(DISTINCT e.id_espectaculo) as asientos 
	FROM (SELECT * FROM Espectaculos
	where id_publicacion = 7809) AS e
	JOIN UbicacionXEspectaculo uxe ON (e.id_espectaculo = uxe.id_espectaculo)
	JOIN Ubicaciones u ON (uxe.id_ubicacion = u.id_ubicacion)
	GROUP BY tipo_ubicacion, sin_numerar, precio
end