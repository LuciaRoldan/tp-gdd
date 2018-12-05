CREATE PROCEDURE getFuncionalidadesDeRol_sp
@nombre_rol VARCHAR(50)
AS
BEGIN
	SELECT DISTINCT f.nombre FROM Funcionalidades f
	JOIN Roles r ON (r.nombre = @nombre_rol)
	JOIN FuncionalidadXRol fxr ON(fxr.id_rol= r.id_rol AND fxr.id_funcionalidad = f.id_funcionalidad)
END
