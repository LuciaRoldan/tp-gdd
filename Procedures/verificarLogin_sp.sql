CREATE PROCEDURE verificarLogin_sp
@usuario VARCHAR(255),
@encriptada VARCHAR(255),
@contrasenia VARCHAR (255)
AS
	IF EXISTS(SELECT * FROM Usuarios WHERE username = @usuario AND (password = @contrasenia OR password = @encriptada) AND intentos_fallidos < 3) BEGIN
		UPDATE Usuarios
		SET intentos_fallidos = 0
		WHERE username = @usuario AND (password = @contrasenia OR password = @encriptada)

		DECLARE @debe_cambiar_pass BIT
		SET @debe_cambiar_pass = (SELECT debe_cambiar_pass FROM Usuarios WHERE username = @usuario AND (password = @contrasenia OR password = @encriptada))
		IF @debe_cambiar_pass = 1 BEGIN
			UPDATE Usuarios SET debe_cambiar_pass = 0 WHERE username = @usuario AND (password = @contrasenia OR password = @encriptada)
		END
		SELECT @debe_cambiar_pass debe_cambiar_pass
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

-- drop procedure verificarLogin_sp

--update Usuarios set debe_cambiar_pass = 1 where username like '3'

--select * from Usuarios where username like '3'

