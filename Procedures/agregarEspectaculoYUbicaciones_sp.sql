CREATE PROCEDURE agregarUbicacionXEspectaculo_sp(
@id_ubicacion INT,
@id_espectaculo INT
)
AS
BEGIN
	INSERT INTO UbicacionXEspectaculo(id_ubicacion, id_espectaculo)
	VALUES(@id_ubicacion, @id_espectaculo)
END