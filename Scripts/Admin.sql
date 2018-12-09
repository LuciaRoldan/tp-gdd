INSERT INTO Usuarios(username, password, habilitado, alta_logica, intentos_fallidos)
VALUES('admin', LOWER(CONVERT(char(100),HASHBYTES('SHA2_256', 'w23e'),2)), 1, GETDATE(), 0)

INSERT INTO dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM dbo.Usuarios WHERE username like 'admin'), 1)

INSERT INTO dbo.FuncionalidadXRol(id_rol, id_funcionalidad)
SELECT 1, id_funcionalidad
FROM Funcionalidades