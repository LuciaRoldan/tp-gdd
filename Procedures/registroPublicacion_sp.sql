CREATE PROCEDURE registrarPublicacion_sp(
@nombre_empresa NVARCHAR(255),
@grado_publicacion NVARCHAR(20),
@rubro NVARCHAR(100),
@descripcion NVARCHAR(255),
@estado_publicacion CHAR(15),
@direccion NVARCHAR(80)
)
AS
BEGIN
	DECLARE @id_empresa INT = (SELECT id_empresa FROM Empresas e JOIN Usuarios u ON (e.id_usuario = u.id_usuario)
																 WHERE username = @nombre_empresa)
	DECLARE @id_grado_publicacion INT = (SELECT id_grado_publicacion FROM Grados_publicacion WHERE nombre = @grado_publicacion)
	DECLARE @id_rubro INT =  (SELECT id_rubro FROM Rubros WHERE descripcion = @rubro)
	INSERT INTO Publicaciones(id_empresa, id_grado_publicacion, id_rubro, descripcion, direccion)
	VALUES (@id_empresa, @id_grado_publicacion, @id_rubro, @descripcion, @direccion)

	SELECT SCOPE_IDENTITY() AS id_publicacion
END