--.--.--.--.--.--.--.--PROCEDURES--.--.--.--.--.--.--.--
-----DROP PROCEDURES-----
DROP PROCEDURE MATE_LAVADO.verificarLogin_sp
DROP PROCEDURE MATE_LAVADO.getRolesDeUsuario_sp
DROP PROCEDURE MATE_LAVADO.getFuncionalidadesDeUsuario_sp
DROP PROCEDURE MATE_LAVADO.getRubrosDePublicacion_sp 
DROP PROCEDURE MATE_LAVADO.buscarUsuarioPorCriterio_sp
DROP PROCEDURE MATE_LAVADO.modificarCliente_sp
DROP PROCEDURE MATE_LAVADO.buscarClientePorUsername_sp
DROP PROCEDURE MATE_LAVADO.getPuntos_sp
DROP PROCEDURE MATE_LAVADO.getRubros_sp
DROP PROCEDURE MATE_LAVADO.buscarPublicacionesPorCriterio_sp
DROP PROCEDURE MATE_LAVADO.registroCliente_sp
DROP PROCEDURE MATE_LAVADO.registroEmpresa_sp
DROP PROCEDURE MATE_LAVADO.getPremios_sp
DROP PROCEDURE MATE_LAVADO.borrarPuntos_sp
DROP PROCEDURE MATE_LAVADO.historialClienteConOffset_sp
DROP PROCEDURE MATE_LAVADO.registrarCompra_sp
DROP PROCEDURE MATE_LAVADO.registrarCompraExU_sp
DROP PROCEDURE MATE_LAVADO.getPublicacionesDeUsuario_sp
DROP PROCEDURE MATE_LAVADO.registrarPublicacion_sp
DROP PROCEDURE MATE_LAVADO.agregarEspectaculo_sp
DROP PROCEDURE MATE_LAVADO.agregarUbicaciones_sp
DROP PROCEDURE MATE_LAVADO.agregarUbicacionSinNumerar_sp
DROP PROCEDURE MATE_LAVADO.agregarUbicacionNumerada_sp
DROP PROCEDURE MATE_LAVADO.actualizarGradoPublicacion_sp
DROP PROCEDURE MATE_LAVADO.buscarEspectaculosPorPublicacion_sp
DROP PROCEDURE MATE_LAVADO.getMediosDePago_sp
DROP PROCEDURE MATE_LAVADO.registrarMedioDePago_sp
DROP PROCEDURE MATE_LAVADO.buscarUbicacionesPorPublicacion_sp
DROP PROCEDURE MATE_LAVADO.buscarEmpresaPorCriterio_sp
DROP PROCEDURE MATE_LAVADO.modificarEmpresa_sp
DROP PROCEDURE MATE_LAVADO.agregarRol_sp
DROP PROCEDURE MATE_LAVADO.getFuncionalidadesDeRol_sp
DROP PROCEDURE MATE_LAVADO.eliminarFuncionalidadesRol_sp
DROP PROCEDURE MATE_LAVADO.agregarFuncionalidadARol_sp
DROP PROCEDURE MATE_LAVADO.modificarNombreRol_sp
DROP PROCEDURE MATE_LAVADO.modificarRol_sp
DROP PROCEDURE MATE_LAVADO.top5ClientesPuntosVencidos_sp
DROP PROCEDURE MATE_LAVADO.traerTodasRazonesSociales_sp
DROP PROCEDURE MATE_LAVADO.top5ClienteComprasParaUnaEmpresa_sp
DROP PROCEDURE MATE_LAVADO.top5EmpresasLocalidadesNoVendidas_sp
DROP PROCEDURE MATE_LAVADO.buscarComprasNoFacturadas_sp
DROP PROCEDURE MATE_LAVADO.actualizarCompraFactura_sp
DROP PROCEDURE MATE_LAVADO.agregarFactura_sp
DROP PROCEDURE MATE_LAVADO.actualizarPublicacion_sp
DROP PROCEDURE MATE_LAVADO.actualizarUsuarioYContrasenia_sp
DROP PROCEDURE MATE_LAVADO.agregarUbicacionXEspectaculo_sp
DROP PROCEDURE MATE_LAVADO.buscarEmpresaPorUsername_sp
DROP PROCEDURE MATE_LAVADO.buscarPublicacionesPorEmpresa_sp
DROP PROCEDURE MATE_LAVADO.vaciarEspectaculosPublicacion_sp
DROP PROCEDURE MATE_LAVADO.filasDisponiblesSegunEspectaculo_sp
DROP PROCEDURE MATE_LAVADO.asientosDisponiblesSegunEspectaculoYFila_sp

DROP TRIGGER MATE_LAVADO.insertarNuevoEspectaculo
DROP TRIGGER MATE_LAVADO.insertarNuevaFactura
DROP TRIGGER MATE_LAVADO.rolInhabilitado_tr
DROP TRIGGER MATE_LAVADO.insertarNuevaCompra
DROP TRIGGER MATE_LAVADO.finalizarEspectaculoAgotado_tg

DROP FUNCTION MATE_LAVADO.getCantidadEntradasEspectaculo
DROP FUNCTION MATE_LAVADO.getCantidadEntradasVendidas

GO
-----CREAR PROCEDURES-----
-----verificarLogin-----
CREATE PROCEDURE MATE_LAVADO.verificarLogin_sp
@usuario VARCHAR(255),
@encriptada VARCHAR(255)
AS
BEGIN
	IF EXISTS(SELECT * FROM MATE_LAVADO.Usuarios WHERE username = @usuario AND  password = @encriptada AND intentos_fallidos < 3)
		BEGIN
		UPDATE MATE_LAVADO.Usuarios
		SET intentos_fallidos = 0
		WHERE username = @usuario AND password = @encriptada

		DECLARE @debe_cambiar_pass BIT
		SET @debe_cambiar_pass = (SELECT debe_cambiar_pass FROM MATE_LAVADO.Usuarios WHERE username = @usuario AND password = @encriptada)
		IF @debe_cambiar_pass = 1
			BEGIN
			UPDATE MATE_LAVADO.Usuarios SET debe_cambiar_pass = 0 WHERE username = @usuario AND password = @encriptada
			END
		SELECT @debe_cambiar_pass debe_cambiar_pass
	END
	ELSE --existe el usuario pero la contrasenia es otra
	BEGIN
		IF EXISTS(SELECT * FROM MATE_LAVADO.Usuarios WHERE username = @usuario)
		BEGIN
		IF((SELECT intentos_fallidos FROM MATE_LAVADO.Usuarios WHERE username = @usuario) < 3)
			BEGIN
			UPDATE MATE_LAVADO.Usuarios
			SET intentos_fallidos = (SELECT intentos_fallidos FROM MATE_LAVADO.Usuarios WHERE username = @usuario) + 1
			WHERE username = @usuario;
			RAISERROR('Contrase�a invalida', 16, 1)
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
GO

-----getPremios-----
CREATE PROCEDURE MATE_LAVADO.getPremios_sp
AS
BEGIN
	SELECT descripcion, puntos FROM MATE_LAVADO.Premios
END
GO

