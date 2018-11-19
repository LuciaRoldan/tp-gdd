CREATE PROCEDURE registroEmpresa(@username VARCHAR(25), @password VARCHAR(25),  @razon_social nvarchar(255), @mail nvarchar(50), 
 @cuit nvarchar(255), @calle nvarchar(50), @numero_calle NUMERIC(18,0), @piso NUMERIC(18,0), @depto nvarchar(50), @codigo_postal nvarchar(50))
AS
BEGIN 
IF NOT EXISTS (SELECT * FROM dbo.Usuarios u JOIN dbo.Empresas e ON (u.id_usuario = e.id_usuario) WHERE username = @username OR cuit = @cuit OR mail = @mail) 
BEGIN
begin transaction
INSERT INTO dbo.Usuarios(username, password, habilitado, alta_logica) VALUES (@username, @password, '1', GETDATE())
INSERT INTO dbo.Empresas(id_usuario, razon_social, mail, cuit, fecha_creacion, calle, numero_calle, piso, depto, codigo_postal) VALUES ((SELECT id_usuario FROM dbo.Usuarios WHERE username like @username), @razon_social, @mail, @cuit, GETDATE(), @calle, @numero_calle, @piso, @depto, @codigo_postal)
commit transaction
END
ELSE
RAISERROR( 'La empresa ya existe',16,217) WITH SETERROR
END 
GO