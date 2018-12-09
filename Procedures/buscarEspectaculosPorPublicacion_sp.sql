

create procedure buscarEspectaculosPorPublicacion_sp (@id_publicacion int) as begin
	select fecha_evento from Espectaculos where id_publicacion = @id_publicacion
end