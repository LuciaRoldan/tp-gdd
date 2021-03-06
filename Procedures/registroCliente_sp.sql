CREATE PROCEDURE registroCliente_sp
(@username VARCHAR(25),
@password VARCHAR(255), 
@nombre nvarchar(255),
@apellido nvarchar(255),
@tipo_documento CHAR(3),
@documento NUMERIC(18,0),
@cuil NUMERIC(18,0),
@mail NVARCHAR(50),
@telefono NUMERIC(15),
@fecha_nacimiento DATETIME,
@calle nvarchar(255),
@numero_calle NUMERIC(18,0),
@piso NUMERIC(18,0),
@depto nvarchar(255),
@codigo_postal nvarchar(50),
@cambio_pass BIT,
@fechaCreacion datetime)
AS
BEGIN 
	IF NOT EXISTS (SELECT * FROM dbo.Usuarios u JOIN dbo.Clientes c ON (u.id_usuario = c.id_usuario)
					WHERE username = @username OR cuil = @cuil OR documento = @documento OR mail = @mail) 
		BEGIN
		BEGIN TRANSACTION
		INSERT INTO dbo.Usuarios(username, password, habilitado, alta_logica, intentos_fallidos, debe_cambiar_pass) VALUES (@username, @password, '1', @fechaCreacion, 0, @cambio_pass)
		INSERT INTO dbo.Clientes(id_usuario, nombre, apellido, tipo_documento, documento, cuil, mail, telefono, fecha_creacion, fecha_nacimiento, calle, numero_calle, piso, depto, codigo_postal) VALUES ((SELECT id_usuario FROM dbo.Usuarios WHERE username like @username), @nombre, @apellido, @tipo_documento, @documento, @cuil, @mail, @telefono, GETDATE(), @fecha_nacimiento, @calle, @numero_calle, @piso, @depto, @codigo_postal)
		INSERT INTO dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM dbo.Usuarios WHERE username like @username), 3)
		COMMIT TRANSACTION
		END
	ELSE
	RAISERROR('El Cliente ya existe', 20, 1) WITH LOG
END

--drop procedure registroCliente_sp