-----borrarPuntos-----
CREATE PROCEDURE MATE_LAVADO.borrarPuntos_sp
@cantidad int,
@username varchar(30)
AS
BEGIN
	DECLARE @puntos INT
	DECLARE @fecha_vencimiento DATETIME
	DECLARE @id_punto INT
	DECLARE @id_cliente int = (SELECT id_cliente FROM MATE_LAVADO.Clientes c JOIN MATE_LAVADO.Usuarios u ON (u.id_usuario = c.id_usuario)
											 WHERE username = @username)
	

	DECLARE administradorPuntos CURSOR FOR 
	SELECT id_punto, cantidad_puntos, fecha_vencimiento FROM MATE_LAVADO.Puntos p
												WHERE p.id_cliente = @id_cliente
												GROUP BY id_punto, cantidad_puntos, fecha_vencimiento
												ORDER BY fecha_vencimiento

	OPEN administradorPuntos
	FETCH NEXT FROM administradorPuntos
	INTO @id_punto, @puntos, @fecha_vencimiento

	WHILE(@@FETCH_STATUS = 0)
	BEGIN
		WHILE(@cantidad != 0)
		BEGIN
			IF (@puntos < @cantidad)
			BEGIN
				SET @cantidad = @cantidad - @puntos
				DELETE  
				FROM MATE_LAVADO.Puntos
				WHERE id_punto = @id_punto
			END
			ELSE 
			BEGIN
				UPDATE MATE_LAVADO.Puntos 
				SET cantidad_puntos = cantidad_puntos - @cantidad
				WHERE id_punto = @id_punto
				SET @cantidad = 0
			END		 
		END

		FETCH NEXT FROM administradorPuntos
		INTO @id_punto, @puntos, @fecha_vencimiento
	END

	CLOSE administradorPuntos
	DEALLOCATE administradorPuntos
END
GO

-----getRolesDeUsuario-----
CREATE PROCEDURE MATE_LAVADO.getRolesDeUsuario_sp
@usuario VARCHAR(50)
AS
BEGIN
	SELECT DISTINCT nombre FROM MATE_LAVADO.Roles r
	JOIN MATE_LAVADO.Usuarios u ON(u.username = @usuario)
	JOIN MATE_LAVADO.UsuarioXRol uxr ON(uxr.id_usuario = u.id_usuario AND uxr.id_rol = r.id_rol)
END
GO

-----getFuncionalidadesDeUsuario-----
CREATE PROCEDURE MATE_LAVADO.getFuncionalidadesDeUsuario_sp
@usuario VARCHAR(50)
AS
BEGIN
	SELECT DISTINCT nombre FROM MATE_LAVADO.Funcionalidades f
	JOIN MATE_LAVADO.FuncionalidadXRol fxr ON(f.id_funcionalidad = fxr.id_funcionalidad)
	JOIN MATE_LAVADO.UsuarioXRol uxr ON(uxr.id_rol = fxr.id_rol)
	JOIN MATE_LAVADO.Usuarios u ON(u.id_usuario = uxr.id_usuario AND u.username = @usuario)
END
GO

-----eliminarFuncionalidadDeRol-----
CREATE PROCEDURE MATE_LAVADO.eliminarFuncionalidadesRol_sp
@rol_nombre VARCHAR(50)
AS
BEGIN
	DECLARE @id_rol INT = (SELECT id_rol FROM MATE_LAVADO.Roles WHERE nombre = @rol_nombre)

	DELETE FROM MATE_LAVADO.FuncionalidadXRol
	WHERE id_rol = @id_rol

END
GO

-----modificarRol-----
CREATE PROCEDURE MATE_LAVADO.modificarRol_sp(
@nombre VARCHAR(50),
@habilitado BIT)
AS
BEGIN
	IF EXISTS (SELECT nombre FROM MATE_LAVADO.Roles WHERE nombre = @nombre)
	BEGIN
		UPDATE MATE_LAVADO.Roles
		SET habilitado = @habilitado
		WHERE nombre = @nombre
	END
	ELSE
	BEGIN
		RAISERROR('Rol inexistente', 16, 1)
	END
END
GO

-----modificarNombreRol-----
CREATE PROCEDURE MATE_LAVADO.modificarNombreRol_sp
@nombre_viejo VARCHAR(50),
@nombre_nuevo VARCHAR(50)
AS
BEGIN
	IF EXISTS (SELECT nombre FROM MATE_LAVADO.Roles WHERE nombre = @nombre_viejo)
	BEGIN
		UPDATE MATE_LAVADO.Roles
		SET nombre = @nombre_nuevo
		WHERE nombre = @nombre_viejo
	END
	ELSE
	BEGIN
		RAISERROR('Rol inexistente', 16, 1)
	END
END
GO

-----getRubrosDePublicacion-----
CREATE PROCEDURE MATE_LAVADO.getRubrosDePublicacion_sp 
@id_publicacion NUMERIC
AS
BEGIN
	SELECT r.id_rubro, r.descripcion FROM MATE_LAVADO.Rubros r JOIN MATE_LAVADO.Publicaciones p ON (r.id_rubro = p.id_rubro)
	WHERE id_publicacion = @id_publicacion
END
GO

-----agregarFuncionalidadARol-----
CREATE PROCEDURE MATE_LAVADO.AgregarFuncionalidadARol_sp
@nombre_rol VARCHAR(50),
@nombre_funcionalidad VARCHAR(50)
AS
BEGIN
	DECLARE @id_rol INT, @id_funcionalidad INT
	SET @id_rol = (SELECT id_rol FROM MATE_LAVADO.Roles WHERE nombre = @nombre_rol)
	SET @id_funcionalidad = (SELECT id_funcionalidad FROM MATE_LAVADO.Funcionalidades WHERE nombre = @nombre_funcionalidad)

	IF NOT EXISTS (SELECT nombre FROM MATE_LAVADO.Roles WHERE nombre = @nombre_rol)
		BEGIN
		RAISERROR('Rol inexistente', 20, 1) WITH LOG
		END

	IF NOT EXISTS (SELECT nombre FROM MATE_LAVADO.Funcionalidades WHERE nombre = @nombre_funcionalidad)
		BEGIN
		RAISERROR('Funcionalidad inexistente', 20, 1) WITH LOG
		END

	IF NOT EXISTS (SELECT id_rol, id_funcionalidad FROM MATE_LAVADO.FuncionalidadXRol
					WHERE id_rol = @id_rol AND id_funcionalidad = @id_funcionalidad)
		BEGIN
			INSERT INTO MATE_LAVADO.FuncionalidadXRol(id_funcionalidad, id_rol)
			VALUES (@id_funcionalidad, @id_rol)
		END
	
	RAISERROR('Funcionalidad ya existente para ese rol', 16, 1) --no es grave lol podria tb no hacer nada
END
GO

