
BEGIN TRANSACTION

update Usuarios set password = LOWER(CONVERT(char(100),HASHBYTES('SHA2_256', password),2)) where id_usuario < 112

COMMIT
                                 