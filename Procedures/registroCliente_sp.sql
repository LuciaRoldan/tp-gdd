CREATE PROCEDURE registroCliente_sp
(@username VARCHAR(25),
@password VARCHAR(25), 
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
@codigo_postal nvarchar(50))
AS
BEGIN 
	IF NOT EXISTS (SELECT * FROM dbo.Usuarios u JOIN dbo.Clientes c ON (u.id_usuario = c.id_usuario)
					WHERE username = @username OR cuil = @cuil OR documento = @documento OR mail = @mail) 
		BEGIN
		INSERT INTO dbo.Usuarios(username, password, habilitado, alta_logica) VALUES (@username, @password, '1', GETDATE())
		INSERT INTO dbo.Clientes(id_usuario, nombre, apellido, tipo_documento, documento, cuil, mail, telefono,
		fecha_creacion, fecha_nacimiento, calle, numero_calle, piso, depto, codigo_postal)
		VALUES ((SELECT id_usuario FROM dbo.Usuarios WHERE username = @username), @nombre, @apellido, @tipo_documento,
		@documento, @cuil, @mail, @telefono, GETDATE(), @fecha_nacimiento, @calle, @numero_calle, @piso, @depto, @codigo_postal)
		END
	ELSE
	RAISERROR('El Cliente ya existe', 16, 217) WITH SETERROR
END