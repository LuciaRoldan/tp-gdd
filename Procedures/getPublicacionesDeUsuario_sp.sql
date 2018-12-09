CREATE PROCEDURE getPublicacionesDeUsuario_sp
@usuario VARCHAR(50)
AS
BEGIN
	SELECT descripcion, e.id_empresa FROM Publicaciones p
	JOIN Empresas e ON(e.id_empresa = p.id_empresa)
	JOIN Usuarios u ON (e.id_usuario = u.id_usuario)
	WHERE username = @usuario
END

select * from Publicaciones
drop procedure getPublicacionesDeUsuario_sp