CREATE PROCEDURE getRubrosDePublicacion_sp 
@id_publicacion int
AS
BEGIN
	SELECT r.id_rubro, r.descripcion FROM Rubros r JOIN Publicaciones p ON (r.id_rubro = p.id_rubro)
	WHERE id_publicacion = @id_publicacion
END
select * from Usuarios

