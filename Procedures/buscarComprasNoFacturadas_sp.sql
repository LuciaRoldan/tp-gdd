
create procedure buscarComprasNoFacturadas_sp (@razonSocial varchar(255)) as begin
	select c.id_compra, descripcion, coalesce(comision, 0) comision, coalesce(importe, 0) importe from Compras c 
	join UbicacionXEspectaculo u on c.id_compra = u.id_compra
	join Espectaculos es on es.id_espectaculo = u.id_espectaculo
	join Publicaciones p on p.id_publicacion = es.id_publicacion
	join Empresas e on e.id_empresa = p.id_empresa and e.razon_social = @razonSocial
	where id_factura is null 
end
