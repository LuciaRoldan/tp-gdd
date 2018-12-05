CREATE PROCEDURE verificarLogin_sp
@usuario VARCHAR(255),
@encriptada VARCHAR(255),
@contrasenia VARCHAR (255)
AS
	IF EXISTS(SELECT * FROM Usuarios WHERE username = @usuario AND (password = @contrasenia OR password = @encriptada))
	BEGIN
		UPDATE Usuarios
		SET intentos_fallidos = 0

		SELECT COUNT(*)cantidad FROM Usuarios WHERE username = @usuario AND (password = @contrasenia OR password = @encriptada)
	END
	ELSE --existe el usuario pero la contrasenia es otra
	BEGIN
		IF EXISTS(SELECT * FROM Usuarios WHERE username = @usuario)
		BEGIN
			IF((SELECT intentos_fallidos FROM Usuarios WHERE username = @usuario) < 3)
				BEGIN
				UPDATE Usuarios
				SET intentos_fallidos = (SELECT intentos_fallidos FROM Usuarios WHERE username = @usuario) + 1
				WHERE username = @usuario;
				RAISERROR('Contraseña invalida', 16, 1)

				SELECT COUNT(*)cantidad FROM Usuarios WHERE username = @usuario AND (password = @contrasenia OR password = @encriptada)
			END
			ELSE --tiene 3 intentos fallidos
				BEGIN
				RAISERROR('Su usuario esta bloqueado', 16, 1)
			END	
		END	
		ELSE --no existe el usuario
			BEGIN
			RAISERROR('Usuario inexistente', 16, 1)
		END
	END

	DROP PROCEDURE verificarLogin_sp