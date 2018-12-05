CREATE PROCEDURE modificarNombreRol_sp
@nombre_viejo VARCHAR(50),
@nombre_nuevo VARCHAR(50)
AS
	IF EXISTS (SELECT nombre FROM Roles WHERE nombre = @nombre_viejo)
	BEGIN
		UPDATE Roles
		SET nombre = @nombre_nuevo
		WHERE nombre = @nombre_viejo
	END
	ELSE
		BEGIN
		RAISERROR('Rol inexistente', 16, 1)
		END