-----buscarUsuarioPorCriterio-----
CREATE PROCEDURE MATE_LAVADO.buscarUsuarioPorCriterio_sp
@nombre VARCHAR(255),
@apellido VARCHAR(255),
@dni VARCHAR(18),
@email NVARCHAR(50)
AS
BEGIN
	SELECT id_cliente, nombre, apellido, coalesce(cuil,0) cuil, mail, coalesce(telefono,0) telefono, tipo_documento, fecha_nacimiento,
		fecha_creacion, coalesce(documento,0) documento, calle, coalesce(numero_calle,0) numero_calle, codigo_postal, depto, piso
	FROM MATE_LAVADO.Clientes
	WHERE (nombre LIKE '%' + @nombre + '%'
		AND apellido LIKE '%' + @apellido + '%'
		AND documento = CAST(@dni AS INT)
		AND mail LIKE '%' + @email + '%')
		OR (nombre LIKE '%' + @nombre + '%'
		AND apellido LIKE '%' + @apellido + '%'
		AND @dni LIKE ''
		AND mail LIKE '%' + @email + '%')
END
GO

-----modificarCliente-----
CREATE PROCEDURE MATE_LAVADO.modificarCliente_sp
@id_cliente INT,
@nombre nvarchar(255),
@apellido nvarchar(255),
@mail NVARCHAR(50),
@documento NUMERIC(18,0),
@cuil NUMERIC(18,0),
@telefono NUMERIC(15),
@fecha_nacimiento DATETIME
AS
BEGIN 
	IF EXISTS (SELECT * FROM MATE_LAVADO.dbo.Clientes WHERE id_cliente = @id_cliente) 
		BEGIN
		BEGIN TRANSACTION
		UPDATE MATE_LAVADO.dbo.Clientes
		SET nombre = @nombre, apellido = @apellido, mail = @mail, documento = @documento, cuil = @cuil, telefono = @telefono, fecha_creacion = @fecha_nacimiento
		COMMIT TRANSACTION
		END
	ELSE
	RAISERROR('El cliente no existe', 20, 1) WITH LOG
END
GO

-----registroCliente-----
CREATE PROCEDURE MATE_LAVADO.registroCliente_sp
(@username VARCHAR(25),
@password VARCHAR(255), 
@nombre nvarchar(255),
@apellido nvarchar(255),
@tipo_documento CHAR(3),
@documento NUMERIC(18,0),
@cuil NUMERIC(18,0),
@mail NVARCHAR(50),
@telefono NUMERIC(15),
@fecha_nacimiento VARCHAR(30),
@calle nvarchar(255),
@numero_calle NUMERIC(18,0),
@piso NUMERIC(18,0),
@depto nvarchar(255),
@codigo_postal nvarchar(50),
@cambio_pass BIT,
@fecha_creacion VARCHAR(30))
AS
BEGIN 
	IF NOT EXISTS (SELECT * FROM MATE_LAVADO.dbo.Usuarios u JOIN MATE_LAVADO.dbo.Clientes c ON (u.id_usuario = c.id_usuario)
					WHERE username = @username OR cuil = @cuil OR documento = @documento OR mail = @mail) 
		BEGIN
		BEGIN TRANSACTION
		INSERT INTO MATE_LAVADO.dbo.Usuarios(username, password, habilitado, alta_logica, intentos_fallidos, debe_cambiar_pass) VALUES (@username, @password, '1', CONVERT(DATETIME, @fecha_creacion, 120), 0, @cambio_pass)
		INSERT INTO MATE_LAVADO.dbo.Clientes(id_usuario, nombre, apellido, tipo_documento, documento, cuil, mail, telefono, fecha_creacion, fecha_nacimiento,
			calle, numero_calle, piso, depto, codigo_postal)
		VALUES ((SELECT id_usuario FROM MATE_LAVADO.dbo.Usuarios WHERE username like @username), @nombre, @apellido, @tipo_documento, @documento, @cuil, @mail,
			@telefono, CONVERT(DATETIME, @fecha_creacion, 120), CONVERT(DATETIME, @fecha_nacimiento, 120), @calle, @numero_calle, @piso, @depto, @codigo_postal)
		INSERT INTO MATE_LAVADO.dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM MATE_LAVADO.dbo.Usuarios WHERE username like @username), 3)
		COMMIT TRANSACTION
		END
	ELSE
	RAISERROR('El Cliente ya existe', 20, 1) WITH LOG
END
GO

-----registroEmpresa-----
CREATE PROCEDURE MATE_LAVADO.registroEmpresa_sp(@username VARCHAR(255), @password VARCHAR(255),  @razon_social nvarchar(255), @mail nvarchar(50), 
 @cuit nvarchar(255), @calle nvarchar(50), @numero_calle NUMERIC(18,0), @piso NUMERIC(18,0), @depto nvarchar(50), @codigo_postal nvarchar(50), @cambio_pass BIT, @fecha_creacion VARCHAR(30))
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM MATE_LAVADO.dbo.Usuarios u JOIN MATE_LAVADO.dbo.Empresas e ON (u.id_usuario = e.id_usuario) 
	WHERE username = @username OR cuit = @cuit OR mail = @mail OR razon_social = @razon_social)
	BEGIN
		BEGIN TRANSACTION
		INSERT INTO MATE_LAVADO.dbo.Usuarios(username, password, habilitado, alta_logica, intentos_fallidos, debe_cambiar_pass) VALUES (@username, @password, '1',
			CONVERT(DATETIME, @fecha_creacion, 120)
			--CAST(@fecha_creacion AS DATETIME)
			, 0, @cambio_pass)
		INSERT INTO MATE_LAVADO.dbo.Empresas(id_usuario, razon_social, mail, cuit, fecha_creacion, calle, numero_calle, piso, depto, codigo_postal)
		VALUES ((SELECT id_usuario FROM MATE_LAVADO.dbo.Usuarios WHERE username like @username), @razon_social, @mail, @cuit, CONVERT(DATETIME, @fecha_creacion, 120),
			@calle, @numero_calle, @piso, @depto, @codigo_postal)
		INSERT INTO MATE_LAVADO.dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM MATE_LAVADO.dbo.Usuarios WHERE username like @username), 2)
		COMMIT TRANSACTION
	END
	ELSE
		RAISERROR( 'La empresa ya existe',20,1) WITH LOG
END
GO

-----buscarClientePorUsername-----
CREATE PROCEDURE MATE_LAVADO.buscarClientePorUsername_sp
@username VARCHAR(255)
AS
BEGIN
	SELECT * FROM MATE_LAVADO.Usuarios u JOIN MATE_LAVADO.Clientes c ON (u.id_usuario = c.id_usuario)
	WHERE username LIKE @username
END
GO

-----getPuntos-----
CREATE PROCEDURE MATE_LAVADO.getPuntos_sp
@username varchar(30),
@fecha VARCHAR(30)
AS
BEGIN
	SELECT COALESCE(SUM(cantidad_puntos), 0) 'cantidad_puntos' FROM MATE_LAVADO.Usuarios u JOIN MATE_LAVADO.Clientes c ON (u.id_usuario = c.id_usuario)
								  JOIN MATE_LAVADO.Puntos p ON (p.id_cliente = c.id_cliente)
	WHERE username = @username AND fecha_vencimiento IS NOT NULL AND fecha_vencimiento > CONVERT(DATETIME, @fecha, 121)

END
GO

