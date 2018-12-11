--EXEC dbo.buscarPublicacionesPorCriterio_sp null, null, '12/10/2018 7:27:52 PM', '2/9/2019 7:27:52 PM'

go
create procedure buscarPublicacionesPorCriterio_sp (@descripcion varchar(255), @categorias varchar(255), @desde date, @hasta date, @offset INT) as begin
	declare @query nvarchar(2000)
	set @query = 
	'select p.descripcion descripcion, r.descripcion rubro, direccion, p.id_publicacion id from Publicaciones p join Espectaculos e on p.id_publicacion = e.id_publicacion 
	join Rubros r on r.id_rubro = p.id_rubro
	where e.estado_espectaculo = ''Publicada'''

	if ( @desde is not null and @hasta is not null) begin set @query = 
		@query + ' and e.fecha_evento between ''' + (select convert(varchar, @desde, 22)) + ''' and ''' +  (select convert(varchar, @hasta, 22)) + ''' ' end
	if ( @descripcion is not null) begin set @query = @query + 'and p.descripcion like ''%' + @descripcion + '%''' end
	if ( @categorias is not null) begin set @query = @query + 'and r.descripcion in (' + (@categorias) + ')' end

	set @query = @query + 'order by p.id_grado_publicacion ASC offset ' + @offset + ' rows fetch next 10 rows only'

	print @query
	exec sp_executesql @query
end

--drop procedure buscarPublicacionesPorCriterio_sp

--EXEC dbo.buscarPublicacionesPorCriterio_sp 'Descripcion' , null, null, null, 0

select p.descripcion descripcion, r.descripcion rubro, direccion, p.id_publicacion id from Publicaciones p join Espectaculos e on p.id_publicacion = e.id_publicacion 
	join Rubros r on r.id_rubro = p.id_rubro
	where e.estado_espectaculo = 'Publicada'and p.descripcion like '%Descripcion%'
	order by p.id_grado_publicacion ASC offset 0  rows fetch next 10 rows only


--select * from clientes

