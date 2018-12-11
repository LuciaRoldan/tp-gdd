--.--.--.--.--.--.--.--PROCEDURES--.--.--.--.--.--.--.--

-----verificarLogin-----
CREATE PROCEDURE verificarLogin_sp
@usuario VARCHAR(255),
@encriptada VARCHAR(255)
AS
BEGIN
	IF EXISTS(SELECT * FROM Usuarios WHERE username = @usuario AND  password = @encriptada AND intentos_fallidos < 3)
		BEGIN
		UPDATE Usuarios
		SET intentos_fallidos = 0
		WHERE username = @usuario AND password = @encriptada

		DECLARE @debe_cambiar_pass BIT
		SET @debe_cambiar_pass = (SELECT debe_cambiar_pass FROM Usuarios WHERE username = @usuario AND password = @encriptada)
		IF @debe_cambiar_pass = 1
			BEGIN
			UPDATE Usuarios SET debe_cambiar_pass = 0 WHERE username = @usuario AND password = @encriptada
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
END


-----getRolesDeUsuario-----
CREATE PROCEDURE getRolesDeUsuario_sp
@usuario VARCHAR(50)
AS
BEGIN
	SELECT DISTINCT nombre FROM Roles r
	JOIN Usuarios u ON(u.username = @usuario)
	JOIN UsuarioXRol uxr ON(uxr.id_usuario = u.id_usuario AND uxr.id_rol = r.id_rol)
END


-----getFuncionalidadesDeUsuario-----
CREATE PROCEDURE getFuncionalidadesDeUsuario_sp
@usuario VARCHAR(50)
AS
BEGIN
	SELECT DISTINCT nombre FROM Funcionalidades f
	JOIN FuncionalidadXRol fxr ON(f.id_funcionalidad = fxr.id_funcionalidad)
	JOIN UsuarioXRol uxr ON(uxr.id_rol = fxr.id_rol)
	JOIN Usuarios u ON(u.id_usuario = uxr.id_usuario AND u.username = @usuario)
END


-----getRubrosDePublicacion-----
CREATE PROCEDURE getRubrosDePublicacion_sp 
@id_publicacion NUMERIC
AS
BEGIN
	SELECT r.id_rubro, r.descripcion FROM Rubros r JOIN Publicaciones p ON (r.id_rubro = p.id_rubro)
	WHERE id_publicacion = @id_publicacion
END


-----buscarUsuarioPorCriterio-----
CREATE PROCEDURE buscarUsuarioPorCriterio_sp
@nombre VARCHAR(255),
@apellido VARCHAR(255),
@dni VARCHAR(18),
@email NVARCHAR(50)
AS
BEGIN
	SELECT id_cliente, nombre, apellido, coalesce(cuil,0) cuil, mail, coalesce(telefono,0) telefono, tipo_documento, fecha_nacimiento, fecha_creacion, coalesce(documento,0) documento, calle, coalesce(numero_calle,0) numero_calle
	FROM Clientes
	WHERE (nombre LIKE '%' + @nombre + '%'
		AND apellido LIKE '%' + @apellido + '%'
		AND documento = CAST(@dni AS INT)
		AND mail LIKE '%' + @email + '%')
		OR (nombre LIKE '%' + @nombre + '%'
		AND apellido LIKE '%' + @apellido + '%'
		AND @dni LIKE ''
		AND mail LIKE '%' + @email + '%')
END

select * from Clientes
-----

