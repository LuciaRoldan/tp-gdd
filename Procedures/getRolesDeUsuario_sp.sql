CREATE PROCEDURE getRolesDeUsuario_sp
@usuario VARCHAR(50)
AS
BEGIN
	SELECT nombre FROM Roles r
	JOIN Usuarios u ON(u.username = @usuario)
	JOIN UsuarioXRol uxr ON(uxr.id_usuario = u.id_usuario AND uxr.id_rol = r.id_rol)