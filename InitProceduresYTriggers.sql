--.--.--.--.--.--.--.--PROCEDURES--.--.--.--.--.--.--.--
-----DROP PROCEDURES-----
DROP PROCEDURE verificarLogin_sp
DROP PROCEDURE getRolesDeUsuario_sp
DROP PROCEDURE getFuncionalidadesDeUsuario_sp
DROP PROCEDURE getRubrosDePublicacion_sp 
DROP PROCEDURE buscarUsuarioPorCriterio_sp
DROP PROCEDURE modificarCliente_sp
DROP PROCEDURE buscarClientePorUsername_sp
DROP PROCEDURE getPuntos_sp
DROP PROCEDURE getRubros_sp
DROP PROCEDURE buscarPublicacionesPorCriterio_sp
DROP PROCEDURE registroCliente_sp
DROP PROCEDURE registroEmpresa_sp
DROP PROCEDURE getPremios_sp
DROP PROCEDURE borrarPuntos_sp
DROP PROCEDURE historialClienteConOffset_sp
DROP PROCEDURE registrarCompra_sp
DROP PROCEDURE registrarCompraExU_sp
DROP PROCEDURE getPublicacionesDeUsuario_sp
DROP PROCEDURE registrarPublicacion_sp
DROP PROCEDURE agregarEspectaculo_sp
DROP PROCEDURE agregarUbicaciones_sp
DROP PROCEDURE agregarUbicacionSinNumerar_sp
DROP PROCEDURE agregarUbicacionNumerada_sp
DROP PROCEDURE actualizarGradoPublicacion_sp
DROP PROCEDURE buscarEspectaculosPorPublicacion_sp
DROP PROCEDURE getMediosDePago_sp
DROP PROCEDURE registrarMedioDePago_sp
DROP PROCEDURE buscarUbicacionesPorPublicacion_sp
DROP PROCEDURE buscarEmpresaPorCriterio_sp
DROP PROCEDURE modificarEmpresa_sp
DROP PROCEDURE agregarRol_sp
DROP PROCEDURE getFuncionalidadesDeRol_sp
DROP PROCEDURE eliminarFuncionalidadesRol_sp
DROP PROCEDURE agregarFuncionalidadARol_sp
DROP PROCEDURE modificarNombreRol_sp
DROP PROCEDURE modificarRol_sp
DROP PROCEDURE top5ClientesPuntosVencidos_sp
DROP PROCEDURE traerTodasRazonesSociales_sp
DROP PROCEDURE top5ClienteComprasParaUnaEmpresa_sp
DROP PROCEDURE top5EmpresasLocalidadesNoVendidas_sp
DROP PROCEDURE buscarComprasNoFacturadas_sp
DROP PROCEDURE actualizarCompraFactura_sp
DROP PROCEDURE agregarFactura_sp
DROP PROCEDURE actualizarPublicacion_sp
DROP PROCEDURE actualizarUsuarioYContrasenia_sp
DROP PROCEDURE agregarUbicacionXEspectaculo_sp
DROP PROCEDURE buscarEmpresaPorUsername_sp
DROP PROCEDURE buscarPublicacionesPorEmpresa_sp
DROP PROCEDURE vaciarEspectaculosPublicacion_sp
DROP PROCEDURE filasDisponiblesSegunEspectaculo_sp
DROP PROCEDURE asientosDisponiblesSegunEspectaculoYFila_sp

DROP TRIGGER insertarNuevoEspectaculo
DROP TRIGGER insertarNuevaFactura
DROP TRIGGER rolInhabilitado_tr
DROP TRIGGER insertarNuevaCompra
DROP TRIGGER finalizarEspectaculoAgotado_tg

DROP FUNCTION getCantidadEntradasEspectaculo
DROP FUNCTION getCantidadEntradasVendidas
GO
-----CREAR PROCEDURES-----
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
GO

-----getPremios-----
CREATE PROCEDURE getPremios_sp
AS
BEGIN
	SELECT descripcion, puntos FROM Premios
END
GO

-----borrarPuntos-----
CREATE PROCEDURE borrarPuntos_sp
@cantidad int,
@username varchar(30)
AS
BEGIN
	DECLARE @puntos INT
	DECLARE @fecha_vencimiento DATETIME
	DECLARE @id_punto INT
	DECLARE @id_cliente int = (SELECT id_cliente FROM Clientes c JOIN Usuarios u ON (u.id_usuario = c.id_usuario)
											 WHERE username = @username)
	

	DECLARE administradorPuntos CURSOR FOR 
	SELECT id_punto, cantidad_puntos, fecha_vencimiento FROM Puntos p
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
				FROM Puntos
				WHERE id_punto = @id_punto
			END
			ELSE 
			BEGIN
				UPDATE Puntos 
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
CREATE PROCEDURE getRolesDeUsuario_sp
@usuario VARCHAR(50)
AS
BEGIN
	SELECT DISTINCT nombre FROM Roles r
	JOIN Usuarios u ON(u.username = @usuario)
	JOIN UsuarioXRol uxr ON(uxr.id_usuario = u.id_usuario AND uxr.id_rol = r.id_rol)
END
GO

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
GO


