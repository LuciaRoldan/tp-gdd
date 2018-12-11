

create procedure historialClienteConOffset_sp (@id_cliente int, @offset int) as begin
	select p.descripcion, e.fecha_evento, coalesce(c.importe, 0) importe, COUNT(uxe.id_ubicacion_espectaculo) 'cantidad_asientos', coalesce(RIGHT(mp.nro_tarjeta, 4),0) nro
	from  Compras c
	join UbicacionXEspectaculo uxe on uxe.id_compra = c.id_compra
	join Espectaculos e on e.id_espectaculo = uxe.id_espectaculo
	join Publicaciones p on p.id_publicacion = e.id_publicacion
	join Medios_de_pago mp on mp.id_medio_de_pago = c.id_medio_de_pago
	where c.id_cliente = @id_cliente
	group by c.id_compra, p.descripcion, e.fecha_evento, c.importe, mp.descripcion, mp.nro_tarjeta
	order by e.fecha_evento offset @offset rows fetch next 10 rows only
end