-----getRubros-----
CREATE PROCEDURE MATE_LAVADO.getRubros_sp
AS
BEGIN
	SELECT descripcion FROM MATE_LAVADO.Rubros WHERE descripcion <> ''
END
GO

-----buscarPublicacionesPorEmpresa-----
create PROCEDURE MATE_LAVADO.buscarPublicacionesPorEmpresa_sp (@razon_social nvarchar(255)) as begin
	select g.nombre grado, r.descripcion rubro, p.descripcion, direccion, p.id_publicacion id, es.estado_espectaculo estado FROM MATE_LAVADO.Publicaciones p 
	JOIN MATE_LAVADO.Empresas e on e.id_empresa = p.id_empresa 
	JOIN MATE_LAVADO.Grados_publicacion g on p.id_grado_publicacion = g.id_grado_publicacion
	JOIN MATE_LAVADO.Rubros r on r.id_rubro = p.id_rubro
	JOIN MATE_LAVADO.Espectaculos es on es.id_publicacion = p.id_publicacion
	where razon_social = @razon_social and estado_espectaculo = 'Borrador'
	group by p.id_publicacion, p.descripcion, direccion, g.nombre, r.descripcion, es.estado_espectaculo
end
GO

-----historialClienteConOffset-----
create PROCEDURE MATE_LAVADO.historialClienteConOffset_sp (@id_cliente int, @offset int) as begin
	select p.descripcion, e.fecha_evento, coalesce(c.importe, 0) importe, COUNT(uxe.id_ubicacion_espectaculo) 'cantidad_asientos', coalesce(RIGHT(mp.nro_tarjeta, 4),0) nro
	FROM MATE_LAVADO. Compras c
	JOIN MATE_LAVADO.UbicacionXEspectaculo uxe on uxe.id_compra = c.id_compra
	JOIN MATE_LAVADO.Espectaculos e on e.id_espectaculo = uxe.id_espectaculo
	JOIN MATE_LAVADO.Publicaciones p on p.id_publicacion = e.id_publicacion
	JOIN MATE_LAVADO.Medios_de_pago mp on mp.id_medio_de_pago = c.id_medio_de_pago
	where c.id_cliente = 768
	group by c.id_compra, p.descripcion, e.fecha_evento, c.importe, mp.descripcion, mp.nro_tarjeta
	order by e.fecha_evento offset @offset rows fetch next 10 rows only
end
GO

-----registrarCompra-----
CREATE PROCEDURE MATE_LAVADO.registrarCompra_sp
@id_cliente INT,
@id_medio_de_pago INT,
@importe BIGINT,
@fecha VARCHAR(30)
AS
BEGIN
	SET IDENTITY_INSERT MATE_LAVADO.Compras ON
	INSERT INTO MATE_LAVADO.Compras(id_cliente, id_medio_de_pago, id_factura, fecha, importe)
	VALUES(@id_cliente, @id_medio_de_pago, NULL, CONVERT(DATETIME, @fecha, 121), @importe)
	SET IDENTITY_INSERT MATE_LAVADO.Compras OFF
END
go

-----registrarCompraEXU-----
CREATE PROCEDURE MATE_LAVADO.registrarCompraExU_sp (@id_compra INT, @id_ubicacion INT, @id_espectaculo BIGINT)
AS
BEGIN
	declare @id_ubicacion_espectaculo int
	set @id_ubicacion_espectaculo = (select id_ubicacion_espectaculo FROM MATE_LAVADO.UbicacionXEspectaculo where id_ubicacion = @id_ubicacion and id_espectaculo = @id_espectaculo)
	
	UPDATE MATE_LAVADO.UbicacionXEspectaculo
	SET id_compra = @id_compra
	WHERE id_ubicacion_espectaculo = @id_ubicacion_espectaculo
END
GO

-----getPublicacionesDeUsuario-----
CREATE PROCEDURE MATE_LAVADO.getPublicacionesDeUsuario_sp
@usuario VARCHAR(50)
AS
BEGIN
	SELECT id_publicacion, descripcion, direccion FROM MATE_LAVADO.Publicaciones p
	JOIN MATE_LAVADO.Empresas e ON(e.id_empresa = p.id_empresa)
	JOIN MATE_LAVADO.Usuarios u ON (e.id_usuario = u.id_usuario)
	WHERE username = @usuario
END
GO

-----buscarEmpresaPorCriterio-----
CREATE PROCEDURE MATE_LAVADO.buscarEmpresaPorCriterio_sp
@cuit VARCHAR(20),
@razon_social VARCHAR(20),
@email VARCHAR(20)
AS
BEGIN
	SELECT razon_social, mail, coalesce(cuit,null) cuit, mail, calle, numero_calle, piso, depto, fecha_creacion, codigo_postal  FROM MATE_LAVADO.Empresas
	WHERE (razon_social LIKE '%' + @razon_social + '%'
		AND mail LIKE '%' + @email + '%'
		AND cuit = @cuit)
		OR (razon_social LIKE '%' + @razon_social + '%'
		AND mail LIKE '%' + @email + '%'
		AND @cuit LIKE '')
END
GO

-----buscarEmpresaPorUsername-----
CREATE PROCEDURE MATE_LAVADO.buscarEmpresaPorUsername_sp
@username VARCHAR(255)
AS
BEGIN
	SELECT * FROM MATE_LAVADO.Usuarios u JOIN MATE_LAVADO.Empresas e ON (u.id_usuario = e.id_usuario)
	WHERE username LIKE @username
END
GO

-----modificarEmpresa-----
CREATE PROCEDURE MATE_LAVADO.modificarEmpresa_sp
@cuit_viejo varchar(20),
@razon_social varchar(20),
@mail varchar(20),
@cuit varchar(20)
AS
BEGIN
	IF EXISTS (SELECT cuit FROM MATE_LAVADO.Empresas WHERE cuit = @cuit_viejo)
	BEGIN
		IF(@cuit = @cuit_viejo)
			BEGIN
			UPDATE MATE_LAVADO.Empresas
				SET razon_social = @razon_social,
					mail = @mail,
					cuit = @cuit
				WHERE cuit like @cuit_viejo
			END
			ELSE
				IF NOT EXISTS (SELECT cuit FROM MATE_LAVADO.Empresas where cuit like @cuit)
				BEGIN
					UPDATE MATE_LAVADO.Empresas
						SET razon_social = @razon_social,
						mail = @mail,
						cuit = @cuit
					WHERE cuit like @cuit_viejo
				END
				ELSE
				BEGIN
					RAISERROR('Su cuit ya existe', 16, 1)
				END
			END
		ELSE
		BEGIN
			RAISERROR('El cuit es invalido o no existe, pruebe nuevamente', 16, 1)
		END
END
GO

-----agregarRol-----
CREATE PROCEDURE MATE_LAVADO.agregarRol_sp 
@nombre_rol VARCHAR(50)
AS
BEGIN
	IF NOT EXISTS (SELECT nombre FROM MATE_LAVADO.Roles WHERE nombre = @nombre_rol)
		BEGIN
		INSERT INTO MATE_LAVADO.Roles(nombre, habilitado)
		VALUES(@nombre_rol, 1)		
		END
	ELSE
		BEGIN
		RAISERROR('Este Rol ya existe', 16, 1)
		END
