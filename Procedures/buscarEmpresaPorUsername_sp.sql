CREATE PROCEDURE buscarEmpresaPorUsername_sp
@username VARCHAR(255)
AS
BEGIN
	SELECT * FROM Usuarios u JOIN Empresas e ON (u.id_usuario = e.id_usuario)
	WHERE username LIKE @username
END
