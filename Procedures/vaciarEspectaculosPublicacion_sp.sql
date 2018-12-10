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