END
GO

-----getFuncionalidadesDeRol-----
CREATE PROCEDURE MATE_LAVADO.getFuncionalidadesDeRol_sp
@nombre_rol VARCHAR(50)
AS
BEGIN
	SELECT DISTINCT f.nombre FROM MATE_LAVADO.Funcionalidades f
	JOIN MATE_LAVADO.Roles r ON (r.nombre = @nombre_rol)
	JOIN MATE_LAVADO.FuncionalidadXRol fxr ON(fxr.id_rol= r.id_rol AND fxr.id_funcionalidad = f.id_funcionalidad)
END
GO

-----registrarPublicacion-----
CREATE PROCEDURE MATE_LAVADO.registrarPublicacion_sp
(
@nombre_empresa NVARCHAR(255),
@grado_publicacion NVARCHAR(20),
@rubro NVARCHAR(100),
@descripcion NVARCHAR(255),
@estado_publicacion CHAR(15),
@direccion NVARCHAR(80)
)
AS
BEGIN
	DECLARE @id_empresa INT = (SELECT id_empresa FROM MATE_LAVADO.Empresas e WHERE @nombre_empresa = razon_social)
	DECLARE @id_grado_publicacion INT = (SELECT id_grado_publicacion FROM MATE_LAVADO.Grados_publicacion WHERE nombre = @grado_publicacion)
	DECLARE @id_rubro INT =  (SELECT id_rubro FROM MATE_LAVADO.Rubros WHERE descripcion = @rubro)
	INSERT INTO MATE_LAVADO.Publicaciones(id_empresa, id_grado_publicacion, id_rubro, descripcion, direccion)
	VALUES (@id_empresa, @id_grado_publicacion, @id_rubro, @descripcion, @direccion)

	SELECT SCOPE_IDENTITY() AS 'id_publicacion'
END
GO

-----buscarEspectaculosPorPublicacion-----
create PROCEDURE MATE_LAVADO.buscarEspectaculosPorPublicacion_sp (@id_publicacion int) as begin
	select fecha_evento, id_espectaculo FROM MATE_LAVADO.Espectaculos where id_publicacion = @id_publicacion
end
GO

-----buscarPublicacionesPorCriterio_sp-----
create PROCEDURE MATE_LAVADO.buscarPublicacionesPorCriterio_sp (@descripcion varchar(255), @categorias varchar(255), @desde datetime, @hasta datetime, @offset INT) as begin
	declare @query nvarchar(2000)
	set @query = 
	'select p.descripcion descripcion, r.descripcion rubro, direccion, p.id_publicacion id FROM MATE_LAVADO.Publicaciones p JOIN MATE_LAVADO.Espectaculos e on p.id_publicacion = e.id_publicacion 
	JOIN MATE_LAVADO.Rubros r on r.id_rubro = p.id_rubro
	where e.estado_espectaculo = ''Publicada'''

	if ( @desde is not null and @hasta is not null) begin set @query = 
		@query + ' and e.fecha_evento between ' + 
		'CONVERT(DATETIME, ''' + (select convert(varchar, @desde, 25)) + ''', 121)' + ' and ' +  
		'CONVERT(DATETIME, ''' + (select convert(varchar, @hasta, 25)) + ''', 121)' + ' ' end
	if ( @descripcion is not null) begin set @query = @query + 'and p.descripcion like ''%' + @descripcion + '%''' end
	if ( @categorias is not null) begin set @query = @query + 'and r.descripcion in (' + (@categorias) + ') ' end

	set @query = @query + 'order by p.id_grado_publicacion ASC offset ' + (select convert(varchar, @offset)) + ' rows fetch next 10 rows only'

	exec sp_executesql @query
end
GO

-----agregarEspectaculo-----
CREATE PROCEDURE MATE_LAVADO.agregarEspectaculo_sp(
@id_publicacion INT,
@fecha VARCHAR(30),
@estado_publicacion CHAR(20),
@fecha_creacion VARCHAR(30)
)
AS
BEGIN
	IF (CONVERT(DATETIME, @fecha, 120) <  CONVERT(DATETIME, @fecha_creacion, 120))
		BEGIN
		RAISERROR('La fecha del evento no puede ser anterior a la fecha actual', 16, 1)
		END

	IF EXISTS (SELECT * FROM MATE_LAVADO.Espectaculos WHERE fecha_evento =  CONVERT(DATETIME, @fecha, 120) AND @id_publicacion = id_publicacion)
		BEGIN
		RAISERROR('No pueden existir dos funciones para una publicacion con la misma fecha', 16, 1)
		END

	INSERT INTO MATE_LAVADO.Espectaculos(id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo)
	VALUES(@id_publicacion,  CONVERT(DATETIME, @fecha_creacion, 120),  CONVERT(DATETIME, @fecha, 120), @estado_publicacion)

	SELECT MAX(id_espectaculo) AS id_espectaculo FROM MATE_LAVADO.Espectaculos
END
GO

-----agregarUbicaciones-----
CREATE PROCEDURE MATE_LAVADO.agregarUbicaciones_sp(
@tipo_ubicacion NVARCHAR(20),
@cantidad INT,
@filas INT,
@precio NUMERIC(18)
)
AS
BEGIN
	
	CREATE TABLE #UbicacionesInsertadas(
	id_ubicacion INT)
 	IF(@filas > 0) -- Caso numerado
	BEGIN
		EXEC MATE_LAVADO.agregarUbicacionNumerada_sp @tipo_ubicacion, @cantidad, @filas, @precio
	END
	ELSE
	BEGIN
		EXEC MATE_LAVADO.agregarUbicacionSinNumerar_sp @tipo_ubicacion, @cantidad, @precio
	END
 	DROP TABLE MATE_LAVADO.#UbicacionesInsertadas
END
GO

-----agregarUbicacionNumerada-----
CREATE PROCEDURE MATE_LAVADO.agregarUbicacionNumerada_sp(
@tipo_ubicacion NVARCHAR(20),
@cantidad INT,
@filas INT,
@precio NUMERIC(18)
)
AS
BEGIN
	DECLARE @contador_filas INT = 0
	DECLARE @asientos_por_fila INT = @cantidad / @filas
	DECLARE @id_ubicacion INT

	IF NOT EXISTS (SELECT * FROM MATE_LAVADO.TiposDeUbicacion WHERE descripcion = @tipo_ubicacion)
		BEGIN
			INSERT INTO MATE_LAVADO.TiposDeUbicacion(id_tipo_ubicacion, descripcion) values (
			(SELECT MAX(id_tipo_ubicacion)+1 FROM MATE_LAVADO.TiposDeUbicacion), @tipo_ubicacion)
		END

	WHILE(@contador_filas < @filas)
	BEGIN
		
		DECLARE @contador_asientos INT = 0

		WHILE(@contador_asientos < @asientos_por_fila)
		BEGIN

			INSERT INTO MATE_LAVADO.Ubicaciones(fila, asiento, sin_numerar, precio, codigo_tipo_ubicacion)
			SELECT CHAR(ASCII('A')+@contador_filas), @contador_asientos, 0, @precio, id_tipo_ubicacion 
			FROM MATE_LAVADO.TiposDeUbicacion WHERE descripcion = @tipo_ubicacion

			INSERT INTO MATE_LAVADO.#UbicacionesInsertadas
			SELECT SCOPE_IDENTITY()

			SET @contador_asientos += 1
		END

		SET @contador_filas +=1
	END

	SELECT * FROM MATE_LAVADO.#UbicacionesInsertadas
