--drop procedure buscarUbicacionessPorPublicacion_sp

create procedure buscarUbicacionessPorPublicacion_sp (@id_publicacion int) as begin
	select t.descripcion descripcion, count(distinct asiento)*count(distinct fila) asientos, sin_numerar, precio, count(distinct fila) filas, u.id_ubicacion
	from Ubicaciones u 
	join UbicacionXEspectaculo e on e.id_ubicacion = u.id_ubicacion 
	join TiposDeUbicacion t on t.id_tipo_ubicacion = u.codigo_tipo_ubicacion
	join Espectaculos ee on ee.id_espectaculo = e.id_espectaculo
	join Publicaciones p on p.id_publicacion = ee.id_publicacion
	where p.id_publicacion = @id_publicacion and e.id_compra is null
	group by t.descripcion, sin_numerar, precio, u.id_ubicacion
end


