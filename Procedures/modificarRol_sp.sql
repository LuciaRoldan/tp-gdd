CREATE PROCEDURE modificarRol_sp
@nombre_rol VARCHAR(50),
@habilitado BIT
AS
BEGIN
	IF EXISTS (SELECT nombre FROM Roles WHERE nombre = @nombre_rol)
	BEGIN
		UPDATE Roles
		SET nombre = @nombre_rol, habilitado = @habilitado
		WHERE nombre = @nombre_rol
	END
	ELSE
	BEGIN
		RAISERROR('Rol inexistente', 16, 1)
	END
END