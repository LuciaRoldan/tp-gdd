CREATE PROCEDURE inhabilitarUsuario_sp
@usuario VARCHAR(50)
AS
BEGIN
	UPDATE Usuarios
	SET habilitado = 1
	WHERE username = @usuario
END