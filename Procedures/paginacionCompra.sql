CREATE PROCEDURE paginacionCompra
@numeroDePagina INT
AS
BEGIN
DECLARE @maximoPorPagina INT = 10
DECLARE @offset INT = @numeroDePagina * @maximoPorPagina
IF((SELECT COUNT(*) FROM Compras) > @offset)
BEGIN
--Pendiente la forma de ordenamiento
SELECT * FROM Compras ORDER BY id_compra OFFSET @offset ROWS FETCH NEXT @maximoPorPagina ROWS ONLY
END
ELSE 
BEGIN
RAISERROR('Acceso a una p�gina fuera del limite', 14, 1)
END
END

/* Pruebas
exec paginacionCompra 9413 --Ante�ltima hoja
exec paginacionCompra 9414 --Muestra los ultimos 2 de la �ltima p�gina
exec paginacionCompra 9415 --Falla
*/
