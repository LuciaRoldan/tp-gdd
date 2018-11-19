CREATE PROCEDURE verificarLogin_sp
@usuario VARCHAR(50),
@contrasenia VARCHAR(50)
AS
	IF EXISTS(SELECT * FROM Usuarios WHERE username = @usuario AND password = @contrasenia)
	BEGIN
		UPDATE Usuarios
		SET intentos_fallidos = 0

		SELECT COUNT(*) FROM Usuarios WHERE username = @usuario AND password = @contrasenia
	END
	ELSE --existe el usuario pero la contrasenia es otra
	BEGIN
		IF EXISTS(SELECT * FROM Usuarios WHERE username = @usuario)
		BEGIN
			UPDATE Usuarios
			SET intentos_fallidos = (SELECT intentos_fallidos FROM Usuarios WHERE username = @usuario) + 1

			SELECT COUNT(*) FROM Usuarios WHERE username = @usuario AND password = @contrasenia
		END
		ELSE --no existe el usuario
			BEGIN
			RAISERROR('Usuario inexistente', 16, 1)
			END
	END