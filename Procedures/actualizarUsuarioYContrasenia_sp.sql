
create procedure actualizarUsuarioYContrasenia_sp (@usernameV nvarchar(255), @usernameN nvarchar(255), @encriptada nvarchar(255)) as begin
	update Usuarios set username = @usernameN, password = @encriptada where username = @usernameV
end

--drop procedure actualizarUsuarioYContrasenia_sp

--update Usuarios set username = 'chauchi', password = 'chauchi' where username = '1'

--select * from Usuarios where username = 'chauchi'