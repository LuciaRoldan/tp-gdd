create procedure buscarPublicacionesPorEmpresa_sp (@razon_social nvarchar(255)) as begin
	select g.nombre grado, r.descripcion rubro, p.descripcion, direccion, p.id_publicacion id, es.estado_espectaculo estado from Publicaciones p 
	join Empresas e on e.id_empresa = p.id_empresa 
	join Grados_publicacion g on p.id_grado_publicacion = g.id_grado_publicacion
	join Rubros r on r.id_rubro = p.id_rubro
	join Espectaculos es on es.id_publicacion = p.id_publicacion
	where razon_social = @razon_social and estado_espectaculo = 'Borrador'
	group by p.id_publicacion, p.descripcion, direccion, g.nombre, r.descripcion, es.estado_espectaculo
end