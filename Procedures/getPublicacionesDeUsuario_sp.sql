CREATE PROCEDURE getPublicacionesDeUsuario_sp
@usuario VARCHAR(50)
AS
BEGIN
	SELECT id_publicacion, descripcion, direccion FROM Publicaciones p
	JOIN Empresas e ON(e.id_empresa = p.id_empresa)
	JOIN Usuarios u ON (e.id_usuario = u.id_usuario)
	WHERE username = @usuario
END