END
GO

-----agregarUbicacionSinNumerar-----
CREATE PROCEDURE MATE_LAVADO.agregarUbicacionSinNumerar_sp(
@tipo_ubicacion NVARCHAR(20),
@cantidad INT,
@precio NUMERIC(18)
)
AS
BEGIN
	DECLARE @contador INT = 0
	DECLARE @id_ubicacion INT

	WHILE(@contador < @cantidad)
	BEGIN

		IF NOT EXISTS (SELECT * FROM MATE_LAVADO.TiposDeUbicacion WHERE descripcion = @tipo_ubicacion)
		BEGIN
			INSERT INTO MATE_LAVADO.TiposDeUbicacion(id_tipo_ubicacion, descripcion) values(
			(SELECT MAX(id_tipo_ubicacion) + 1 FROM MATE_LAVADO.TiposDeUbicacion), @tipo_ubicacion)
		END

		INSERT INTO MATE_LAVADO.Ubicaciones(fila, asiento, sin_numerar, precio, codigo_tipo_ubicacion)
		SELECT NULL, NULL, 1, @precio, id_tipo_ubicacion
		FROM MATE_LAVADO.TiposDeUbicacion WHERE descripcion = @tipo_ubicacion

		INSERT INTO MATE_LAVADO.#UbicacionesInsertadas
		SELECT SCOPE_IDENTITY()

		SET @contador +=1

	END

	SELECT * FROM MATE_LAVADO.#UbicacionesInsertadas
END
GO

-----agregarUbicacionXEspectaculo_sp-----
CREATE PROCEDURE MATE_LAVADO.agregarUbicacionXEspectaculo_sp(
@id_ubicacion INT,
@id_espectaculo INT
)
AS
BEGIN
	INSERT INTO MATE_LAVADO.UbicacionXEspectaculo(id_ubicacion, id_espectaculo)
	VALUES(@id_ubicacion, @id_espectaculo)
END
GO

-----actualizarGradoPublicacion-----
CREATE PROCEDURE MATE_LAVADO.actualizarGradoPublicacion_sp
@id_publicacion INT,
@grado VARCHAR(10)
AS
BEGIN
	IF EXISTS (SELECT descripcion FROM MATE_LAVADO.Publicaciones WHERE id_publicacion = @id_publicacion)
	BEGIN
		UPDATE MATE_LAVADO.Publicaciones 
		SET id_grado_publicacion = (SELECT id_grado_publicacion FROM MATE_LAVADO.Grados_publicacion WHERE nombre like @grado)
		WHERE id_publicacion like @id_publicacion
	END
	ELSE
	BEGIN
		RAISERROR('Publicacion inexistente', 16, 1)
	END
END
GO
-----actualizarPublicacion-----
create PROCEDURE MATE_LAVADO.actualizarPublicacion_sp (@id int, @descripcion nvarchar(255), @direccion nvarchar(80), @estado char(15), @rubro nvarchar(100)) as begin
	UPDATE MATE_LAVADO.Publicaciones set descripcion = @descripcion, direccion = @direccion, id_rubro = (select id_rubro FROM MATE_LAVADO.Rubros where descripcion = @rubro) where id_publicacion = @id
	UPDATE MATE_LAVADO.Espectaculos set estado_espectaculo = @estado where id_publicacion = @id
end
GO

-----actualizarUsuarioYContrasenia-----
create PROCEDURE MATE_LAVADO.actualizarUsuarioYContrasenia_sp (@usernameV nvarchar(255), @usernameN nvarchar(255), @encriptada nvarchar(255)) as begin
	UPDATE MATE_LAVADO.Usuarios set username = @usernameN, password = @encriptada where username = @usernameV
end
GO

-----getMediosDePago-----
CREATE PROCEDURE MATE_LAVADO.getMediosDePago_sp
@id_cliente INT
AS
BEGIN
	SELECT id_medio_de_pago, coalesce(RIGHT(nro_tarjeta, 4),0) digitos FROM MATE_LAVADO.Medios_de_pago WHERE id_cliente =  @id_cliente
END
GO

-----registrarMedioDePago-----
CREATE PROCEDURE MATE_LAVADO.registrarMedioDePago_sp
@id_cliente INT,
@numero_tarjeta INT,
@titular varchar
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM MATE_LAVADO.Medios_de_pago mp WHERE nro_tarjeta = @numero_tarjeta AND id_cliente = @id_cliente)
		BEGIN
		INSERT INTO MATE_LAVADO.Medios_de_pago(id_cliente, descripcion, nro_tarjeta, titular)
		VALUES(@id_cliente, 'Tarjeta', @numero_tarjeta, @titular)
		END
		ELSE
		BEGIN
		RAISERROR('Tarjeta ya registrada', 16, 1)
		END
END
GO

-----buscarUbicacionesPorPublicacion-----
create PROCEDURE MATE_LAVADO.buscarUbicacionesPorPublicacion_sp (@id_publicacion int) as begin
	select t.descripcion descripcion, count(distinct asiento)*count(distinct fila) asientos, sin_numerar, precio, count(distinct fila) filas, u.id_ubicacion
	FROM MATE_LAVADO.Ubicaciones u 
	JOIN MATE_LAVADO.UbicacionXEspectaculo e on e.id_ubicacion = u.id_ubicacion 
	JOIN MATE_LAVADO.TiposDeUbicacion t on t.id_tipo_ubicacion = u.codigo_tipo_ubicacion
	JOIN MATE_LAVADO.Espectaculos ee on ee.id_espectaculo = e.id_espectaculo
	JOIN MATE_LAVADO.Publicaciones p on p.id_publicacion = ee.id_publicacion
	where p.id_publicacion = 1 and e.id_compra is null
	group by t.descripcion, sin_numerar, precio, u.id_ubicacion
end
GO

-----traerTodasRazonesSociales-----
create PROCEDURE MATE_LAVADO.traerTodasRazonesSociales_sp as begin
	select distinct razon_social FROM MATE_LAVADO.Empresas
end
GO

-----top5ClientesPuntosVencidos-----
CREATE PROCEDURE MATE_LAVADO.top5ClientesPuntosVencidos_sp(
@fecha_inicio VARCHAR(30),
@fecha_fin VARCHAR(30),
@fecha_actual VARCHAR(30)
)
AS
BEGIN
	SELECT TOP 5 nombre, apellido, SUM(cantidad_puntos) 'Puntos Vencidos' FROM MATE_LAVADO.Clientes c
	JOIN MATE_LAVADO.Puntos p ON(p.id_cliente = c.id_cliente)
	WHERE fecha_vencimiento < CONVERT(DATETIME, @fecha_actual, 121) AND (fecha_vencimiento BETWEEN CONVERT(DATETIME, @fecha_inicio, 121) AND CONVERT(DATETIME, @fecha_fin, 121))
	GROUP BY nombre, apellido
	ORDER BY SUM(cantidad_puntos) DESC