-----eliminarFuncionalidadDeRol-----
CREATE PROCEDURE eliminarFuncionalidadesRol_sp
@rol_nombre VARCHAR(50)
AS
BEGIN
	DECLARE @id_rol INT = (SELECT id_rol FROM Roles WHERE nombre = @rol_nombre)

	DELETE FROM FuncionalidadXRol
	WHERE id_rol = @id_rol

END
GO

-----modificarRol-----
CREATE PROCEDURE modificarRol_sp(
@nombre VARCHAR(50),
@habilitado BIT)
AS
BEGIN
	IF EXISTS (SELECT nombre FROM Roles WHERE nombre = @nombre)
	BEGIN
		UPDATE Roles
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
CREATE PROCEDURE modificarNombreRol_sp
@nombre_viejo VARCHAR(50),
@nombre_nuevo VARCHAR(50)
AS
BEGIN
	IF EXISTS (SELECT nombre FROM Roles WHERE nombre = @nombre_viejo)
	BEGIN
		UPDATE Roles
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
CREATE PROCEDURE getRubrosDePublicacion_sp 
@id_publicacion NUMERIC
AS
BEGIN
	SELECT r.id_rubro, r.descripcion FROM Rubros r JOIN Publicaciones p ON (r.id_rubro = p.id_rubro)
	WHERE id_publicacion = @id_publicacion
END
GO

-----agregarFuncionalidadARol-----
CREATE PROCEDURE AgregarFuncionalidadARol_sp
@nombre_rol VARCHAR(50),
@nombre_funcionalidad VARCHAR(50)
AS
BEGIN
	DECLARE @id_rol INT, @id_funcionalidad INT
	SET @id_rol = (SELECT id_rol FROM Roles WHERE nombre = @nombre_rol)
	SET @id_funcionalidad = (SELECT id_funcionalidad FROM Funcionalidades WHERE nombre = @nombre_funcionalidad)

	IF NOT EXISTS (SELECT nombre FROM Roles WHERE nombre = @nombre_rol)
		BEGIN
		RAISERROR('Rol inexistente', 20, 1) WITH LOG
		END

	IF NOT EXISTS (SELECT nombre FROM Funcionalidades WHERE nombre = @nombre_funcionalidad)
		BEGIN
		RAISERROR('Funcionalidad inexistente', 20, 1) WITH LOG
		END

	IF NOT EXISTS (SELECT id_rol, id_funcionalidad FROM FuncionalidadXRol
					WHERE id_rol = @id_rol AND id_funcionalidad = @id_funcionalidad)
		BEGIN
			INSERT INTO FuncionalidadXRol(id_funcionalidad, id_rol)
			VALUES (@id_funcionalidad, @id_rol)
		END
	
	RAISERROR('Funcionalidad ya existente para ese rol', 16, 1) --no es grave lol podria tb no hacer nada
END
GO

-----buscarUsuarioPorCriterio-----
CREATE PROCEDURE buscarUsuarioPorCriterio_sp
@nombre VARCHAR(255),
@apellido VARCHAR(255),
@dni VARCHAR(18),
@email NVARCHAR(50)
AS
BEGIN
	SELECT id_cliente, nombre, apellido, coalesce(cuil,0) cuil, mail, coalesce(telefono,0) telefono, tipo_documento, fecha_nacimiento,
		fecha_creacion, coalesce(documento,0) documento, calle, coalesce(numero_calle,0) numero_calle, codigo_postal, depto, piso
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
GO

-----modificarCliente-----
CREATE PROCEDURE modificarCliente_sp
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
	IF EXISTS (SELECT * FROM dbo.Clientes WHERE id_cliente = @id_cliente) 
		BEGIN
		BEGIN TRANSACTION
		UPDATE dbo.Clientes
		SET nombre = @nombre, apellido = @apellido, mail = @mail, documento = @documento, cuil = @cuil, telefono = @telefono, fecha_creacion = @fecha_nacimiento
		COMMIT TRANSACTION
		END
	ELSE
	RAISERROR('El cliente no existe', 20, 1) WITH LOG
END
GO

-----registroCliente-----
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
	IF NOT EXISTS (SELECT * FROM dbo.Usuarios u JOIN dbo.Clientes c ON (u.id_usuario = c.id_usuario)
					WHERE username = @username OR cuil = @cuil OR documento = @documento OR mail = @mail) 
		BEGIN
		BEGIN TRANSACTION
		INSERT INTO dbo.Usuarios(username, password, habilitado, alta_logica, intentos_fallidos, debe_cambiar_pass) VALUES (@username, @password, '1', CONVERT(DATETIME, @fecha_creacion, 120), 0, @cambio_pass)
		INSERT INTO dbo.Clientes(id_usuario, nombre, apellido, tipo_documento, documento, cuil, mail, telefono, fecha_creacion, fecha_nacimiento,
			calle, numero_calle, piso, depto, codigo_postal)
		VALUES ((SELECT id_usuario FROM dbo.Usuarios WHERE username like @username), @nombre, @apellido, @tipo_documento, @documento, @cuil, @mail,
			@telefono, CONVERT(DATETIME, @fecha_creacion, 120), CONVERT(DATETIME, @fecha_nacimiento, 120), @calle, @numero_calle, @piso, @depto, @codigo_postal)
		INSERT INTO dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM dbo.Usuarios WHERE username like @username), 3)
		COMMIT TRANSACTION
		END
	ELSE
	RAISERROR('El Cliente ya existe', 20, 1) WITH LOG
