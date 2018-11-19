CREATE PROCEDURE AgregarFuncionalidadARol_sp
@nombre_rol VARCHAR(50),
@nombre_funcionalidad VARCHAR(50)
AS
BEGIN
	DECLARE @id_rol INT, @id_funcinalidad INT
	SET @id_rol = (SELECT id_rol FROM Roles WHERE nombre = @nombre_rol)
	SET @id_funcinalidad = (SELECT id_funcionalidad FROM Funcionalidades WHERE nombre = @nombre_funcionalidad)

	IF NOT EXISTS (SELECT nombre FROM Roles WHERE nombre = @nombre_rol)
		BEGIN
		RAISERROR('Rol inexistente', 16, 1)
		END

	IF NOT EXISTS (SELECT nombre FROM Funcionalidades WHERE nombre = @nombre_funcionalidad)
		BEGIN
		RAISERROR('Funcionalidad inexistente', 16, 1)
		END

	IF NOT EXISTS (SELECT id_rol, id_funcionalidad FROM FuncionalidadXRol
					WHERE id_rol = @id_rol AND id_funcionalidad = @id_funcinalidad)
		BEGIN
			INSERT INTO FuncionalidadXRol(id_funcionalidad, id_rol)
			VALUES (@id_funcinalidad, @id_rol)
		END
	ELSE
		RAISERROR('Funcionalidad ya existente para ese rol', 16, 1) --no es grave lol podria tb no hacer nada
END