CREATE PROCEDURE AgregarFuncionalidadARol_sp
@nombre_rol VARCHAR(50),
@nombre_funcionalidad VARCHAR(50)
AS
BEGIN
	DECLARE @id_rol INT, @id_funcionalidad INT
	SET @id_rol = (SELECT id_rol FROM Roles WHERE nombre = @nombre_rol)
	SET @id_funcionalidad = (SELECT id_funcionalidad FROM Funcionalidades WHERE nombre = @nombre_funcionalidad)

	IF NOT EXISTS (SELECT nombre FROM Roles WHERE nombre = @nombre_rol)
		BEGIN
		RAISERROR('Rol inexistente', 20, 1) WITH LOG
		END

	IF NOT EXISTS (SELECT nombre FROM Funcionalidades WHERE nombre = @nombre_funcionalidad)
		BEGIN
		RAISERROR('Funcionalidad inexistente', 20, 1) WITH LOG
		END

	IF NOT EXISTS (SELECT id_rol, id_funcionalidad FROM FuncionalidadXRol
					WHERE id_rol = @id_rol AND id_funcionalidad = @id_funcionalidad)
		BEGIN
			INSERT INTO FuncionalidadXRol(id_funcionalidad, id_rol)
			VALUES (@id_funcionalidad, @id_rol)
		END
	
	--RAISERROR('Funcionalidad ya existente para ese rol', 16, 1) --no es grave lol podria tb no hacer nada
END
