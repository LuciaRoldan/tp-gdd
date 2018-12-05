CREATE PROCEDURE getFuncionalidadesDeUsuario_sp
@usuario VARCHAR(50)
AS
BEGIN
	SELECT DISTINCT nombre FROM Funcionalidades f
	JOIN FuncionalidadXRol fxr ON(f.id_funcionalidad = fxr.id_funcionalidad)
	JOIN UsuarioXRol uxr ON(uxr.id_rol = fxr.id_rol)
	JOIN Usuarios u ON(u.id_usuario = uxr.id_usuario AND u.username = @usuario)
END