END
GO

-----registroEmpresa-----
CREATE PROCEDURE registroEmpresa_sp(@username VARCHAR(255), @password VARCHAR(255),  @razon_social nvarchar(255), @mail nvarchar(50), 
 @cuit nvarchar(255), @calle nvarchar(50), @numero_calle NUMERIC(18,0), @piso NUMERIC(18,0), @depto nvarchar(50), @codigo_postal nvarchar(50), @cambio_pass BIT, @fecha_creacion VARCHAR(30))
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM dbo.Usuarios u JOIN dbo.Empresas e ON (u.id_usuario = e.id_usuario) 
	WHERE username = @username OR cuit = @cuit OR mail = @mail OR razon_social = @razon_social)
	BEGIN
		BEGIN TRANSACTION
		INSERT INTO dbo.Usuarios(username, password, habilitado, alta_logica, intentos_fallidos, debe_cambiar_pass) VALUES (@username, @password, '1',
			CONVERT(DATETIME, @fecha_creacion, 120)
			--CAST(@fecha_creacion AS DATETIME)
			, 0, @cambio_pass)
		INSERT INTO dbo.Empresas(id_usuario, razon_social, mail, cuit, fecha_creacion, calle, numero_calle, piso, depto, codigo_postal)
		VALUES ((SELECT id_usuario FROM dbo.Usuarios WHERE username like @username), @razon_social, @mail, @cuit, CONVERT(DATETIME, @fecha_creacion, 120),
			@calle, @numero_calle, @piso, @depto, @codigo_postal)
		INSERT INTO dbo.UsuarioXRol(id_usuario, id_rol) VALUES((SELECT id_usuario FROM dbo.Usuarios WHERE username like @username), 2)
		COMMIT TRANSACTION
	END
	ELSE
		RAISERROR( 'La empresa ya existe',20,1) WITH LOG
END
GO

-----buscarClientePorUsername-----
CREATE PROCEDURE buscarClientePorUsername_sp
@username VARCHAR(255)
AS
BEGIN
	SELECT * FROM Usuarios u JOIN Clientes c ON (u.id_usuario = c.id_usuario)
	WHERE username LIKE @username
END
GO

-----getPuntos-----
CREATE PROCEDURE getPuntos_sp
@username varchar(30),
@fecha VARCHAR(30)
AS
BEGIN
	SELECT COALESCE(SUM(cantidad_puntos), 0) 'cantidad_puntos' FROM Usuarios u JOIN Clientes c ON (u.id_usuario = c.id_usuario)
								  JOIN Puntos p ON (p.id_cliente = c.id_cliente)
	WHERE username = @username AND fecha_vencimiento IS NOT NULL AND fecha_vencimiento > CONVERT(DATETIME, @fecha, 121)

END
GO

-----getRubros-----
CREATE PROCEDURE getRubros_sp
AS
BEGIN
	SELECT descripcion FROM Rubros WHERE descripcion <> ''
END
GO

-----buscarPublicacionesPorEmpresa-----
create procedure buscarPublicacionesPorEmpresa_sp (@razon_social nvarchar(255)) as begin
	select g.nombre grado, r.descripcion rubro, p.descripcion, direccion, p.id_publicacion id, es.estado_espectaculo estado from Publicaciones p 
	join Empresas e on e.id_empresa = p.id_empresa 
	join Grados_publicacion g on p.id_grado_publicacion = g.id_grado_publicacion
	join Rubros r on r.id_rubro = p.id_rubro
	join Espectaculos es on es.id_publicacion = p.id_publicacion
	where razon_social = @razon_social and estado_espectaculo = 'Borrador'
	group by p.id_publicacion, p.descripcion, direccion, g.nombre, r.descripcion, es.estado_espectaculo
end
GO

-----historialClienteConOffset-----
create procedure historialClienteConOffset_sp (@id_cliente int, @offset int) as begin
	select p.descripcion, e.fecha_evento, coalesce(c.importe, 0) importe, COUNT(uxe.id_ubicacion_espectaculo) 'cantidad_asientos', coalesce(RIGHT(mp.nro_tarjeta, 4),0) nro
	from  Compras c
	join UbicacionXEspectaculo uxe on uxe.id_compra = c.id_compra
	join Espectaculos e on e.id_espectaculo = uxe.id_espectaculo
	join Publicaciones p on p.id_publicacion = e.id_publicacion
	join Medios_de_pago mp on mp.id_medio_de_pago = c.id_medio_de_pago
	where c.id_cliente = 768
	group by c.id_compra, p.descripcion, e.fecha_evento, c.importe, mp.descripcion, mp.nro_tarjeta
	order by e.fecha_evento offset @offset rows fetch next 10 rows only
end
GO