END
GO


-----top5ClientesComprasParaUnaEmpresa----
CREATE PROCEDURE MATE_LAVADO.top5ClienteComprasParaUnaEmpresa_sp
(@razon_social NVARCHAR(50),
@fecha_inicio VARCHAR(30),
@fecha_fin VARCHAR(30)
)
AS
BEGIN
	SELECT TOP 5 nombre, apellido, documento, COUNT(id_ubicacion) 'Cantidad de compras', razon_social
	FROM MATE_LAVADO.Clientes cc
	JOIN MATE_LAVADO.Compras c ON(c.id_cliente = cc.id_cliente)
	JOIN MATE_LAVADO.UbicacionXEspectaculo uxe ON(uxe.id_compra = c.id_compra)
	JOIN MATE_LAVADO.Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
	JOIN MATE_LAVADO.Publicaciones p ON (p.id_publicacion = e.id_publicacion)
	JOIN MATE_LAVADO.Empresas ee ON(p.id_empresa = ee.id_empresa)

	WHERE c.fecha > CONVERT(DATETIME, @fecha_inicio, 121) AND fecha < CONVERT(DATETIME, @fecha_fin, 121)  AND ee.razon_social = @razon_social
	GROUP BY ee.id_empresa, razon_social, nombre, apellido, documento
	ORDER BY COUNT(id_ubicacion) DESC
END
GO


-----top5LocalidadesNoVendidasEmpresa-----
CREATE PROCEDURE MATE_LAVADO.top5EmpresasLocalidadesNoVendidas_sp
@grado VARCHAR(20),
@fecha_inicio VARCHAR(30),
@fecha_fin VARCHAR(30)
AS
BEGIN
	SELECT TOP 5 razon_social 'Razon social', cuit, COUNT(id_ubicacion) 'Ubicaciones no vendidas' FROM MATE_LAVADO.Publicaciones p
	JOIN MATE_LAVADO.Espectaculos e ON (e.id_publicacion = p.id_publicacion)
	JOIN MATE_LAVADO.UbicacionXEspectaculo uxe ON(uxe.id_espectaculo = e.id_espectaculo)
	JOIN MATE_LAVADO.Grados_publicacion gp ON(gp.id_grado_publicacion = p.id_grado_publicacion)
	JOIN MATE_LAVADO.Empresas emp ON(emp.id_empresa = p.id_empresa)
	WHERE uxe.id_compra IS NULL
		AND gp.nombre = @grado
		AND e.fecha_evento> CONVERT(DATETIME, @fecha_inicio, 121) AND e.fecha_evento < CONVERT(DATETIME, @fecha_fin, 121)
	GROUP BY razon_social, cuit, p.id_publicacion, fecha_evento, comision
	ORDER BY fecha_evento ASC, comision DESC
END
GO



-----agregarFactura-----
create PROCEDURE MATE_LAVADO.agregarFactura_sp (@razonSocial NVARCHAR(255), @total NUMERIC(18,2)) as begin
	declare @id_empresa int
	set @id_empresa = (select id_empresa FROM MATE_LAVADO.Empresas where razon_social = @razonSocial)
	INSERT INTO MATE_LAVADO.Facturas (id_empresa, fecha_facturacion, importe_total) values (@id_empresa, getdate(), @total)

	select top 1 id_factura FROM MATE_LAVADO.Facturas where id_empresa = @id_empresa and importe_total = @total order by fecha_facturacion desc
end
GO

-----actualizarCompraFactura-----
create PROCEDURE MATE_LAVADO.actualizarCompraFactura_sp (@id_factura int, @id_compra int) as begin
	UPDATE MATE_LAVADO.Compras set id_factura = @id_factura where id_compra = @id_compra
end
GO

-----buscarComprasNoFacturadas-----
create PROCEDURE MATE_LAVADO.buscarComprasNoFacturadas_sp (@razonSocial varchar(255)) as begin
	select c.id_compra, descripcion, coalesce(comision, 0) comision, coalesce(importe, 0) importe FROM MATE_LAVADO.Compras c 
	JOIN MATE_LAVADO.UbicacionXEspectaculo u on c.id_compra = u.id_compra
	JOIN MATE_LAVADO.Espectaculos es on es.id_espectaculo = u.id_espectaculo
	JOIN MATE_LAVADO.Publicaciones p on p.id_publicacion = es.id_publicacion
	JOIN MATE_LAVADO.Empresas e on e.id_empresa = p.id_empresa and e.razon_social = @razonSocial
	where id_factura is null 
end
GO

----------TRIGGERS----------

-----insertarNuevoEspectaculo-----
CREATE TRIGGER MATE_LAVADO.insertarNuevoEspectaculo ON MATE_LAVADO.Espectaculos
INSTEAD OF INSERT
AS
BEGIN
DECLARE @id_publicacion INT, @fecha_inicio DATETIME, @fecha_evento DATETIME, @estado_espectaculo CHAR(15) 
DECLARE cur CURSOR FOR 
SELECT id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo FROM MATE_LAVADO.inserted
DECLARE @last_id INT
SET @last_id = (SELECT MAX(id_espectaculo) FROM MATE_LAVADO.Espectaculos) + 1
OPEN cur
FETCH NEXT FROM cur INTO @id_publicacion, @fecha_inicio, @fecha_evento, @estado_espectaculo
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO MATE_LAVADO.Espectaculos(id_espectaculo, id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo)
VALUES (@last_id, @id_publicacion, @fecha_inicio, @fecha_evento, @estado_espectaculo)
SET @last_id += 1
FETCH NEXT FROM cur INTO @id_publicacion, @fecha_inicio, @fecha_evento, @estado_espectaculo
END
CLOSE cur
DEALLOCATE cur
END
GO

-----insertarNuevaFactura-----
CREATE TRIGGER MATE_LAVADO.insertarNuevaFactura ON MATE_LAVADO.Facturas
INSTEAD OF INSERT
AS
BEGIN
DECLARE @fecha_facturacion DATETIME, @importe_total NUMERIC(18,2), @id_empresa INT 
DECLARE cur CURSOR FOR 
SELECT fecha_facturacion, importe_total, id_empresa FROM MATE_LAVADO.inserted
DECLARE @last_id INT
SET @last_id = (SELECT MAX(id_factura) FROM MATE_LAVADO.Facturas) + 1
OPEN cur
FETCH NEXT FROM cur INTO @fecha_facturacion, @importe_total, @id_empresa
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO MATE_LAVADO.Facturas(id_factura, fecha_facturacion, importe_total, id_empresa)
VALUES (@last_id, @fecha_facturacion, @importe_total, @id_empresa)
SET @last_id += 1
FETCH NEXT FROM cur INTO @fecha_facturacion, @importe_total, @id_empresa
END
CLOSE cur
DEALLOCATE cur
END
GO

