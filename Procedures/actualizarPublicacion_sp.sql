
create procedure actualizarPublicacion_sp (@id int, @descripcion nvarchar(255), @direccion nvarchar(80), @estado char(15), @rubro nvarchar(100)) as begin
	update Publicaciones set descripcion = @descripcion, direccion = @direccion, id_rubro = (select id_rubro from Rubros where descripcion = @rubro) where id_publicacion = @id
	update Espectaculos set estado_espectaculo = @estado where id_publicacion = @id
end
