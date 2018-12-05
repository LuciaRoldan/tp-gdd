CREATE PROCEDURE agregarRol_sp 
@nombre_rol VARCHAR(50)
AS
BEGIN
	IF NOT EXISTS (SELECT nombre FROM Roles WHERE nombre = @nombre_rol)
		BEGIN
		INSERT INTO Roles(nombre, habilitado)
		VALUES(@nombre_rol, 1)		
		END
	ELSE
		BEGIN
		RAISERROR('Este Rol ya existe', 16, 1)
		END
END

