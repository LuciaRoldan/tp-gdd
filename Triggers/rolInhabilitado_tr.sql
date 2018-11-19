CREATE TRIGGER rolInhabilitado_tr
ON Roles
AFTER UPDATE
AS
BEGIN	
	IF((SELECT habilitado FROM DELETED) <> (SELECT habilitado FROM INSERTED))
	BEGIN
		DECLARE @id_rol_modificado INT
		SET @id_rol_modificado = (SELECT id_rol FROM DELETED)

		DELETE FROM UsuarioXRol
		WHERE id_rol = @id_rol_modificado

	END
END