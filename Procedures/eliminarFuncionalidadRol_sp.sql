CREATE PROCEDURE eliminarFuncionalidadesRol_sp
@rol_nombre VARCHAR(50)
AS
BEGIN
	DECLARE @id_rol INT = (SELECT id_rol FROM Roles WHERE nombre = @rol_nombre)

	DELETE FROM FuncionalidadXRol
	WHERE id_rol = @id_rol

END
