CREATE PROCEDURE getPuntos_sp
@username varchar(30)
AS
BEGIN
	SELECT cantidad_puntos FROM Usuarios u JOIN Clientes c ON (u.id_usuario = c.id_usuario)
								  JOIN Puntos p ON (p.id_cliente = c.id_cliente)
	WHERE username = @username
END
