--.--.--.--.--.--.--.--Script inicializacion BD--.--.--.--.--.--.--.--


CREATE PROCEDURE inicializarBD_sp
AS
BEGIN
	EXEC eliminarTablas_sp
	EXEC crearTablas_sp
	EXEC migrarTablas_sp

	EXEC calcularTotalesCompras_sp
END



EXEC inicializarBD_sp
DROP PROCEDURE inicializarBD_sp




CREATE PROCEDURE dropProcedures_sp
AS
BEGIN
	DROP PROCEDURE crearTablas_sp
	DROP PROCEDURE migrarTablas_sp
	DROP PROCEDURE eliminarTablas_sp
	
	DROP PROCEDURE calcularTotalesCompras_sp



END



EXEC dropProcedures_sp