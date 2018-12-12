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
DROP procedure buscarPublicacionesPorCriterio_sp
DROP PROCEDURE registroCliente_sp
DROP PROCEDURE registroEmpresa_sp
DROP PROCEDURE getPremios_sp
DROP PROCEDURE borrarPuntos_sp
DROP PROCEDURE historialClienteConOffset_sp
DROP PROCEDURE registrarCompra_sp
DROP PROCEDURE registrar
DROP PROCEDURE getPublicacionesDeUsuario_sp
DROP PROCEDURE registarPublicacion_sp
DROP PROCEDURE agregarEspectaculo_sp
DROP PROCEDURE agregarUbicaciones_sp
DROP PROCEDURE agregarUbicacionSinNumerar_sp
DROP PROCEDURE agregarUbicacionNumerada_sp
DROP PROCEDURE actualizarGradoPublicacion_sp

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

drop procedure borrarPuntos_sp

EXEC borrarPuntos_sp 300, 'ro'


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


-----getRubrosDePublicacion-----
CREATE PROCEDURE getRubrosDePublicacion_sp 
@id_publicacion NUMERIC
AS
BEGIN
	SELECT r.id_rubro, r.descripcion FROM Rubros r JOIN Publicaciones p ON (r.id_rubro = p.id_rubro)
	WHERE id_publicacion = @id_publicacion
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
		fecha_creacion, coalesce(documento,0) documento, calle, coalesce(numero_calle,0) numero_calle
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
	WHERE username = @username AND fecha_vencimiento IS NOT NULL AND fecha_vencimiento > CONVERT(DATETIME, @fecha, 120)

END
GO


-----getRubros-----
CREATE PROCEDURE getRubros_sp
AS
BEGIN
	SELECT descripcion FROM Rubros WHERE descripcion <> ''
END
GO


-----buscarPublicacionesPorCriterio-----
create procedure buscarPublicacionesPorCriterio_sp (@descripcion varchar(255), @categorias varchar(255), @desde date, @hasta date, @offset INT) as begin
	declare @query nvarchar(2000)
	set @query = 
	'select p.descripcion descripcion, r.descripcion rubro, direccion, p.id_publicacion id from Publicaciones p join Espectaculos e on p.id_publicacion = e.id_publicacion 
	join Rubros r on r.id_rubro = p.id_rubro
	where e.estado_espectaculo = ''Publicada'''

	if ( @desde is not null and @hasta is not null) begin set @query = 
		@query + ' and e.fecha_evento between ''' + (select convert(varchar, @desde, 22)) + ''' and ''' +  (select convert(varchar, @hasta, 22)) + ''' ' end

	print @query
	if ( @descripcion is not null) begin set @query = @query + 'and p.descripcion like ' + (@descripcion) + ' ' end
	if ( @categorias is not null) begin set @query = @query + 'and r.descripcion in [' + (@categorias) + '] ' end

	set @query = @query + 'order by p.id_grado_publicacion ASC offset ' + @offset + ' rows fetch next 10 rows only'

	exec sp_executesql @query
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
	INSERT INTO Compras(id_cliente, id_medio_de_pago, id_factura, fecha, importe)
	VALUES(@id_cliente, @id_medio_de_pago, NULL, CONVERT(DATETIME, @fecha, 120), @importe)
END
go


select fecha FROM Compras
EXEC registrarCompra_sp 768, 1, 300, '2019-02-13 00:00:00'

select * from clientes order by id_usuario desc


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


-----registrarPublicacion-----
CREATE PROCEDURE registrarPublicacion_sp(
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

	SELECT SCOPE_IDENTITY() AS id_publicacion
END
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