CREATE PROCEDURE buscarClientePorUsername_sp
@username VARCHAR(255)
AS
BEGIN
	SELECT * FROM Usuarios u JOIN Clientes c ON (u.id_usuario = c.id_usuario)
	WHERE username LIKE @username
END
