CREATE PROCEDURE historialCliente_sp
@cliente_id INT --o dni ?¿?
AS
BEGIN
	SELECT p.descripcion, e.fecha_evento, c.importe, COUNT(uxe.id_ubicacion_espectaculo) 'cantidad_asientos', mp.descripcion, RIGHT(mp.nro_tarjeta, 4)
	FROM Compras c
	JOIN UbicacionXEspectaculo uxe ON(uxe.id_compra = c.id_compra)
	JOIN Espectaculos e ON(e.id_espectaculo = uxe.id_espectaculo)
	JOIN Publicaciones p ON(p.id_publicacion = e.id_publicacion)
	JOIN Medios_de_pago mp ON(mp.id_medio_de_pago = c.id_medio_de_pago)
	WHERE c.id_cliente = @cliente_id
	GROUP BY c.id_compra, p.descripcion, e.fecha_evento, c.importe, mp.descripcion, mp.nro_tarjeta
	ORDER BY fecha_evento DESC
END

--Test
--EXEC historialCliente_sp 1
--SELECT * FROM Clientes WHERE id_cliente = 1
--SELECT * FROM gd_esquema.Maestra WHERE Cli_Dni = 45023700