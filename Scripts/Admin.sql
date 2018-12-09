INSERT INTO Usuarios(username, password, habilitado, alta_logica, intentos_fallidos)
VALUES('admin', LOWER(CONVERT(char(100),HASHBYTES('SHA2_256', 'w23e'),2)), 1, GETDATE(), 0)

INSERT INTO Usuarios(username, password, habilitado, alta_logica, intentos_fallidos)
VALUES('sa', LOWER(CONVERT(char(100),HASHBYTES('SHA2_256', 'gestiondedatos'),2)), 1, GETDATE(), 0)

INSERT INTO dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM dbo.Usuarios WHERE username like 'admin'), 4)
INSERT INTO dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM dbo.Usuarios WHERE username like 'sa'), 1)

INSERT INTO Roles(nombre, habilitado)
VALUES ('adminOP', 1)

INSERT INTO dbo.FuncionalidadXRol(id_rol, id_funcionalidad)
SELECT 1, id_funcionalidad
FROM Funcionalidades
WHERE nombre IN('ABM de cliente', 'ABM de empresa de espectaculos', 'Generar pago de comisiones', 'Listado estadistico', 'Registro de usuario')

INSERT INTO dbo.FuncionalidadXRol(id_rol, id_funcionalidad)
SELECT 2, id_funcionalidad
FROM Funcionalidades
WHERE nombre IN('ABM de categoria', 'ABM grado de publicacion', 'Editar publicacion', 'Generar publicacion')

INSERT INTO dbo.FuncionalidadXRol(id_rol, id_funcionalidad)
SELECT 3, id_funcionalidad
FROM Funcionalidades
WHERE nombre IN('Canje y administracion de puntos', 'Comprar', 'Historial del cliente')

INSERT INTO dbo.FuncionalidadXRol(id_rol, id_funcionalidad)
SELECT 4, id_funcionalidad
FROM Funcionalidades

exec getFuncionalidadesDeUsuario_sp 'sa'
exec getFuncionalidadesDeUsuario_sp '20959835'
exec getFuncionalidadesDeUsuario_sp '19-67139304-09'
exec getFuncionalidadesDeUsuario_sp 'admin'