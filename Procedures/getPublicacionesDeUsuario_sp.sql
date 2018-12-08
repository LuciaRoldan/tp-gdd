CREATE PROCEDURE getPublicacionesDeUsuario_sp
@usuario VARCHAR(50),
@id_empresa INT OUTPUT
AS
BEGIN
	SELECT descripcion FROM Publicaciones p
	JOIN Empresas e ON(e.id_empresa = p.id_empresa)
	JOIN Usuarios u ON (e.id_usuario = u.id_usuario)
	WHERE username = @usuario
	SET @id_empresa = (SELECT id_empresa FROM Empresas e JOIN Usuarios u ON (e.id_usuario = u.id_usuario)
							WHERE username like @usuario)
END

select * from Publicaciones
drop procedure getPublicacionesDeUsuario_sp