-----registrarCompra-----
CREATE PROCEDURE registrarCompra_sp
@id_cliente INT,
@id_medio_de_pago INT,
@importe BIGINT,
@fecha VARCHAR(30)
AS
BEGIN
	SET IDENTITY_INSERT Compras ON
	INSERT INTO Compras(id_cliente, id_medio_de_pago, id_factura, fecha, importe)
	VALUES(@id_cliente, @id_medio_de_pago, NULL, CONVERT(DATETIME, @fecha, 121), @importe)
	SET IDENTITY_INSERT Compras OFF
END
go

-----registrarCompraEXU-----
CREATE PROCEDURE registrarCompraExU_sp (@id_compra INT, @id_ubicacion INT, @id_espectaculo BIGINT)
AS
BEGIN
	declare @id_ubicacion_espectaculo int
	set @id_ubicacion_espectaculo = (select id_ubicacion_espectaculo from UbicacionXEspectaculo where id_ubicacion = @id_ubicacion and id_espectaculo = @id_espectaculo)
	
	UPDATE UbicacionXEspectaculo
	SET id_compra = @id_compra
	WHERE id_ubicacion_espectaculo = @id_ubicacion_espectaculo
END
GO

-----getPublicacionesDeUsuario-----
CREATE PROCEDURE getPublicacionesDeUsuario_sp
@usuario VARCHAR(50)
AS
BEGIN
	SELECT id_publicacion, descripcion, direccion FROM Publicaciones p
	JOIN Empresas e ON(e.id_empresa = p.id_empresa)
	JOIN Usuarios u ON (e.id_usuario = u.id_usuario)
	WHERE username = @usuario
END
GO

-----buscarEmpresaPorCriterio-----
CREATE PROCEDURE buscarEmpresaPorCriterio_sp
@cuit VARCHAR(20),
@razon_social VARCHAR(20),
@email VARCHAR(20)
AS
BEGIN
	SELECT razon_social, mail, coalesce(cuit,null) cuit, mail, calle, numero_calle, piso, depto, fecha_creacion, codigo_postal  FROM Empresas
	WHERE (razon_social LIKE '%' + @razon_social + '%'
		AND mail LIKE '%' + @email + '%'
		AND cuit = @cuit)
		OR (razon_social LIKE '%' + @razon_social + '%'
		AND mail LIKE '%' + @email + '%'
		AND @cuit LIKE '')
END
GO

-----buscarEmpresaPorUsername-----
CREATE PROCEDURE buscarEmpresaPorUsername_sp
@username VARCHAR(255)
AS
BEGIN
	SELECT * FROM Usuarios u JOIN Empresas e ON (u.id_usuario = e.id_usuario)
	WHERE username LIKE @username
END
GO

-----modificarEmpresa-----
CREATE PROCEDURE modificarEmpresa_sp
@cuit_viejo varchar(20),
@razon_social varchar(20),
@mail varchar(20),
@cuit varchar(20)
AS
BEGIN
	IF EXISTS (SELECT cuit FROM Empresas WHERE cuit = @cuit_viejo)
	BEGIN
		IF(@cuit = @cuit_viejo)
			BEGIN
			UPDATE Empresas
				SET razon_social = @razon_social,
					mail = @mail,
					cuit = @cuit
				WHERE cuit like @cuit_viejo
			END
			ELSE
				IF NOT EXISTS (SELECT cuit FROM Empresas where cuit like @cuit)
				BEGIN
					UPDATE Empresas
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
CREATE PROCEDURE agregarRol_sp 
@nombre_rol VARCHAR(50)
AS
BEGIN
	IF NOT EXISTS (SELECT nombre FROM Roles WHERE nombre = @nombre_rol)
		BEGIN
		INSERT INTO Roles(nombre, habilitado)
		VALUES(@nombre_rol, 1)		
		END
	ELSE
		BEGIN
		RAISERROR('Este Rol ya existe', 16, 1)
		END
END
GO

-----getFuncionalidadesDeRol-----
CREATE PROCEDURE getFuncionalidadesDeRol_sp
@nombre_rol VARCHAR(50)
AS
BEGIN
	SELECT DISTINCT f.nombre FROM Funcionalidades f
	JOIN Roles r ON (r.nombre = @nombre_rol)
	JOIN FuncionalidadXRol fxr ON(fxr.id_rol= r.id_rol AND fxr.id_funcionalidad = f.id_funcionalidad)
END
GO

-----registrarPublicacion-----
CREATE PROCEDURE registrarPublicacion_sp
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
	DECLARE @id_empresa INT = (SELECT id_empresa FROM Empresas e WHERE @nombre_empresa = razon_social)
	DECLARE @id_grado_publicacion INT = (SELECT id_grado_publicacion FROM Grados_publicacion WHERE nombre = @grado_publicacion)
	DECLARE @id_rubro INT =  (SELECT id_rubro FROM Rubros WHERE descripcion = @rubro)
	INSERT INTO Publicaciones(id_empresa, id_grado_publicacion, id_rubro, descripcion, direccion)
	VALUES (@id_empresa, @id_grado_publicacion, @id_rubro, @descripcion, @direccion)

	SELECT SCOPE_IDENTITY() AS 'id_publicacion'
END
GO

-----buscarEspectaculosPorPublicacion-----
create procedure buscarEspectaculosPorPublicacion_sp (@id_publicacion int) as begin
	select fecha_evento, id_espectaculo from Espectaculos where id_publicacion = @id_publicacion
