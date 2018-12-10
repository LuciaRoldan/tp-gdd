CREATE PROCEDURE registroEmpresa_sp(@username VARCHAR(255), @password VARCHAR(255),  @razon_social nvarchar(255), @mail nvarchar(50), 
 @cuit nvarchar(255), @calle nvarchar(50), @numero_calle NUMERIC(18,0), @piso NUMERIC(18,0), @depto nvarchar(50), @codigo_postal nvarchar(50))
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM dbo.Usuarios u JOIN dbo.Empresas e ON (u.id_usuario = e.id_usuario) 
	WHERE username = @username OR cuit = @cuit OR mail = @mail OR razon_social = @razon_social)
	BEGIN
		BEGIN TRANSACTION
		INSERT INTO dbo.Usuarios(username, password, habilitado, alta_logica) VALUES (@username, @password, '1', GETDATE())
		INSERT INTO dbo.Empresas(id_usuario, razon_social, mail, cuit, fecha_creacion, calle, numero_calle, piso, depto, codigo_postal) VALUES ((SELECT id_usuario FROM dbo.Usuarios WHERE username like @username), @razon_social, @mail, @cuit, GETDATE(), @calle, @numero_calle, @piso, @depto, @codigo_postal)
		INSERT INTO dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM dbo.Usuarios WHERE username like @username), 2)
		COMMIT TRANSACTION
	END
	ELSE
		RAISERROR( 'La empresa ya existe',20,1) WITH LOG
END 
