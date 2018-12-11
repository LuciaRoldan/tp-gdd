CREATE PROCEDURE modificarCliente_sp
@id_cliente INT,
@nombre nvarchar(255),
@apellido nvarchar(255),
@mail NVARCHAR(50),
@documento NUMERIC(18,0),
@cuil NUMERIC(18,0),
@telefono NUMERIC(15),
@fecha_nacimiento DATETIME,
@tipo_documento varchar(5)
AS
BEGIN 
	IF EXISTS (SELECT * FROM dbo.Clientes WHERE id_cliente = @id_cliente) 
		BEGIN
		BEGIN TRANSACTION
		UPDATE dbo.Clientes
		SET nombre = @nombre, apellido = @apellido, mail = @mail, documento = @documento, cuil = @cuil, telefono = @telefono, fecha_creacion = @fecha_nacimiento, tipo_documento = @tipo_documento
		WHERE id_cliente = @id_cliente
		COMMIT TRANSACTION
		END
	ELSE
	RAISERROR('El cliente no existe', 20, 1) WITH LOG
END