-----rollInhabilitado-----
CREATE TRIGGER MATE_LAVADO.rolInhabilitado_tr
ON MATE_LAVADO.Roles
AFTER UPDATE
AS
BEGIN	
	IF((SELECT habilitado FROM MATE_LAVADO.DELETED) <> (SELECT habilitado FROM MATE_LAVADO.INSERTED))
	BEGIN
		DECLARE @id_rol_modificado INT
		SET @id_rol_modificado = (SELECT id_rol FROM MATE_LAVADO.DELETED)

		DELETE FROM MATE_LAVADO.UsuarioXRol
		WHERE id_rol = @id_rol_modificado

	END
END
GO

-----insertarNuevaCompra-----
CREATE TRIGGER MATE_LAVADO.insertarNuevaCompra ON MATE_LAVADO.Compras
INSTEAD OF INSERT
AS
BEGIN
	DECLARE @id_cliente INT, @id_medio_de_pago INT, @fecha DATETIME, @importe BIGINT
	DECLARE cur CURSOR FOR 
	SELECT id_cliente, id_medio_de_pago, fecha, importe FROM MATE_LAVADO.inserted
	DECLARE @last_id INT
	SET @last_id = (SELECT MAX(id_compra) FROM MATE_LAVADO.Compras) + 1
	OPEN cur
		FETCH NEXT FROM cur INTO @id_cliente, @id_medio_de_pago, @fecha, @importe
		WHILE @@FETCH_STATUS = 0
		BEGIN
		INSERT INTO MATE_LAVADO.Compras(id_cliente, id_medio_de_pago, id_factura, fecha, importe, id_compra)
		VALUES (@id_cliente, @id_medio_de_pago, NULL, @fecha, @importe, @last_id)

		INSERT INTO Puntos(cantidad_puntos, id_cliente, fecha_vencimiento)
		VALUES(@importe, @id_cliente, DATEADD(year, 1, @fecha))

		SET @last_id += 1
		FETCH NEXT FROM cur INTO @id_cliente, @id_medio_de_pago, @fecha, @importe
		END
	CLOSE cur
	DEALLOCATE cur
	select @last_id-1 id
END
GO


-----finalizarEspectaculo-----
CREATE TRIGGER MATE_LAVADO.finalizarEspectaculoAgotado_tg
ON MATE_LAVADO.Compras
AFTER INSERT
AS
BEGIN
	DECLARE @id_espectaculo INT = (SELECT DISTINCT e.id_espectaculo FROM MATE_LAVADO.Espectaculos e
								JOIN MATE_LAVADO.UbicacionXEspectaculo uxe ON(uxe.id_espectaculo = e.id_espectaculo)
								WHERE uxe.id_compra = (SELECT id_compra FROM INSERTED))
	--DECLARE @id_publicacion INT = (SELECT id_publicacion FROM Espectaculos WHERE id_espectaculo = @id_espectaculo)
	IF (MATE_LAVADO.getCantidadEntradasEspectaculo(@id_espectaculo) = MATE_LAVADO.getCantidadEntradasVendidas(@id_espectaculo))
	BEGIN
		UPDATE MATE_LAVADO.Espectaculos
		SET estado_espectaculo = 'Finalizada'
		WHERE id_espectaculo = @id_espectaculo
	END
END
GO


----------FUNCIONES----------

-----getCantidadEntradasPublicacion-----
CREATE FUNCTION MATE_LAVADO.getCantidadEntradasEspectaculo(@id_publicacion INT)
RETURNS INT
AS
BEGIN
	RETURN (SELECT COUNT(DISTINCT id_ubicacion_espectaculo) FROM MATE_LAVADO.UbicacionXEspectaculo uxe
			JOIN MATE_LAVADO.Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
			WHERE e.id_publicacion = @id_publicacion)
END
GO

-----getCantidadEntradasVendidas-----
CREATE FUNCTION MATE_LAVADO.getCantidadEntradasVendidas(@id_espectaculo INT)
RETURNS INT
AS
BEGIN
	RETURN (SELECT COUNT(DISTINCT id_ubicacion_espectaculo) FROM MATE_LAVADO.UbicacionXEspectaculo uxe
			WHERE id_espectaculo = @id_espectaculo AND id_compra IS NOT NULL)
END
GO

-----vaciarEspectaculosPublicacion-----
CREATE PROCEDURE MATE_LAVADO.vaciarEspectaculosPublicacion_sp(
@id_publicacion INT)
AS
BEGIN

CREATE TABLE #UbicacionesDeUnaPublicacion(
id_espectaculo INT,
id_ubicacion INT,
id_ubicacion_espectaculo INT
)

INSERT INTO MATE_LAVADO.#UbicacionesDeUnaPublicacion(id_espectaculo, id_ubicacion, id_ubicacion_espectaculo)
SELECT e.id_espectaculo, u.id_ubicacion, uxe.id_ubicacion_espectaculo FROM MATE_LAVADO.Espectaculos e
JOIN MATE_LAVADO.UbicacionXEspectaculo uxe ON (e.id_espectaculo = uxe.id_espectaculo)
JOIN MATE_LAVADO.Ubicaciones u ON (u.id_ubicacion = uxe.id_ubicacion)
WHERE e.id_publicacion = @id_publicacion

DELETE UbicacionXEspectaculo
WHERE id_ubicacion_espectaculo IN (SELECT id_ubicacion_espectaculo FROM MATE_LAVADO.#UbicacionesDeUnaPublicacion) OR id_ubicacion IN (SELECT id_ubicacion FROM MATE_LAVADO.#UbicacionesDeUnaPublicacion) OR id_espectaculo IN (SELECT id_espectaculo FROM MATE_LAVADO.#UbicacionesDeUnaPublicacion)

DELETE Ubicaciones 
WHERE id_ubicacion IN (SELECT id_ubicacion FROM MATE_LAVADO.#UbicacionesDeUnaPublicacion)

DELETE Espectaculos 
WHERE id_espectaculo IN (SELECT id_espectaculo FROM MATE_LAVADO.#UbicacionesDeUnaPublicacion)

DROP TABLE MATE_LAVADO.#UbicacionesDeUnaPublicacion

END
GO

CREATE PROCEDURE MATE_LAVADO.filasDisponiblesSegunEspectaculo_sp
@id_espectaculo INT,
@precio INT
AS
BEGIN
	SELECT fila
	FROM MATE_LAVADO.UbicacionXEspectaculo uxe
	JOIN MATE_LAVADO.Ubicaciones u ON (u.id_ubicacion = uxe.id_ubicacion)
	WHERE uxe.id_espectaculo = @id_espectaculo AND id_compra is NULL AND @precio = precio
	GROUP BY fila
END
GO

CREATE PROCEDURE MATE_LAVADO.asientosDisponiblesSegunEspectaculoYFila_sp
@id_espectaculo INT,
@fila CHAR,
@precio INT
AS
BEGIN
	SELECT asiento
	FROM MATE_LAVADO.UbicacionXEspectaculo uxe
	JOIN MATE_LAVADO.Ubicaciones u ON (u.id_ubicacion = uxe.id_ubicacion)
	WHERE uxe.id_espectaculo = @id_espectaculo AND id_compra is NULL AND fila = @fila AND @precio = precio
	GROUP BY asiento
END
GO