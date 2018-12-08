CREATE PROCEDURE registrarPublicacion_sp(
@id_empresa INT,
@grado_publicacion NVARCHAR(20),
@rubro NVARCHAR(100),
@descripcion NVARCHAR(255),
@estado_publicacion CHAR(15),
@direccion NVARCHAR(80),
@id_publicacion INT OUTPUT
)
AS
BEGIN
	DECLARE @id_grado_publicacion INT = (SELECT id_grado_publicacion FROM Grados_publicacion WHERE nombre = @grado_publicacion)
	DECLARE @id_rubro INT =  (SELECT id_rubro FROM Rubros WHERE descripcion = @rubro)
	INSERT INTO Publicaciones(id_empresa, id_grado_publicacion, id_rubro, descripcion, direccion)
	VALUES (@id_empresa, @id_grado_publicacion, @id_rubro, @descripcion, @direccion)

	SET @id_publicacion = SCOPE_IDENTITY()
END