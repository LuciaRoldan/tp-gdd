CREATE PROCEDURE buscarEmpresaPorUsername_sp
@username VARCHAR(255)
AS
BEGIN
	SELECT * FROM Usuarios u JOIN Empresas e ON (u.id_usuario = e.id_usuario)
	WHERE username LIKE @username
END

--Información adicional: Instrucción INSERT en conflicto con la restricción CHECK "CK__Espectacu__estad__04FA9675". El conflicto ha aparecido en la base de datos "GD2C2018", tabla "dbo.Espectaculos", column 'estado_espectaculo'.