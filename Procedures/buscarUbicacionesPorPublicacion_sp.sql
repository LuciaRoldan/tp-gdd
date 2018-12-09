--drop procedure buscarUbicacionessPorPublicacion_sp

create procedure buscarUbicacionessPorPublicacion_sp (@id_publicacion int) as begin
	select distinct(tipo_ubicacion) tipo, sin_numerar, precio, count(distinct fila) as filas, count(asiento) as asientos from Ubicaciones u
	join dbo.UbicacionXEspectaculo e on e.id_ubicacion = u.id_ubicacion join Espectaculos ee on ee.id_espectaculo = e.id_espectaculo
	where id_publicacion = @id_publicacion
	group by tipo_ubicacion, sin_numerar,precio
end