end
GO

-----buscarPublicacionesPorCriterio_sp-----
create procedure buscarPublicacionesPorCriterio_sp (@descripcion varchar(255), @categorias varchar(255), @desde datetime, @hasta datetime, @offset INT) as begin
	declare @query nvarchar(2000)
	set @query = 
	'select p.descripcion descripcion, r.descripcion rubro, direccion, p.id_publicacion id from Publicaciones p join Espectaculos e on p.id_publicacion = e.id_publicacion 
	join Rubros r on r.id_rubro = p.id_rubro
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
CREATE PROCEDURE agregarEspectaculo_sp(
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

	IF EXISTS (SELECT * FROM Espectaculos WHERE fecha_evento =  CONVERT(DATETIME, @fecha, 120) AND @id_publicacion = id_publicacion)
		BEGIN
		RAISERROR('No pueden existir dos funciones para una publicacion con la misma fecha', 16, 1)
		END

	INSERT INTO Espectaculos(id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo)
	VALUES(@id_publicacion,  CONVERT(DATETIME, @fecha_creacion, 120),  CONVERT(DATETIME, @fecha, 120), @estado_publicacion)

	SELECT MAX(id_espectaculo) AS id_espectaculo FROM Espectaculos
END
GO


-----agregarUbicacionNumerada-----
CREATE PROCEDURE agregarUbicacionNumerada_sp(
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

	IF NOT EXISTS (SELECT * FROM TiposDeUbicacion WHERE descripcion = @tipo_ubicacion)
		BEGIN
			INSERT INTO TiposDeUbicacion(id_tipo_ubicacion, descripcion) values (
			(SELECT MAX(id_tipo_ubicacion)+1 FROM TiposDeUbicacion), @tipo_ubicacion)
		END

	WHILE(@contador_filas < @filas)
	BEGIN
		
		DECLARE @contador_asientos INT = 0

		WHILE(@contador_asientos < @asientos_por_fila)
		BEGIN

			INSERT INTO Ubicaciones(fila, asiento, sin_numerar, precio, codigo_tipo_ubicacion)
			SELECT CHAR(ASCII('A')+@contador_filas), @contador_asientos, 0, @precio, id_tipo_ubicacion 
			FROM TiposDeUbicacion WHERE descripcion = @tipo_ubicacion

			INSERT INTO #UbicacionesInsertadas
			SELECT SCOPE_IDENTITY()

			SET @contador_asientos += 1
		END

		SET @contador_filas +=1
	END

	SELECT * FROM #UbicacionesInsertadas
END
GO

-----agregarUbicacionSinNumerar-----
CREATE PROCEDURE agregarUbicacionSinNumerar_sp(
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

		IF NOT EXISTS (SELECT * FROM TiposDeUbicacion WHERE descripcion = @tipo_ubicacion)
		BEGIN
			INSERT INTO TiposDeUbicacion(id_tipo_ubicacion, descripcion) values(
			(SELECT MAX(id_tipo_ubicacion) + 1 FROM TiposDeUbicacion), @tipo_ubicacion)
		END

		INSERT INTO Ubicaciones(fila, asiento, sin_numerar, precio, codigo_tipo_ubicacion)
		SELECT NULL, NULL, 1, @precio, id_tipo_ubicacion
		FROM TiposDeUbicacion WHERE descripcion = @tipo_ubicacion

		INSERT INTO #UbicacionesInsertadas
		SELECT SCOPE_IDENTITY()

		SET @contador +=1

	END

	SELECT * FROM #UbicacionesInsertadas
END
GO


-----agregarUbicaciones-----
CREATE PROCEDURE agregarUbicaciones_sp(
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
		EXEC agregarUbicacionNumerada_sp @tipo_ubicacion, @cantidad, @filas, @precio
	END
	ELSE
	BEGIN
		EXEC agregarUbicacionSinNumerar_sp @tipo_ubicacion, @cantidad, @precio
	END
 	DROP TABLE #UbicacionesInsertadas
END
GO

-----agregarUbicacionXEspectaculo_sp-----
CREATE PROCEDURE agregarUbicacionXEspectaculo_sp(
@id_ubicacion INT,
@id_espectaculo INT
)
AS
BEGIN
	INSERT INTO UbicacionXEspectaculo(id_ubicacion, id_espectaculo)
	VALUES(@id_ubicacion, @id_espectaculo)
END
GO

-----actualizarGradoPublicacion-----
CREATE PROCEDURE actualizarGradoPublicacion_sp
@id_publicacion INT,
@grado VARCHAR(10)
AS
BEGIN
	IF EXISTS (SELECT descripcion FROM Publicaciones WHERE id_publicacion = @id_publicacion)
	BEGIN
		UPDATE Publicaciones 
		SET id_grado_publicacion = (SELECT id_grado_publicacion FROM Grados_publicacion WHERE nombre like @grado)
		WHERE id_publicacion like @id_publicacion
	END
	ELSE
	BEGIN
		RAISERROR('Publicacion inexistente', 16, 1)
	END
END
GO
-----actualizarPublicacion-----
create procedure actualizarPublicacion_sp (@id int, @descripcion nvarchar(255), @direccion nvarchar(80), @estado char(15), @rubro nvarchar(100)) as begin
	update Publicaciones set descripcion = @descripcion, direccion = @direccion, id_rubro = (select id_rubro from Rubros where descripcion = @rubro) where id_publicacion = @id
	update Espectaculos set estado_espectaculo = @estado where id_publicacion = @id
end
GO

-----actualizarUsuarioYContrasenia-----
create procedure actualizarUsuarioYContrasenia_sp (@usernameV nvarchar(255), @usernameN nvarchar(255), @encriptada nvarchar(255)) as begin
	update Usuarios set username = @usernameN, password = @encriptada where username = @usernameV
end
GO

-----getMediosDePago-----
CREATE PROCEDURE getMediosDePago_sp
@id_cliente INT
AS
BEGIN
	SELECT id_medio_de_pago, coalesce(RIGHT(nro_tarjeta, 4),0) digitos FROM Medios_de_pago WHERE id_cliente =  @id_cliente
END
GO

-----registrarMedioDePago-----
CREATE PROCEDURE registrarMedioDePago_sp
@id_cliente INT,
@numero_tarjeta INT,
@titular varchar
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Medios_de_pago mp WHERE nro_tarjeta = @numero_tarjeta AND id_cliente = @id_cliente)
		BEGIN
		INSERT INTO Medios_de_pago(id_cliente, descripcion, nro_tarjeta, titular)
		VALUES(@id_cliente, 'Tarjeta', @numero_tarjeta, @titular)
		END
		ELSE
		BEGIN
		RAISERROR('Tarjeta ya registrada', 16, 1)
		END
END
GO

-----buscarUbicacionesPorPublicacion-----
create procedure buscarUbicacionesPorPublicacion_sp (@id_publicacion int) as begin
	select t.descripcion descripcion, count(distinct asiento)*count(distinct fila) asientos, sin_numerar, precio, count(distinct fila) filas, u.id_ubicacion
	from Ubicaciones u 
	join UbicacionXEspectaculo e on e.id_ubicacion = u.id_ubicacion 
	join TiposDeUbicacion t on t.id_tipo_ubicacion = u.codigo_tipo_ubicacion
	join Espectaculos ee on ee.id_espectaculo = e.id_espectaculo
	join Publicaciones p on p.id_publicacion = ee.id_publicacion
	where p.id_publicacion = 1 and e.id_compra is null
	group by t.descripcion, sin_numerar, precio, u.id_ubicacion
end
GO

-----traerTodasRazonesSociales-----
create procedure traerTodasRazonesSociales_sp as begin
	select distinct razon_social from Empresas
end
GO

-----top5ClientesPuntosVencidos-----
CREATE PROCEDURE top5ClientesPuntosVencidos_sp(
@fecha_inicio VARCHAR(30),
@fecha_fin VARCHAR(30),
@fecha_actual VARCHAR(30)
)
AS
BEGIN
	
	SELECT TOP 5 nombre, apellido, SUM(cantidad_puntos) 'Puntos Vencidos' FROM Clientes c
	JOIN Puntos p ON(p.id_cliente = c.id_cliente)
	WHERE fecha_vencimiento < CONVERT(DATETIME, @fecha_actual, 127) AND (fecha_vencimiento BETWEEN CONVERT(DATETIME, @fecha_inicio, 127) AND CONVERT(DATETIME, @fecha_fin, 127))
	GROUP BY nombre, apellido
	ORDER BY SUM(cantidad_puntos) DESC
END
GO

-----top5ClientesComprasParaUnaEmpresa----
CREATE PROCEDURE top5ClienteComprasParaUnaEmpresa_sp
(@razon_social NVARCHAR(50),
@fecha_inicio VARCHAR(30),
@fecha_fin VARCHAR(30)
)
AS
BEGIN
	SELECT TOP 5 nombre, apellido, documento, COUNT(id_ubicacion) 'Cantidad de compras', razon_social
	FROM Clientes cc
	JOIN Compras c ON(c.id_cliente = cc.id_cliente)
	JOIN UbicacionXEspectaculo uxe ON(uxe.id_compra = c.id_compra)
	JOIN Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
	JOIN Publicaciones p ON (p.id_publicacion = e.id_publicacion)
	JOIN Empresas ee ON(p.id_empresa = ee.id_empresa)

	WHERE c.fecha > CONVERT(DATETIME, @fecha_inicio, 127) AND fecha < CONVERT(DATETIME, @fecha_fin, 127)  AND ee.razon_social = @razon_social
	GROUP BY ee.id_empresa, razon_social, nombre, apellido, documento
	ORDER BY COUNT(id_ubicacion) DESC
END
GO

-----top5LocalidadesNoVendidasEmpresa-----
CREATE PROCEDURE top5EmpresasLocalidadesNoVendidas_sp
@grado VARCHAR(20),
@fecha_inicio VARCHAR(30),
@fecha_fin VARCHAR(30)
AS
BEGIN
	SELECT TOP 5 razon_social 'Razon social', cuit, COUNT(id_ubicacion) 'Ubicaciones no vendidas' FROM Publicaciones p
	JOIN Espectaculos e ON (e.id_publicacion = p.id_publicacion)
	JOIN UbicacionXEspectaculo uxe ON(uxe.id_espectaculo = e.id_espectaculo)
	JOIN Grados_publicacion gp ON(gp.id_grado_publicacion = p.id_grado_publicacion)
	JOIN Empresas emp ON(emp.id_empresa = p.id_empresa)
	WHERE uxe.id_compra IS NULL
		AND gp.nombre = @grado
		AND e.fecha_evento> CONVERT(DATETIME, @fecha_inicio, 127) AND e.fecha_evento < CONVERT(DATETIME, @fecha_fin, 127)
	GROUP BY razon_social, cuit, p.id_publicacion, fecha_evento, comision
	ORDER BY fecha_evento ASC, comision DESC
END
GO

-----agregarFactura-----
create procedure agregarFactura_sp (@razonSocial NVARCHAR(255), @total NUMERIC(18,2)) as begin
	declare @id_empresa int
	set @id_empresa = (select id_empresa from Empresas where razon_social = @razonSocial)
	insert into Facturas (id_empresa, fecha_facturacion, importe_total) values (@id_empresa, getdate(), @total)

	select top 1 id_factura from Facturas where id_empresa = @id_empresa and importe_total = @total order by fecha_facturacion desc
end
GO

-----actualizarCompraFactura-----
create procedure actualizarCompraFactura_sp (@id_factura int, @id_compra int) as begin
	update Compras set id_factura = @id_factura where id_compra = @id_compra
end
GO

-----buscarComprasNoFacturadas-----
create procedure buscarComprasNoFacturadas_sp (@razonSocial varchar(255)) as begin
	select c.id_compra, descripcion, coalesce(comision, 0) comision, coalesce(importe, 0) importe from Compras c 
	join UbicacionXEspectaculo u on c.id_compra = u.id_compra
	join Espectaculos es on es.id_espectaculo = u.id_espectaculo
	join Publicaciones p on p.id_publicacion = es.id_publicacion
	join Empresas e on e.id_empresa = p.id_empresa and e.razon_social = @razonSocial
	where id_factura is null 
end
GO

----------TRIGGERS----------

-----insertarNuevoEspectaculo-----
CREATE TRIGGER insertarNuevoEspectaculo ON Espectaculos
INSTEAD OF INSERT
AS
BEGIN
DECLARE @id_publicacion INT, @fecha_inicio DATETIME, @fecha_evento DATETIME, @estado_espectaculo CHAR(15) 
DECLARE cur CURSOR FOR 
SELECT id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo FROM inserted
DECLARE @last_id INT
SET @last_id = (SELECT MAX(id_espectaculo) FROM Espectaculos) + 1
OPEN cur
FETCH NEXT FROM cur INTO @id_publicacion, @fecha_inicio, @fecha_evento, @estado_espectaculo
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO Espectaculos(id_espectaculo, id_publicacion, fecha_inicio, fecha_evento, estado_espectaculo)
VALUES (@last_id, @id_publicacion, @fecha_inicio, @fecha_evento, @estado_espectaculo)
SET @last_id += 1
FETCH NEXT FROM cur INTO @id_publicacion, @fecha_inicio, @fecha_evento, @estado_espectaculo
END
CLOSE cur
DEALLOCATE cur
END
GO

-----insertarNuevaFactura-----
CREATE TRIGGER insertarNuevaFactura ON Facturas
INSTEAD OF INSERT
AS
BEGIN
DECLARE @fecha_facturacion DATETIME, @importe_total NUMERIC(18,2), @id_empresa INT 
DECLARE cur CURSOR FOR 
SELECT fecha_facturacion, importe_total, id_empresa FROM inserted
DECLARE @last_id INT
SET @last_id = (SELECT MAX(id_factura) FROM Facturas) + 1
OPEN cur
FETCH NEXT FROM cur INTO @fecha_facturacion, @importe_total, @id_empresa
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO Facturas(id_factura, fecha_facturacion, importe_total, id_empresa)
VALUES (@last_id, @fecha_facturacion, @importe_total, @id_empresa)
SET @last_id += 1
FETCH NEXT FROM cur INTO @fecha_facturacion, @importe_total, @id_empresa
END
CLOSE cur
DEALLOCATE cur
END
GO

-----rollInhabilitado-----
CREATE TRIGGER rolInhabilitado_tr
ON Roles
AFTER UPDATE
AS
BEGIN	
	IF((SELECT habilitado FROM DELETED) <> (SELECT habilitado FROM INSERTED))
	BEGIN
		DECLARE @id_rol_modificado INT
		SET @id_rol_modificado = (SELECT id_rol FROM DELETED)

		DELETE FROM UsuarioXRol
		WHERE id_rol = @id_rol_modificado

	END
END
GO

-----insertarNuevaCompra-----
CREATE TRIGGER insertarNuevaCompra ON Compras
INSTEAD OF INSERT
AS
BEGIN
	DECLARE @id_cliente INT, @id_medio_de_pago INT, @fecha DATETIME, @importe BIGINT
	DECLARE cur CURSOR FOR 
	SELECT id_cliente, id_medio_de_pago, fecha, importe FROM inserted
	DECLARE @last_id INT
	SET @last_id = (SELECT MAX(id_compra) FROM Compras) + 1
	OPEN cur
		FETCH NEXT FROM cur INTO @id_cliente, @id_medio_de_pago, @fecha, @importe
		WHILE @@FETCH_STATUS = 0
		BEGIN
		INSERT INTO Compras(id_cliente, id_medio_de_pago, id_factura, fecha, importe, id_compra)
		VALUES (@id_cliente, @id_medio_de_pago, NULL, @fecha, @importe, @last_id)
		SET @last_id += 1
		FETCH NEXT FROM cur INTO @id_cliente, @id_medio_de_pago, @fecha, @importe
		END
	CLOSE cur
	DEALLOCATE cur
	select @last_id-1 id
END
GO


-----finalizarEspectaculo-----
CREATE TRIGGER finalizarEspectaculoAgotado_tg
ON Compras
AFTER INSERT
AS
BEGIN
	DECLARE @id_espectaculo INT = (SELECT DISTINCT e.id_espectaculo FROM Espectaculos e
								JOIN UbicacionXEspectaculo uxe ON(uxe.id_espectaculo = e.id_espectaculo)
								WHERE uxe.id_compra = (SELECT id_compra FROM INSERTED))
	--DECLARE @id_publicacion INT = (SELECT id_publicacion FROM Espectaculos WHERE id_espectaculo = @id_espectaculo)
	IF (dbo.getCantidadEntradasEspectaculo(@id_espectaculo) = dbo.getCantidadEntradasVendidas(@id_espectaculo))
	BEGIN
		UPDATE Espectaculos
		SET estado_espectaculo = 'Finalizada'
		WHERE id_espectaculo = @id_espectaculo
	END
END
GO


----------FUNCIONES----------

-----getCantidadEntradasPublicacion-----
CREATE FUNCTION getCantidadEntradasEspectaculo(@id_espectaculo INT)
RETURNS INT
AS
BEGIN
	RETURN (SELECT COUNT(DISTINCT id_ubicacion_espectaculo) FROM UbicacionXEspectaculo uxe
			JOIN Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
			WHERE e.id_espectaculo = @id_espectaculo)
END
GO


-----getCantidadEntradasVendidas-----
CREATE FUNCTION getCantidadEntradasVendidas(@id_espectaculo INT)
RETURNS INT
AS
BEGIN
	RETURN (SELECT COUNT(DISTINCT id_ubicacion_espectaculo) FROM UbicacionXEspectaculo uxe
			WHERE id_espectaculo = @id_espectaculo AND id_compra IS NOT NULL)
END
GO

-----vaciarEspectaculosPublicacion-----
CREATE PROCEDURE vaciarEspectaculosPublicacion_sp(
@id_publicacion INT)
AS
BEGIN

CREATE TABLE #UbicacionesDeUnaPublicacion(
id_espectaculo INT,
id_ubicacion INT,
id_ubicacion_espectaculo INT
)

INSERT INTO #UbicacionesDeUnaPublicacion(id_espectaculo, id_ubicacion, id_ubicacion_espectaculo)
SELECT e.id_espectaculo, u.id_ubicacion, uxe.id_ubicacion_espectaculo FROM Espectaculos e
JOIN UbicacionXEspectaculo uxe ON (e.id_espectaculo = uxe.id_espectaculo)
JOIN Ubicaciones u ON (u.id_ubicacion = uxe.id_ubicacion)
WHERE e.id_publicacion = @id_publicacion

DELETE UbicacionXEspectaculo
WHERE id_ubicacion_espectaculo IN (SELECT id_ubicacion_espectaculo FROM #UbicacionesDeUnaPublicacion) OR id_ubicacion IN (SELECT id_ubicacion FROM #UbicacionesDeUnaPublicacion) OR id_espectaculo IN (SELECT id_espectaculo FROM #UbicacionesDeUnaPublicacion)

DELETE Ubicaciones 
WHERE id_ubicacion IN (SELECT id_ubicacion FROM #UbicacionesDeUnaPublicacion)

DELETE Espectaculos 
WHERE id_espectaculo IN (SELECT id_espectaculo FROM #UbicacionesDeUnaPublicacion)

DROP TABLE #UbicacionesDeUnaPublicacion

END
GO

CREATE PROCEDURE filasDisponiblesSegunEspectaculo_sp
@id_espectaculo INT,
@precio INT
AS
BEGIN
	SELECT fila
	FROM UbicacionXEspectaculo uxe
	JOIN Ubicaciones u ON (u.id_ubicacion = uxe.id_ubicacion)
	WHERE uxe.id_espectaculo = @id_espectaculo AND id_compra is NULL AND @precio = precio
	GROUP BY fila
END
GO

CREATE PROCEDURE asientosDisponiblesSegunEspectaculoYFila_sp
@id_espectaculo INT,
@fila CHAR,
@precio INT
AS
BEGIN
	SELECT asiento
	FROM UbicacionXEspectaculo uxe
	JOIN Ubicaciones u ON (u.id_ubicacion = uxe.id_ubicacion)
	WHERE uxe.id_espectaculo = @id_espectaculo AND id_compra is NULL AND fila = @fila AND @precio = precio
	GROUP BY asiento
END
GO