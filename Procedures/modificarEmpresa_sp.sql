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

drop procedure modificarEmpresa_sp