
create procedure actualizarUsuarioYContrasenia_sp (@usernameV nvarchar(255), @usernameN nvarchar(255), @encriptada nvarchar(255)) as begin
	update Usuarios set username = @usernameN, password = @encriptada where username = @usernameV
end

--drop procedure actualizarUsuarioYContrasenia_sp

--45023700